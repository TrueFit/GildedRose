import { createAction } from "redux-actions";
import { InventoryModel } from "models";

export namespace InventoryActions {
  export enum Type {
    ADD_OVERWRITE_INVENTORY = "ADD_OVERWRITE_INVENTORY",
    SET_PAGE_NUMBER = "SET_PAGE_NUMBER",
  }

  export const AddOverwriteInventory = createAction<InventoryModel[]>(Type.ADD_OVERWRITE_INVENTORY);
  export const SetPageNumber = createAction<number>(Type.SET_PAGE_NUMBER);

}

export type InventoryActions = Omit<typeof InventoryActions, "Type">;
