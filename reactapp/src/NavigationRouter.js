import React from 'react';
import { Switch, Route } from 'react-router-dom';

import NavigationBar from './NavigationBar';
import Home from './Home';
import InventoryList from './Inventory/InventoryList';
import InventoryItem from './Inventory/InventoryItem';
import TrashList from './Inventory/TrashList';

export default (props) => {
    return (
        <div>
            <h3>Gilded Rose Inventory Management Solution</h3>
            <NavigationBar />
            <Switch>
                <Route path="/Inventory" render={() =>
                    <InventoryList />
                } />
                <Route exact path={`/Item/:ItemName`} render={(routeprops) =>
                    <InventoryItem itemName={routeprops.match.params.ItemName}/>
                } />
                <Route path="/Trash" render={() =>
                    <TrashList />
                } />
                <Route path="/" render={() =>
                    <Home />
                } />
            </Switch>
        </div>
    )
}