from django.urls import include, path
from rest_framework import routers
from . import views

router = routers.DefaultRouter()
router.register(r'items', views.ItemViewSet)
router.register(r'categories', views.CategoryViewSet)
router.register(r'qualitymodels', views.QualityModelViewSet)

urlpatterns = [
    path('api/', include(router.urls))
]
