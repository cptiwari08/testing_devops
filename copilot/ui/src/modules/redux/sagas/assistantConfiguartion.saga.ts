import { call, put, takeLatest } from "redux-saga/effects";
import { assistantConfigurationAction } from "../../pages/AssistantConfigurations/AssistantConfigurationsSlice";
import { AssistantService } from "../apis/assistantConfigApis";

function* saveProjectProjectContext(action: any): Generator<any, void, any> {
  try {
    const data = yield call(
      AssistantService.saveProjectConfiguartion,
      action.payload
    );
    yield put(assistantConfigurationAction.saveProjectContextSuccess(data));
  } catch (error: any) {
    yield put(assistantConfigurationAction.saveProjectContextFailure(error));
  }
}
function* saveAssistantToggle(action: any): Generator<any, void, any> {
  try {
    const data = yield call(
      AssistantService.saveProjectConfiguartion,
      action.payload
    );
    yield put(assistantConfigurationAction.saveAssistantToggleSuccess(data));
  } catch (error: any) {
    yield put(assistantConfigurationAction.saveAssistantToggleFailure(error));
  }
}
function* addSuggestion(action: any): Generator<any, void, any> {
  try {
    const data = yield call(
      AssistantService.addSuggesion,
      action.payload
    );
    yield put(assistantConfigurationAction.addSuggestionSuccess(data));
  } catch (error: any) {
    yield put(assistantConfigurationAction.addSuggestionFailure(error));
  }
}
function* deleteSuggestion(action: any): Generator<any, void, any> {
  try {
    const data = yield call(
      AssistantService.deleteSuggesion,
      action.payload
    );
    yield put(assistantConfigurationAction.deleteSuggestionSuccess(data));
  } catch (error: any) {
    yield put(assistantConfigurationAction.deleteSuggestionFailure(error));
  }
}
function* saveSuggestion(action: any): Generator<any, void, any> {
  try {
    const data = yield call(
      AssistantService.saveSuggesion,
      action.payload
    );
    yield put(assistantConfigurationAction.saveSuggestionSuccess(data));
  } catch (error: any) {
    yield put(assistantConfigurationAction.saveSuggestionFailure(error));
  }
}

function* getProjectContext(action: any): Generator<any, void, any> {
  try {
    const data = yield call(
      AssistantService.getProjectConfiguration,
      action.payload.Key
    );
    yield put(assistantConfigurationAction.getProjectContextSuccess(data));
  } catch (error: any) {
    yield put(assistantConfigurationAction.getProjectContextFailure(error));
  }
}
function* getProjectAssistantFlag(action: any): Generator<any, void, any> {
  try {
    const data = yield call(
      AssistantService.getProjectConfiguration,
      action.payload.Key
    );
    yield put(assistantConfigurationAction.getProjectAssistantFlagSuccess(data));
  } catch (error: any) {
    yield put(assistantConfigurationAction.getProjectAssistantFlagFailure(error));
  }
}

const saveProjectContextWatcher = function* (): Generator {
  yield takeLatest(
    assistantConfigurationAction.saveProjectContext,
    saveProjectProjectContext
  );
};
const saveAssistantToggleWatcher = function* (): Generator {
  yield takeLatest(
    assistantConfigurationAction.saveAssistantToggle,
    saveAssistantToggle
  );
};

const addSuggestionWatcher= function* (): Generator {
  yield takeLatest(
    assistantConfigurationAction.addSuggestion,
    addSuggestion
  );
};
const deleteSuggestionWatcher= function* (): Generator {
      yield takeLatest(
        assistantConfigurationAction.deleteSuggestion,
        deleteSuggestion
      );
    };

const saveSuggestionWatcher = function* (): Generator {
  yield takeLatest(
    assistantConfigurationAction.saveSuggestion,
    saveSuggestion
  );
};
const getProjectContextWatcher = function* (): Generator {
  yield takeLatest(
    assistantConfigurationAction.getProjectContext,
    getProjectContext
  );
};
const getProjectAssistantFlagWatcher = function* (): Generator {
  yield takeLatest(
    assistantConfigurationAction.getProjectAssistantFlag,
    getProjectAssistantFlag
  );
};

export {
  saveProjectContextWatcher,
  saveAssistantToggleWatcher,
  addSuggestionWatcher,
  deleteSuggestionWatcher,
  saveSuggestionWatcher,
  getProjectContextWatcher,
  getProjectAssistantFlagWatcher
};
