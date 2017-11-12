import S3 from 'aws-sdk/clients/s3';
import uuid from 'uuid/v4';

const s3Client = new S3({
    signatureVersion: 'v4',
    accessKeyId: process.env.ak,
    secretAccessKey: process.env.sak
});

const INVENTORY_BUCKET = "gilded-rose-inventry";
const INVENTORY_KEY = "inventory.json";
const INITIAL_STATE_INVENTORY_KEY = "initialStateIventory.json"

function getInventory() {
    return new Promise((resolve, reject) => {
        // console.log("Attempting to get inventory",process.env.ak,process.env.sak);
        s3Client.getObject({
            Bucket: INVENTORY_BUCKET,
            Key: INVENTORY_KEY
        }, (err, data) => {
            if (err) {
                console.error(err);
                reject({
                    message: "Unable to retrieve current inventory",
                    code: "EX_INV_001",
                    err
                });
            } else {
                let _data = data.Body;
                if (Buffer.isBuffer(_data)) {
                    _data = _data.toString('utf-8')
                }

                try {
                    resolve(JSON.parse(_data));
                } catch (err) {
                    console.error(err);
                    reject({
                        message: "Unable to retrieve current inventory, inventory returned couldn't be parsed",
                        code: "EX_INV_006",
                        err
                    })
                }
            }
        })
    });
}

function saveInventory(inventory) {
    return new Promise((resolve, reject) => {
        s3Client.putObject({
            Bucket: INVENTORY_BUCKET,
            Key: INVENTORY_KEY,
            Body: JSON.stringify(inventory, null, '\t')
        }, (err, data) => {
            if (err) {
                console.error(err);
                reject({
                    message: "Unable to save new inventory",
                    code: "EX_INV_002",
                    err
                });
            } else {
                let _data = data.Body;
                if (Buffer.isBuffer(_data)) {
                    _data = _data.toString('utf-8')
                }

                resolve(_data);
            }
        })
    });
}

/**
 * Will reset the inventory to its initial state
 */
export function resetInventory() {
    return new Promise((resolve, reject) => {
        s3Client.getObject({
            Bucket: INVENTORY_BUCKET,
            Key: INITIAL_STATE_INVENTORY_KEY
        }, (err, data) => {
            if (err) {
                reject({
                    message: "An error occurred while attempting to reset the inventory",
                    code: "EX_INV_003",
                    err
                });
            } else {
                let _data = data.Body;
                if (Buffer.isBuffer(_data)) {
                    _data = _data.toString('utf-8')
                }

                // Another promise here so going to resolve, reject based on what is
                // happening while saving
                saveInventory(_data).then(() => {
                    resolve();
                }).catch((err) => {
                    reject({
                        message: "An error occurred while attempting to reset the inventory",
                        code: "EX_INV_004",
                        err
                    });
                })
            }
        })
    });
}

function updateItem(item) {
    /**
     * Here are the basic rules for the system that we need:

        All items have a SellIn value which denotes the number of days we have to sell the item
        All items have a Quality value which denotes how valuable the item is
        At the end of each day our system lowers both values for every item
        Since this is the real world, there are some edge cases we need for you to account for as well:

        Once the sell by date has passed, Quality degrades twice as fast
        The Quality of an item is never negative
        "Aged Brie" actually increases in Quality the older it gets
        The Quality of an item is never more than 50
        "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but Quality drops to 0 after the concert
        "Conjured" items degrade in Quality twice as fast as normal items
        An item can never have its Quality increase above 50, however "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters.
     */
    if (item.category !== 'Sulfuras') {
        item.sellIn -= 1;
        const degradeMultiplier = (item.sellIn < 0) ? 2:1;
        switch (item.category) {
            case 'Conjured': {
                item.quality-=2*degradeMultiplier;
                break;
            }
            case 'Backstage passes': {
                if (sellIn > 10) {
                    item.quality += 1;
                } else if (item.sellIn <= 10 && item.sellIn > 5) {
                    item.quality += 2;
                } else if (item.sellIn <= 5 && item.sellIn > 0) {
                    item.quality += 3;
                } else if (item.sellIn === 0) {
                    item.quality = 0;
                }
                break;
            }
            default: {
                if (item.label !== 'Aged Brie') {
                item.quality-=1*degradeMultiplier;
                } else {
                    item.quality+=1;
                }
            }
        }
        // Ensure boundaries
        if (iten.quality > 50) {
            item.quality = 50;
        }
        if (item.quality < 0) {
            item.quality = 0;
        }
    }
    
}

