# from django.shortcuts import render
from .models import Item, Category, QualityModel
from rest_framework import viewsets, permissions
from .serializers import ItemSerializer, CategorySerializer, QualityModelSerializer


class ItemViewSet(viewsets.ModelViewSet):
    queryset = Item.objects.all()
    serializer_class = ItemSerializer
    permission_classes = [permissions.IsAuthenticated]


class CategoryViewSet(viewsets.ModelViewSet):
    queryset = Category.objects.all()
    serializer_class = CategorySerializer
    permission_classes = [permissions.IsAuthenticated]


class QualityModelViewSet(viewsets.ModelViewSet):
    queryset = QualityModel.objects.all()
    serializer_class = QualityModelSerializer
    permission_classes = [permissions.IsAuthenticated]
