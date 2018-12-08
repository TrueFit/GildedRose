import * as React from "react";
import * as style from "./style.css";
import { connect } from "react-redux";
import { bindActionCreators, Dispatch } from "redux";
import { RouteComponentProps } from "react-router";
import { InventoryActions } from "app/actions";
import { RootState } from "app/reducers";
import { InventoryModel } from "models";
import { omit } from "core/utils";

const FILTER_VALUES = (Object.keys(InventoryModel.Filter) as Array<keyof typeof InventoryModel.Filter>)
  .map(
    key => InventoryModel.Filter[key],
  );

// const FILTER_FUNCTIONS: Record<InventoryModel.Filter, (Inventory: InventoryModel) => boolean> = {
//   [InventoryModel.Filter.SHOW_ALL]: () => true,
//   [InventoryModel.Filter.SHOW_TRASH]: (Inventory: InventoryModel) => !Inventory,
// };

export namespace ListInventory {
  export interface Props extends RouteComponentProps<void> {
    InventoryState: RootState.InventoryState;
    actions: InventoryActions;
    filter: InventoryModel.Filter;
  }
}

@connect(
  (state: RootState, ownProps): Pick<ListInventory.Props, "InventoryState" | "filter"> => {
    const hash = ownProps.location && ownProps.location.hash.replace("#", "");
    const filter = FILTER_VALUES.find(value => value === hash) || InventoryModel.Filter.SHOW_ALL;
    return { InventoryState: state.inventory, filter };
  },
  (dispatch: Dispatch): Pick<ListInventory.Props, "actions"> => ({
    actions: bindActionCreators(omit(InventoryActions, "Type"), dispatch),
  }),
)

export class ListInventoryComponent extends React.Component<ListInventory.Props> {
  public static defaultProps: Partial<ListInventory.Props> = {
    filter: InventoryModel.Filter.SHOW_ALL,
  };

  // tslint:disable-next-line:no-any
  constructor(props: ListInventory.Props, context?: any) {
    super(props, context);

    /* tslint:disable */
    debugger;
    /* tslint:enable */

    this.handleClearCompleted = this.handleClearCompleted.bind(this);
    this.handleFilterChange = this.handleFilterChange.bind(this);
  }

  public render(): JSX.Element {

    return (
      <div className={style.normal}>
        THIS IS THE INVENTORY ROUTE!!! WOO HOO
        {/* <InventoryGrid Inventory={filteredInventory} actions={actions} /> */}
      </div>
    );
  }

  private handleClearCompleted(): void {
    // const hasCompletedInventory = this.props.Inventory.some((Inventory: InventoryModel) => Inventory.completed || false);
    // if (hasCompletedInventory) {
    //   this.props.actions.clearCompleted();
    // }
  }

  private handleFilterChange(filter: InventoryModel.Filter): void {
    // this.props.history.push(`#${filter}`);
  }
}
