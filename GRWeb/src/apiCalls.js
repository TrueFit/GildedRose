import React from 'react';


// Need to refactor these to return promises...
/*
export function CallInventoryApi(apiUrl, reponseFunction, errorFunction){
    fetch(apiUrl)
        .then(
            function(response) {
            if (response.status === 204){
                console.log("No info Returned");
                return [];
            }
            else{
                let data = response.json();
                return data;
            }

        })
        .then(data => reponseFunction(data))
        .catch(error => errorFunction(error));
};
*/

export function CallInventoryApi(apiUrl){
    return fetch(apiUrl)
        .then(
            function(response) {
            if (response.status === 204){
                return [];
            }
            else{
                let data = response.json();
                return data;
            }
        });
};

export function PostEndOfDay(){
    let apiUrl = "http://localhost:5000/api/Inventory/end-of-day";
    return fetch(apiUrl,{
        method: "POST"           
    }).then(response => {
        return response;
    });
};

export function PostToInventoryApi(apiUrl, responseFunction, errorFunction){
    fetch(apiUrl,{
        method: "POST"           
    }).then(response => responseFunction())
    .catch(error => errorFunction(error));
};

