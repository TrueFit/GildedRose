import React from 'react';

export function ApiCall(apiUrl, reponseFunction, errorFunction){
    fetch(apiUrl)
        .then(function(response) {
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

