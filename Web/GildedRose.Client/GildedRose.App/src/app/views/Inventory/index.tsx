import * as React from "react";
import { bindActionCreators, Dispatch } from "redux";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router";
import { InventoryActions } from "app/actions";
import * as apiService from "app/services";
import { Header } from "app/components/Header";
import { Footer } from "app/components/Footer";
import { InventoryGrid } from "../../components/InventoryGrid/InventoryGrid";
import { GridData, InventoryModel } from "models";
import { RootState } from "app/reducers";
import { omit } from "core/utils";

const FILTER_VALUES =
  (Object.keys(InventoryModel.Filter) as Array<keyof typeof InventoryModel.Filter>)
    .map(key => InventoryModel.Filter[key]);

export namespace InventoryView {
  export interface Params {
    id: string;
  }
  export interface Props extends RouteComponentProps<Params> {
    InventoryState: RootState.InventoryState;
    actions: InventoryActions;
    filter: InventoryModel.Filter;
  }
}

@connect(
  (state: RootState, ownProps): Pick<InventoryView.Props, "InventoryState" | "filter"> => {
    const hash = ownProps.location && ownProps.location.hash.replace("#", "");
    const filter = FILTER_VALUES.find(value => value === hash) || InventoryModel.Filter.SHOW_ALL;
    return { InventoryState: state.inventoryData, filter: filter };
  },
  (dispatch: Dispatch): Pick<InventoryView.Props, "actions"> => ({
    actions: bindActionCreators(omit(InventoryActions, "Type"), dispatch),
  }),
)

export class InventoryView extends React.Component<InventoryView.Props> {
  public static defaultProps: Partial<InventoryView.Props> = {
    filter: InventoryModel.Filter.SHOW_ALL,
  };

  // tslint:disable-next-line:no-any
  constructor(props: InventoryView.Props, context?: any) {
    super(props, context);
  }

  public async componentDidMount(): Promise<void> {
    const data = await apiService.getInventoryByDateViewed(new Date("12/01/2018"));
    this.props.actions.AddOverwriteInventory(data);
  }

  public render(): JSX.Element {

    if (!this.props.InventoryState || this.props.InventoryState.length === 0) {
      return (<div>Loading Data...</div>);
    }

    const onPageSizeChange = (newPageSize: number, newPage: number) => {
      alert("page size changed");
    };

    const onPageChange = (page: number) => {
      alert("page size changed");
    };

    const loginStyle = {
      textAlign: "center",
    } as React.CSSProperties;

    const pageSize = 10;
    const totalItems = this.props.InventoryState.length;
    const totalPages = pageSize > totalItems ? totalItems / pageSize : 1;

    const dto = this.props.InventoryState.map((x: InventoryModel) => {
      return {
        id: x.identifier,
        name: x.name,
        categoryId: x.categoryId,
        categoryName: x.categoryName,
        quality: {
          current: x.currentQuality,
          initial: x.initialQuality,
          max: x.maxQuality,
        },
        sellIn: x.sellIn,
        isLegendary: x.isLegendary,
      } as GridData;
    });

    return (
      <>
        <Header title={"Login Screen"} isAuthenticated={false} />
        <div>
          <div style={loginStyle}>
            <div>
              <InventoryGrid
                Data={dto}
                PageSize={pageSize}
                TotalPages={totalPages}
                PageNumber={1}
                OnPageSizeChange={onPageSizeChange}
                OnPageChange={onPageChange}

              />
            </div>

          </div>
        </div>
        <Footer language={"© Copyright 2018 GildedRose LLC"} />
      </>
    );
  }
}
