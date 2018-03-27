import * as types from "./actionTypes";
import * as ApiCalls from "../apiCalls";
import {beginApiCall}from './apiCallStatusActions';

export function loadTrashItemsSuccess(trashList){
    return {
        type: types.LOAD_TRASH_ITEMS_SUCCESS, trashList
    }
}


export function loadTrashItems(trashList){
    let apiUrl = "http://localhost:5000/api/Inventory/trash";
    return function (dispatch){
        dispatch(beginApiCall());
        return ApiCalls.CallInventoryApi(apiUrl)
        .then(trashList => {
            dispatch(loadTrashItemsSuccess(trashList));
        })
        .catch(error => handleErrors(error));
    };
}

// Error Handling
export function handleErrors(error){
    console.log("ERROR: " + error);
}



