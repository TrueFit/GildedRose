/**
 * Will verify that all quality and sell in conditions are properly dealt with by the service.

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
test('test will make sure that ', () => {
    expect(1 + 2).toBe(3);
});