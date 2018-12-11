import * as React from "react";
import * as ReactDOM from "react-dom";
import { Provider } from "react-redux";
import { createBrowserHistory } from "history";
import { configureStore } from "app/store";
import { Router } from "react-router";
import { App } from "app/App";
import "styles/app.css";

// tslint:disable-next-line:no-var-requires
require("font-awesome/css/font-awesome.css");
// tslint:disable-next-line:no-var-requires
require("purecss/build/pure.css");

const history = createBrowserHistory();
const store = configureStore();

ReactDOM.render(
  <Provider store={store}>
    <Router history={history}>
      <App />
    </Router>
  </Provider>,
  document.getElementById("root"),
);
