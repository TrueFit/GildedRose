import * as types from '../actions/actionTypes';
import initialState from './initialState';

export default function itemReducer(state = initialState.item, action){
    switch(action.type){
        case types.LOAD_ITEM_SUCCESS:
            // if no item is passed in with the action, return the initial state item.
            // else return the item that is passed in.
            if (action.item === null){
                return Object.assign({}, state);
            }
            else{
                return Object.assign({}, action.item);
            }
        default:
            return state;
    }
}