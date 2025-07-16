import { createSlice } from "@reduxjs/toolkit";
import { ISourceConfig, IsourceConfiguration } from "../../model";

interface User{
  key: string;
  name:string;
}

interface IState {
  isLoading: boolean;
  sourceConfigs: ISourceConfig[];
  sourceConfigurationData:IsourceConfiguration,
  userApps: User[], 
}

const initialState: IState = {
  isLoading: true,
  sourceConfigs: [],
  sourceConfigurationData:{} as IsourceConfiguration,
  userApps: []
};

const projectConfigurationSlice = createSlice({
  name: "projectConfig",
  initialState,
  reducers: {
    getProjectConfigByKey: (state, action) => {
      state.isLoading = true;
    },
    getProjectConfigByKeySuccess: (state, action) => {
      state.isLoading = false;
      try {
        state.sourceConfigurationData = action.payload;
        state.sourceConfigs = action.payload?.value ? JSON.parse(action.payload?.value): null;
      } catch(e) {
        state.sourceConfigs = [];
      }
    },
    getProjectConfigByKeyFailure: (state, action) => {
      state.isLoading = false;
    },
    getUserApps: (state) => {
      state.isLoading = true;
    },
    getUserAppsSuccess: (state, action) => {
      state.isLoading = false;
      state.userApps = action.payload
    },
    getUserAppsFailure: (state, action) => {
      state.isLoading = false;
    }
  },
});

export const { getProjectConfigByKey, getProjectConfigByKeySuccess, getProjectConfigByKeyFailure, getUserApps, getUserAppsSuccess, getUserAppsFailure} = projectConfigurationSlice.actions;

export default projectConfigurationSlice.reducer;
