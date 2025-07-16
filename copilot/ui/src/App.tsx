import { Route, Routes } from "react-router-dom";
import PCOHome from "./modules/pages/PCOHome/PCOHome";
import "./App.css";
import AssistantConfigurations from "./modules/pages/AssistantConfigurations/AssistantConfigurations";
import { useAppDispatch, useAppSelector } from "./modules/hooks/hooks";
import { useEffect, useMemo, useRef, useState } from "react";
import { fetchUserDetails } from "./modules/redux/slices/userSlice";
import { SOURCE_CONFIGS_KEY, SessionKeys } from "./common/utility/constants";
import { getProjectDocs } from "./modules/components/ViewDocsModal/ViewDocsModalSlice";
import { appInsights } from "./common/utility/appInsights";
import { getProjectConfigByKey, getUserApps } from "./modules/redux/slices/projectConfig";
import "ag-grid-enterprise";
import { LicenseManager } from "ag-grid-enterprise";
import config from "./common/configs/env.config";

const TRACKING_PERIOD = 60 * 1000;
const IDLE_TIME = 2 * 60 * 1000;
const AG_GRID_LICENSE_KEY = config.AG_GRID_LICENSE_KEY|| "";
LicenseManager.setLicenseKey(AG_GRID_LICENSE_KEY);

const App = () => {
  const [isIdle, setIsIdle] = useState(false);
  const idleTimeoutRef = useRef<NodeJS.Timeout | null>(null);
  const intervalIdRef = useRef<NodeJS.Timeout | null>(null);

  const dispatch = useAppDispatch();
  const userAuthDetails = useAppSelector((state) => state.userAuth.userAuthDetails);
  const userDetails = useAppSelector((state) => state.user.userDetails);

  const encodedEmail = useMemo(() => {
    return userAuthDetails?.email ? btoa(userAuthDetails.email) : null;
  }, [userAuthDetails?.email]);

  const chatId = useAppSelector((state) => sessionStorage.getItem(SessionKeys.CHAT_ID) || state.chat.chatId);

  useEffect(() => {
      dispatch(getProjectDocs());
  }, []);
  
  useEffect(() => {
    dispatch(getUserApps());
    dispatch(getProjectConfigByKey({ key: SOURCE_CONFIGS_KEY }));
  }, [dispatch]);

  useEffect(() => {
    if (!userDetails && encodedEmail) {
      dispatch(fetchUserDetails(encodedEmail));
    }
  }, [userDetails, encodedEmail, dispatch]);

  useEffect(() => {
    const trackTimeSpent = () => {
      if (!isIdle) {
        appInsights.trackMetric({
          name: "PageVisitTime",
          average: TRACKING_PERIOD,
          properties: {
            CapitalEdgeAppName: "Capital Edge Assistant",
            CapitalEdgeAppPlatformName: "Capital Edge Assistant",
            CapitalEdgePageName: "Assistant",
            CapitalEdgeProjectFriendlyId: userAuthDetails?.project_friendly_id,
            CapitalEdgeUserEmail: userAuthDetails?.email,
            PageUrl: window.location.href,
          },
        });
      }
    };

    const startTracking = () => {
      if (!intervalIdRef.current) {
        intervalIdRef.current = setInterval(trackTimeSpent, TRACKING_PERIOD);
      }
    };

    const stopTracking = () => {
      if (intervalIdRef.current) {
        clearInterval(intervalIdRef.current);
        intervalIdRef.current = null;
      }
    };

    const resetIdleTimeout = () => {
      if (isIdle) {
        setIsIdle(false);
        startTracking();
      }

      if (idleTimeoutRef.current) {
        clearTimeout(idleTimeoutRef.current);
      }

      idleTimeoutRef.current = setTimeout(() => {
        setIsIdle(true);
        stopTracking();
      }, IDLE_TIME);
    };

    const debounceResetIdleTimeout = debounce(resetIdleTimeout, 2000);

    window.addEventListener('keypress', debounceResetIdleTimeout);
    window.addEventListener('click', debounceResetIdleTimeout);
    startTracking();

    return () => {
      stopTracking();
      window.removeEventListener('keypress', debounceResetIdleTimeout);
      window.removeEventListener('click', debounceResetIdleTimeout);

      if (idleTimeoutRef.current) {
        clearTimeout(idleTimeoutRef.current);
      }
    };
  }, [isIdle]);

  const debounce = (func: () => void, delay: number) => {
    let timeoutId: NodeJS.Timeout | null = null;

    return function () {
      clearTimeout(timeoutId!);
      timeoutId = setTimeout(func, delay);
    };
  };

  return (
    <Routes>
      <Route path="/:siteName/copilot/configurations" element={<AssistantConfigurations />} />
      <Route path="/:siteName/copilot" element={<PCOHome />} />
      <Route path="/:siteName/" element={<PCOHome />} />
    </Routes>
  );
};

export default App;