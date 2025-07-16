import { API_URL, REQUEST_TYPE, createAxiosRequest } from "./api";

export const getProjectDocs = (documentIds: string[] = []) => {
  const GET_PROJECT_DOCS = `${API_URL}/user/projectdocs/details`;
  return createAxiosRequest(REQUEST_TYPE.POST, GET_PROJECT_DOCS, [
    ...documentIds,
  ]);
};

export const getUserAppList = () => {
  const GET_USER_APPS = `${API_URL}/user/apps`;
  return createAxiosRequest(REQUEST_TYPE.GET, GET_USER_APPS);
};

export const getAllSuggestion = () => {
  const GET_PROJECT_TYPES_ENDPOINT = `${API_URL}/suggestions`;
  return createAxiosRequest(REQUEST_TYPE.GET, GET_PROJECT_TYPES_ENDPOINT);
};

export const getAllSuggestionVisibleToAssistant = () => {
  const GET_PROJECT_TYPES_ENDPOINT = `${API_URL}/suggestions?filterVisibleToAssistant=true`;
  return createAxiosRequest(REQUEST_TYPE.GET, GET_PROJECT_TYPES_ENDPOINT);
};

export const clearProjectDocsRedis = () => {
  const GET_PROJECT_DOCS = `${API_URL}/user/projectdocs`;
  return createAxiosRequest(REQUEST_TYPE.DELETE, GET_PROJECT_DOCS);
};
