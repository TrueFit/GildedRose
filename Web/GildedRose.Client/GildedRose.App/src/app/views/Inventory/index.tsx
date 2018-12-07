import * as React from "react";
import * as style from "./style.css";
import { connect } from "react-redux";
import { bindActionCreators, Dispatch } from "redux";
import { RouteComponentProps } from "react-router";
import { InventoryActions } from "app/actions";
import { RootState } from "app/reducers";
import { InventoryModel } from "models";
import { omit } from "app/utils";
// import * as proxy from "../../../../../GildedRose.Core/src/services/apiProxy";
import * as proxy from "core/services/apiProxy";
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
  export interface Props extends RouteComponentProps<void> {
    InventoryState: RootState.InventoryState;
    actions: InventoryActions;
    filter: InventoryModel.Filter;
  }
}

@connect(
  (state: RootState, ownProps): Pick<Inventory.Props, "InventoryState" | "filter"> => {
    const hash = ownProps.location && ownProps.location.hash.replace("#", "");
    const filter = FILTER_VALUES.find(value => value === hash) || InventoryModel.Filter.SHOW_ALL;
    return { InventoryState: state.inventory, filter };
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

    /* tslint:disable */
    debugger;
    /* tslint:enable */

    this.handleClearCompleted = this.handleClearCompleted.bind(this);
    this.handleFilterChange = this.handleFilterChange.bind(this);
  }

  public async componentDidMount(): Promise<void> {
    /* tslint:disable */
    proxy.get<InventoryModel[]>("/Item/getall/?viewByDate=12/26/2018").then((data: InventoryModel[]) => {
      debugger;
      data.forEach((item: InventoryModel) => {
        console.log(item);
      });
    });
    /* tslint:enable */
  }

  public render(): JSX.Element {
    /* tslint:disable */
    const aaa = this.props.location;
    const bbb = this.props.match;
    const ccc = this.props.match.url;
    const ddd = this.props.match.path;
    const eee = this.props.match.params;
    const fff = this.props.location.pathname;

    console.log(aaa);
    console.log(bbb);
    console.log(ccc);
    console.log(ddd);
    console.log(eee);
    console.log(fff);

    // proxy.get<InventoryModel[]>("/Item/getall/?viewByDate=12/26/2018").then((data: InventoryModel[]) => {
    //   debugger;
    //   data.forEach((item: InventoryModel) => {
    //     console.log(item);
    //   });
    // });
    /* tslint:disable */

    //debugger;
    /* tslint:enable */
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
