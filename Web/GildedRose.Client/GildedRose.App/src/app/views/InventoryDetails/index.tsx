import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router";
import { Header } from "app/components/Header";
import { Footer } from "app/components/Footer";
import { RootState } from "app/reducers";
import * as _ from "lodash";

export namespace InventoryDetailsView {
  export interface Params {
    id: string;
  }
  export interface RouteProps extends RouteComponentProps<Params> {
  }
  export interface FluxProps {
    InventoryState: RootState.InventoryState;
    AuthenticationState: RootState.AuthenticationState;
  }
}

@connect(
  (state: RootState, ownProps): Pick<InventoryDetailsView.FluxProps, "InventoryState" | "AuthenticationState"> => {
    return { InventoryState: state.inventoryData, AuthenticationState: state.authenticationData };
  },
)

export class InventoryDetailsView extends React.Component<InventoryDetailsView.FluxProps & InventoryDetailsView.RouteProps> {
  // tslint:disable-next-line:no-any
  constructor(props: InventoryDetailsView.FluxProps & InventoryDetailsView.RouteProps, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    const params = this.props.match.params;
    if (!params || !params.id) {
      return (<div> Item Could not be resolved!!! </div>);
    }
    const result = _.find(this.props.InventoryState, x => x.identifier === params.id);
    if (!result) {
      return (<div> Item Could not be resolved!!! </div>);
    }

    return (
      <>
        <Header title={"GildedRose"} isAuthenticated={false} />
        <div>
          <div>
            <div>
              Hello Inventory Details {result.name}
            </div>
          </div>
        </div>
        <Footer language={"© Copyright 2018 GildedRose LLC"} />
      </>
    );
  }
}
