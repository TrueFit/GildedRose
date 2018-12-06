import { combineReducers } from "redux";
import { RootState } from "./state";
import { todoReducer } from "./todos";

export { RootState };

// NOTE: current type definition of Reducer in "redux-actions" module
// doesn"t go well with redux@4
export const rootReducer = combineReducers<RootState>({
  // tslint:disable-next-line:no-any
  todos: todoReducer as any,
});
