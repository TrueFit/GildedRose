import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router";
import { RootState } from "app/reducers";
import * as _ from "lodash";
import { Shell } from "app/components/Shell/shell";
import { Route } from "react-router-dom";

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

    const detailsContainer = {
      textAlign: "center",
      marginTop: "24px",
    } as React.CSSProperties;
    const rowStyle = {
      paddingTop: "8px",
    } as React.CSSProperties;

    return (
      <>
        <Shell>
          <div>
            <Route render={({ history }) => (
              <button
                type="button"
                className={"pure-button pure-button-primary"}
                onClick={() => {
                  history.push("/inventory");
                }}>
                Back
              </button>
            )} />
          </div>
          <div style={detailsContainer}>
            <div>
              <div style={rowStyle}>
                Name: {result.name}
              </div>
              <div style={rowStyle}>
                Category: {result.categoryName}
              </div>
              <div style={rowStyle}>
                Current Quality: {result.currentQuality}
              </div>
              <div style={rowStyle}>
                Initial Quality: {result.initialQuality}
              </div>
              <div style={rowStyle}>
                Sell In (days): {result.sellIn}
              </div>
              {result.isLegendary &&
                <div style={rowStyle}>
                  This is a Legendary product
              </div>}
            </div>
          </div>
        </Shell>
      </>
    );
  }
}
