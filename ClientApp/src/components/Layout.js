import React, { Component } from "react";
import { Container } from "reactstrap";
import { store } from "../store";
import { NavMenu } from "./NavMenu";

export function Layout(props) {
  return (
    <div>
      <NavMenu store={store} />
      <Container>{props.children}</Container>
    </div>
  );
}
