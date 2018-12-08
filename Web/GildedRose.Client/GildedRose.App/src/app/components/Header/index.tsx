import * as React from "react";
// import { TodoTextInput } from "components/TodoTextInput";
import { RouteComponentProps } from "react-router";
import { Navbar, Nav, NavItem, Glyphicon } from "react-bootstrap";

export namespace Header {
  export interface Props extends RouteComponentProps<void> {

  }
}

export class Header extends React.Component<Header.Props> {
  // tslint:disable-next-line:no-any
  constructor(props: Header.Props, context?: any) {
    super(props, context);
    this.handleSave = this.handleSave.bind(this);
  }

  public render(): JSX.Element {
    return (
      <Navbar fixedTop>
        <Navbar.Header>
          <Navbar.Brand>
            <a href="/">
              Our Awesome Store
          </a>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav pullRight>
            <NavItem
              eventKey={1}
              href="#">
              Home
          </NavItem>
            <NavItem
              eventKey={2}
              href="#">
              Shop
          </NavItem>
            <NavItem
              eventKey={3}
              href="#">
              <Glyphicon glyph="shopping-cart" />
              {"Cart"}
            </NavItem>
          </Nav>
        </Navbar.Collapse>
      </Navbar>

      // <header>

      //   <h1>Todos</h1>
      //   <TodoTextInput newTodo onSave={this.handleSave} placeholder="What needs to be done?" />

      // </header>
    );
  }
}
