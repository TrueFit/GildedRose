import * as React from "react";
import { Route } from "react-router-dom";
import "styles/nav.css";
import * as Cookie from "js-cookie";

export namespace Header {
  export interface Props {
    title?: string;
    isAuthenticated: boolean;
  }
}

export class Header extends React.Component<Header.Props> {
  // tslint:disable-next-line:no-any
  constructor(props: Header.Props, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    const renderLink = (label: string, route: string) => (
      <Route render={({ history }) => (
        <a onClick={() => {
          history.push(route);
        }}>
          {label}
        </a>
      )} />);

    const logoutLink = () => (
      <Route render={({ history }) => (
        <a onClick={() => {
          Cookie.remove("Authorization");
          history.push("/");
        }}>
          {"Logout"}
        </a>
      )} />);

    return (
      <div className="nav">
        <div className="nav-header">
          <div className="nav-title">
            {renderLink(this.props.title || "", "/")}
          </div>
        </div>
        <div className="nav-btn">
          <label htmlFor="nav-check">
            <span></span>
            <span></span>
            <span></span>
          </label>
        </div>
        <input type="checkbox" id="nav-check" />
        <div className="nav-links">
          {renderLink("Inventory", "/Inventory")}
          {!this.props.isAuthenticated && renderLink("Login", "/Login")}
          {this.props.isAuthenticated && logoutLink()}
        </div>
      </div>
    );
  }
}
