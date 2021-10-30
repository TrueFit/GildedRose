import csv
from django.core.management.base import BaseCommand, CommandError

from inventory.models import *

class Command(BaseCommand):
    help = 'Update the realworld edge cases'

    # def add_arguments(self, parser):
    #     parser.add_argument('inventory_file', type=str)

    def handle(self, *args, **options):
        
        c,_=Category.objects.get_or_create(name='Sulfuras')
        c.minq=80
        c.maxq=80
        c.never_expires=True
        c.save()
        qm=QualityModel(name='Aged Brie', quality_delta_per_day=1, expired_scale=0)
        qm.item=Item.objects.get_or_create(name='Aged Brie')[0]
        qm.save()
        qm=QualityModel.objects.get_or_create(name='Sulfuras default QM')[0]
        qm.delete()
        qm=QualityModel.objects.get_or_create(name='Backstage Passes default QM')[0]
        qm.delete()
        qm=QualityModel(name='Backstage Passes 10+ QM',valid_until_n_days_before_expire=10,quality_delta_per_day=1,expired_scale=100)
        qm.category=Category.objects.get_or_create(name='Backstage Passes')[0]
        qm.save()
        qm=QualityModel(name='Backstage Passes 10-5 QM',valid_from_n_days_before_expire=10,valid_until_n_days_before_expire=5,quality_delta_per_day=2,expired_scale=100)
        qm.category=Category.objects.get_or_create(name='Backstage Passes')[0]
        qm.save()
        qm=QualityModel(name='Backstage Passes 5-0 QM',valid_from_n_days_before_expire=5,valid_until_n_days_before_expire=0,quality_delta_per_day=3,expired_scale=100)
        qm.category=Category.objects.get_or_create(name='Backstage Passes')[0]
        qm.save()
        qm=QualityModel(name='Backstage Passes 0+ QM',valid_from_n_days_before_expire=0,quality_delta_per_day=-100,expired_scale=100)
        qm.category=Category.objects.get_or_create(name='Backstage Passes')[0]
        qm.save()

        qm=QualityModel.objects.get_or_create(name='Conjured default QM')[0]
        qm.quality_delta_per_day=-2
        qm.save()

        self.stdout.write("Models updated")