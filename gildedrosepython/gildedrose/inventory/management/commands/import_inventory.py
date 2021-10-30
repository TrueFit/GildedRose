import csv
from django.core.management.base import BaseCommand, CommandError

from inventory.models import *


class Command(BaseCommand):
    help = 'Import the inventory'

    def add_arguments(self, parser):
        parser.add_argument('inventory_file', type=str)

    def handle(self, *args, **options):
        header = ['Item Name', 'Item Category', 'Sell In', 'Quality']
        with open(options['inventory_file']) as f:
            for x in csv.reader(f):
                qm, created = QualityModel.objects.get_or_create(
                    name=x[1] + " default QM")
                c, created = Category.objects.get_or_create(name=x[1])
                qm.category = c
                i = Item()
                i.name = x[0]
                i.category = c
                i.initial_sell_in = int(x[2])
                i.initial_quality = int(x[3])

                i.save()
                qm.save()

        self.stdout.write("Successfully imported inventory")
