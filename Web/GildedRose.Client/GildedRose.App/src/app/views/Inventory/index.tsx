import * as React from "react";
import { bindActionCreators, Dispatch } from "redux";
import { connect } from "react-redux";
import { InventoryActions } from "app/actions";
import { getInventoryByDateViewed } from "app/services";
import { Shell } from "app/components/Shell/shell";
import { InventoryGrid } from "app/components/InventoryGrid/InventoryGrid";
import { GridData, InventoryModel } from "models";
import { RootState } from "app/reducers";
import { omit } from "core/utils";

const FILTER_VALUES =
  (Object.keys(InventoryModel.Filter) as Array<keyof typeof InventoryModel.Filter>)
    .map(key => InventoryModel.Filter[key]);

export namespace InventoryView {
  export interface LocalState {
    pageNumber: number;
    pageSize: number;
  }
  export interface FluxProps {
    InventoryState: RootState.InventoryState;
    AuthenticationState: RootState.AuthenticationState;
    actions: InventoryActions;
    filter: InventoryModel.Filter;
  }
}

@connect(
  (state: RootState, ownProps): Pick<InventoryView.FluxProps, "InventoryState" | "AuthenticationState" | "filter"> => {
    const hash = ownProps.location && ownProps.location.hash.replace("#", "");
    const filter = FILTER_VALUES.find(value => value === hash) || InventoryModel.Filter.SHOW_ALL;
    return { InventoryState: state.inventoryData, filter: filter, AuthenticationState: state.authenticationData };
  },
  (dispatch: Dispatch): Pick<InventoryView.FluxProps, "actions"> => ({
    actions: bindActionCreators(omit(InventoryActions, "Type"), dispatch),
  }),
)

export class InventoryView extends React.Component<InventoryView.FluxProps, InventoryView.LocalState> {
  public static defaultProps: Partial<InventoryView.FluxProps> = {
    filter: InventoryModel.Filter.SHOW_ALL,
  };

  // tslint:disable-next-line:no-any
  constructor(props: InventoryView.FluxProps, context?: any) {
    super(props, context);
    this.state = {
      pageSize: 10,
      pageNumber: 1,
    };
  }

  public async componentDidMount(): Promise<void> {
    const data = await getInventoryByDateViewed(new Date("12/01/2018"));
    this.props.actions.AddOverwriteInventory(data);
  }

  public render(): JSX.Element {

    if (!this.props.InventoryState || this.props.InventoryState.length === 0) {
      return (<div>Loading Data...</div>);
    }

    const onPageSizeChange = (newPageSize: number, newPageIndex: number) => {
      this.setState({
        pageSize: newPageSize,
        pageNumber: newPageIndex + 1,
      });
    };

    const onPageChange = (newPageIndex: number) => {
      this.setState({
        pageNumber: newPageIndex + 1,
      });
    };

    const pageSize = this.state.pageSize;
    const pageNumber = this.state.pageNumber;
    const totalItems = this.props.InventoryState.length;
    const totalPages = pageSize < totalItems ? totalItems / pageSize : 1;
    const startPosition = pageNumber === 1 ? 0 : ((pageSize * pageNumber) - (pageSize));
    const endPosition = pageSize + startPosition;
    const dto = this.props.InventoryState
      .slice(startPosition, endPosition)
      .map((x: InventoryModel) => {
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
        <Shell hideFooter={true}>
          <div>
            <div>
              <InventoryGrid
                Data={dto}
                PageSize={pageSize}
                TotalPages={totalPages}
                PageNumber={pageNumber}
                OnPageSizeChange={onPageSizeChange}
                OnPageChange={onPageChange}
              />
            </div>
          </div>
        </Shell>
      </>
    );
  }
}
