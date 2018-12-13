import { InventoryModel, AuthenticationModel } from "models";

export interface RootState {
  inventoryData: RootState.InventoryState;
  authenticationData: RootState.AuthenticationState;
  // tslint:disable-next-line:no-any
  router?: any;
}

export namespace RootState {
  export type InventoryState = InventoryModel[];
  export type AuthenticationState = AuthenticationModel;
}
