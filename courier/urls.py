from django.urls import path

from . import views

urlpatterns = [
    path("", views.list_couriers, name="list_couriers"),
    path("create/", views.create_courier, name="create_courier"),
    path("update/<int:id>/", views.update_courier, name="update_courier"),
    path("delete/<int:id>/", views.delete_courier, name="delete_courier"),
    path("detail/<int:id>/", views.courier_detail, name="courier_detail"),
]
