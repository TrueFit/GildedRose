import * as React from "react";
import { LoginBox } from "core/components/LoginBox";
import { getToken } from "app/services";
import * as Cookie from "js-cookie";

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

  private async LoginUser(username: string, password: string): Promise<void> {
    const response = await getToken(username, password);
    if (response && response.token) {
      Cookie.set("Authorization", response.token);
    }
  }

  private handleQuit = () => {
    alert("quit");
  }

  private handleLogin = async (username: string, password: string) => {
    await this.LoginUser(username, password);
  }
}
