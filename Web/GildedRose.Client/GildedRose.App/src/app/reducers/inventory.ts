import { handleActions } from "redux-actions";
import { RootState } from "./state";
import { InventoryActions } from "../actions/InventoryActions";
import { InventoryModel } from "../models";

const initialState: RootState.InventoryState = [];
export const inventoryReducer = handleActions<RootState.InventoryState, InventoryModel>(
  {
    [InventoryActions.Type.LIST_INVENTORY]: (state, action) => {
      return state;
    },
    [InventoryActions.Type.LIST_INVENTORY_TRASH]: (state, action) => {
      return state.filter(Inventory => Inventory.currentQuality > 0);
    },
  },
  initialState,
);
