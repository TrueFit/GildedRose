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

export const initialState = { inventory: [] }

export const appReducer = (state = initialState, action) => {
    switch (action.type) {
        case GET_ALL_ITEMS:
            return await getAllItems();
        case GET_TRASH:
            return await getTrash();
        case ADVANCE_DAY:
            let advance_day_resp = await postAdvanceDay();
            return {
                'inventory': advance_day_resp.results,
            };
        default:
            return state
    }
}
