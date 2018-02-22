
from rest_framework import viewsets, mixins, generics, status
from rest_framework.decorators import list_route
from rest_framework.response import Response
from django_filters.rest_framework import DjangoFilterBackend

from .models import Item
from .serializers import ItemSerializer
from .services import InventoryService

# Create your views here.
class ItemViewSet(viewsets.ModelViewSet):
    serializer_class = ItemSerializer
    queryset = Item.objects.all()
    filter_backends = (DjangoFilterBackend,)
    filter_fields = ('quality',)

    @list_route(methods=['post'], url_path='end-day')
    def end_day(self, request, *args, **kwargs):
        """
        Post to this endpoint will manually end the current day
        and perform the updates to the inventory
        """

        InventoryService.end_day()

        return Response(status=status.HTTP_204_NO_CONTENT)
