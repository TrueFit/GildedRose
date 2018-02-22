from django.db.models import Q

from .models import Item

class InventoryService:
    """
    Business logic related to the Inventory app
    """

    @staticmethod
    def end_day():
        """
        This method closes out the current day and updates
        the sell_in and quality fields for all items in
        the inventory
        """

        # DONT FORGET TO REMOVE LOGGNG AFTER TESTING
        import logging
        logging.error('UPDATING INVENTORY!!!')

        # get all the items except for "sulfuras" types which never change and items that
        # have reached a quality of 0 and are not "Aged Brie" whose quality increases with time.
        #
        items = Item.objects.select_related('category').exclude(category__name='Sulfuras')

        # if sell_in is negative, degrade quality twice as fast

        # The quality of item is never negative

        # "Aged Brie" actually increaes in quality as it gets older

        # Quality should never exceed 50(except for "Sulfuras" types, which never changes sell_in or quality)

        # "Backstage passes", like aged brie, increases in Quality as it's SellIn value approaches; Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but Quality drops to 0 after the concert

        # "Conjured" items degrade in Quality twice as fast as normal items

        # iterate the items and make appropriate changes to sell_in and quality values
        for item in items:
            logging.error('ITEM: {0}'.format(item))

            # day has ended, decrement the sell_in value
            item.sell_in = item.sell_in - 1

            if item.name != "Aged Brie" and item.category.name != "Backstage Passes":
                if item.quality > 0:
                    logging.error('decrementing item quality')
                    item.quality = item.quality - 1

                    # for conjured types or after sell_in passes, double the rate of decrease
                    if item.category.name == "Conjured" or item.sell_in < 0:
                        if item.quality > 0:
                            item.quality = item.quality - 1
            else:
                # These items increase in quality or have other special rules
                if item.category.name == "Backstage Passes":
                    if item.sell_in < 0:
                        # past the date so set quality to 0
                        item.quality = 0
                    else:
                        if item.quality < 50:
                            item.quality = item.quality + 1

                            # increase at higher rate as we get close to sell_in but don't exceed 50
                            if item.sell_in < 11 and item.quality < 50:
                                item.quality = item.quality + 1
                            if item.sell_in < 6 and item.quality < 50:
                                item.quality = item.quality + 1
                else:
                    # Aged Brie - quality increases with age
                    if item.quality < 50:
                        item.quality = item.quality + 1

                        # double the rate after sell_in passes
                        if item.sell_in < 0 and item.quality < 50:
                            item.quality = item.quality + 1

            # save the object TODO: maybe refactor to bulk save for performance
            item.save()
