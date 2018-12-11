import * as React from "react";
// import { TodoTextInput } from "components/TodoTextInput";
import { RouteComponentProps } from "react-router";

export namespace Header {
  export interface Props extends RouteComponentProps<void> {

  }
}

export class Header extends React.Component<Header.Props> {
  // tslint:disable-next-line:no-any
  constructor(props: Header.Props, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    return (
      <div>Header</div>
    );
  }
}
