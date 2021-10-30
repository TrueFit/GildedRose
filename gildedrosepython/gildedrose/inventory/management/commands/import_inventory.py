import csv
from django.core.management.base import BaseCommand, CommandError

from inventory.models import *

class Command(BaseCommand):
    help = 'Closes the specified poll for voting'

    def add_arguments(self, parser):
        parser.add_argument('inventory_file', type=str)

    def handle(self, *args, **options):
        header=['Item Name','Item Category','Sell In','Quality']
        with open(options['inventory_file']) as f:
            for x in csv.reader(f):
                qm,created=QualityModel.objects.get_or_create(name=x[1] +" default QM")
                c,created=Category.objects.get_or_create(name=x[1])
                qm.category=c
                i = Item()
                i.name=x[0]
                i.category=c
                i.initial_sell_in=int(x[2])
                i.initial_quality=int(x[3])
                
                i.save()
                qm.save()

        c=Category.objects.get(name='Sulfuras')
        c.minq=80
        c.maxq=80
        c.save()
        qm=QualityModel(name='Aged Brie', quality_delta_per_day=2, expired_scale=1)
        qm.item=Item.objects.get(name='Aged Brie')
        qm.save()
        qm=QualityModel.objects.get(name='Sulfuras default QM')
        qm.delete()
        qm=QualityModel.objects.get(name='Backstage Passes default QM')
        qm.delete()
        qm=QualityModel(name='Backstage Passes 10+ QM',valid_until_n_days_before_expire=10,quality_delta_per_day=1,expired_scale=-100)
        qm.category=Category.objects.get(name='Backstage Passes')
        qm.save()
        qm=QualityModel(name='Backstage Passes 10-5 QM',valid_from_n_days_before_expire=10,valid_until_n_days_before_expire=5,quality_delta_per_day=2,expired_scale=-100)
        qm.category=Category.objects.get(name='Backstage Passes')
        qm.save()
        qm=QualityModel(name='Backstage Passes 5-0 QM',valid_from_n_days_before_expire=5,valid_until_n_days_before_expire=0,quality_delta_per_day=3,expired_scale=-100)
        qm.category=Category.objects.get(name='Backstage Passes')
        qm.save()

        qm=QualityModel.objects.get(name='Conjured default QM')
        qm.quality_delta_per_day=-2
        qm.save
     



        self.stdout.write("Successfully imported inventory")