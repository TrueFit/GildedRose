import * as React from "react";
import { LoginBox } from "core/components/LoginBox";

export class Login extends React.Component<{}> {
  // tslint:disable-next-line:no-any
  constructor(props: {}, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    return (
      <>
        <LoginBox handleQuit={this.handleQuit} handleLogin={this.handleLogin} />
      </>
    );
  }

  private handleLogin = () => {
    alert("login");
  }

  private handleQuit = () => {
    alert("login");
  }
}
