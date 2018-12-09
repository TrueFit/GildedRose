import * as React from "react";
import { RouteComponentProps } from "react-router";
import { Header } from "components/Header";

export namespace App {
  export interface Props extends RouteComponentProps<void> {
  }
}

export class App extends React.Component<App.Props> {
  // tslint:disable-next-line:no-any
  constructor(props: App.Props, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    return (
      <>
        <Header history={this.props.history} match={this.props.match} location={this.props.location} />
      </>
    );
  }
}
