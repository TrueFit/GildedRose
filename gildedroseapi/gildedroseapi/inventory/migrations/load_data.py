import csv,os

from django.conf import settings
from django.db import migrations


def forwards_func(apps, schema_editor):
    # We get the model from the versioned app registry;
    # if we directly import it, it'll be the wrong version

    Item = apps.get_model("inventory", "Item")
    ItemCategory = apps.get_model("inventory", "ItemCategory")

    # we read the txt file and insert data into our sqlite db
    path_to_txt = os.path.abspath(os.path.join(settings.BASE_DIR, 'inventory.txt'))
    with open(path_to_txt, "r") as filestream:
        for line in filestream:
            # split the line by the commas
            currentline = line.split(',')

            # get or create the category from second item in csv
            category = ItemCategory.objects.get_or_create(name=currentline[1])[0]

            item = Item()
            item.category = category
            item.name = currentline[0]
            item.sell_in = currentline[2]
            item.quality = currentline[3]
            item.save()


def reverse_func(apps, schema_editor):
    # reverse_func() deletes all imported data from DB

    Item = apps.get_model("inventory", "Item")
    ItemCategory = apps.get_model("inventory", "ItemCategory")

    Item.objects.all().delete()
    ItemCategory.objects.all().delete()

class Migration(migrations.Migration):
    """
    The requirement is to load our initial inventory from a comma separated txt file.
    We are using Django's built in migrations to create a custom import script.
    After our initial DB is created, we will read from the txt and insert our objects
    """

    dependencies = [
        ('inventory', '0001_initial'),
    ]

    operations = [
        migrations.RunPython(forwards_func, reverse_func),
    ]
