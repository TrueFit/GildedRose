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
    class Meta:
        verbose_name_plural = "categories"

    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    name = models.CharField(max_length=30, unique=True)
    minq = models.IntegerField(default=0)
    maxq = models.IntegerField(default=50)
    never_expires = models.BooleanField(default=False)

    def __str__(self) -> str:
        return self.name


class Item(models.Model):
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    received_on = models.DateField()
    sell_by = models.DateField()

    name = models.CharField(max_length=30, unique=True)
    category = models.ForeignKey(Category, on_delete=models.PROTECT)
    initial_sell_in = models.IntegerField()
    initial_quality = models.FloatField()

    def __str__(self) -> str:
        return '{} ({}) -- Q:{} -- SELL IN:{} -- AGE:{}'.format(self.name, self.category, self.current_quality, self.current_sell_in, self.get_days_old())

    def current_sell_in(self):
        if self.category.never_expires:
            return self.initial_sell_in
        else:
            now = get_now()
            return (self.sell_by - now.date()).days
    current_sell_in.short_description = 'Sell In'
    current_sell_in = property(current_sell_in)

    def get_days_old(self):
        now = get_now()
        return (now.date()-self.received_on).days

    
    def current_quality(self):
        quality_models = QualityModel.objects.filter(
            item=self).order_by('valid_from_n_days_before_expire')
        if not quality_models:
            quality_models = QualityModel.objects.filter(
                category=self.category).order_by('valid_from_n_days_before_expire')
        q = self.initial_quality
        days_until_expire = self.current_sell_in
        for d in range(self.get_days_old()):
            # print("day: {}".format(d))
            for m in quality_models:
                dq = m.get_quality_delta(self.current_sell_in+d)
                # print(m.name, dq)
                q += dq
        return max(self.category.minq, min(self.category.maxq, q))
    # current_quality.admin_order_field = 'current_quality'
    current_quality.short_description = 'Quality'
    current_quality = property(current_quality)


class QualityModel(models.Model):
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    name = models.CharField(max_length=30, unique=True)
    item = models.ForeignKey(
        Item, related_name="models", on_delete=models.CASCADE, default=None, null=True, blank=True)
    category = models.ForeignKey(
        Category, related_name="models", on_delete=models.CASCADE, default=None, null=True, blank=True)

    valid_from_n_days_before_expire = models.IntegerField(default=MAX_DAYS)
    valid_until_n_days_before_expire = models.IntegerField(
        default=MIN_DAYS)  # negative values = after expire
    quality_delta_per_day = models.IntegerField(default=-1)
    expired_scale = models.IntegerField(default=2)

    def get_quality_delta(self, days_until_expire: int) -> int:
        if self.valid_from_n_days_before_expire > days_until_expire >= self.valid_until_n_days_before_expire:
            q = self.quality_delta_per_day * \
                (self.expired_scale if days_until_expire < 0 else 1)
            return q
        else:
            return 0

    def __str__(self) -> str:
        return self.name


@receiver(pre_save, sender=Item)
def update_sell_by(sender, instance, *args, **kwargs):
    if not instance.received_on:
        instance.received_on = datetime.datetime.utcnow().astimezone()
    instance.sell_by = instance.received_on + \
        datetime.timedelta(days=instance.initial_sell_in)
