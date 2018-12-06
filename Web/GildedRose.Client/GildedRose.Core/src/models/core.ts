import { Dispatch } from "redux";

export interface Action<T> {
    value: T;
    type: string;
}

export type SelectProps = {
    value: string,
    label: string,
};

export type FormProps = {
    name?: string,
    // tslint:disable-next-line:no-any
    input: any,
    label?: string,
    subLabel?: string,
    placeHolder?: string,
    type?: string,
    maxLength?: number,
    disabled?: boolean,
    options?: SelectProps[],
    meta?: {
        touched: boolean,
        error: string,
        warning: string,
    },
};

export type Entity<T> = {
    hasInitialized?: boolean,
    inProgress?: boolean,
    data?: T,
    error?: {},
};

// tslint:disable-next-line:no-any
export type ThunkAction<S, T> = (fn: Dispatch<any>, getState: () => S) => Promise<T>;
