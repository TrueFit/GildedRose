import { TodoModel } from "../../app/models";

export interface RootState {
  todos: RootState.TodoState;
  // tslint:disable-next-line:no-any
  router?: any;
}

export namespace RootState {
  export type TodoState = TodoModel[];
}
