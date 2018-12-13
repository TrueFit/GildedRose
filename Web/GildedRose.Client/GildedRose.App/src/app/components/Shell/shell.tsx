import * as React from "react";
import { Header } from "app/components/Header";
import { Footer } from "app/components/Footer";
import { RootState } from "app/reducers";
import { connect } from "react-redux";
import { bindActionCreators, Dispatch } from "redux";
import { AuthenticationActions } from "app/actions";
import { omit } from "core/utils";
import { AuthenticationModel } from "models";

export namespace Shell {
  export interface Props {
    hideFooter?: boolean;
  }
  export interface FluxProps {
    AuthenticationState?: RootState.AuthenticationState;
    actions?: AuthenticationActions;
  }
}

@connect(
  (state: RootState, ownProps): Pick<Shell.FluxProps, "AuthenticationState"> => {
    return { AuthenticationState: state.authenticationData };
  },
  (dispatch: Dispatch): Pick<Shell.FluxProps, "actions"> => ({
    actions: bindActionCreators(omit(AuthenticationActions, "Type"), dispatch),
  }),
)

export class Shell extends React.Component<Shell.FluxProps & Shell.Props> {
  // tslint:disable-next-line:member-ordering
  constructor(props: Shell.FluxProps & Shell.Props) {
    super(props);
  }

  public render(): JSX.Element {
    const isAuthenticated = this.props.AuthenticationState && this.props.AuthenticationState.isAuthenticated || false;
    return (
      <>
        <Header title={"GildedRose"} isAuthenticated={isAuthenticated} logout={this.logout} />
        {this.props.children}
        {!this.props.hideFooter && <Footer language={"© Copyright 2018 GildedRose LLC"} />}
      </>
    );
  }

  public logout = () => {
    if (this.props.actions) {
      const auth = { isAuthenticated: false } as AuthenticationModel;
      this.props.actions.SetAuthenticationStatus(auth);
    }
  }
}
