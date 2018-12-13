import { combineReducers, Reducer } from "redux";
import { RootState } from "app/reducers/state";
import { inventoryReducer } from "app/reducers/inventory";
import { authenticationReducer } from "app/reducers/authentication";
import { InventoryModel, AuthenticationModel } from "models";
export { RootState };

export const rootReducer = combineReducers<RootState>({
  /* tslint:disable */
  inventoryData: inventoryReducer as Reducer<InventoryModel[], any>,
  authenticationData: authenticationReducer as Reducer<AuthenticationModel, any>
  /* tslint:enable */
});
