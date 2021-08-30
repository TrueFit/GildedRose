"""
Class InventoryItem - Contains base inventory item class
and definitions for derived (special items). Includes
a factory method to generate class instances based on
name and/or category.

To add more special requirements adjust factory
function (create_item) and add new class definition.
"""


class InventoryItem:
    def __init__(self, name, category, sell_in, quality):
        self.item_name = name
        self.item_category = category
        self.item_sell_in = sell_in
        self.item_quality = quality

    def __str__(self):
        return (f'{self.item_name} - (Category: {self.item_category})'
                f' : Sell in {self.item_sell_in} days : Quality {self.item_quality}')

    def adjust_quality_at_end_of_day(self):
        #print("ADJUSTING IN InventoryItem")

        self.item_sell_in -= 1

        if self.item_sell_in < 0:
            """1. Once the sell by date has passed, Quality degrades twice as fast"""
            self.item_quality += -2
        else:
            self.item_quality += -1

        InventoryItem.general_limits_on_quality(self)

    def general_limits_on_quality(self):
        """# 2. The Quality of an item is never negative"""
        if self.item_quality < 0:
            self.item_quality = 0

        """# 4. The Quality of an item is never more than 50"""
        if self.item_quality > 50:
            self.item_quality = 50


def create_item(name, category, sell_in, quality):

    if category == 'Sulfuras':
        item = ItemSulfuras(name, category, sell_in, quality)
    elif name == 'Aged Brie' and category == 'Food':
        item = ItemAgedBrie(name, category, sell_in, quality)
    elif category == 'Conjured':
        item = ItemConjured(name, category, sell_in, quality)
    elif category == 'Backstage Passes':
        item = ItemBackstagePasses(name, category, sell_in, quality)
    else:
        item = InventoryItem(name, category, sell_in, quality)

    return item


class ItemBackstagePasses(InventoryItem):
    def adjust_quality_at_end_of_day(self):
        #print("ADJUSTING IN BACKSTAGE PASSES")
        """
        #6. "Backstage passes", like aged brie, increases in Quality as it's SellIn value
            approaches; Quality increases by 2 when there are 10 days or less and by 3 when
            there are 5 days or less but Quality drops to 0 after the concert
        """
        self.item_sell_in -= 1

        if self.item_sell_in > 10:
            self.item_quality += 1
        if 10 >= self.item_sell_in > 5:
            self.item_quality += 2
        if 5 >= self.item_sell_in >= 0:
            self.item_quality += 3
        if self.item_sell_in < 0:
            self.item_quality = 0

        InventoryItem.general_limits_on_quality(self)


class ItemAgedBrie(InventoryItem):
    def adjust_quality_at_end_of_day(self):
        #print("ADJUSTING IN AGED BRIE")
        """
        # 3. "Aged Brie" actually increases in Quality the older it gets
        """
        self.item_sell_in -= 1
        self.item_quality += 1

        # assumption made that brie just keeps getting better beyond sell by

        InventoryItem.general_limits_on_quality(self)


class ItemConjured(InventoryItem):
    def adjust_quality_at_end_of_day(self):
        #print("ADJUSTING IN CONJURED")
        """
         #7. "Conjured" items degrade in Quality twice as fast as normal items
         """
        self.item_sell_in -= 1
        self.item_quality += -2

        # assumption made that conjured items consistently degrade even beyond sell date

        InventoryItem.general_limits_on_quality(self)


class ItemSulfuras(InventoryItem):
    def adjust_quality_at_end_of_day(self):
        """
        # 8. "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters
        """
        #print("ADJUSTING IN SULFURAS")
        self.item_quality = self.item_quality
        self.item_sell_in = self.item_sell_in


if __name__ == '__main__':
    print('module not runnable')
    pass
