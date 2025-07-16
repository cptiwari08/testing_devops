import {
    API_URL,
    REQUEST_TYPE,
    createAxiosRequest,
  } from "./api";

  export class FooterService {
    public static saveFeedback(feedback:any): Promise<string> {
        const SAVE_ASSISTANT_FEEDBACK = `${API_URL}/chat/copilot-feedback`;
        return createAxiosRequest(REQUEST_TYPE.POST,SAVE_ASSISTANT_FEEDBACK,feedback);
      }
    
      
  }