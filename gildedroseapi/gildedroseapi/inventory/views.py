
from rest_framework import viewsets, mixins, generics
from django_filters.rest_framework import DjangoFilterBackend

from .models import Item
from .serializers import ItemSerializer

# Create your views here.
class ItemViewSet(viewsets.ModelViewSet):
    serializer_class = ItemSerializer
    queryset = Item.objects.all()
    filter_backends = (DjangoFilterBackend,)
    filter_fields = ('quality',)
