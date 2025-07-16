import { call, put, takeLatest } from "redux-saga/effects";
import { getAllSuggestion, getAllSuggestionVisibleToAssistant } from "../apis/projectApis";
import {
  getAllSuggestionsFailure,
  getAllSuggestionsSuccess,
  getAllSuggestionsVissibleToAssistantFailure,
  getAllSuggestionsVissibleToAssistantSuccess,
} from "../../components/Suggestions/SuggestionsSlice";

function* getAllSuggestions(): Generator<any, void, any> {
  try {
    const data = yield call(getAllSuggestion);
    yield put(getAllSuggestionsSuccess(data));
  } catch (error: any) {
    yield put(getAllSuggestionsFailure(error));
  }
}
function* getAllSuggestionsVisibleToAssistant(): Generator<any, void, any> {
  try {
    const data = yield call(getAllSuggestionVisibleToAssistant);
    yield put(getAllSuggestionsVissibleToAssistantSuccess(data));
  } catch (error: any) {
    yield put(getAllSuggestionsVissibleToAssistantFailure(error));
  }
}

const getSuggestionsActionWatcher = function* (): Generator {
  yield takeLatest("suggestion/getAllSuggestions", getAllSuggestions);
}
const getSuggestionsVisibleToAssisatantActionWatcher = function* (): Generator {
  yield takeLatest("suggestion/getAllSuggestionsVissibleToAssistant", getAllSuggestionsVisibleToAssistant);
}

export {getSuggestionsActionWatcher,getSuggestionsVisibleToAssisatantActionWatcher}
