import * as React from "react";
import ReactTable, { Column, FinalState, Instance } from "react-table";
import "./InventoryGrid.css";
import "react-table/react-table.css";
import { DonutChart } from "core/components/DonutChart/DonutChart";
import "core/components/DonutChart/DonutChart.css";
import "core/components/styles/flexbox.css";
import { GridData, Quality } from "models";
import { Route } from "react-router-dom";

type Props = {
  Data: GridData[];
  PageSize: number;
  TotalPages: number;
  PageNumber: number;
  OnPageSizeChange?: (newPageSize: number, newPage: number) => void;
  OnPageChange?: (page: number) => void;
  OnSortingChange?: () => void;
};

const columns: Array<Column<GridData>> = [
  {
    Header: "Name",
    columns: [
      {
        Header: "Name",
        accessor: "name",
      },
      {
        Header: "Category",
        accessor: "categoryName",
      },
      {
        Header: "Quality",
        accessor: "quality",
        sortable: false,
        Cell: (row: { value: Quality }) => {
          const percentageValue = (row.value.max > 0 ? row.value.current / row.value.max : 0) * 100;
          const renderQuality = (text: string, value: number): JSX.Element => {
            const qualityStyle = {
              fontWeight: "bold",
              width: "50px",
              height: "20px",
            } as React.CSSProperties;
            return (
              <div className={"quality-item"}><span>{text}</span><span style={qualityStyle}>{value.toString()}</span></div>
            );
          };

          return (
            <div className="panelflex">
              <div className="panelflex panelrow" style={{ border: "none" }}>
                <div className="panelrow panelitem" style={{ flex: "0 0 50%" }}>
                  <section className={"quality-panel"}>
                    {renderQuality("Quality: ", row.value.current)}
                    {renderQuality("Initial Quality: ", row.value.initial)}
                    {renderQuality("Max Quality: ", row.value.max)}
                  </section>
                </div>
                <div className="panelrow panelitem" style={{ flex: "0 0 50%" }}>
                  <DonutChart
                    value={percentageValue}
                    size={70}
                    strokewidth={10}
                    averagePercent={100}
                    fontSize={12}
                  />
                </div>
              </div>
            </div>
          );
        },
      },
      {
        Header: "",
        accessor: "id",
        maxWidth: 80,
        minWidth: 80,
        Cell: (row: { value: string }) => {
          const route = `/inventory/details/${row.value}`;
          const linkStyle = {
            cursor: "pointer",
            marginTop: "28px",
          } as React.CSSProperties;
          return (
            <div style={linkStyle}>
              <Route render={({ history }) => (
                <a onClick={() => { history.push(route); }}>
                  <i className="fa fa-info" />
                </a>
              )} />
            </div>
          );
        },
      },
    ],
  },
];

export class InventoryGrid extends React.Component<Props> {
  // public componentWillMount(): void {}

  public render(): JSX.Element {

    const data: GridData[] = this.props.Data;
    return (
      <>
        <ReactTable
          loading={false}
          data={data}
          columns={columns}
          className="inventoryGrid -striped -highlight"
          collapseOnDataChange={false}
          collapseOnSortingChange={false}
          pageSize={this.props.PageSize}
          pages={this.props.TotalPages}
          onPageSizeChange={this.props.OnPageSizeChange}
          onPageChange={this.props.OnPageChange}
          manual={true}
          resizable={false}
          page={this.props.PageNumber - 1}
          previousText="Previous"
          nextText="Next"
          loadingText="Loading..."
          noDataText="No rows found"
          pageText="Page"
          ofText="of"
          rowsText="rows"
        >
          {(
            state: FinalState<GridData>,
            makeTable: () => React.ReactChild,
            instance: Instance<GridData>,
          ) => {
            return (
              <div
                style={{
                  borderRadius: "5px",
                  overflow: "hidden",
                  padding: "5px",
                }}
              >
                {/* <pre>
                            <code>
                            state.allVisibleColumns ==={" "}
                            {JSON.stringify(state.allVisibleColumns, null, 4)}
                            </code>
                        </pre> */}
                {makeTable()}
              </div>
            );
          }}
        </ReactTable>
      </>
    );
  }
}
