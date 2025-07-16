import { call, put, takeLatest } from 'redux-saga/effects';
import { getCeToken } from '../apis/sspApis';
import { PayloadAction } from '@reduxjs/toolkit';
import { fetchCeToken, fetchCeTokenFailed, fetchCeTokenSuccess } from '../slices/userAuth';
import { FetchCETokenPayload } from '../../model';

function* fetchTokenSaga(action: PayloadAction<FetchCETokenPayload>): any {
  try {
    const ceToken = yield call(getCeToken, action.payload);
    yield put(fetchCeTokenSuccess(ceToken));
  } catch (error) {
    yield put(fetchCeTokenFailed(error));
  }
}

function* sspApisActionWatcher(): Generator {
  yield takeLatest(fetchCeToken.type, fetchTokenSaga);
}

export { sspApisActionWatcher };