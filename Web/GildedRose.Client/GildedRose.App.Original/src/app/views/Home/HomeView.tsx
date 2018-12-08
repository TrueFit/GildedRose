import * as React from "react";
// import * as style from "./style.css";
import { connect } from "react-redux";
import { bindActionCreators, Dispatch } from "redux";
import { RouteComponentProps } from "react-router";
import { InventoryActions } from "app/actions";
import { RootState } from "app/reducers";
import { InventoryModel } from "models";
import { omit } from "core/utils";
import { Header } from "app/shell/Header";
// import { Footer } from "app/shell/Footer";
import { Link, Route } from "react-router-dom";
// import * as apiService from "app/services";
// import { InventoryGrid } from "../../../../../GildedRose.Core/src/components/InventoryGrid";

const FILTER_VALUES = (Object.keys(InventoryModel.Filter) as Array<keyof typeof InventoryModel.Filter>)
  .map(
    key => InventoryModel.Filter[key],
  );

// const FILTER_FUNCTIONS: Record<InventoryModel.Filter, (Inventory: InventoryModel) => boolean> = {
//   [InventoryModel.Filter.SHOW_ALL]: () => true,
//   [InventoryModel.Filter.SHOW_TRASH]: (Inventory: InventoryModel) => !Inventory,
// };

export namespace HomeView {
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
  (state: RootState, ownProps): Pick<HomeView.Props, "InventoryState" | "filter" | "blurb"> => {
    const hash = ownProps.location && ownProps.location.hash.replace("#", "");
    const filter = FILTER_VALUES.find(value => value === hash) || InventoryModel.Filter.SHOW_ALL;
    return { InventoryState: state.inventory, filter: filter, blurb: "cock Muncher" };
  },
  (dispatch: Dispatch): Pick<HomeView.Props, "actions"> => ({
    actions: bindActionCreators(omit(InventoryActions, "Type"), dispatch),
  }),
)

export class HomeView extends React.Component<HomeView.Props> {
  public static defaultProps: Partial<HomeView.Props> = {
    filter: InventoryModel.Filter.SHOW_ALL,
  };

  // tslint:disable-next-line:no-any
  constructor(props: HomeView.Props, context?: any) {
    super(props, context);
  }

  // public async componentDidMount(): Promise<void> { }

  public render(): JSX.Element {
    // /* tslint:disable */
    // const params = this.props.match.params;
    // if (params && params.id) {
    //   console.log("has parameter: " + params.id)

    // }
    // else {
    //   console.log("No parameters:");
    // }
    // var val = this.props.blurb;
    // console.log(val);
    // debugger;
    // /* tslint:enable */

    const linkButtonWithParam = () => (
      <Route render={({ history }) => (
        <a onClick={() => { history.push("inventory/id/5"); }}>
          Inventory with 5
        </a>
      )} />);

    return (
      <>
        <nav>
          <ul>
            <li><Link to="/">Home</Link></li>
            <li><Link to="/inventory">Inventory</Link></li>
            <li>{linkButtonWithParam()}</li>
          </ul>
        </nav>
        {/* <div className={style.normal}>
          <Header />
        </div> */}
        <div>
          <Header />
        </div>
      </>
    );
  }
}
