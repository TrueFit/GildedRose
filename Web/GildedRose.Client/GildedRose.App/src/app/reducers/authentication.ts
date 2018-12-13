import { handleActions } from "redux-actions";
import { RootState } from "app/reducers/state";
import { AuthenticationActions } from "app/actions/AuthenticationActions";
import { AuthenticationModel } from "models";

const initialState: RootState.AuthenticationState = { isAuthenticated: false };
export const authenticationReducer = handleActions<RootState.AuthenticationState, AuthenticationModel>(
  {
    [AuthenticationActions.Type.SET_AUTHENTICATION_STATUS]: (state, action) => {
      if (action.payload) {
        return action.payload;
      }
      return state;
    },
  },
  initialState,
);
