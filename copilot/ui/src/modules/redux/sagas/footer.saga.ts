import { call, put, takeLatest } from "redux-saga/effects";
import { FooterService } from "../apis/footerApis";
import { footerSliceAction } from "../../components/Footer/FooterSlice";

function* submitAssistantFeedback(action: any): any {
  try {
    const response = yield call(FooterService.saveFeedback, action.payload);
    yield put(footerSliceAction.saveFeebackSuccess(response));
  } catch (error) {
    yield put(footerSliceAction.saveFeebackError(error));
  }
}

function* footerApiActionWatcher(): Generator {
  yield takeLatest(footerSliceAction.saveFeedback, submitAssistantFeedback);
}

export { footerApiActionWatcher };
