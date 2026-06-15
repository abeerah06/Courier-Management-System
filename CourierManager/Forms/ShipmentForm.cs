using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using CourierApp.Core.Utilities;

namespace CourierManager.Forms
{
    public partial class ShipmentForm : Form
    {
        private readonly IShipmentService _service;
        private readonly ICustomerService _customerService;
        private readonly IDriverService _driverService;
        private readonly ShipmentFormModeEnum _mode;
        private readonly Shipment _shipment;

        public ShipmentForm(IShipmentService service, ICustomerService customerService,
            IDriverService driverService, ShipmentFormModeEnum mode, Shipment? shipment = null)
        {
            InitializeComponent();
            _service = service;
            _customerService = customerService;
            _driverService = driverService;
            _mode = mode;
            _shipment = shipment ?? new Shipment();

            LoadCustomers();
            LoadDrivers();
            PopulateFields();
            SetupMode();
        }

        private void LoadCustomers()
        {
            var customers = _customerService.GetAll();
            cmbCustomer.DataSource = customers;
            cmbCustomer.DisplayMember = "Name";
            cmbCustomer.ValueMember = "Id";
        }

        private void LoadDrivers()
        {
            // "Unassigned" option (Id 0) plus all active drivers
            var drivers = _driverService.GetActive();
            drivers.Insert(0, new Driver { Id = 0, Name = "-- Unassigned --" });
            cmbDriver.DataSource = drivers;
            cmbDriver.DisplayMember = "Name";
            cmbDriver.ValueMember = "Id";
        }

        private void PopulateFields()
        {
            if (_shipment.CustomerId > 0)
                cmbCustomer.SelectedValue = _shipment.CustomerId;

            cmbDriver.SelectedValue = _shipment.DriverId ?? 0;

            txtTrackingNo.Text = _shipment.Id == 0
                ? _service.GenerateTrackingNo()
                : _shipment.TrackingNo;

            txtOrigin.Text = _shipment.Origin;
            txtDestination.Text = _shipment.Destination;
            numWeight.Value = _shipment.WeightKg > 0 ? _shipment.WeightKg : 1;
            numCost.Value = _shipment.Cost;
            dtpPickup.Value = _shipment.PickupDate == default ? DateTime.Today : _shipment.PickupDate;
            cmbStatus.SelectedItem = _shipment.Status == string.Empty ? "Pending" : _shipment.Status;
            txtNotes.Text = _shipment.Notes ?? string.Empty;
        }

        private void SetupMode()
        {
            switch (_mode)
            {
                case ShipmentFormModeEnum.Add:
                    Text = "Add Shipment";
                    cmbStatus.SelectedItem = "Pending";
                    break;
                case ShipmentFormModeEnum.Edit:
                    Text = "Edit Shipment";
                    break;
                case ShipmentFormModeEnum.View:
                    Text = "View Shipment";
                    cmbCustomer.Enabled = false;
                    cmbDriver.Enabled = false;
                    txtTrackingNo.ReadOnly = true;
                    txtOrigin.ReadOnly = true;
                    txtDestination.ReadOnly = true;
                    numWeight.Enabled = false;
                    numCost.Enabled = false;
                    dtpPickup.Enabled = false;
                    cmbStatus.Enabled = false;
                    txtNotes.ReadOnly = true;
                    btnSave.Visible = false;
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = cmbCustomer.SelectedItem as Customer;
                _shipment.CustomerId = selected?.Id ?? 0;
                _shipment.CustomerName = selected?.Name ?? string.Empty;

                var driver = cmbDriver.SelectedItem as Driver;
                _shipment.DriverId = (driver == null || driver.Id == 0) ? null : driver.Id;
                _shipment.DriverName = (driver == null || driver.Id == 0) ? "Unassigned" : driver.Name;

                _shipment.TrackingNo = txtTrackingNo.Text.Trim();
                _shipment.Origin = txtOrigin.Text.Trim();
                _shipment.Destination = txtDestination.Text.Trim();
                _shipment.WeightKg = numWeight.Value;
                _shipment.Cost = numCost.Value;
                _shipment.PickupDate = dtpPickup.Value.Date;
                _shipment.Status = cmbStatus.SelectedItem?.ToString() ?? "Pending";
                _shipment.Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

                var validation = _service.Validate(_shipment);
                if (!validation.IsValid)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, validation.Errors),
                        "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_mode == ShipmentFormModeEnum.Add)
                    _service.Add(_shipment);
                else if (_mode == ShipmentFormModeEnum.Edit)
                    _service.Update(_shipment);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
