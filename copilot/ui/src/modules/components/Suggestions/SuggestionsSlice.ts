import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ISuggestionsData } from "../../model";

interface IState {
  suggestions: ISuggestionsData[];
  isLoading: boolean;
  selectedSuggestion: ISuggestionsData | null;
  suggestionsVisibleToAssistant:ISuggestionsData[]
}

const initialState: IState = {
  suggestions: [],
  suggestionsVisibleToAssistant:[],
  isLoading: false,
  selectedSuggestion: null,
};

const suggestionSlice = createSlice({
  name: "suggestion",
  initialState,
  reducers: {
    getAllSuggestions: (state) => {
      state.isLoading = true;
    },
    getAllSuggestionsSuccess: (state, action) => {
      state.isLoading = false;
      state.suggestions = action.payload;
    },
    getAllSuggestionsFailure: (state) => {
      state.isLoading = false;
    },
    getAllSuggestionsVissibleToAssistant: (state) => {
      state.isLoading = true;
    },
    getAllSuggestionsVissibleToAssistantSuccess: (state, action) => {
      state.isLoading = false;
      state.suggestionsVisibleToAssistant = action.payload;
    },
    getAllSuggestionsVissibleToAssistantFailure: (state) => {
      state.isLoading = false;
    },
    
    sendSuggestionToBot: (state, { payload }: PayloadAction<ISuggestionsData>) => {
      state.selectedSuggestion = payload;
    },
    resetSelectedSuggestion: (state) => {
      state.selectedSuggestion = null;
    }
  },
});
export const {
  getAllSuggestions,
  getAllSuggestionsFailure,
  getAllSuggestionsSuccess,
  getAllSuggestionsVissibleToAssistant,
  getAllSuggestionsVissibleToAssistantSuccess,
  getAllSuggestionsVissibleToAssistantFailure,
  sendSuggestionToBot,
  resetSelectedSuggestion
} = suggestionSlice.actions;

export default suggestionSlice.reducer;
