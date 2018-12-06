import * as React from "react";
import { Route, Switch } from "react-router";
import { App as TodoApp } from "../app/containers/App";
import { Flap as FlapApp } from "../app/containers/Flap";
import { hot } from "react-hot-loader";

export const App = hot(module)(() => (
  <Switch>
    <Route path="/flap" component={FlapApp} />
    <Route path="/" component={TodoApp} />
  </Switch>
));
