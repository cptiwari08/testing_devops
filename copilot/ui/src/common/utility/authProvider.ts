import { Configuration, PublicClientApplication, AccountInfo, BrowserAuthError, InteractionRequiredAuthError } from "@azure/msal-browser";
import { DEFAULT_TOKEN_RENEWAL_OFFSET_SECONDS, isIframe } from "./constants";
import config from "../configs/env.config";

const msalConfig: Configuration = {
    auth: {
        clientId: config.AZURE_AD_CLIENT_ID || '',
        authority: config.AZURE_AD_AUTHORITY,
        postLogoutRedirectUri: null,
        redirectUri: config.REACT_APP_URL || window.location.href,
    },
    system: {
        allowRedirectInIframe: true,
        loadFrameTimeout: 11000,
        tokenRenewalOffsetSeconds: parseInt(config.TOKEN_RENEWAL_OFFSET_SECONDS ?? DEFAULT_TOKEN_RENEWAL_OFFSET_SECONDS)
    },
    cache: {
        cacheLocation: "sessionStorage",
        storeAuthStateInCookie: false
    },
};
const loginRequest = {
    scopes: [config.AZURE_AD_APP_SCOPE || ''],
};
let pca: PublicClientApplication | null = null;

const initializeAuthProvider = async (): Promise<PublicClientApplication> => {
    pca = new PublicClientApplication(msalConfig);
    try {
        await pca.initialize();
        await handleRedirectResponse();
        return pca;
    } catch (error) {
        console.error("Error initializing auth provider:", error);
        throw error;
    }
};

const handleRedirectResponse = async (): Promise<void> => {
    const response = await pca?.handleRedirectPromise();
    if (response && response.account !== null) {
        const { account } = response;
        pca?.setActiveAccount(account);
    } else {
        await handleNullAccount();
    }
};

const handleNullAccount = async (): Promise<AccountInfo | null> => {
    const account = await handleSetAccount();
    if (account) {
        return account;
    }
    await authProvider.login();
    return pca?.getActiveAccount() || null;
};


const handleSetAccount = async (): Promise<AccountInfo | null> => {
    const currentAccounts = pca?.getAllAccounts();
    if (currentAccounts && currentAccounts?.length === 1) {
        pca?.setActiveAccount(currentAccounts[0]);
        return currentAccounts[0];
    } else if (currentAccounts && currentAccounts?.length > 1) {
        console.warn('Multiple accounts detected. Handling of multiple accounts has not been implemented.');
    }
    return null;
};

let isInteractionInProgress = false;

const handleLogin = async (forceToLogin: boolean = false): Promise<void> => {
    const accounts = pca?.getAllAccounts();
    if (!forceToLogin && accounts && accounts.length > 0) {
        pca?.setActiveAccount(accounts[0]);
        return;
    }
    if (isInteractionInProgress) {
        console.log("An interaction process is already in progress.");
        return;
    }

    try {
        const account = pca?.getActiveAccount();
        if (!forceToLogin && account !== null) {
            return
        }
       
        isInteractionInProgress = true;
        try {
            const loginResponse = await pca?.ssoSilent(loginRequest);
            if (loginResponse && loginResponse.account !== null) {
                const { account } = loginResponse;
                pca?.setActiveAccount(account);
            }
        } catch (err) {
            console.warn("SSO Silent Login error, falling back to interactive login.");
            if (isIframe) {
                await pca?.loginPopup(loginRequest);
            } else {
                await pca?.loginRedirect(loginRequest);
            }
        }
    } catch (error) {
        if (error instanceof BrowserAuthError && error.errorCode === "interaction_in_progress") {
            console.warn("Interaction is currently in progress. Please ensure that this interaction has been completed before calling an interactive API.");
            return;
        }
        if (error instanceof BrowserAuthError && error.errorCode === "popup_window_error") {
            console.warn("Popup window error occurred in silent SSO. Falling back to interactive login.");
            await pca?.loginRedirect(loginRequest);
        }
        console.error("Interactive login error:", error);
    } finally {
        isInteractionInProgress = false;
    }
};


export const authProvider = {
    pca,
    initializeAuthProvider,
    login: async (): Promise<void> => {
        try {
            await handleLogin();
        } catch (error) {
            console.error("Login error:", error);
            await handleLogin();
        }
    },

    logout: (): void => {
        if (!pca) {
            throw new Error("AuthProvider has not been initialized.");
        }
        pca.logoutRedirect();
    },

    getAccount: (): AccountInfo | null => {
        if (!pca) {
            throw new Error("AuthProvider has not been initialized.");
        }

        return pca.getActiveAccount() || null;
    },

    getToken: async (): Promise<string> => {
        if (!pca) {
            throw new Error("AuthProvider has not been initialized.");
        }

        try {
            const account = pca.getActiveAccount() || await handleNullAccount();
            const response = await pca.acquireTokenSilent({
                scopes: [config.AZURE_AD_API_SCOPE || ''],
                account: account || undefined,
                authority: config.AZURE_AD_AUTHORITY,
                extraQueryParameters: {
                    login_hint: account?.idTokenClaims?.preferred_username || ''
                }
            });
            return response.accessToken || "";
        } catch (error) {
            console.error("Error acquiring token:", error);
            if (error instanceof InteractionRequiredAuthError) {
                await handleLogin(true);
                return await authProvider.getToken();
            }
            throw error;
        }
    }
};
