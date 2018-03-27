import * as types from "./actionTypes";
import * as ApiCalls from "../apiCalls";
import {beginApiCall}from './apiCallStatusActions';

export function loadAllItemsSuccess(itemsList){
    return {
        type: types.LOAD_ALL_ITEMS_SUCCESS, itemsList: itemsList
    }
}

//Thunks
export function loadAllItems(itemsList){
    let apiUrl = "http://localhost:5000/api/Inventory";
    return function(dispatch){
        dispatch(beginApiCall());
        return ApiCalls.CallInventoryApi(apiUrl)
        .then(itemsList => { 
            dispatch(loadAllItemsSuccess(itemsList));
        })
        .catch(error => handleErrors(error));
    };
}

// Error Handling
export function handleErrors(error){
    console.log("ERROR: " + error);
}

