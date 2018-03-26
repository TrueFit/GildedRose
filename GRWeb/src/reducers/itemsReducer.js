import * as types from '../actions/actionTypes';
import initialState from './initialState';

export default function itemsReducer(state = initialState.itemsList, action){
    switch(action.type){
        /*
        case types.CREATE_COURSE:
            return [...state,
                Object.assign({}, action.course)
            ];
        */
        case types.LOAD_ALL_ITEMS_SUCCESS:
            return action.itemsList;
        case types.LOAD_TRASH_ITEMS_SUCCESS:
            return action.itemsList;
        default:
            return state;
    }
}