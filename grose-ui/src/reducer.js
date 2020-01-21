import {
    UI_RECEIVE_INVENTORY,
    UI_RECEIVE_SEARCH,
} from './actions';


export const initialState = { inventory: [] }

export const appReducer = (state = initialState, action) => {
    switch (action.type) {
        case UI_RECEIVE_INVENTORY:
            return {
                ...state,
                inventory: action.inventory,
            };
        case UI_RECEIVE_SEARCH:
            return {
                ...state,
                inventory: action.results,
            };
        default:
            return state
    }
}
