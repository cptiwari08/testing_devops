import { call, put, takeLatest } from 'redux-saga/effects';
import { getProjectConfigByKey, getProjectConfigByKeyFailure, getProjectConfigByKeySuccess, getUserApps, getUserAppsSuccess, getUserAppsFailure} from '../slices/projectConfig';
import { AssistantService } from '../apis/assistantConfigApis';
import { assistantConfigurationAction } from '../../pages/AssistantConfigurations/AssistantConfigurationsSlice';
import { getUserAppList } from '../apis/projectApis';
  
function* fetchProjectConfigByKey(action: any): Generator<any, void, any> {
  try {
    const data = yield call(
        AssistantService.getProjectConfiguration,
        action.payload.key
      );
    yield put(getProjectConfigByKeySuccess(data));
    yield put(assistantConfigurationAction.updateProjectConfigByKeySuccess(data));
  } catch (e) {
    yield put(getProjectConfigByKeyFailure(e));
  }
}
function* updateProjectConfig(action: any): Generator<any, void, any> {
  try {
   const data = yield call(
    AssistantService.updateProjectConfiguration,
    action.payload
   );
   yield put(getProjectConfigByKey(action.payload));
  } catch (e) {
    yield put(assistantConfigurationAction.updateProjectConfigByKeyFailure(e));
  }
}

function* fetchUserApps(action: any): Generator<any, void, any> {
  try {
    const data = yield call(getUserAppList);
    yield put(getUserAppsSuccess(data));
  } catch (e) {
    yield put(getUserAppsFailure(e));
  }
}

function* projectConfigSaga(): Generator {
  yield takeLatest(getProjectConfigByKey.type, fetchProjectConfigByKey);
}
function* updateProjectConfigByKeyWatcher(): Generator {
  yield takeLatest(assistantConfigurationAction.updateProjectConfigByKey.type,updateProjectConfig)
}

function* fetchUserAppsWatcher(): Generator {
  yield takeLatest(getUserApps.type, fetchUserApps);
}


export  {projectConfigSaga,updateProjectConfigByKeyWatcher, fetchUserAppsWatcher};
