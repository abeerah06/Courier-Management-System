from django.contrib import messages
from django.shortcuts import get_object_or_404, redirect, render

from .forms import CourierForm
from .models import Courier


def list_couriers(request):
    couriers = Courier.objects.all()
    return render(request, "list.html", {"couriers": couriers})


def create_courier(request):
    if request.method == "POST":
        form = CourierForm(request.POST)
        if form.is_valid():
            form.save()
            messages.success(request, "Courier created successfully.")
            return redirect("list_couriers")
        messages.error(request, "Please correct the errors below.")
    else:
        form = CourierForm()
    return render(request, "form.html", {"form": form, "title": "Create Courier"})


def update_courier(request, id):
    courier = get_object_or_404(Courier, id=id)
    if request.method == "POST":
        form = CourierForm(request.POST, instance=courier)
        if form.is_valid():
            form.save()
            messages.success(request, "Courier updated successfully.")
            return redirect("list_couriers")
        messages.error(request, "Please correct the errors below.")
    else:
        form = CourierForm(instance=courier)
    return render(request, "form.html", {"form": form, "title": "Update Courier"})


def delete_courier(request, id):
    courier = get_object_or_404(Courier, id=id)
    if request.method == "POST":
        courier.delete()
        messages.success(request, "Courier deleted successfully.")
        return redirect("list_couriers")
    return render(request, "detail.html", {"courier": courier, "confirm_delete": True})


def courier_detail(request, id):
    courier = get_object_or_404(Courier, id=id)
    return render(request, "detail.html", {"courier": courier, "confirm_delete": False})
