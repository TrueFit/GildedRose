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

// Create a Redux store holding the state of your app.
// Its API is { subscribe, dispatch, getState }.
// let store = createStore(appReducer);

// The only way to mutate the internal state is to dispatch an action.
// The actions can be serialized, logged or stored and later replayed.
// store.dispatch({ type: 'INCREMENT' })
// // 1
// store.dispatch({ type: 'INCREMENT' })
// // 2
// store.dispatch({ type: 'DECREMENT' })
// // 1
