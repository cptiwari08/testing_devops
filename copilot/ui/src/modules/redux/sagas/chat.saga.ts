import { call, delay, put, takeLatest } from "redux-saga/effects";
import { chatActions } from "../slices/chatSlice";
import { ChatService } from "../apis/chatApis";
import { IChatStatusQuery, IchatResponseHistory } from "../../model";
import { SessionKeys } from "../../../common/utility/constants";

function* handleStartChat(): Generator {
  try {
    const chatId: any = yield call(ChatService.startChat);
    yield put(chatActions.startChatSuccess(chatId));
    sessionStorage.setItem(SessionKeys.CHAT_ID, chatId);
  } catch (error) {
    yield put(chatActions.startChatError());
  }
}
const startChatWatcher = function* () {
  yield takeLatest(chatActions.startChat, handleStartChat);
};
function* pollStatusQueryURI(payload: IChatStatusQuery, id: string): Generator {
  try {
    while (true) {
      const response: any = yield call(
        ChatService.sendStatusURIRequest,
        payload,
        id
      );
      if (response.status === 200) {
        yield put(chatActions.chatbotQuestionSuccess(response.data));
        break;
      }
      // If response is not 200 OK, wait for 1 second before polling again
      yield delay(3000);
    }
  } catch (error) {
    yield put(chatActions.chatbotQuestionError(error));
  }
}

function* sendChatbotQuestion(action: any): Generator {
  try {
    const data: any = yield call(
      ChatService.sendChatBotQuestion,
      action.payload
    );
    const chatId = sessionStorage.getItem(SessionKeys.CHAT_ID);
    if (chatId) {
      yield call(pollStatusQueryURI, { uri: data.statusQueryGetUri, instanceId: data.id, messageId: action.payload.messageId }, chatId);
    }
  } catch (error) {
    yield put(chatActions.chatbotQuestionError(error));
  }
}

function* sendResponseFeedBack(action: any): Generator {
  try {
    const chatId: string = action.payload.ChatId;
    if (chatId) {
      const data = yield call(
        ChatService.sendChatFeedback,
        chatId,
        action.payload
      );
      yield put(chatActions.feedbackMessageSuccess(data));
    }
  } catch (error: any) {
    yield put(chatActions.feedbackMessageError(error));
  }
}

const sendResponseFeedBackWatcher = function* (): Generator {
  yield takeLatest(chatActions.feedbackMessage, sendResponseFeedBack);
};

const sendChatbotQuestionWatcher = function* () {
  yield takeLatest(chatActions.chatbotQuestion, sendChatbotQuestion);
};

function* getChatHistory(): Generator {
  try {
    const chatId = sessionStorage.getItem(SessionKeys.CHAT_ID) as string;
    const chatHistory = (yield call(
      ChatService.getChatHistory,
      chatId
    )) as IchatResponseHistory;
    yield put(chatActions.getChatHistorySuccess(chatHistory?.content));
  } catch (error) {
    yield put(chatActions.getChatHistoryError());
  }
}

const getChatHistoryWatcher = function* () {
  yield takeLatest(chatActions.getChatHistory, getChatHistory);
};

export {
  startChatWatcher,
  sendChatbotQuestionWatcher,
  sendResponseFeedBackWatcher,
  getChatHistoryWatcher,
};
