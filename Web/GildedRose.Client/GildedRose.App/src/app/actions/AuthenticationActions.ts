import { createAction } from "redux-actions";
import { AuthenticationModel } from "models";

export namespace AuthenticationActions {
  export enum Type {
    SET_AUTHENTICATION_STATUS = "SET_AUTHENTICATION_STATUS",
  }

  export const SetAuthenticationStatus = createAction<AuthenticationModel>(Type.SET_AUTHENTICATION_STATUS);
}

export type AuthenticationActions = Omit<typeof AuthenticationActions, "Type">;
