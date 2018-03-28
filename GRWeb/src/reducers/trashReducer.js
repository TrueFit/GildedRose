import * as types from '../actions/actionTypes';
import initialState from './initialState';

export default function itemsReducer(state = initialState.trashList, action){
    switch(action.type){
        case types.LOAD_TRASH_ITEMS_SUCCESS:
            return action.trashList;
        default:
            return state;
    }
}