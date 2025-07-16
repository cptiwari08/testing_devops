import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { IChatQuestion, IChatOutput, IChatState, IResetChatSession } from "../../model";
import { ResponseStatuses, ChatRole } from "../../../common/utility/constants";
import t from "../../../locales/en.json";
import { ChatService } from "../apis/chatApis";

const initialState: IChatState = {
  isLoading: false,
  chatId: "",
  question: {
    messageId: 0,
    question: "",
    chatId: "",
    sources: [],
    context: {},
  },
  output: {
    messageId: 0,
    role: "",
    content: "",
    InstanceId: "",
    sources: [],
    context: {},
    response: [],
    createdTime: "",
    lastUpdatedTime: "",
    status: "",
    inputSources:[]
  },
  feedbackResponse: {},
  intermediaryResponse: {
    id: "",
    purgeHistoryDeleteUri: "",
    sendEventPostUri: "",
    statusQueryGetUri: "",
    terminatePostUri: "",
  },
  messageList: [],
  resetChatMessages: false,
};

const chatSlice = createSlice({
  name: "chat",
  initialState,
  reducers: {
    startChat: (state) => {
      state.isLoading = true;
    },
    startChatError: (state) => {
      state.chatId = "";
      state.isLoading = false;
    },
    startChatSuccess: (state, action: PayloadAction<string>) => {
      state.chatId = action.payload;
      state.isLoading = false;
    },
    chatbotUpdateQuestion: (
      state,
      { payload }: PayloadAction<IChatOutput>
    ) => {
      state.isLoading = true;
      state.output = payload;
      state.output.status = ResponseStatuses.IN_PROGRESS;
    },
    chatbotQuestion: (state, { payload }: PayloadAction<IChatQuestion>) => {
      state.isLoading = true;
      state.output.status = ResponseStatuses.IN_PROGRESS;
    },
    chatbotQuestionSuccess: (
      state,
      { payload }: PayloadAction<IChatOutput>
    ) => {
      state.output = payload;
      state.output.status = ResponseStatuses.SUCCESS;
      state.isLoading = false;
    },
    chatbotQuestionError: (state, { payload }: PayloadAction<any>) => {
      state.isLoading = false;
      state.output.role = ChatRole.ASSISTANT;
      state.output.content = t.errorOnResponse;
      state.output.status = ResponseStatuses.ERROR;
      state.output.InstanceId = payload.response.data.InstanceId;
      state.output.messageId = payload.response.data.messageId;
      state.output.StatusCode = payload.status;
    },
    chatbotResetChatMessages: (state, { payload }: PayloadAction<IResetChatSession>) => {
      state.resetChatMessages = payload.reset;
      if (payload?.chatId) {
        ChatService.clearSessionHistory(payload.chatId);
      }
    },
    feedbackMessage: (state, { payload }: PayloadAction<any>) => {
     //If invoked for consent feedback

      if(typeof payload.Consent !== "undefined") {
        state.output.context = {...state.output.context, consent: payload.Consent};
        state.output.InstanceId = payload.InstanceId;
        state.output.messageId = payload.MessageID;
        state.output.role = ChatRole.ASSISTANT;
      }
    },

    feedbackMessageSuccess: (state, { payload }: PayloadAction<any>) => {
    },

    feedbackMessageError: (state, { payload }: PayloadAction<any>) => {
    },

    getChatHistory: (state) => {
      state.isLoading = true;
    },
    getChatHistorySuccess: (state, { payload }: PayloadAction<any>) => {
      state.messageList = payload;
      state.output.status = ResponseStatuses.SUCCESS;
      state.isLoading = false;
    },
    getChatHistoryError: (state) => {
      state.isLoading = false;
    },
  },
});

export const chatActions = chatSlice.actions;

export default chatSlice.reducer;
