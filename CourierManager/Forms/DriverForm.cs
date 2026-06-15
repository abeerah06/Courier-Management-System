using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using CourierApp.Core.Utilities;

namespace CourierManager.Forms
{
    public partial class DriverForm : Form
    {
        private readonly IDriverService _service;
        private readonly DriverFormModeEnum _mode;
        private readonly Driver _driver;

        public DriverForm(IDriverService service, DriverFormModeEnum mode, Driver? driver = null)
        {
            InitializeComponent();
            _service = service;
            _mode = mode;
            _driver = driver ?? new Driver();
            PopulateFields();
            SetupMode();
        }

        private void PopulateFields()
        {
            txtName.Text = _driver.Name;
            txtPhone.Text = _driver.Phone;
            cmbVehicleType.SelectedItem = _driver.VehicleType;
            txtVehicleNo.Text = _driver.VehicleNo;
            txtCity.Text = _driver.City;
            chkActive.Checked = _driver.IsActive;
        }

        private void SetupMode()
        {
            switch (_mode)
            {
                case DriverFormModeEnum.Add:
                    Text = "Add Driver";
                    cmbVehicleType.SelectedItem = "Bike";
                    chkActive.Checked = true;
                    break;
                case DriverFormModeEnum.Edit:
                    Text = "Edit Driver";
                    break;
                case DriverFormModeEnum.View:
                    Text = "View Driver";
                    txtName.ReadOnly = true;
                    txtPhone.ReadOnly = true;
                    cmbVehicleType.Enabled = false;
                    txtVehicleNo.ReadOnly = true;
                    txtCity.ReadOnly = true;
                    chkActive.Enabled = false;
                    btnSave.Visible = false;
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _driver.Name = txtName.Text.Trim();
                _driver.Phone = txtPhone.Text.Trim();
                _driver.VehicleType = cmbVehicleType.SelectedItem?.ToString() ?? "Bike";
                _driver.VehicleNo = txtVehicleNo.Text.Trim();
                _driver.City = txtCity.Text.Trim();
                _driver.IsActive = chkActive.Checked;

                var validation = _service.Validate(_driver);
                if (!validation.IsValid)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, validation.Errors),
                        "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_mode == DriverFormModeEnum.Add)
                    _service.Add(_driver);
                else if (_mode == DriverFormModeEnum.Edit)
                    _service.Update(_driver);

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
