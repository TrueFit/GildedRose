// Main Entry Point of the React App

// Npm Modules
import React from 'react';
import {render} from 'react-dom';
import {BrowserRouter} from 'react-router-dom';
import {Provider} from 'react-redux'

// js files
import routes from './routes';
import configureStore from './store/configureStore';
import {loadAllItems} from './actions/itemsListActions';
import {loadTrashItems} from './actions/trashActions';
import {loadItem} from './actions/itemActions';
import initialState from './reducers/initialState';

const store = configureStore(initialState);

render(
    <Provider store ={store}>
        <BrowserRouter  children={ routes } basename={ "/" }/>
    </Provider>, 
    document.getElementById("react-app")
);