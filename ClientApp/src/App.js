import React, { Component } from "react";
import { Redirect, Route } from "react-router";
import { Layout } from "./components/Layout";
import { List } from "./components/List";
import { Detail } from "./components/Detail";
import { Trashed } from "./components/Trashed";

import "./custom.css";
import { store } from "./store";

export default function App() {
  return (
    <Layout>
      <Route exact path="/goods">
        <List list={store.list} />
      </Route>
      <Route exact path="/goods/:name">
        <Detail item={store.item} />
      </Route>
      <Route path="/trashed">
        <Trashed trashed={store.trashed} />
      </Route>
      <Redirect exact from="/" to="/goods" />
    </Layout>
  );
}
