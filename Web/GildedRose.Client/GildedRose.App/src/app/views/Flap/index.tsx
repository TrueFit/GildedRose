import * as React from "react";
import * as style from "./style.css";
import { RouteComponentProps } from "react-router";
import { TodoActions } from "../../actions";
import { RootState } from "../../reducers";
import { TodoModel } from "../../models";

export namespace Flap {
  export interface Props extends RouteComponentProps<void> {
    todos: RootState.TodoState;
    actions: TodoActions;
    filter: TodoModel.Filter;
  }
}

export class Flap extends React.Component<Flap.Props> {
  public static defaultProps: Partial<Flap.Props> = {
    filter: TodoModel.Filter.SHOW_ALL,
  };

  // tslint:disable-next-line:no-any
  constructor(props: Flap.Props, context?: any) {
    super(props, context);
  }

  public render(): JSX.Element {
    return (
      <div className={style.normal}>
        FLAP
      </div>
    );
  }
}
