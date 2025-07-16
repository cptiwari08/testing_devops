import axios from "axios";
import config from "../../../common/configs/env.config";
export const API_URL = config.REACT_APP_API_URL;
export const SSP_API_URL = config.SSP_API_URL;

export enum REQUEST_TYPE {
  GET = "get",
  PUT = "put",
  DELETE = "delete",
  POST = "post",
  PATCH = "patch",
}

export const createAxiosRequest = <T>(
  type: REQUEST_TYPE,
  url: string,
  payload: Record<string, any> = {},
  headers?: { [key: string]: any }
): Promise<T> => {
  return axios[type](
    url,
    type === REQUEST_TYPE.DELETE ? { data: payload } : payload,
    headers
  )
    .then((response: any) => {
      return response.data;
    })
    .catch((error: any) => {
      throw error;
    });
};

export const createAxiosRequestWithFullResponse = <T>(
  type: REQUEST_TYPE,
  url: string,
  payload: Record<string, any> = {},
  headers?: { [key: string]: any }
): Promise<T> => {
  return axios[type](
    url,
    type === REQUEST_TYPE.DELETE ? { data: payload } : payload,
    headers
  )
    .then((response: any) => {
      return response;
    })
    .catch((error: any) => {
      throw error;
    });
};
