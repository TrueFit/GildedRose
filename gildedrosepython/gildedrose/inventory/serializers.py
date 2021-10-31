from rest_framework import serializers
from .models import Item, Category, QualityModel


class ItemSerializer(serializers.ModelSerializer):
    class Meta:
        model = Item
        fields = '__all__'
        depth = 2


class CategorySerializer(serializers.ModelSerializer):
    class Meta:
        model = Category
        fields = '__all__'
        depth = 2


class QualityModelSerializer(serializers.ModelSerializer):
    class Meta:
        model = QualityModel
        fields = '__all__'
        depth = 2
