import {updateItem, getItemCategories, validateItem, getExceptionCodes} from '../../../src/js/service/serviceImpl';
import _ from 'lodash';
import uuid from 'uuid/v4';

/**
 * This isn't exhaustive testing, but decent coverage to cover the next day requirements as well as item validation.
 * In an exhaustive test plan we would go as far as adding mocks for S3 to detect what would be sent to S3 and to mock retrieving data from S3.
 */
const ITEM_CATEGORIES = getItemCategories();
const EXCEPTION_CODES = getExceptionCodes();

function buildItem(label, category, quality, sellIn) {
    return {
        label,
        category,
        quality,
        sellIn
    }
}

const ITEM_TEMPLATE_SULFURAS = buildItem("Some Legendary Purple Item",ITEM_CATEGORIES.ITEM_CATEGORY_SULFURAS,80, 80);
const ITEM_TEMPLATE_CONJURED = buildItem("Great Conjured Item",ITEM_CATEGORIES.ITEM_CATEGORY_CONJURED,50,20);
const ITEM_TEMPLATE_BACKSTAGE_PASS_MORE_THAN_10 = buildItem("Benny and the Ogres",ITEM_CATEGORIES.ITEM_CATEGORY_BACKSTAGE_PASSES,20,20);
const ITEM_TEMPLATE_BACKSTAGE_PASS_LESS_THAN_10_GREATER_THAN_5 = buildItem("Benny and the Ogres",ITEM_CATEGORIES.ITEM_CATEGORY_BACKSTAGE_PASSES,20,7);
const ITEM_TEMPLATE_BACKSTAGE_PASS_LESS_THAN_5 = buildItem("Benny and the Ogres",ITEM_CATEGORIES.ITEM_CATEGORY_BACKSTAGE_PASSES,20,4);
const ITEM_TEMPLATE_BACKSTAGE_PASS_EXACTLY_10 = buildItem("Benny and the Ogres",ITEM_CATEGORIES.ITEM_CATEGORY_BACKSTAGE_PASSES,20,10);
const ITEM_TEMPLATE_BACKSTAGE_PASS_EXACTLY_5 = buildItem("Benny and the Ogres",ITEM_CATEGORIES.ITEM_CATEGORY_BACKSTAGE_PASSES,20,5);
const ITEM_TEMPLATE_BACKSTAGE_PASS_CONCERT_OVER = buildItem("Benny and the Ogres",ITEM_CATEGORIES.ITEM_CATEGORY_BACKSTAGE_PASSES,20,1);

test("test will make sure that Sulfuras never decrease in quality and that the sellIn doesn't change", () => {
    const sulfurasItem = _.clone(ITEM_TEMPLATE_SULFURAS);
    updateItem(sulfurasItem);
    expect(sulfurasItem.quality).toBe(80);
    expect(sulfurasItem.sellIn).toBe(80);
});

test("test will make sure that quality decreases twice as fast for conjured items.", () => {
    const conjuredItem = _.clone(ITEM_TEMPLATE_CONJURED);
    updateItem(conjuredItem);
    expect(conjuredItem.quality).toBe(48);
    expect(conjuredItem.sellIn).toBe(19);
});

test("test will make sure that quality increases normally for Backstage Passes when the sellIn is above 10.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_MORE_THAN_10);
    updateItem(backstagePassItem);
    expect(backstagePassItem.quality).toBe(21);
    expect(backstagePassItem.sellIn).toBe(19);
});

test("test will make sure that quality increases double for Backstage Passes when the sellIn is below 10 and more than 5.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_LESS_THAN_10_GREATER_THAN_5);
    updateItem(backstagePassItem);
    expect(backstagePassItem.quality).toBe(22);
    expect(backstagePassItem.sellIn).toBe(6);
});

test("test will make sure that quality increases triple for Backstage Passes when the sellIn is below 5.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_LESS_THAN_5);
    updateItem(backstagePassItem);
    expect(backstagePassItem.quality).toBe(23);
    expect(backstagePassItem.sellIn).toBe(3);
});

test("test will make sure that quality increases triple for Backstage Passes when the sellIn is exactly 5.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_EXACTLY_5);
    updateItem(backstagePassItem);
    expect(backstagePassItem.quality).toBe(23);
    expect(backstagePassItem.sellIn).toBe(4);
});

test("test will make sure that quality increases double for Backstage Passes when the sellIn is exactly 10.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_EXACTLY_10);
    updateItem(backstagePassItem);
    expect(backstagePassItem.quality).toBe(22);
    expect(backstagePassItem.sellIn).toBe(9);
});

test("test will make sure that quality decreases to 0 once a backstage pass reaches a sellIn of 0 (concert over).", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_CONCERT_OVER);
    updateItem(backstagePassItem);
    expect(backstagePassItem.quality).toBe(0);
    expect(backstagePassItem.sellIn).toBe(0);
});

// Item Validation tests
test("test will make sure that an item with a wrong category is properly detected.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_CONCERT_OVER);
    const validationResult = validateItem({
        label:"Test",
        category:"Doesn't exist",
        sellIn:2,
        quality:2
    });
    expect(validationResult.length).toBe(1);
    expect(validationResult[0].code).toBe(EXCEPTION_CODES.EX_ITEM_VAL_003);
});
test("test will make sure that an item with a sellIn property that is not a Number is properly validated.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_CONCERT_OVER);
    const validationResult = validateItem({
        label:"Test",
        category:ITEM_CATEGORIES.ITEM_CATEGORY_ARMOR,
        sellIn:"2",
        quality:2
    });
    expect(validationResult.length).toBe(1);
    expect(validationResult[0].code).toBe(EXCEPTION_CODES.EX_ITEM_VAL_002);
});
test("test will make sure that an item with a quality property that is not a Number is properly validated.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_CONCERT_OVER);
    const validationResult = validateItem({
        label:"Test",
        category:ITEM_CATEGORIES.ITEM_CATEGORY_ARMOR,
        sellIn:2,
        quality:"2"
    });
    expect(validationResult.length).toBe(1);
    expect(validationResult[0].code).toBe(EXCEPTION_CODES.EX_ITEM_VAL_002);
});
test("test will make sure that an item with a category property that is not a String is properly validated.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_CONCERT_OVER);
    const validationResult = validateItem({
        label:"Test",
        category:false,
        sellIn:2,
        quality:2
    });
    // Will have to validation errors: 1) Category is not a String 2) It's not one of the valid categories
    expect(validationResult.length).toBe(2);
});
test("test will make sure that an item with a label property that is not a String is properly validated.", () => {
    const backstagePassItem = _.clone(ITEM_TEMPLATE_BACKSTAGE_PASS_CONCERT_OVER);
    const validationResult = validateItem({
        label:false,
        category:ITEM_CATEGORIES.ITEM_CATEGORY_ARMOR,
        sellIn:2,
        quality:2
    });
    // Will have to validation errors: 1) Category is not a String 2) It's not one of the valid categories
    expect(validationResult.length).toBe(1);
    expect(validationResult[0].code).toBe(EXCEPTION_CODES.EX_ITEM_VAL_002);
});
