from rest_framework import serializers

from .models import Item

class ItemSerializer(serializers.ModelSerializer):
    category_name = serializers.CharField(source='category.name', read_only=True)

    class Meta:
        model = Item
        fields = ('id', 'name', 'category', 'category_name', 'sell_in', 'quality')
