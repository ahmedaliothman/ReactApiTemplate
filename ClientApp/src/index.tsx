import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';

import { Provider } from "react-redux";
import { createStore, applyMiddleware } from "redux";
import promise from "redux-promise";
import createSagaMiddleware from "redux-saga";

import rootReducer from "./reducers/index";
import sagas from "./sagas/sagas";
import {StoreState} from "./Store/types/index"

const sagaMiddleware = createSagaMiddleware();
const store = createStore<StoreState.All,any,any,any>(rootReducer, applyMiddleware(sagaMiddleware));
sagaMiddleware.run(sagas);

ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>,
  document.getElementById('root')
);

