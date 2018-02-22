
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

        NOTE: This is kept simple by calling the service
        method synchronously for the purpose of this sample.

        Real world, it would likely be a better idea to run this
        as an async task. We'd also probably make this a scheduled
        job to run nightly instead of making an API endpoint
        that needs to be called manually.
        """

        InventoryService.end_day()

        return Response(status=status.HTTP_204_NO_CONTENT)
