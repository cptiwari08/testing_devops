import { combineReducers } from "redux";
import projectDocsSlice from "../../components/ViewDocsModal/ViewDocsModalSlice";
import suggestionsSlice from "../../components/Suggestions/SuggestionsSlice";
import ChooseSourceSlice from "../../components/ChooseSources/ChooseSourceSlice";
import userAuth from "../slices/userAuth";
import chatSlice from "../slices/chatSlice";
import AssistantConfigurationsSlice from "../../pages/AssistantConfigurations/AssistantConfigurationsSlice";
import FooterSlice from "../../components/Footer/FooterSlice";
import user from "../slices/userSlice";
import projectConfigurationSlice from "../slices/projectConfig";

const rootReducer = combineReducers({
  suggestions: suggestionsSlice,
  chooseSource: ChooseSourceSlice,
  projectDocs: projectDocsSlice,
  userAuth,
  chat: chatSlice,
  AssistantConfigurationsSlice: AssistantConfigurationsSlice,
  FooterSlice: FooterSlice,
  user,
  projectConfig: projectConfigurationSlice
});

export default rootReducer;
