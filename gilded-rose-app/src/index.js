import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

import { Router, Route, browserHistory, IndexRoute } from 'react-router';
import {getInventory} from "./actions/appActions";


import homePage from './pages/home';
import currentInventoryPage from './pages/currentInventory';
import expiredInventoryPage from './pages/expiredInventory';

getInventory().then(()=>{
    ReactDOM.render( <Router history={browserHistory}>
        <Route path='/' component={App}>
          <IndexRoute component={homePage} />
          <Route path='/currentInventory' component={currentInventoryPage} />
          <Route path='/expiredInventory' component={expiredInventoryPage} />
        </Route>
      </Router>, document.getElementById('root'));
}).catch(err=>{
    alert("Unable to get initial inventory, please check console for more detailed error message.")
})

registerServiceWorker();

