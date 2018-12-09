import { combineReducers, Reducer } from "redux";
import { RootState } from "app/reducers/state";
import { inventoryReducer } from "app/reducers/inventory";
import { InventoryModel } from "models";
export { RootState };

// NOTE: current type definition of Reducer in "redux-actions" module
// doesn"t go well with redux@4
export const rootReducer = combineReducers<RootState>({
  /* tslint:disable */
  inventory: inventoryReducer as Reducer<InventoryModel[], any>,
  /* tslint:enable */
});
