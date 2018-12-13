import * as React from "react";
import * as Moment from "moment";
import * as DateTime from "react-datetime";

type DatePickerProps = {
  value: Moment.Moment;
  timeSelectionEnabled?: boolean;
  // tslint:disable-next-line:no-any
  onBlur?: DateTime.EventOrValueHandler<React.FocusEvent<any>>;
  onDateChange?: (momentOrInputString: Moment.Moment | string) => void;
};

export const DatePicker = (props: DatePickerProps) => {
  // tslint:disable-next-line:no-require-imports
  require("react-datetime/css/react-datetime.css");
  // tslint:disable-next-line:no-any
  let datepickerComponent: any;
  const onIconClick = () => {
    datepickerComponent.openCalendar();
  };

  const iconContainerStyle = {
    fontSize: "1.1em",
    position: "absolute",
    zIndex: "100",
    top: "0px",
    right: "0px",
    height: "35px",
    width: "39px",
    borderBottomRightRadius: "4px",
    borderTopRightRadius: "4px",
    cursor: "pointer",
  } as {};

  const iconStyle: React.CSSProperties = {
    position: "absolute",
    top: "9px",
    right: "12px",
    color: "#000",
  };

  const timeFormat = props.timeSelectionEnabled ? "h:mm a" : false;

  return (
    <div
      style={{ position: "relative", display: "inline-block", width: "100%" }}>
      <div style={iconContainerStyle} onClick={onIconClick}>
        <i className="fa fa-calendar" style={iconStyle}></i>
      </div>
      <DateTime
        value={props.value}
        timeFormat={timeFormat}
        onChange={props.onDateChange}
        ref={(x => datepickerComponent = (x))}
        onBlur={props.onBlur}
        inputProps={{ placeholder: "MM/DD/YYYY" }}
        closeOnSelect={true}
      />
    </div>
  );
};
