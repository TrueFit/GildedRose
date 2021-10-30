from django.db import models
import datetime
from django.db.models.fields import IntegerField
from django.dispatch import receiver
from django.db.models.signals import pre_save

from .util import get_now

MIN_DAYS = -2**16
MAX_DAYS = 2**16


# Create your models here.


class Category(models.Model):
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    name = models.CharField(max_length=30, unique=True)
    minq = models.IntegerField(default=0)
    maxq = models.IntegerField(default=50)

    def __str__(self) -> str:
        return self.name


class Item(models.Model):
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    received_on = models.DateTimeField()
    sell_by = models.DateTimeField()

    name = models.CharField(max_length=30, unique=True)
    category = models.ForeignKey(Category, on_delete=models.PROTECT)
    initial_sell_in = models.IntegerField()
    initial_quality = models.FloatField()

    def __str__(self) -> str:
        return '{} -- C:{} -- Q:{} -- X:{}'.format(self.name, self.category, self.get_quality(), self.get_sell_in())

    def get_sell_in(self):
        now = get_now()
        return (self.sell_by - now).days + 1

    def get_quality(self):
        quality_models = QualityModel.objects.filter(
            category=self.category).order_by('valid_from_n_days_before_expire')
        q = self.initial_quality
        days_until_expire = self.get_sell_in()
        expired = days_until_expire < 0
        for m in quality_models:
            if m.valid_from_n_days_before_expire >= days_until_expire > m.valid_until_n_days_before_expire:
                q = q+m.quality_delta_per_day * (m.expired_scale if expired else 1) #FIXME need to account for number of days
        return max(self.category.minq, min(self.category.maxq, q))


class QualityModel(models.Model):
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    name = models.CharField(max_length=30, unique=True)
    item = models.ForeignKey(
        Item, related_name="models", on_delete=models.CASCADE, default=None, null=True)
    category = models.ForeignKey(
        Category, related_name="models", on_delete=models.CASCADE, default=None, null=True)

    valid_from_n_days_before_expire = models.IntegerField(default=MAX_DAYS)
    valid_until_n_days_before_expire = models.IntegerField(
        default=MIN_DAYS)  # negative values = after expire
    quality_delta_per_day = models.IntegerField(default=-1)
    expired_scale = models.IntegerField(default=2)

    def __str__(self) -> str:
        return self.name


@receiver(pre_save, sender=Item)
def update_sell_by(sender, instance, *args, **kwargs):
    if not instance.received_on:
        instance.received_on = datetime.datetime.utcnow().astimezone()
    instance.sell_by = instance.received_on + \
        datetime.timedelta(days=instance.initial_sell_in)
