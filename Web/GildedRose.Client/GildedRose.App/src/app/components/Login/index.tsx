import * as React from "react";
import { LoginBox } from "core/components/LoginBox";
import { getToken } from "app/services";
import * as Cookie from "js-cookie";

export namespace Login {
  export interface Props {
    loginSuccess: () => void;
    cancelLogin: () => void;
  }
  export interface LocalState {
    invalidCredentials: boolean;
  }
}

export class Login extends React.Component<Login.Props, Login.LocalState> {
  // tslint:disable-next-line:no-any
  constructor(props: Login.Props, context?: any) {
    super(props, context);
    this.state = { invalidCredentials: false };
  }

  public render(): JSX.Element {
    return (
      <>
        <LoginBox cancelLogin={this.props.cancelLogin} handleLogin={this.handleLogin} invalidCredentials={this.state.invalidCredentials} />
      </>
    );
  }

  private async LoginUser(username: string, password: string): Promise<void> {
    const response = await getToken(username, password);
    if (response) {
      if (response.token) {
        this.setState({ invalidCredentials: false });
        Cookie.set("Authorization", response.token);
        this.props.loginSuccess();
      } else if (response.hint && response.hint === "401") {
        this.setState({ invalidCredentials: true });
      }
    }
  }

  private handleLogin = async (username: string, password: string) => {
    await this.LoginUser(username, password);
  }
}
