// Main Entry Point of the React App
import React from 'react';
import {render} from 'react-dom';
import {BrowserRouter} from 'react-router-dom';
import routes from './routes';


    render(
        <BrowserRouter  children={ routes } basename={ "/" }/>, 
        document.getElementById("react-app")
    );