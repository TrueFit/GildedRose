import { InventoryModel } from "models";

export interface RootState {
  inventory: RootState.InventoryState;
  // tslint:disable-next-line:no-any
  router?: any;
}

export namespace RootState {
  export type InventoryState = InventoryModel[];
}
