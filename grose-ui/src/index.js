import React from 'react';
import { Provider } from 'react-redux';
import { createStore } from 'redux';
import ReactDOM from 'react-dom';

import ConnectedApp from './components/ConnectedApp';
import { appReducer, initialState } from './reducer';
import * as serviceWorker from './serviceWorker';

import './styles/index.css';

const store = createStore(appReducer, initialState);

ReactDOM.render(<Provider store={store}><ConnectedApp /></Provider>, document.getElementById('root'));
serviceWorker.unregister();
