import { Dispatch } from "redux";

// tslint:disable-next-line:no-any
export type ThunkAction<S, T> = (fn: Dispatch<any>, getState: () => S) => Promise<T>;
