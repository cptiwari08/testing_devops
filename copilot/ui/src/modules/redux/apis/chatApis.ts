import {
  IResponseFeedback,
  IChatIntermediaryResponse,
  IChatQuestion,
  IChatStatusQuery,
  IchatResponseHistory,
} from "../../model";
import {
  API_URL,
  REQUEST_TYPE,
  createAxiosRequest,
  createAxiosRequestWithFullResponse,
} from "./api";

export class ChatService {
  public static startChat(): Promise<string> {
    const GET_CHAT_START_ENDPOINT = `${API_URL}/chat/start`;
    return createAxiosRequest(REQUEST_TYPE.GET, GET_CHAT_START_ENDPOINT);
  }
  public static getChatHistory(
    chatId: string
  ): Promise<IchatResponseHistory[]> {
    const GET_CHAT_HISTORY_ENDPOINT = `${API_URL}/user/chat/${chatId}`;
    return createAxiosRequest(REQUEST_TYPE.GET, GET_CHAT_HISTORY_ENDPOINT);
  }

  public static async sendChatBotQuestion(
    request: IChatQuestion
  ): Promise<IChatIntermediaryResponse> {
    const POST_CHAT_BOT_QUESTION = `${API_URL}/chat`;
    return createAxiosRequest(
      REQUEST_TYPE.POST,
      POST_CHAT_BOT_QUESTION,
      request
    );
  }

  public static async sendChatFeedback(
    chatId: string,
    request: IResponseFeedback
  ): Promise<any> {
    const POST_CHAT_FEEDBACK = `${API_URL}/chat/feedback`;
    return createAxiosRequest(REQUEST_TYPE.POST, POST_CHAT_FEEDBACK, request);
  }

  public static async sendStatusURIRequest(
    payload: IChatStatusQuery,
    id: string
  ): Promise<any> {
    const POST_CHAT_BOT_QUESTION = `${API_URL}/chat/status/${id}`;
    return createAxiosRequestWithFullResponse(
      REQUEST_TYPE.POST,
      POST_CHAT_BOT_QUESTION,
      payload
    );
  }

  public static getDocumentByGuid(
    documentGUID: string,
    sourceName: string
  ): Promise<IchatResponseHistory[]> {
    const GET_DOCUMENT_ENDPOINT = `${API_URL}/documents/${documentGUID}?source=${sourceName}`;
    return createAxiosRequest(REQUEST_TYPE.GET, GET_DOCUMENT_ENDPOINT, {
      responseType: "blob",
    });
  }

  public static async clearSessionHistory(chatId: string): Promise<string> {
    const DELETE_CHAT_HISTORY = `${API_URL}/user/chat/${chatId}`;
    return createAxiosRequest(REQUEST_TYPE.DELETE, DELETE_CHAT_HISTORY);
  }

  public static downloadChatHistory(
    conversationId: string,
    messageId: string,
    fileType: string
  ) {
    const GET_DOWNLOAD_CHAT_API_URL = `${API_URL}/chat/${conversationId}/messages/${messageId}/history/export/${fileType}`;
    return createAxiosRequest(REQUEST_TYPE.GET, GET_DOWNLOAD_CHAT_API_URL, {
      responseType: "blob",
    });
  }
}
