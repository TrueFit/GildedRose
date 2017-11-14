import S3 from 'aws-sdk/clients/s3';
import uuid from 'uuid/v4';
import _ from 'lodash';

// Constants
const s3Client = new S3({
    signatureVersion: 'v4',
    accessKeyId: process.env.ak,
    secretAccessKey: process.env.sak
});

const INVENTORY_BUCKET = "gilded-rose-inventry";
const INVENTORY_KEY = "inventory.json";
const INITIAL_STATE_INVENTORY_KEY = "initialStateInventory.json";

const ITEM_CATEGORY_SULFURAS = "Sulfuras";
const ITEM_CATEGORY_CONJURED = "Conjured";
const ITEM_CATEGORY_BACKSTAGE_PASSES = "Backstage passes";
const ITEM_CATEGORY_FOOD = "Food";
const ITEM_CATEGORY_WEAPON = "Weapon";
const ITEM_CATEGORY_ARMOR = "Armor";
const ITEM_CATEGORY_POTION = "Potion";
const ITEM_CATEGORY_MISC = "Misc";

const ITEM_CATEGORIES = {
    ITEM_CATEGORY_CONJURED,
    ITEM_CATEGORY_SULFURAS,
    ITEM_CATEGORY_BACKSTAGE_PASSES,
    ITEM_CATEGORY_FOOD,
    ITEM_CATEGORY_WEAPON,
    ITEM_CATEGORY_ARMOR,
    ITEM_CATEGORY_POTION,
    ITEM_CATEGORY_MISC
}

const VALID_ITEM_CATEGORIES = [
    ITEM_CATEGORY_CONJURED,
    ITEM_CATEGORY_SULFURAS,
    ITEM_CATEGORY_BACKSTAGE_PASSES,
    ITEM_CATEGORY_FOOD,
    ITEM_CATEGORY_WEAPON,
    ITEM_CATEGORY_ARMOR,
    ITEM_CATEGORY_POTION,
    ITEM_CATEGORY_MISC
];

const ITEM_PROP_CATEGORY = "category";
const ITEM_PROP_LABEL = "label";
const ITEM_PROP_SELLIN = "sellIn";
const ITEM_PROP_QUALITY = "quality";

const ITEM_PROPS = {
    ITEM_PROP_CATEGORY,
    ITEM_PROP_LABEL,
    ITEM_PROP_SELLIN,
    ITEM_PROP_QUALITY
}

const ITEM_PROP_TYPES = {
    ITEM_PROP_CATEGORY: { validateFunc: _.isString, typeLabel: "String" },
    ITEM_PROP_LABEL: { validateFunc: _.isString, typeLabel: "String" },
    ITEM_PROP_SELLIN: { validateFunc: _.isNumber, typeLabel: "Number" },
    ITEM_PROP_QUALITY: { validateFunc: _.isNumber, typeLabel: "Number" }
}


const EX_ITEM_VAL_001 = "EX_ITEM_VAL_001";
const EX_ITEM_VAL_002 = "EX_ITEM_VAL_002";
const EX_ITEM_VAL_003 = "EX_ITEM_VAL_003";
const EX_INV_001 = "EX_INV_001";
const EX_INV_002 = "EX_INV_002";
const EX_INV_003 = "EX_INV_003";
const EX_INV_004 = "EX_INV_004";
const EX_INV_005 = "EX_INV_005";
const EX_INV_006 = "EX_INV_006";
const EX_ITEM_ADD_001 = "EX_ITEM_ADD_001";
const EX_ITEM_ADD_002 = "EX_ITEM_ADD_002";
const EX_ITEM_ADD_003 = "EX_ITEM_ADD_003";
const EX_ITEM_GET_001 = "EX_ITEM_GET_001";
const EX_ITEM_DELETE_001 = "EX_ITEM_DELETE_001";
const EX_ITEM_DELETE_002 = "EX_ITEM_DELETE_002";

const EXCEPTION_CODES = {
    EX_ITEM_VAL_001,
    EX_ITEM_VAL_002,
    EX_ITEM_VAL_003,
    EX_INV_001,
    EX_INV_002,
    EX_INV_003,
    EX_INV_004,
    EX_INV_005,
    EX_INV_006,
    EX_ITEM_ADD_001,
    EX_ITEM_ADD_002,
    EX_ITEM_ADD_003,
    EX_ITEM_GET_001,
    EX_ITEM_DELETE_001,
    EX_ITEM_DELETE_002
}

export function getItemCategories() {
    return ITEM_CATEGORIES;
}

export function getExceptionCodes() {
    return EXCEPTION_CODES;
}

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
                    code: EX_INV_001,
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
                        code: EX_INV_006,
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
                    code: EX_INV_002,
                    err
                });
            } else {
                resolve(inventory);
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
                    code: EX_INV_003,
                    err
                });
            } else {
                let _data = data.Body;
                if (Buffer.isBuffer(_data)) {
                    _data = _data.toString('utf-8')
                }

                // Another promise here so going to resolve, reject based on what is
                // happening while saving
                saveInventory(JSON.parse(_data)).then((inventory) => {
                    resolve(inventory);
                }).catch((err) => {
                    reject({
                        message: "An error occurred while attempting to reset the inventory",
                        code: EX_INV_004,
                        err
                    });
                })
            }
        })
    });
}

