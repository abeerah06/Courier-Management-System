from django import forms

from .models import Courier


class CourierForm(forms.ModelForm):
    class Meta:
        model = Courier
        fields = [
            "tracking_id",
            "sender_name",
            "receiver_name",
            "origin",
            "destination",
            "weight",
            "status",
        ]

    def clean_weight(self):
        weight = self.cleaned_data.get("weight")
        if weight is None or weight <= 0:
            raise forms.ValidationError("Weight must be greater than 0.")
        return weight

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        for name, field in self.fields.items():
            field.widget.attrs["class"] = "form-select" if name == "status" else "form-control"
