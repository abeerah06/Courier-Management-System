using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using CourierApp.Core.Utilities;

namespace CourierManager.Forms
{
    public partial class CustomerForm : Form
    {
        private readonly ICustomerService _service;
        private readonly CustomerFormModeEnum _mode;
        private readonly Customer _customer;

        public CustomerForm(ICustomerService service, CustomerFormModeEnum mode, Customer? customer = null)
        {
            InitializeComponent();
            _service = service;
            _mode = mode;
            _customer = customer ?? new Customer();
            PopulateFields();
            SetupMode();
        }

        private void PopulateFields()
        {
            txtName.Text = _customer.Name;
            txtPhone.Text = _customer.Phone;
            txtEmail.Text = _customer.Email;
            txtAddress.Text = _customer.Address;
            txtCity.Text = _customer.City;
        }

        private void SetupMode()
        {
            switch (_mode)
            {
                case CustomerFormModeEnum.Add:
                    Text = "Add Customer";
                    break;
                case CustomerFormModeEnum.Edit:
                    Text = "Edit Customer";
                    break;
                case CustomerFormModeEnum.View:
                    Text = "View Customer";
                    txtName.ReadOnly = true;
                    txtPhone.ReadOnly = true;
                    txtEmail.ReadOnly = true;
                    txtAddress.ReadOnly = true;
                    txtCity.ReadOnly = true;
                    btnSave.Visible = false;
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _customer.Name = txtName.Text.Trim();
                _customer.Phone = txtPhone.Text.Trim();
                _customer.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
                _customer.Address = txtAddress.Text.Trim();
                _customer.City = txtCity.Text.Trim();

                var validation = _service.Validate(_customer);
                if (!validation.IsValid)
                {
                    MessageBox.Show(string.Join(Environment.NewLine, validation.Errors),
                        "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_mode == CustomerFormModeEnum.Add)
                    _service.Add(_customer);
                else if (_mode == CustomerFormModeEnum.Edit)
                    _service.Update(_customer);

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
