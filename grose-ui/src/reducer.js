import {
    GET_ALL_ITEMS,
    GET_TRASH,
    ADVANCE_DAY,
} from './actions';
import {
    getAllItems,
    getTrash,
    postAdvanceDay,
} from './api';

export const initialState = { inventory: [] };


export const appReducer = (state = initialState, action) => {
    let resp;
    switch (action.type) {
        case GET_ALL_ITEMS:
            resp = getAllItems();
            return resp;
        case GET_TRASH:
            resp = getTrash();
            return resp;
        case ADVANCE_DAY:
            resp = postAdvanceDay();
            return {
                'inventory': resp.results,
            };
        default:
            return state
    }
}