/**
 * Will handle 
 * @param {*} item 
 */
export function updateItem(item) {
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

    // Skip Sulfuras since their quality and sellIn never changes, as they are truly legendary
    if (item.category !== ITEM_CATEGORY_SULFURAS) {
        // Decrease sellIn by one
        item.sellIn -= 1;
        // Quality degrades double as fast when 
        const degradeMultiplier = (item.sellIn < 0) ? 2 : 1;
        switch (item.category) {
            case ITEM_CATEGORY_CONJURED: {
                item.quality -= 2 * degradeMultiplier;
                break;
            }
            case ITEM_CATEGORY_BACKSTAGE_PASSES: {
                if (item.sellIn > 10) {
                    item.quality += 1;
                } else if (item.sellIn <= 10 && item.sellIn > 5) {
                    item.quality += 2;
                } else if (item.sellIn <= 5 && item.sellIn > 0) {
                    item.quality += 3;
                } else if (item.sellIn <= 0) {
                    item.quality = 0;
                }
                break;
            }
            default: {
                if (item.label !== 'Aged Brie') {
                    item.quality -= 1 * degradeMultiplier;
                } else {
                    // Making an assumption here, since Brie actually increases in quality over time
                    // I assume it increases double as fast once the sellIn date has passed.
                    item.quality += 1 * degradeMultiplier;
                }
            }
        }
        // Ensure boundaries
        if (item.quality > 50) {
            item.quality = 50;
        }
        if (item.quality < 0) {
            item.quality = 0;
        }

        if (item.sellIn <= 0) {
            item.trash=true;
        }
    }

}

/**
 * Will move time forward one day
 */
export function nextDay() {
    return new Promise((resolve, reject) => {
        getInventory().then((inventory) => {
            inventory.forEach(item => {
                updateItem(item);
            });

            saveInventory(inventory).then(() => {
                // Once saved communicate the new inventory
                resolve(inventory);
            }).catch((err) => {
                console.error(err);
                reject({
                    code: EX_ITEM_ADD_001,
                    message: "Unable to add new item, failed to save new inventory.",
                    err
                });
            })
        }).catch((err) => {
            console.error(err);
            reject({
                code: EX_ITEM_ADD_002,
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
                code: EX_ITEM_GET_001,
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
                    code: EX_ITEM_DELETE_001,
                    message: `Unable to delete item(s) (${itemIds}), failed to save new inventory.`,
                    err
                });
            })
        }).catch((err) => {
            console.error(err);
            reject({
                code: EX_ITEM_DELETE_002,
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
 * Ensure the item adheres to a simple base schema, could use JSON Schema here but
 * it's simple enough we'll just do it manually :)
 * @param {*} item 
 */
export function validateItem(item) {

    const errors = [];

    // If this is missing an
    _.keys(ITEM_PROPS).forEach(propKey => {
        const prop = ITEM_PROPS[propKey];
        if (!item.hasOwnProperty(prop)) {
            errors.push({
                code: EX_ITEM_VAL_001,
                message: `Missing required property ${prop}`
            });
        } else {
            // Do some basic type validation
            if (!ITEM_PROP_TYPES[propKey].validateFunc(item[prop])) {
                errors.push({
                    code: EX_ITEM_VAL_002,
                    message: `Expected item property ${prop} to be of type ${ITEM_PROP_TYPES[propKey].typeLabel} but got ${item[prop]}`
                })
            }
        }
    });

    if (item.hasOwnProperty(ITEM_PROP_CATEGORY)) {
        // Might be missing which we should've captured earlier, determine if it is one of the categories we expect
        if (_.indexOf(VALID_ITEM_CATEGORIES, item[ITEM_PROP_CATEGORY]) === -1) {
            errors.push({
                code: EX_ITEM_VAL_003,
                message: `An item must be in one of the following categories: ${VALID_ITEM_CATEGORIES.join(', ')} but got '${item[ITEM_PROP_CATEGORY]}'`
            })
        }
    }

    return errors;
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
            const validationErrors = [];
            items.forEach(item => {
                let validationResult = validateItem(item);
                if (_.isEmpty(validationResult)) {
                    // Assign the item id
                    item.id = uuid();
                    // Add the item
                    inventory.push(item);
                } else {
                    validationErrors.push({
                        item,
                        validationResult
                    })
                }
            });
            if (!_.isEmpty(validationErrors)) {
                let errorDetails = {
                    code: EX_ITEM_ADD_003,
                    message: "Unable to add items to inventory, some validation errors were detected.",
                    err: validationErrors
                }
                reject(errorDetails);
            } else {
                saveInventory(inventory).then((newInventory) => {
                    // Once saved communicate the new inventory
                    resolve(newInventory);
                }).catch((err) => {
                    console.error(err);
                    reject({
                        code: EX_ITEM_ADD_001,
                        message: "Unable to add new item, failed to save new inventory.",
                        err
                    });
                })
            }
        }).catch((err) => {
            console.error(err);
            reject({
                code: EX_ITEM_ADD_002,
                message: "Unable to add new item, failed to get existing inventory.",
                err
            });
        });
    });
}