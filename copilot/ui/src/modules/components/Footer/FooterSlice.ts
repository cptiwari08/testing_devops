import { createSlice } from "@reduxjs/toolkit";

interface IState {
  isLoading: boolean;
  feedback: IFeedback;
}

interface IFeedback {
  Rating: number;
  FeedbackText: string;
}

const initialState: IState = {
  isLoading: false,
  feedback: {} as IFeedback,
};

const footerSlice = createSlice({
  name: "footerSlice",
  initialState,
  reducers: {
    saveFeedback: (state, action) => {
      state.isLoading = true;
      state.feedback = action.payload;
    },
    saveFeebackSuccess: (state, action) => {
      state.isLoading = false;
    },
    saveFeebackError: (state, action) => {
      state.isLoading = false;
    },
  },
});

export const footerSliceAction = footerSlice.actions;

export default footerSlice.reducer;
