import { combineReducers, Reducer } from "redux";
import { RootState } from "app/reducers/state";
import { todoReducer } from "app/reducers/todos";
import { inventoryReducer } from "app/reducers/inventory";
import { TodoModel, InventoryModel } from "models";

export { RootState };

// NOTE: current type definition of Reducer in "redux-actions" module
// doesn"t go well with redux@4
export const rootReducer = combineReducers<RootState>({
  /* tslint:disable */
  todos: todoReducer as Reducer<TodoModel[], any>,
  inventory: inventoryReducer as Reducer<InventoryModel[], any>,
  /* tslint:enable */
});
