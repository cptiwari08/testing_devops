import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IUserDetails } from '../../model';

interface UserState {
  userDetails: IUserDetails | null;
  loading: boolean;
  error: string | null;
}

const initialState: UserState = {
  userDetails: null,
  loading: false,
  error: null,
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    fetchUserDetails(state, action: PayloadAction<any>) {
      state.loading = true;
      state.error = null;
    },
    fetchUserDetailsSuccess(state, action: PayloadAction<any>) {
      const { data } = action.payload;
      state.userDetails = data && data.length > 0 ? data[0] : null;
      state.loading = false;
    },
    fetchUserDetailsFailure(state, action: PayloadAction<any>) {
      state.error = action.payload?.message || "";
      state.loading = false;
    },
  },
});

export const { fetchUserDetails, fetchUserDetailsSuccess, fetchUserDetailsFailure } = userSlice.actions;
export default userSlice.reducer;
