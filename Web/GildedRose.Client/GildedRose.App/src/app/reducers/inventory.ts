import { handleActions } from "redux-actions";
import { RootState } from "app/reducers/state";
import { InventoryActions } from "app/actions/InventoryActions";
import { InventoryModel } from "models";

const initialState: RootState.InventoryState = [];
export const inventoryReducer = handleActions<RootState.InventoryState, InventoryModel[]>(
  {
    [InventoryActions.Type.ADD_OVERWRITE_INVENTORY]: (state, action) => {
      if (action.payload) {
        return action.payload;
      }
      return state;
    },
  },
  initialState,
);
