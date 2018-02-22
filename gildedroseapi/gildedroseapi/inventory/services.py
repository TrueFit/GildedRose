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
        the inventory according to the business rules.
        """

        # get all the items except for "sulfuras" types which never change
        items = Item.objects.select_related('category').exclude(category__name='Sulfuras')

        # iterate the items and make appropriate changes to sell_in and quality values
        for item in items:
            # day has ended, decrement the sell_in value
            item.sell_in = item.sell_in - 1

            if item.name != "Aged Brie" and item.category.name != "Backstage Passes":
                if item.quality > 0:
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

            # save the object
            item.save()
