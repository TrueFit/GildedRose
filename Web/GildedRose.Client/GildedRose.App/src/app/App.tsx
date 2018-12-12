import * as React from "react";
import { Route, Switch } from "react-router";
import { App as GildedRoseApp } from "views/App";
import { LoginView } from "views/Login";
import { hot } from "react-hot-loader";

export const App = hot(module)(() => (
  <>
    <Switch>
      <Route path="/login" component={LoginView} />
      <Route path="" component={GildedRoseApp} />
    </Switch>
  </>
));
