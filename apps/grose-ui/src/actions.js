import wretch from 'wretch';

export const UI_READY = "READY";
export const UI_RECEIVE_INVENTORY = "RECEIVE_INVENTORY";
export const UI_RECEIVE_SEARCH = "RECEIVE_RESULTS";
export const UI_LOAD = "LOADING";
export const UI_ERROR = "ERROR";

export const GET_ALL_ITEMS = "GET_ALL_ITEMS";
export const GET_TRASH = "GET_TRASH";
export const ADVANCE_DAY = "ADVANCE_DAY";

const API_URL = process.env.REACT_APP_API_URL;

export const uiReady = (data) => {
    return (dispatch) => dispatch({
        type: UI_READY,
        data,
    });
}

export const uiInventorySuccess = (data) => {
    console.log(data);
    return (dispatch) => dispatch({
        type: UI_RECEIVE_INVENTORY,
        data,
    });
}

export const uiSearchSuccess = (data) => {
    return (dispatch) => dispatch({
        type: UI_RECEIVE_SEARCH,
        data,
    });
}

export const uiLoading = () => {
    return (dispatch) => dispatch({
        type: UI_LOAD,
    });
}

export const uiError = (err) => {
    return (dispatch) => dispatch({
        type: UI_ERROR,
    });
}

export const getAllItems = () => {
    return async (dispatch) => {
        dispatch(uiLoading());
        const url = `${API_URL}/items`;
        return await wretch(url).get()
            .json(json => dispatch(uiInventorySuccess(json)))
            .catch(err => dispatch(uiError(err)));
    }
}

export const getTrash = () => {
    return async (dispatch) => {
        dispatch(uiLoading());
        const url = `${API_URL}/items?trash`;
        return await wretch(url).get()
            .json(json => dispatch(uiInventorySuccess(json)))
            .catch(err => dispatch(uiError(err)));
    }
}

export const advanceDay = () => {
    return async (dispatch) => {
        dispatch(uiLoading());
        const url = `${API_URL}/nextday`;
        return await wretch(url).post(null)
            .json(json => dispatch(uiSearchSuccess(json)))
            .catch(err => dispatch(uiError(err)));
    };
}
