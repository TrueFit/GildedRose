import { combineReducers, Reducer } from "redux";
import { RootState } from "app/reducers/state";
import { inventoryReducer } from "app/reducers/inventory";
import { InventoryModel } from "models";
export { RootState };

export const rootReducer = combineReducers<RootState>({
  /* tslint:disable */
  inventoryData: inventoryReducer as Reducer<InventoryModel[], any>,
  /* tslint:enable */
});
