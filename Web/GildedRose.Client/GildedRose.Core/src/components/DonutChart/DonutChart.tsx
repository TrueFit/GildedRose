import * as React from "react";
import "./DonutChart.css";

type DonutPropTypes = {
  value: number | string,  // value the chart should show
  size: number,            // diameter of chart
  strokewidth: number      // width of chart line
  valuelabel?: string,     // label for the chart
  averagePercent?: number,
  precision?: number,
  isPercentage?: boolean,
  fontSize?: number,
};

export const DonutChart = (props: DonutPropTypes) => {
  const value = parseFloat(props.value as string);
  const halfsize = (props.size * 0.5);
  const radius = halfsize - (props.strokewidth * 0.5);
  const circumference = 2 * Math.PI * radius;
  const strokeval = ((value * circumference) / 100);
  const dashval = (strokeval + " " + circumference);
  const trackstyle = { strokeWidth: props.strokewidth };
  const indicatorstyle = { strokeWidth: props.strokewidth, strokeDasharray: dashval };
  const rotateval = "rotate(-90 " + halfsize + "," + halfsize + ")";

  let label;
  if (props.valuelabel) {
    label = <tspan x={halfsize} y={halfsize + 10}>{props.valuelabel}</tspan>;
  }

  const fontSize = props.fontSize || 20;
  const fontSizeStyle = { fontSize: fontSize.toString() + "px" } as React.CSSProperties;

  const displayValue = value === 100 ? "100" : value.toFixed(props.precision == undefined ? 2 : props.precision);

  let benchmark;
  if (props.averagePercent === 0) {
    const degreeLine = (360 * props.averagePercent / 100) - 90;
    const x2 = halfsize + (halfsize * Math.cos((degreeLine * Math.PI) / 180));
    const y2 = halfsize + (halfsize * Math.sin((degreeLine * Math.PI) / 180));
    const x3 = halfsize + ((halfsize - props.strokewidth) * Math.cos((degreeLine * Math.PI) / 180));
    const y3 = halfsize + ((halfsize - props.strokewidth) * Math.sin((degreeLine * Math.PI) / 180));
    benchmark = <line x1={x3} y1={y3} x2={x2} y2={y2} style={{ stroke: "rgb(10,6,6)", strokeWidth: "3" }} />;
  }
  return (
    <svg width={props.size} height={props.size} className="donutchart">
      <circle r={radius} cx={halfsize} cy={halfsize} transform={rotateval} style={trackstyle} className="donutchart-track" />
      <circle r={radius} cx={halfsize} cy={halfsize} transform={rotateval} style={indicatorstyle} className="donutchart-indicator" />
      {benchmark}
      <text className="donutchart-text" x={halfsize} y={halfsize + 5} style={{ textAnchor: "middle" }}>
        <tspan style={{ ...fontSizeStyle, fontWeight: "bold" }}>
          {displayValue}
        </tspan>
        {props.isPercentage !== false && <tspan style={fontSizeStyle}>%</tspan>}
        {label}
      </text>
    </svg>
  );
};
