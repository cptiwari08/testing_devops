import { call, put, takeLatest } from "redux-saga/effects";
import { getProjectDocs, clearProjectDocsRedis } from "../apis/projectApis";
import {
  getProjectDocsSuccess,
  getProjectDocsFailure,
  refreshProjectDocsFailure,
} from "../../components/ViewDocsModal/ViewDocsModalSlice";

function* getProjectDocuments(): Generator<any, void, any> {
  try {
    const data = yield call(getProjectDocs);
    yield put(getProjectDocsSuccess(data));
  } catch (error: any) {
    yield put(getProjectDocsFailure(error));
  }
}

function* clearViewDocsRedisCache(): Generator<any, void, any> {
  try {
    yield call(clearProjectDocsRedis);
    yield call(getProjectDocuments);
  } catch (error: any) {
    yield put(refreshProjectDocsFailure(error));
  }
}

const getProjecDocsActionWatcher = function* (): Generator {
  yield takeLatest("projectDocs/getProjectDocs", getProjectDocuments);
};
const refreshProjectDocsActionWatcher = function* (): Generator {
  yield takeLatest("projectDocs/refreshProjectDocs", clearViewDocsRedisCache);
};
export { getProjecDocsActionWatcher, refreshProjectDocsActionWatcher };
