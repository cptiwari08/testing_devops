import { all } from "redux-saga/effects";
import { getSuggestionsActionWatcher, getSuggestionsVisibleToAssisatantActionWatcher } from "./suggestions.saga";
import { sspApisActionWatcher } from "./ssp.saga";
import { getProjecDocsActionWatcher, refreshProjectDocsActionWatcher } from "./projectdocs.saga";
import {
  getChatHistoryWatcher,
  sendResponseFeedBackWatcher,
  sendChatbotQuestionWatcher,
  startChatWatcher,
} from "./chat.saga";
import {
  getProjectAssistantFlagWatcher,
  getProjectContextWatcher,
  saveAssistantToggleWatcher,
  saveProjectContextWatcher,
  addSuggestionWatcher,
  deleteSuggestionWatcher,
  saveSuggestionWatcher,
} from "./assistantConfiguartion.saga";
import { footerApiActionWatcher } from "./footer.saga";
import userSagaWatcher from "./user.saga";
import { fetchUserAppsWatcher, projectConfigSaga, updateProjectConfigByKeyWatcher } from "./projectConfig.saga";

export default function* rootSaga() {
  yield all([
    getSuggestionsActionWatcher(),
    sspApisActionWatcher(),
    startChatWatcher(),
    getProjecDocsActionWatcher(),
    sendChatbotQuestionWatcher(),
    sendResponseFeedBackWatcher(),
    getChatHistoryWatcher(),
    saveProjectContextWatcher(),
    addSuggestionWatcher(),
    deleteSuggestionWatcher(),
    saveSuggestionWatcher(),
    saveAssistantToggleWatcher(),
    getProjectContextWatcher(),
    getProjectAssistantFlagWatcher(),
    footerApiActionWatcher(),
    userSagaWatcher(),
    refreshProjectDocsActionWatcher(),
    projectConfigSaga(),
    fetchUserAppsWatcher(),
    updateProjectConfigByKeyWatcher(),
    getSuggestionsVisibleToAssisatantActionWatcher()
  ]);
}
