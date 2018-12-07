import { createAction } from "redux-actions";
import { InventoryModel } from "../models";

export namespace InventoryActions {
  export enum Type {
    LIST_INVENTORY = "LIST_INVENTORY",
    LIST_INVENTORY_TRASH = "LIST_INVENTORY_TRASH",
  }
  export const completeAll = createAction<InventoryModel>(Type.LIST_INVENTORY);
  export const clearCompleted = createAction<InventoryModel>(Type.LIST_INVENTORY_TRASH);
}

export type InventoryActions = Omit<typeof InventoryActions, "Type">;
