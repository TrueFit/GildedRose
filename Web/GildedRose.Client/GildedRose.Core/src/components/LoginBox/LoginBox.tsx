import * as React from "react";
import "./LoginBox.css";

export namespace LoginBox {
  export interface Props {
    handleLogin: (username: string, password: string) => void;
    cancelLogin: () => void;
    invalidCredentials: boolean;
  }
  export interface State {
    username: string;
    password: string;
  }
}

export class LoginBox extends React.Component<LoginBox.Props, LoginBox.State> {
  // tslint:disable-next-line:no-any
  constructor(props: LoginBox.Props, context?: any) {
    super(props, context);
    this.state = { username: "", password: "" };
  }

  public render(): JSX.Element {
    const buttonStyle = {
      width: "80px",
    } as React.CSSProperties;
    return (
      <form className="pure-form">
        <fieldset>
          <div>
            <div className="LoginRow">
              <span>
                <legend>Gilded Rose Login</legend>
              </span>
            </div>
            <div className="LoginRow">
              <span>
                <input type="text" placeholder="name" onChange={this.onUsernameChanged} />
              </span>
            </div>
            <div className="LoginRow">
              <span>
                <input type="password" placeholder="Password" onChange={this.onPasswordChanged} />
              </span>
            </div>
            <div className="LoginRow">
              <span style={{ color: "red" }}>
                {this.props.invalidCredentials && <small> Invalid username and password. </small>}
              </span>
            </div>
            <div className="LoginRow">
              <span>
                <label htmlFor="remember">
                  <input id="remember" type="checkbox" /> Remember me
              </label>
              </span>
            </div>
            <div className="LoginRow">
              <span>
                <button
                  type="button"
                  style={buttonStyle}
                  onClick={this.onUserSubmit}
                  className="pure-button pure-button-primary">
                  Sign in
              </button>
                <button
                  type="button"
                  style={{ ...buttonStyle, marginLeft: "12px" }}
                  onClick={this.props.cancelLogin}
                  className="pure-button pure-button-secondary">
                  Cancel
              </button>
              </span>
            </div>
          </div>
        </fieldset>
      </form>
    );
  }

  private onUserSubmit = (e: React.FormEvent<HTMLButtonElement>) => {
    this.props.handleLogin(this.state.username, this.state.password);
  }

  private onPasswordChanged = (e: React.FormEvent<HTMLInputElement>) => {
    this.setState({ password: e.currentTarget.value });
  }

  private onUsernameChanged = (e: React.FormEvent<HTMLInputElement>) => {
    this.setState({ username: e.currentTarget.value });
  }
}
