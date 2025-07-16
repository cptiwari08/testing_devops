import { configureStore } from "@reduxjs/toolkit";
import createSagaMiddleware from "redux-saga";
import rootReducer from "./root.reducer";
import rootSaga from "../sagas/root.saga";
import { setupInterceptors } from "../../../common/utility/interceptor";

const sagaMiddleware = createSagaMiddleware();
export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(sagaMiddleware),
});
sagaMiddleware.run(rootSaga);
setupInterceptors(store); 
export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
