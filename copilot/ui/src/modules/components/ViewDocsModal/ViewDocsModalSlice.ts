import { createSlice } from "@reduxjs/toolkit";
import { IProjectDocs } from "../../model";

interface IState {
  list: IProjectDocs[];
  isLoading: boolean;
}

const initialState: IState = {
  list: [],
  isLoading: false,
};

const projectDocsSlice = createSlice({
  name: "projectDocs",
  initialState,
  reducers: {
    getProjectDocs: (state) => {
      state.isLoading = true;
    },
    getProjectDocsSuccess: (state, action) => {
      state.isLoading = false;
      state.list = action.payload;
    },
    getProjectDocsFailure: (state) => {
      state.isLoading = false;
    },
    refreshProjectDocs:(state)=>{
      state.isLoading = true;
    },
    refreshProjectDocsSuccess:(state)=>{
      state.isLoading = false;
    },
    refreshProjectDocsFailure:(state)=>{
      state.isLoading = false;
    },
  },
});
export const { getProjectDocs, getProjectDocsSuccess, getProjectDocsFailure,refreshProjectDocs,refreshProjectDocsFailure } =
  projectDocsSlice.actions;
export default projectDocsSlice.reducer;
