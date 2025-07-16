import { API_URL, REQUEST_TYPE, createAxiosRequest } from "./api";

export class AssistantService {
  public static saveProjectConfiguartion(projectConfig: any): Promise<string> {
    const SAVE_PROJECT_CONFIG = `${API_URL}/configuration/update`;
    return createAxiosRequest(
      REQUEST_TYPE.POST,
      SAVE_PROJECT_CONFIG,
      projectConfig
    );
  }
  public static getProjectConfiguration(key: string): Promise<string> {
    const GET_PROJECT_CONFIG = `${API_URL}/configuration/${key}`;
    return createAxiosRequest(REQUEST_TYPE.GET, GET_PROJECT_CONFIG);
  }
 
  public static updateProjectConfiguration(sourceConfig:any): Promise<string> {
    const UPDATE_PROJECT_CONFIG = `${API_URL}/configuration/update`;
    return createAxiosRequest(
      REQUEST_TYPE.POST,
      UPDATE_PROJECT_CONFIG,
      sourceConfig
    );
  }
  public static addSuggesion(suggestionList: any): Promise<string> {
    const ADD_SUGGESTION_CONFIG = `${API_URL}/suggestions`;
    return createAxiosRequest(
      REQUEST_TYPE.POST,
      ADD_SUGGESTION_CONFIG,
      suggestionList
    );
  }
  public static deleteSuggesion(id: string): Promise<string> {
    const DELETE_SUGGESTION_CONFIG = `${API_URL}/suggestions/${id}`;
    return createAxiosRequest(
      REQUEST_TYPE.DELETE,
      DELETE_SUGGESTION_CONFIG
    );
  }
  public static saveSuggesion(suggestionList: any): Promise<string> {
    const SAVE_PROJECT_CONFIG = `${API_URL}/suggestions/update`;
    return createAxiosRequest(
      REQUEST_TYPE.POST,
      SAVE_PROJECT_CONFIG,
      suggestionList
    );
  }
  
}
