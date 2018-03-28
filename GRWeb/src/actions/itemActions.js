import * as types from "./actionTypes";
import * as ApiCalls from "../apiCalls";
import {beginApiCall}from './apiCallStatusActions';


export function loadItemSuccess(item){
    return {
        type: types.LOAD_ITEM_SUCCESS, item
    }
}

//Thunks
export function loadItem(itemName){
    console.log("ACtion Item Name " + itemName);
    if (itemName=="" || itemName == null){
        return function(dispatch) {
            dispatch(beginApiCall());
            dispatch(loadItemSuccess(null));
        }
    }
    let apiUrl = "http://localhost:5000/api/Inventory/item/" + itemName;
    return function(dispatch){
        dispatch(beginApiCall());
        return ApiCalls.CallInventoryApi(apiUrl)
        .then(item => { 
            dispatch(loadItemSuccess(item));
        })
        .catch(error => handleErrors(error));
    };
}

// Error Handling
export function handleErrors(error){
    console.log("ERROR: " + error);
}