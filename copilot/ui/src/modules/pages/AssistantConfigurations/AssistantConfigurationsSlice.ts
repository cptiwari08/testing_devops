import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { IAssistantContextDetail, IAssistantEnabledDetails } from "../../model";

export interface IQuestionResponse {
  errorString: string;
  isValid: boolean;
}
export interface IState {
  isLoading: boolean;
  assistantContextDetail: IAssistantContextDetail;
  closeModal: boolean;
  saveContextSuccess: boolean;
  saveAssistantFlagSuccess: boolean;
  assistantEnableDetail: IAssistantEnabledDetails;
  isProjectContextDirty: boolean;
  isSuggestionAddSuccess: boolean;
  isSuggestionSaveSuccess: boolean;
  isSuggestionDeleteSuccess: boolean;
  isSourceNameSaveSuccess: boolean;
  sourceNameFailureMessage: string;
  suggestionSaveResponseList: IQuestionResponse[];
}
interface SetDirtyFlagPayload {
  key: keyof IState;
  value: any;
}
const initialState: IState = {
  isLoading: false,
  assistantContextDetail: {
    id: 0,
    isEnabled: "false",
    key: "",
    title: "",
    value: "",
  },
  closeModal: false,
  assistantEnableDetail: {
    id: 0,
    isEnabled: "false",
    key: "",
    title: "",
    value: "",
  },
  suggestionSaveResponseList: [],
  saveContextSuccess: false,
  saveAssistantFlagSuccess: false,
  //Flags for set dirty if any changes
  isProjectContextDirty: false,
  isSuggestionAddSuccess: false,
  isSuggestionSaveSuccess: false,
  isSuggestionDeleteSuccess: false,
  isSourceNameSaveSuccess: false,
  sourceNameFailureMessage: ""
};

const AssistantConfigurationsSlice = createSlice({
  name: "AssistantConfigurationsSlice",
  initialState,
  reducers: {
    saveAssistantToggle: (state, action) => {
      state.isLoading = true;
    },
    saveAssistantToggleSuccess: (state, action) => {
      state.isLoading = false;
    },
    saveAssistantToggleFailure: (state, action) => {
      state.isLoading = false;
    },
    saveProjectContext: (state, action) => {
      state.isLoading = true;
    },
    saveProjectContextSuccess: (state, action) => {
      state.isLoading = false;
      state.saveContextSuccess = true;
      state.isProjectContextDirty = false;
    },
    saveProjectContextFailure: (state, action) => {
      state.closeModal = true;
      state.isLoading = false;
    },
    addSuggestion: (state, action) => {
      state.isLoading = true;
      state.isSuggestionAddSuccess = false;
    },
    addSuggestionSuccess: (state, action) => {
      state.isLoading = false;
      state.isLoading = false;
      state.suggestionSaveResponseList = [...action.payload];
      const isAnyInvalid: boolean = action.payload.some(
        (item: IQuestionResponse) => !item.isValid
      );
      state.isSuggestionAddSuccess = !isAnyInvalid;
    },
    addSuggestionFailure: (state, action) => {
      state.suggestionSaveResponseList = [{errorString: action.payload.message,
        isValid: false}];
      state.isLoading = false;
    },
    deleteSuggestion: (state, action) => {
      state.isLoading = true;
      state.isSuggestionDeleteSuccess = false;
    },
    deleteSuggestionSuccess: (state, action) => {
      state.isLoading = false;
      state.isSuggestionDeleteSuccess = true;
    },
    deleteSuggestionFailure: (state, action) => {
      state.isLoading = false;
    },
    saveSuggestion: (state, action) => {
      state.isLoading = true;
      state.isSuggestionSaveSuccess = false;
    },
    saveSuggestionSuccess: (state, action) => {
      state.isLoading = false;
      state.suggestionSaveResponseList = [...action.payload];
      const isAnyInvalid: boolean = action.payload.some(
        (item: IQuestionResponse) => !item.isValid
      );

      state.isSuggestionSaveSuccess = !isAnyInvalid;
    },
    saveSuggestionFailure: (state, action) => {
      state.suggestionSaveResponseList = [{errorString: action.payload.message,
        isValid: false}];
      state.isLoading = false;
    },
    getProjectContext: (state, action) => {
      state.isLoading = true;
    },
    getProjectContextSuccess: (state, action) => {
      state.isLoading = false;
      state.assistantContextDetail = action.payload;
    },
    getProjectContextFailure: (state, action) => {
      state.isLoading = false;
    },
    getProjectAssistantFlag: (state, action) => {
      state.isLoading = true;
    },
    getProjectAssistantFlagSuccess: (state, action) => {
      state.isLoading = false;
      state.assistantEnableDetail = action.payload;
    },
    getProjectAssistantFlagFailure: (state) => {
      state.isLoading = false;
    },
    updateProjectConfigByKey: (state, action) => {
      state.isLoading = true;
      state.isSourceNameSaveSuccess = true;
      state.sourceNameFailureMessage = "";
    },
    updateProjectConfigByKeySuccess: (state, action) => {
      state.isLoading = false;
      state.isSourceNameSaveSuccess = false;
    },
    updateProjectConfigByKeyFailure: (state, action) => {
      state.isLoading = false;
      state.isSourceNameSaveSuccess = false;
      state.sourceNameFailureMessage = action.payload?.message;
    },
    setDirtyFlag: (state:any, action: PayloadAction<SetDirtyFlagPayload>) => {
      state.isLoading = false;
      state[action.payload.key] = action.payload.value;
    },
    clearQuestionSaveResponse: (state) => {
      state.suggestionSaveResponseList = [];
    },
  },
});

export const assistantConfigurationAction =
  AssistantConfigurationsSlice.actions;

export default AssistantConfigurationsSlice.reducer;
