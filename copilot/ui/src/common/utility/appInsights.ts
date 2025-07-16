import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import config from "../configs/env.config";

const appInsights = new ApplicationInsights({
  config: {
    instrumentationKey: config.APPLICATION_INSIGHTS_KEY || '',
    enableAutoRouteTracking: true,
    autoTrackPageVisitTime: true,
  },
});

appInsights.loadAppInsights();

export { appInsights };