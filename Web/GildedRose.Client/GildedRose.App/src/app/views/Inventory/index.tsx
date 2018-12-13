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
import { DatePicker } from "core/components/DatePicker/DatePicker";
import * as Moment from "moment";

export namespace InventoryView {
  export interface LocalState {
    pageNumber: number;
    pageSize: number;
    currentDate: Moment.Moment;
    filter: InventoryModel.Filter;
  }
  export interface FluxProps {
    InventoryState: RootState.InventoryState;
    AuthenticationState: RootState.AuthenticationState;
    actions: InventoryActions;
  }
}

@connect(
  (state: RootState, ownProps): Pick<InventoryView.FluxProps, "InventoryState" | "AuthenticationState"> => {
    return { InventoryState: state.inventoryData, AuthenticationState: state.authenticationData };
  },
  (dispatch: Dispatch): Pick<InventoryView.FluxProps, "actions"> => ({
    actions: bindActionCreators(omit(InventoryActions, "Type"), dispatch),
  }),
)

export class InventoryView extends React.Component<InventoryView.FluxProps, InventoryView.LocalState> {
  // tslint:disable-next-line:no-any
  constructor(props: InventoryView.FluxProps, context?: any) {
    super(props, context);
    this.state = {
      pageSize: 10,
      pageNumber: 1,
      currentDate: Moment(),
      filter: InventoryModel.Filter.SHOW_ALL,
    };
  }

  public async componentDidMount(): Promise<void> {
    const data = await getInventoryByDateViewed(new Date(Moment().format("MM/DD/YYYY").toString()));
    this.props.actions.AddOverwriteInventory(data);
  }

  public render(): JSX.Element {

    if (!this.props.InventoryState || this.props.InventoryState.length === 0) {
      return (<div>Loading Data...</div>);
    }

    const onShowAll = () => {
      this.setState({
        filter: InventoryModel.Filter.SHOW_ALL,
        pageNumber: 1,
        pageSize: 10,
      });
    };

    const onShowTrash = () => {
      this.setState({
        filter: InventoryModel.Filter.SHOW_TRASH,
        pageNumber: 1,
        pageSize: 10,
      });
    };

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

    const onDateChange = async (value: Moment.Moment | string) => {
      const data = await getInventoryByDateViewed(new Date(Moment(value).format("MM/DD/YYYY").toString()));
      this.props.actions.AddOverwriteInventory(data);
      this.setState({ currentDate: Moment(value) });
    };

    const pageSize = this.state.pageSize;
    const pageNumber = this.state.pageNumber;
    const totalItems = this.props.InventoryState.length;
    const totalPages = pageSize < totalItems ? totalItems / pageSize : 1;
    const startPosition = pageNumber === 1 ? 0 : ((pageSize * pageNumber) - (pageSize));
    const endPosition = pageSize + startPosition;

    let dto: GridData[];
    if (this.state.filter === InventoryModel.Filter.SHOW_ALL) {
      dto = this.props.InventoryState
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
    } else {
      dto = this.props.InventoryState
        .slice(startPosition, endPosition)
        .filter(x => x.currentQuality === 0)
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
    }

    const dateContainerStyle = {
      width: "220px",
      marginTop: "20px",
    } as React.CSSProperties;

    const tabContainerStyle = {
      width: "800px",
      marginTop: "20px",
    } as React.CSSProperties;

    const buttonStyle = {
      width: "140px",
      marginRight: "1px",
    } as React.CSSProperties;

    return (
      <>
        <Shell hideFooter={true}>
          <div>
            <div>
              <div>
                <div style={dateContainerStyle}>
                  <DatePicker onDateChange={onDateChange} value={this.state.currentDate} />
                </div>
              </div>
              <div style={tabContainerStyle}>
                <button
                  type="button"
                  style={buttonStyle}
                  onClick={onShowAll}
                  className={this.state.filter === InventoryModel.Filter.SHOW_ALL ?
                    "pure-button pure-button-primary" : "pure-button pure-button-secondary"}>
                  Show All
                </button>

                <button
                  type="button"
                  style={buttonStyle}
                  onClick={onShowTrash}
                  className={this.state.filter === InventoryModel.Filter.SHOW_TRASH ?
                    "pure-button pure-button-primary" : "pure-button pure-button-secondary"}>
                  Show Trash
                </button>
              </div>
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
