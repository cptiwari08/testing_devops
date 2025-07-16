import { API_URL, REQUEST_TYPE, createAxiosRequest } from "./api";

export const getUserDetails = (base64EncodedEmail: string) => {
    const GET_USER_DETAILS_API_URL = `${API_URL}/user/${base64EncodedEmail}`;
    return createAxiosRequest(REQUEST_TYPE.GET, GET_USER_DETAILS_API_URL, {});
}