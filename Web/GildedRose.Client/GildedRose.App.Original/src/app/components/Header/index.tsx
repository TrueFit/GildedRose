import * as React from "react";
import { TodoTextInput } from "components/TodoTextInput";
import { TodoActions } from "app/actions";
import { RouteComponentProps } from "react-router";

export namespace Header {
  export interface Props extends RouteComponentProps<void> {
    addTodo: typeof TodoActions.addTodo;
  }
}

export class Header extends React.Component<Header.Props> {
  // tslint:disable-next-line:no-any
  constructor(props: Header.Props, context?: any) {
    super(props, context);
    this.handleSave = this.handleSave.bind(this);
  }

  public render(): JSX.Element {
    /* tslint:disable */
    let h = this.props.history;
    console.log(h);
    debugger;
    /* tslint:enable */
    return (
      <header>

        <h1>Todos</h1>
        <TodoTextInput newTodo onSave={this.handleSave} placeholder="What needs to be done?" />

      </header>
    );
  }

  private handleSave(text: string): void {
    if (text.length) {
      this.props.addTodo({ text });
    }
  }
}
