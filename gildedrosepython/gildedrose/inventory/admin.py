from django.contrib import admin
from inventory.models import *
from django.template.response import TemplateResponse
from django.urls import path

from django.db.models import F

from .util import get_now


# Register your models here.
# myModels = [Item, Category, QualityModel] # iterable list
# admin.site.register(myModels)


admin.site.site_title = "Gilded Rose"
admin.site.site_header = "Gilded Rose"

class TrashItemsFilter(admin.SimpleListFilter):
    title = 'Trash Product'
    parameter_name = 'trash'

    def lookups(self, request, model_admin):
        return (
            ('yes', 'Yes'),
            ('no', 'No'),
        )

    def queryset(self, request, queryset):
        trash = [i.id for i in Item.objects.all() if i.current_quality <= 0]

        if self.value() == 'yes':
            return queryset.filter(id__in=trash)
        if self.value() == 'no':
                        return queryset.exclude(id__in=trash)


@admin.register(Item)
class ItemAdmin(admin.ModelAdmin):
    readonly_fields = ("created_at", "updated_at",)
    ordering = ('sell_by',)
    list_filter = ('category',TrashItemsFilter,)
    list_display = ('name', 'category','current_quality', 'current_sell_in')
    


@admin.register(Category)
class CategoryAdmin(admin.ModelAdmin):
    readonly_fields = ("created_at", "updated_at",)


@admin.register(QualityModel)
class QualityModelAdmin(admin.ModelAdmin):
    readonly_fields = ("created_at", "updated_at",)
