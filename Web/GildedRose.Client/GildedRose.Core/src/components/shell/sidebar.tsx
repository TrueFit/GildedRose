import * as React from "react";
import { User } from "core/models/user";

type Props = {
  user?: User,
};

export const Sidebar = (props: Props) => {
  const sidebarStyle = {
    fontWeight: "bold",
    paddingTop: "10px",
    paddingBottom: "10px",
    paddingLeft: "20px",
    paddingRight: "10px",
    color: "white",
    margin: "0px",
    borderBottom: "solid",
    borderColor: "#1A5690",
    textTransform: "uppercase",
    fontSize: "16px",
    minHeight: "90px",
  } as React.CSSProperties;

  return (
    <div style={{
      background: "#216CB4",
      paddingBottom: "0px",
      display: "table-cell",
    }} className="pure-u-1-5">
      <div className="sidebar-module" style={sidebarStyle}>
        FOO SIDEBAR
            </div>
    </div>
  );
};
