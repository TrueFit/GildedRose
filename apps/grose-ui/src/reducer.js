import {
    UI_RECEIVE_INVENTORY,
    UI_RECEIVE_SEARCH,
} from './actions';


export const initialState = { inventory: [] }

export const appReducer = (state = initialState, action) => {
    switch (action.type) {
        case UI_RECEIVE_INVENTORY:
            return {
                inventory: action.data.inventory,
            };
        case UI_RECEIVE_SEARCH:
            return {
                inventory: action.data.results,
            };
        default:
            return state
    }
}
