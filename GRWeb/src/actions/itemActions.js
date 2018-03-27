import * as types from "./actionTypes";
import * as ApiCalls from "../apiCalls";


export function loadItemSuccess(item){
    return {
        type: types.LOAD_ITEM_SUCCESS, item
    }
}

//Thunks
export function loadItem(itemName){
    if (itemName==""){
        return function(dispatch) {
            dispatch(loadItemSuccess(null));
        }
    }
    let apiUrl = "http://localhost:5000/api/Inventory/item/" + itemName;
    return function(dispatch){
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