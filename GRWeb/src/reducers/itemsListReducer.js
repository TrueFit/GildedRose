import * as types from '../actions/actionTypes';
import initialState from './initialState';

export default function itemsListReducer(state = initialState.itemsList, action){
    switch(action.type){
        case types.LOAD_ALL_ITEMS_SUCCESS:
           return action.itemsList;
        default:
            return state;
    }
}