import { TodoModel, InventoryModel } from "../models";

export interface RootState {
  todos: RootState.TodoState;
  inventory: RootState.InventoryState;
  // tslint:disable-next-line:no-any
  router?: any;
}

export namespace RootState {
  export type TodoState = TodoModel[];
  export type InventoryState = InventoryModel[];
}
