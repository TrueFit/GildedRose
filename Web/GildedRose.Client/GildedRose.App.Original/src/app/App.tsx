import * as React from "react";
import { Route, Switch } from "react-router";
import { App as TodoApp } from "views/App";
import { Flap as FlapApp } from "views/Flap";
import { Inventory as InventoryComponent } from "views/Inventory";

import { hot } from "react-hot-loader";

export const App = hot(module)(() => (
  <>
    <Switch>
      <Route path="/inventory" component={InventoryComponent} />
      <Route path="/inventory/id/:id" component={InventoryComponent} />
      <Route path="flap" component={FlapApp} />
      <Route path="" component={TodoApp} />
    </Switch>
  </>
));
