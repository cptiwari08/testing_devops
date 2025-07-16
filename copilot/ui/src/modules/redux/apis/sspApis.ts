import { authProvider } from "../../../common/utility/authProvider";
import { REQUEST_TYPE, SSP_API_URL, createAxiosRequest } from "./api";

export const getCeToken = async (payload: any) => {
  const apiURL = SSP_API_URL + '/auth/token';
  const accessToken = await authProvider.getToken();
  const header = {
    headers: {
      "Authorization": `Bearer ${accessToken}`,
      "Request-Timestamp": Date.now(),
    },
  };
  return createAxiosRequest(REQUEST_TYPE.POST, apiURL, payload, header);
};
