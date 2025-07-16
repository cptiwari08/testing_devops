import { useEffect, useState } from "react";
import ReactDOM from "react-dom/client";
import reportWebVitals from "./reportWebVitals";
import { MsalProvider } from "@azure/msal-react";
import { BrowserRouter as Router } from "react-router-dom";
import { authProvider } from "./common/utility/authProvider";
import { PublicClientApplication } from "@azure/msal-browser";
import Loader from "./common/components/Loader/Loader";
import "./index.scss";
import App from "./App";
import Authentication from "./Authentication";
import CrashBoundary from "./common/components/CrashBoundary/CrashBoundary";
import { Provider } from "react-redux";
import { store } from "./modules/redux/core/store";
import { EXTERNAL_APP_SETTINGS, isIframe } from "./common/utility/constants";
import { updateExternalAppSettings } from "./modules/redux/slices/userAuth";
import { startTokenMonitor } from "./common/utility/refreshToken";
import { validateDomain } from "./common/utility/utility";
import Unauthorized from "./common/components/Unauthorized/Unauthorized";
import SkeletonLoader from "./common/components/SkeletonLoader/SkeletonLoader";

startTokenMonitor();

const usePostMessageListener = () => {
  const [isSameDomain, setIsSameDomain] = useState<boolean | null>(null);

  useEffect(() => {
    if (!isIframe) {
      setIsSameDomain(false);
      return;
    }
    const handleMessage = (event: MessageEvent) => {
      if (event.data && event.data.type === EXTERNAL_APP_SETTINGS) {
        if (event.origin && validateDomain(event.origin)) {
          setIsSameDomain(true);
          const payload = { origin: event.origin, ...event.data.payload };
          store.dispatch(updateExternalAppSettings(payload));
        } else {
          setIsSameDomain(false);
        }
      }
    };
    window.addEventListener("message", handleMessage);
    return () => {
      window.removeEventListener("message", handleMessage);
    };
  }, []);

  return isSameDomain;
};

const RootComponent = () => {
  const isSameDomain = usePostMessageListener();
  const [pca, setPca] = useState<PublicClientApplication | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const initializeApp = async () => {
      setLoading(true);
      try {
        const pcaInstance = await authProvider.initializeAuthProvider();
        setPca(pcaInstance);
      } catch (error) {
        console.error("Error initializing app:", error);
      } finally {
        setLoading(false);
      }
    };
    if (isSameDomain) {
      initializeApp();
    }
  }, [isSameDomain]);

  if (isSameDomain === false) {
    return <Unauthorized />;
  }

  if (isSameDomain === null || loading || pca === null) {
    return window.location.pathname.includes("/configuration")? <Loader isLoading={true} />: <SkeletonLoader />;
  }

  return (
    <Provider store={store}>
      <Router>
        <MsalProvider instance={pca}>
          <Authentication>
            <CrashBoundary>
              <App />
            </CrashBoundary>
          </Authentication>
        </MsalProvider>
      </Router>
    </Provider>
  );
};

const root = document.getElementById("root");

if (root !== null) {
  ReactDOM.createRoot(root).render(<RootComponent />);
}

reportWebVitals();