/**
 * Will move time forward one day
 */
export function nextDay() {
    return new Promise((resolve, reject) => {
        getInventory().then((inventory) => {
            items.forEach(item => {
                // Assign the item id
                item.id = uuid();
                // Add the item
                inventory.push(item);
            });

            saveInventory(inventory).then(() => {
                // Once saved communicate the new inventory
                resolve(inventory);
            }).catch((err) => {
                console.error(err);
                reject({
                    code: "EX_ITEM_ADD_001",
                    message: "Unable to add new item, failed to save new inventory.",
                    err
                });
            })
        }).catch((err) => {
            console.error(err);
            reject({
                code: "EX_ITEM_ADD_002",
                message: "Unable to add new item, failed to get existing inventory.",
                err
            });
        });
    });
}

/**
 * Retrieve a single item from the inventory by id
 * @param {<Number>} itemId 
 */
export function getItem(itemId) {
    return new Promise((resolve, reject) => {
        getInventory().then((inventory) => {
            const newInventory = [];
            inventory.forEach((item, idx) => {
                if (item.id === itemId) {
                    // Remove target
                    resolve(item);
                }
            });
        }).catch((err) => {
            console.error(err);
            reject({
                code: "EX_ITEM_GET_001",
                message: `Unable to get item (${itemId}), failed to get existing inventory.`,
                err
            });
        });
    });
}

/**
 * Deletes an item from the inventory
 * @param {<Number>} itemId 
 */
export function deleteItem(itemId) {
    return deleteItems([itemId]);
}

/**
 * Will remove all items specified by their ID
 * @param {Array<String>} itemIds
 */
export function deleteItems(itemIds) {
    const itemIdMap = {};
    itemIds.forEach(itemId => {
        itemIdMap[itemId] = true;
    })
    return new Promise((resolve, reject) => {
        getInventory().then((inventory) => {
            const newInventory = [];
            inventory.forEach((item, idx) => {
                if (!itemIdMap[item.id]) {
                    // Remove target
                    newInventory.push(item);
                }
            });

            saveInventory(newInventory).then(() => {
                // Once saved communicate the new inventory
                resolve(newInventory);
            }).catch((err) => {
                console.error(err);
                reject({
                    code: "EX_ITEM_DELETE_001",
                    message: `Unable to delete item(s) (${itemIds}), failed to save new inventory.`,
                    err
                });
            })
        }).catch((err) => {
            console.error(err);
            reject({
                code: "EX_ITEM_DELETE_002",
                message: `Unable to delete item(s) (${itemIds}), failed to get existing inventory.`,
                err
            });
        });
    });
}

/**
 * Will list all items in the current inventory
 */
export function listItems() {
    return getInventory();
}

/**
 * Adds an item to the inventory
 * @param {<Item>{label<String>, category<String>, sellIn<Number>, quality<Number>}} item 
 */
export function addItem(item) {
    return addItems([item]);
}

/**
 * Adds items to the inventory (bulk)
 * @param {Array<Item>} items 
 */
export function addItems(items) {
    return new Promise((resolve, reject) => {
        getInventory().then((inventory) => {
            items.forEach(item => {
                // Assign the item id
                item.id = uuid();
                // Add the item
                inventory.push(item);
            });

            saveInventory(inventory).then(() => {
                // Once saved communicate the new inventory
                resolve(inventory);
            }).catch((err) => {
                console.error(err);
                reject({
                    code: "EX_ITEM_ADD_001",
                    message: "Unable to add new item, failed to save new inventory.",
                    err
                });
            })
        }).catch((err) => {
            console.error(err);
            reject({
                code: "EX_ITEM_ADD_002",
                message: "Unable to add new item, failed to get existing inventory.",
                err
            });
        });
    });
}