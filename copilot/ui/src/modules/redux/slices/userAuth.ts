import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { FetchCETokenPayload, IExternalAppSettings } from "../../model";
import { jwtDecode } from "jwt-decode";
import { UserAuthDetails } from "../../model";
import { getExternalAppKey } from "../../../common/utility/utility";

export interface UserAuthState {
  token: string | null;
  loading: boolean;
  error: string | null;
  userAuthDetails: UserAuthDetails;
  externalAppSettings: IExternalAppSettings;
}

const initialState: UserAuthState = {
  userAuthDetails: {} as UserAuthDetails,
  token: null,
  loading: false,
  error: null,
  externalAppSettings: {} as IExternalAppSettings
};

export const userAuth = createSlice({
  name: "auth",
  initialState,
  reducers: {
    fetchCeToken: (state, action: PayloadAction<FetchCETokenPayload>) => {
      state.loading = true;
    },
    fetchCeTokenSuccess: (state, action) => {
      if (action.payload) {
        const decoded = jwtDecode<UserAuthDetails>(action.payload);
        state.userAuthDetails = decoded;
        state.token = action.payload;
      }
      state.loading = false;
      state.error = null;
    },
    fetchCeTokenFailed: (state, action) => {
      state.error = action.payload?.message || "";
      state.loading = false;
    },
    updateExternalAppSettings: (state, action) => {
      state.externalAppSettings = action.payload;
      // mapping external app key (appAffinity) based on app key
      state.externalAppSettings.externalAppKey = getExternalAppKey(action.payload?.appInfo?.key);
    }
  },
});

export const { fetchCeToken, fetchCeTokenSuccess, fetchCeTokenFailed, updateExternalAppSettings } = userAuth.actions;

export default userAuth.reducer;
