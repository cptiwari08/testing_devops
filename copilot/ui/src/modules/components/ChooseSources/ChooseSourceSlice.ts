import { createSlice } from "@reduxjs/toolkit";

interface IState {
  isLoading: boolean;
  selectedSources: string[];
}

const initialState: IState = {
  isLoading: false,
  selectedSources: [],
};

const chooseSourceSlice = createSlice({
  name: "chooseSource",
  initialState,
  reducers: {
    updateSelectedSources: (state, action) => {
      state.selectedSources = action.payload;
    },
  },
});

export const { updateSelectedSources } = chooseSourceSlice.actions;

export default chooseSourceSlice.reducer;
