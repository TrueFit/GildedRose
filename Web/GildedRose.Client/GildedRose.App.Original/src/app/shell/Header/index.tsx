import * as React from "react";
import { Route } from "react-router-dom";

export namespace Header {
  export interface Props {
  }
}

export class Header extends React.Component<Header.Props> {
  // tslint:disable-next-line:no-any
  constructor(props: Header.Props, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    const linkButtonWithParam = () => (
      <Route render={({ history }) => (
        <a onClick={() => { history.push("inventory/id/5"); }}>
          Inventory with 5
        </a>
      )} />);

    return (
      <header>
        {linkButtonWithParam()}
      </header>
    );
  }
}
