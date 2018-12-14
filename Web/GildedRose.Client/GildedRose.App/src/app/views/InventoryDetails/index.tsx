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
      marginTop: "24px",
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
    } as React.CSSProperties;
    const rowStyle = {
      paddingTop: "8px",
      fontSize: "x-large",
      maxWidth: "400px",
    } as React.CSSProperties;

    const renderRow = (label: string, value: string): JSX.Element => {
      return (
        <div style={rowStyle}>
          <div><strong>{label}:</strong>&nbsp;&nbsp;{value}</div>
        </div>
      );
    };
    return (
      <>
        <Shell>
          <div>
            <Route render={({ history }) => (
              <button
                type="button"
                style={{ marginTop: "4px" }}
                className={"pure-button pure-button-primary"}
                onClick={() => {
                  history.push("/inventory");
                }}>
                &#60;&#60; Back to Inventory
              </button>
            )} />
          </div>
          <div style={detailsContainer}>
            <div>
              {renderRow("Name", result.name)}
              {renderRow("Category", result.categoryName)}
              {renderRow("Current Quality", result.currentQuality.toString())}
              {renderRow("Initial Quality", result.initialQuality.toString())}
              {renderRow("Sell In (days)", result.sellIn.toString())}
              {result.isLegendary &&
                <div style={{ ...rowStyle, ...{ paddingTop: "48px", color: "red" } }}>
                  <i>This is a Legendary product</i>
                </div>}
            </div>
          </div>
        </Shell>
      </>
    );
  }
}
