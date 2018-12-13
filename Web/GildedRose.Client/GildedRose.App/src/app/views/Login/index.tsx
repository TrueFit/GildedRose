import * as React from "react";
import { bindActionCreators, Dispatch } from "redux";
import { Header } from "app/components/Header";
import { Footer } from "app/components/Footer";
import { Login } from "app/components/Login";
import { RouteComponentProps } from "react-router";
import { RootState } from "app/reducers";
import { AuthenticationActions } from "app/actions";
import { connect } from "react-redux";
import { AuthenticationModel } from "models";
import { omit } from "core/utils";

export namespace LoginView {
  export interface RouteProps extends RouteComponentProps<void> {
  }
  export interface FluxProps {
    AuthenticationState: RootState.AuthenticationState;
    actions: AuthenticationActions;
  }
}

@connect(
  (state: RootState, ownProps): Pick<LoginView.FluxProps, "AuthenticationState"> => {
    return { AuthenticationState: state.authenticationData };
  },
  (dispatch: Dispatch): Pick<LoginView.FluxProps, "actions"> => ({
    actions: bindActionCreators(omit(AuthenticationActions, "Type"), dispatch),
  }),
)

export class LoginView extends React.Component<LoginView.RouteProps & LoginView.FluxProps> {
  constructor(props: LoginView.RouteProps & LoginView.FluxProps) {
    super(props);
  }

  public render(): JSX.Element {
    const loginStyle = {
      textAlign: "center",
    } as React.CSSProperties;

    return (
      <>
        <Header title={"GildedRose"} isAuthenticated={this.props.AuthenticationState.isAuthenticated} />
        <div>
          <div style={loginStyle}>
            <div>
              <Login loginSuccess={this.loginUser} cancelLogin={this.cancelLogin} />
            </div>
          </div>
        </div>
        <Footer language={"© Copyright 2018 GildedRose LLC"} />
      </>
    );
  }

  private loginUser = () => {
    const auth = { isAuthenticated: true } as AuthenticationModel;
    this.props.actions.SetAuthenticationStatus(auth);
    this.props.history.push("/");
  }

  private cancelLogin = () => {
    this.props.history.push("/");
  }
}
