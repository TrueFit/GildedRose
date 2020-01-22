import React from 'react';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware } from 'redux';
import ReactDOM from 'react-dom';
import thunk from 'redux-thunk';

import ConnectedApp from './components/ConnectedApp';
import { appReducer, initialState } from './reducer';

import './styles/index.css';

const store = createStore(appReducer, initialState, applyMiddleware(thunk));

ReactDOM.render(<Provider store={store}><ConnectedApp /></Provider>, document.getElementById('root'));
