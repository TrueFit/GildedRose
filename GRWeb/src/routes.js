import React from 'react';
import {Route} from 'react-router-dom';
import AppPage from './components/common/App';
import HomePage from './components/home_page/HomePage';
import ItemsListPage from './components/items_list_page/ItemsListPage';
import TrashPage from './components/trash_page/TrashPage';
import ItemsPage from './components/item_page/ItemPage';
import EndOfDayPage from './components/end_of_day_page/EndOfDayPage';

import './styles/styles.css';


// Always load the AppPage component
export default(
   <AppPage>
       <Route exact path ="/" component = {HomePage} />
       <Route path ="/itemslist" component = {ItemsListPage} />
       <Route path = "/trash" component =  {TrashPage} />
       <Route path = "/endofday" component = {EndOfDayPage} />
       <Route exact path ="/item" component = {ItemsPage} />
       <Route path ="/item/:itemName" component = {ItemsPage} />
    </AppPage>
);



