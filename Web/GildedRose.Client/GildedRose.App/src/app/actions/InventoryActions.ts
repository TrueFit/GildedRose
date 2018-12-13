import { createAction } from "redux-actions";
import { InventoryModel } from "models";

export namespace InventoryActions {
  export enum Type {
    ADD_OVERWRITE_INVENTORY = "ADD_OVERWRITE_INVENTORY",
  }

  export const AddOverwriteInventory = createAction<InventoryModel[]>(Type.ADD_OVERWRITE_INVENTORY);

}

export type InventoryActions = Omit<typeof InventoryActions, "Type">;
