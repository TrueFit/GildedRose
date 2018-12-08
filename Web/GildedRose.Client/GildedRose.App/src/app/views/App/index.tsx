import * as React from "react";
import * as style from "./style.css";
import { connect } from "react-redux";
import { bindActionCreators, Dispatch } from "redux";
import { RouteComponentProps } from "react-router";
import { TodoActions } from "app/actions";
import { RootState } from "app/reducers";
import { TodoModel } from "models";
import { omit } from "core/utils";
import { TodoList } from "components/TodoList";
import { Header } from "components/Header";
import { Footer } from "components/Footer";
import { Link, Route } from "react-router-dom";

const FILTER_VALUES = (Object.keys(TodoModel.Filter) as Array<keyof typeof TodoModel.Filter>)
  .map(
    key => TodoModel.Filter[key],
  );

const FILTER_FUNCTIONS: Record<TodoModel.Filter, (todo: TodoModel) => boolean> = {
  [TodoModel.Filter.SHOW_ALL]: () => true,
  [TodoModel.Filter.SHOW_ACTIVE]: (todo: TodoModel) => !todo.completed,
  [TodoModel.Filter.SHOW_COMPLETED]: (todo: TodoModel) => todo.completed,
};

export namespace App {
  export interface Props extends RouteComponentProps<void> {
    todos: RootState.TodoState;
    actions: TodoActions;
    filter: TodoModel.Filter;
  }
}

@connect(
  (state: RootState, ownProps): Pick<App.Props, "todos" | "filter"> => {
    const hash = ownProps.location && ownProps.location.hash.replace("#", "");
    const filter = FILTER_VALUES.find(value => value === hash) || TodoModel.Filter.SHOW_ALL;
    return { todos: state.todos, filter };
  },
  (dispatch: Dispatch): Pick<App.Props, "actions"> => ({
    actions: bindActionCreators(omit(TodoActions, "Type"), dispatch),
  }),
)
export class App extends React.Component<App.Props> {
  public static defaultProps: Partial<App.Props> = {
    filter: TodoModel.Filter.SHOW_ALL,
  };

  // tslint:disable-next-line:no-any
  constructor(props: App.Props, context?: any) {
    super(props, context);
    this.handleClearCompleted = this.handleClearCompleted.bind(this);
    this.handleFilterChange = this.handleFilterChange.bind(this);
  }

  public render(): JSX.Element {
    const { todos, actions, filter } = this.props;
    const activeCount = todos.length - todos.filter((todo: TodoModel) => {
      return todo.completed;
    }).length;
    const filteredTodos = filter ? todos.filter(FILTER_FUNCTIONS[filter]) : todos;
    const completedCount = todos
      .reduce((count: number, todo: TodoModel) => {
        return (todo.completed ? count + 1 : count);
      }, 0);

    const linkButtonWithParam = () => (
      <Route render={({ history }) => (
        <a onClick={() => { history.push("inventory/id/5"); }}>
          Inventory with 5
        </a>
      )} />);

    return (
      <>
        <nav>
          <ul>
            <li><Link to="/">Home</Link></li>
            <li><Link to="/inventory">Inventory</Link></li>
            <li>{linkButtonWithParam()}</li>
          </ul>
        </nav>
        <div className={style.normal}>
          <Header addTodo={actions.addTodo} history={this.props.history} location={this.props.location} match={this.props.match} />
          <TodoList todos={filteredTodos} actions={actions} />
          <Footer
            filter={filter}
            activeCount={activeCount}
            completedCount={completedCount}
            onClickClearCompleted={this.handleClearCompleted}
            onClickFilter={this.handleFilterChange}
          />
        </div>
      </>
    );
  }

  private handleClearCompleted(): void {
    const hasCompletedTodo = this.props.todos.some((todo: TodoModel) => todo.completed || false);
    if (hasCompletedTodo) {
      this.props.actions.clearCompleted();
    }
  }

  private handleFilterChange(filter: TodoModel.Filter): void {
    this.props.history.push(`#${filter}`);
  }
}
