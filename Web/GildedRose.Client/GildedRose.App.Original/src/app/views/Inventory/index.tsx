import * as React from "react";
import * as style from "./style.css";
import { connect } from "react-redux";
import { bindActionCreators, Dispatch } from "redux";
import { RouteComponentProps } from "react-router";
import { InventoryActions } from "app/actions";
import { RootState } from "app/reducers";
import { InventoryModel } from "models";
import { omit } from "core/utils";
import * as apiService from "app/services";
// import { InventoryGrid } from "../../../../../GildedRose.Core/src/components/InventoryGrid";

const FILTER_VALUES = (Object.keys(InventoryModel.Filter) as Array<keyof typeof InventoryModel.Filter>)
  .map(
    key => InventoryModel.Filter[key],
  );

// const FILTER_FUNCTIONS: Record<InventoryModel.Filter, (Inventory: InventoryModel) => boolean> = {
//   [InventoryModel.Filter.SHOW_ALL]: () => true,
//   [InventoryModel.Filter.SHOW_TRASH]: (Inventory: InventoryModel) => !Inventory,
// };

export namespace Inventory {
  export interface Params {
    id: string;
  }
  export interface Props extends RouteComponentProps<Params> {
    InventoryState: RootState.InventoryState;
    actions: InventoryActions;
    filter: InventoryModel.Filter;
    blurb: string;
  }
}

@connect(
  (state: RootState, ownProps): Pick<Inventory.Props, "InventoryState" | "filter" | "blurb"> => {
    const hash = ownProps.location && ownProps.location.hash.replace("#", "");
    const filter = FILTER_VALUES.find(value => value === hash) || InventoryModel.Filter.SHOW_ALL;
    return { InventoryState: state.inventory, filter: filter, blurb: "cock Muncher" };
  },
  (dispatch: Dispatch): Pick<Inventory.Props, "actions"> => ({
    actions: bindActionCreators(omit(InventoryActions, "Type"), dispatch),
  }),
)

export class Inventory extends React.Component<Inventory.Props> {
  public static defaultProps: Partial<Inventory.Props> = {
    filter: InventoryModel.Filter.SHOW_ALL,
  };

  // tslint:disable-next-line:no-any
  constructor(props: Inventory.Props, context?: any) {
    super(props, context);
  }

  public async componentDidMount(): Promise<void> {
    const inv = await apiService.getInventoryByDateViewed(new Date("12/01/2018"));
    this.props.actions.AddOverwriteInventory(inv);
  }

  public render(): JSX.Element {
    /* tslint:disable */
    const params = this.props.match.params;
    if (params && params.id) {
      console.log("has parameter: " + params.id)
    }
    else {
      console.log("No parameters:");
    }
    var val = this.props.blurb;
    console.log(val);
    debugger;
    /* tslint:enable */
    return (
      <div className={style.normal}>
        THIS IS THE INVENTORY ROUTE!!! WOO HOO
        {/* <InventoryGrid Inventory={filteredInventory} actions={actions} /> */}
      </div>
    );
  }
}
