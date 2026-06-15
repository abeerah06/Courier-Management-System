using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using CourierApp.Core.Utilities;
using CourierManager.Forms;

namespace CourierManager.Views
{
    public partial class ShipmentView : UserControl
    {
        private readonly IShipmentService _service;
        private readonly ICustomerService _customerService;
        private readonly IPaymentService _paymentService;
        private readonly IDriverService _driverService;
        private readonly BindingSource _bindingSource = new();

        public ShipmentView(IShipmentService service, ICustomerService customerService,
            IPaymentService paymentService, IDriverService driverService)
        {
            InitializeComponent();
            _service = service;
            _customerService = customerService;
            _paymentService = paymentService;
            _driverService = driverService;
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = _bindingSource;
            dgv.ColumnHeaderMouseClick += (s, e) => SortByColumn(e.ColumnIndex);
            LoadAsync();
        }

        private async void LoadAsync()
        {
            SetLoading(true);
            var list = await _service.GetAllAsync();
            _bindingSource.DataSource = list;
            lblCount.Text = $"Total: {list.Count}";
            SetLoading(false);
        }

        private void SetLoading(bool loading)
        {
            tsbAdd.Enabled = !loading;
            tsbEdit.Enabled = !loading;
            tsbDelete.Enabled = !loading;
            tsbRefresh.Enabled = !loading;
            if (loading) lblCount.Text = "Loading...";
        }

        private void SortByColumn(int colIndex)
        {
            var list = (_bindingSource.DataSource as List<Shipment>) ?? [];
            _bindingSource.DataSource = colIndex switch
            {
                0 => [.. list.OrderBy(s => s.Id)],
                1 => [.. list.OrderBy(s => s.CustomerName)],
                2 => [.. list.OrderBy(s => s.TrackingNo)],
                3 => [.. list.OrderBy(s => s.Destination)],
                4 => [.. list.OrderBy(s => s.Status)],
                5 => [.. list.OrderBy(s => s.Cost)],
                _ => list
            };
        }

        private async void ApplyFilters()
        {
            string status = cmbStatusFilter.SelectedItem?.ToString() ?? "All";
            var results = await _service.SearchAsync(txtSearch.Text, status);
            _bindingSource.DataSource = results;
            lblCount.Text = $"Showing: {results.Count}";
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            using var form = new ShipmentForm(_service, _customerService, _driverService, ShipmentFormModeEnum.Add);
            if (form.ShowDialog() == DialogResult.OK) LoadAsync();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Shipment s) return;
            using var form = new ShipmentForm(_service, _customerService, _driverService, ShipmentFormModeEnum.Edit, s);
            if (form.ShowDialog() == DialogResult.OK) LoadAsync();
        }

        private void tsbView_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Shipment s) return;
            using var form = new ShipmentForm(_service, _customerService, _driverService, ShipmentFormModeEnum.View, s);
            form.ShowDialog();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Shipment s) return;
            var confirm = MessageBox.Show($"Delete shipment {s.TrackingNo}?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _service.Delete(s.Id);
                    LoadAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "This shipment cannot be deleted because it has a payment record linked to it.\n" +
                        "Delete the related payment first, then try again.\n\nDetails: " + ex.Message,
                        "Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void tsbDeliver_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Shipment s) return;
            try
            {
                _service.UpdateStatus(s.Id, "Delivered");
                var existing = _paymentService.GetByShipmentId(s.Id);
                if (existing == null)
                {
                    _paymentService.Add(new Payment
                    {
                        ShipmentId = s.Id,
                        CustomerId = s.CustomerId,
                        CustomerName = s.CustomerName,
                        TrackingNo = s.TrackingNo,
                        Amount = s.Cost,
                        Status = "Unpaid",
                        PaymentDate = DateTime.Now
                    });
                    MessageBox.Show($"Shipment delivered!\nPayment of Rs {s.Cost:N0} generated for {s.CustomerName}.",
                        "Delivered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Shipment marked as delivered.", "Info",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LoadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbInTransit_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Shipment s) return;
            _service.UpdateStatus(s.Id, "In Transit");
            LoadAsync();
        }

        private void tsbCancel_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Shipment s) return;
            _service.UpdateStatus(s.Id, "Cancelled");
            LoadAsync();
        }

        private void tsbRefresh_Click(object sender, EventArgs e) => LoadAsync();

        private void txtSearch_TextChanged(object sender, EventArgs e) => ApplyFilters();
        private void cmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
    }
}
