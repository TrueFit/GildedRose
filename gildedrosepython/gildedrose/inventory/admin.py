from django.contrib import admin
from inventory.models import *
# Register your models here.
# myModels = [Item, Category, QualityModel] # iterable list
# admin.site.register(myModels)

@admin.register(Item)
class ItemAdmin(admin.ModelAdmin):
    readonly_fields = ("created_at","updated_at",)
@admin.register(Category)
class CategoryAdmin(admin.ModelAdmin):
    readonly_fields = ("created_at","updated_at",)
@admin.register(QualityModel)
class QualityModelAdmin(admin.ModelAdmin):
    readonly_fields = ("created_at","updated_at",)
