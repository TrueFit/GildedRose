"""Class InventoryItem"""


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

        self.item_sell_in -= 1
        self.item_quality += self.quality_change_rate()

        # 2. The Quality of an item is never negative
        if self.item_quality < 0:
            self.item_quality = 0

        # 4. The Quality of an item is never more than 50
        if self.item_quality > 50:
            self.item_quality = 50

        # 8. "Sulfuras" is a legendary item and as such its Quality is 80 and it never alters
        if self.item_category == 'Sulfuras':
            self.item_quality = 80
            self.item_sell_in += 1

    def quality_change_rate(self):

        # default quality change rate is -1 unit
        quality_change_rate = -1

        # 1. Once the sell by date has passed, Quality degrades twice as fast
        if self.item_sell_in < 0:
            quality_change_rate = -2

        # 3. "Aged Brie" actually increases in Quality the older it gets
        if self.item_name == 'Aged Brie' and self.item_category == 'Food':
            quality_change_rate = 1

        # 7. "Conjured" items degrade in Quality twice as fast as normal items
        if self.item_category == 'Conjured':
            quality_change_rate = -2

        # 6. "Backstage passes", like aged brie, increases in Quality as it's SellIn value
        # approaches; Quality increases by 2 when there are 10 days or less and by 3 when
        # there are 5 days or less but Quality drops to 0 after the concert
        if self.item_category == 'Backstage Passes':
            if self.item_sell_in > 10:
                quality_change_rate = 1
            if self.item_sell_in <= 10:
                quality_change_rate = 2
            if self.item_sell_in <= 5:
                quality_change_rate = 3
            if self.item_sell_in < 0:
                quality_change_rate = -self.item_quality

        return quality_change_rate


if __name__ == '__main__':
    print('module not runnable')
    pass
