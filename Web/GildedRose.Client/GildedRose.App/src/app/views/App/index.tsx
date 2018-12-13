import * as React from "react";
import { bindActionCreators, Dispatch } from "redux";
import { Shell } from "app/components/Shell/shell";
import { RootState } from "app/reducers";
import { AuthenticationActions } from "app/actions";
import { connect } from "react-redux";
import { omit } from "core/utils";
import * as Cookie from "js-cookie";
import { AuthenticationModel } from "models";

export namespace App {
  export interface FluxProps {
    AuthenticationState: RootState.AuthenticationState;
    actions: AuthenticationActions;
  }
}

@connect(
  (state: RootState, ownProps): Pick<App.FluxProps, "AuthenticationState"> => {
    return { AuthenticationState: state.authenticationData };
  },
  (dispatch: Dispatch): Pick<App.FluxProps, "actions"> => ({
    actions: bindActionCreators(omit(AuthenticationActions, "Type"), dispatch),
  }),
)

export class App extends React.Component<App.FluxProps> {
  // tslint:disable-next-line:member-ordering
  constructor(props: App.FluxProps) {
    super(props);
  }

  public render(): JSX.Element {
    if (!this.props.AuthenticationState.isAuthenticated) {
      const cookieValue = Cookie.get("Authorization");
      if (cookieValue && cookieValue.length > 0) {
        const auth = { isAuthenticated: true } as AuthenticationModel;
        this.props.actions.SetAuthenticationStatus(auth);
      }
    }

    const containerStyle = {
      paddingTop: "80px",
    } as React.CSSProperties;
    const articleStyle = {
      textAlign: "center",
    } as React.CSSProperties;

    return (
      <>
        <Shell>
          <div style={containerStyle}>
            {!this.props.AuthenticationState.isAuthenticated &&
              <article style={articleStyle}>
                Welcome to the Gilded Rose Inventory Application. Please login or create an account.
          </article>}
            {this.props.AuthenticationState.isAuthenticated &&
              <article style={articleStyle}>
                Welcome to the Gilded Rose Inventory Application. Please choose an option no the navigation bar.
          </article>}
          </div>
        </Shell>
      </>
    );
  }
}
