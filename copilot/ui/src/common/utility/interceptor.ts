import axios from "axios";
import config from "../configs/env.config";
import { EnhancedStore } from "@reduxjs/toolkit";
import { UserAuthState } from "../../modules/redux/slices/userAuth";
import moment from "moment";
import { refreshToken } from "./refreshToken";
import { authProvider } from "./authProvider";

const ANONYMOUS_ACCESS_URLS = ["/auth/token"];
const apiScope = config.AZURE_AD_API_SCOPE;
const authority = config.AZURE_AD_AUTHORITY;

const isAnonymousAccessURL = (url: string) => {
  return ANONYMOUS_ACCESS_URLS.some((bypassUrl) => url.includes(bypassUrl));
};

export function setupInterceptors(store: EnhancedStore) {
  axios.interceptors.request.use(
    async (config) => {
      if (apiScope && authority && !isAnonymousAccessURL(config.url || "")) {
        const { userAuth }: { userAuth: UserAuthState } = store.getState();
        if (userAuth.token) {
          let ceToken: unknown | string = userAuth.token;
          const currentTime = moment.utc().unix();
          const accessToken = await authProvider.getToken();
          config.headers["authorization-idp"] = accessToken;
          const isExpired = currentTime > userAuth.userAuthDetails.exp;
          if (isExpired) {
            ceToken = await refreshToken();
          }
          config.headers.Authorization = `Bearer ${ceToken}`;
        }
      }
      return config;
    },
    (error) => Promise.reject(error)
  );
}
