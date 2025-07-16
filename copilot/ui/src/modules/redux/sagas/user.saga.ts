import { call, put, takeLatest } from 'redux-saga/effects';
import { getUserDetails } from '../apis/userApi';
import { fetchUserDetailsFailure, fetchUserDetails, fetchUserDetailsSuccess } from '../slices/userSlice';
import { PayloadAction } from '@reduxjs/toolkit';
function* fetchUserDetailsSaga(action: PayloadAction<any>): any {
  try {
    const base64EncodedEmail = action.payload;
    const userDetails = yield call(getUserDetails, base64EncodedEmail);
    yield put(fetchUserDetailsSuccess(userDetails));
  } catch (error) {
    yield put(fetchUserDetailsFailure(error));
  }
}

function* userSagaWatcher() {
  yield takeLatest(fetchUserDetails.type, fetchUserDetailsSaga);
}

export default userSagaWatcher;
