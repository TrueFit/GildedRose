import request from 'superagent';
import _ from 'lodash';
import state from '../state/state';
import delay from 'timeout-as-promise';
import promiseSerial from 'promise-serial';

// API Gateway endpoint
const SERVICE_ENDPOINT_PREFIX = "https://ds0pdybji1.execute-api.us-east-1.amazonaws.com/prod/";
const STATE_PROP_GOOD_INVENTORY = "good";
const STATE_PROP_BAD_INVENTORY = "bad";
const STATE_PROP_INVENTORY = "inventory";

export function addItem(item) {
    return new Promise((resolve, reject)=>{
        showSpinner("Creating item and adding to inventory...");
        delay(500).then(() => {
            request
            .post(`${SERVICE_ENDPOINT_PREFIX}/item`)
            .send(item)
            .set('Accept', 'application/json')
            .end((err, res) => {
                // Do something
                if (err) {
                    // Show error alert
                    console.error(err);
                    alert("Failed to create new item, please check console for details, most likely some validation errors!");
                    reject();
                    hideSpinner();
                } else {
                    sortInventory(res.body);
                    resolve();
                    hideSpinner();
                }
            });
        });
    })
    
}

function showSpinner(spinnerText = "") {
    const spinnerCursor = state.select(['ui', 'spinner']);
    spinnerCursor.set('text', spinnerText);
    spinnerCursor.set('show', true);
}

function hideSpinner(spinnerText = "") {
    const spinnerCursor = state.select(['ui', 'spinner']);

    spinnerCursor.set('show', false);
    spinnerCursor.set('text', "");
}

export function deleteItems(itemIds) {
    showSpinner("Deleting items...");
    delay(500).then(() => {
        const promises = [];
        _.keys(itemIds).forEach((itemId) => {
            if (itemIds[itemId] === true) {
                promises.push(() => new Promise((resolve, reject) => {
                    request
                        .delete(`${SERVICE_ENDPOINT_PREFIX}/item/${itemId}`)
                        .end((err, res) => {
                            // Do something
                            if (err) {
                                // Show error alert
                                reject(err);
                            } else {
                                resolve(res.body);
                            }
                        });
                }))
            }
        });

        // Could've built an endpoint to deal with multiple deletes, for now this will do, serialize the promises.
        promiseSerial(promises).then((results) => {
            getInventory();
            hideSpinner();
        }).catch(err => {
            console.error(err);
            alert("An error occurred while attempting to delete items. Please see console for detailed error message");
            hideSpinner();
        })


    })

}

function sortInventory(inventory) {
    const good = [];
    const bad = []
    if (inventory && _.isArray(inventory)) {
        inventory.forEach((item => {
            if (item.trash) {
                bad.push(item);
            } else {
                good.push(item);
            }
        }))
    }
    const inventoryCursor = state.select([STATE_PROP_INVENTORY]);
    inventoryCursor.set(STATE_PROP_GOOD_INVENTORY, good);
    inventoryCursor.set(STATE_PROP_BAD_INVENTORY, bad);
}

export function getInventory() {
    return new Promise((resolve, reject) => {
        request
            .get(`${SERVICE_ENDPOINT_PREFIX}/inventory`)
            .end((err, res) => {
                // Do something
                if (err) {
                    // Show error alert
                    reject(err);
                } else {
                    console.log(res.body);
                    sortInventory(res.body);
                    resolve(res.body);
                }
            });
    });

}

export function nextDay() {
    showSpinner("Moving into the future (1 day to be precise)...");
    delay(500).then(() => {
        request
            .get(`${SERVICE_ENDPOINT_PREFIX}/inventory/nextDay`)
            .end((err, res) => {
                // Do something
                if (err) {
                    // Show error alert
                    console.log(err);
                    alert("Unable to move into the future, an error occured, please check the console for detailed error message.")
                    hideSpinner();
                } else {
                    sortInventory(res.body);
                    hideSpinner();
                }
            });
    });
}

export function resetInventories() {
    showSpinner("Moving back in time...");
    delay(500).then(() => {
        request
            .get(`${SERVICE_ENDPOINT_PREFIX}/inventory/reset`)
            .end((err, res) => {
                // Do something
                if (err) {
                    // Show error alert
                    console.log(err);
                    alert("Unable to move into the past, an error occured, please check the console for detailed error message.");
                    hideSpinner();
                } else {
                    sortInventory(res.body);
                    hideSpinner();
                }
            });
    });
}

export function showModal() {
    const spinnerCursor = state.select(['ui', 'modal']);
    spinnerCursor.set('show', true);
}

export function hideModal() {
    const spinnerCursor = state.select(['ui', 'modal']);
    spinnerCursor.set('show', false);
}