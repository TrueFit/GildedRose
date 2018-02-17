from django.urls import path, include
from rest_framework.routers import DefaultRouter

from .views import ItemViewSet


router = DefaultRouter()
router.register(r'items', ItemViewSet, base_name='item')
urlpatterns = router.urls

urlpatterns = [
    path(r'', include(router.urls)),
]