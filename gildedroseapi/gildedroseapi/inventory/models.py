from django.db import models
from django.core.exceptions import ValidationError
from django.core.validators import MinValueValidator


# Create your models here.
class ItemCategory(models.Model):
    """
    ItemCategory tracks the type of each item.
    """

    name = models.CharField(max_length=255, db_index=True)

    def __str__(self):
        return self.name


class Item(models.Model):
    """
    Instance of an item in the inventory. Items have a Name, Category, Sellin value, and Quality value.
    """

    name = models.CharField(max_length=255, db_index=True)
    category = models.ForeignKey(ItemCategory, on_delete=models.CASCADE)
    sell_in = models.IntegerField() # null value represents no time limit.
    quality = models.PositiveIntegerField()

    def __str__(self):
        return 'Item: {0} Category: {1} Sell In: {2} Quality: {3}' \
            .format(self.name, self.category.name, self.sell_in, self.quality)
