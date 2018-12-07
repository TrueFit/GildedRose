import { combineReducers, Reducer } from "redux";
import { RootState } from "./state";
import { todoReducer } from "./todos";
import { inventoryReducer } from "./inventory";
import { InventoryModel, TodoModel } from "../models";

export { RootState };

// NOTE: current type definition of Reducer in "redux-actions" module
// doesn"t go well with redux@4
export const rootReducer = combineReducers<RootState>({
  /* tslint:disable */
  todos: todoReducer as Reducer<TodoModel[], any>,
  inventory: inventoryReducer as Reducer<InventoryModel[], any>,
  /* tslint:enable */
});
