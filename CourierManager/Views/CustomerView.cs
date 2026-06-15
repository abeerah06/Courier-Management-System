using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using CourierApp.Core.Utilities;
using CourierManager.Forms;

namespace CourierManager.Views
{
    public partial class CustomerView : UserControl
    {
        private readonly ICustomerService _service;
        private readonly BindingSource _bindingSource = new();

        public CustomerView(ICustomerService service)
        {
            InitializeComponent();
            _service = service;
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

        private void Load_() => LoadAsync();

        private void SetLoading(bool loading)
        {
            tsbAdd.Enabled = !loading;
            tsbEdit.Enabled = !loading;
            tsbDelete.Enabled = !loading;
            tsbRefresh.Enabled = !loading;
            lblCount.Text = loading ? "Loading..." : lblCount.Text;
        }

        private void SortByColumn(int colIndex)
        {
            var list = (_bindingSource.DataSource as List<Customer>) ?? [];
            _bindingSource.DataSource = colIndex switch
            {
                0 => [.. list.OrderBy(c => c.Id)],
                1 => [.. list.OrderBy(c => c.Name)],
                2 => [.. list.OrderBy(c => c.Phone)],
                3 => [.. list.OrderBy(c => c.City)],
                _ => list
            };
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            using var form = new CustomerForm(_service, CustomerFormModeEnum.Add);
            if (form.ShowDialog() == DialogResult.OK) LoadAsync();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Customer c) return;
            using var form = new CustomerForm(_service, CustomerFormModeEnum.Edit, c);
            if (form.ShowDialog() == DialogResult.OK) LoadAsync();
        }

        private void tsbView_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Customer c) return;
            using var form = new CustomerForm(_service, CustomerFormModeEnum.View, c);
            form.ShowDialog();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Customer c) return;
            var confirm = MessageBox.Show($"Delete customer '{c.Name}'?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _service.Delete(c.Id);
                    LoadAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "This customer cannot be deleted because they still have shipments on record.\n" +
                        "Delete the customer's shipments first, then try again.\n\nDetails: " + ex.Message,
                        "Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e) => LoadAsync();

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var results = await _service.SearchAsync(txtSearch.Text);
            _bindingSource.DataSource = results;
            lblCount.Text = $"Showing: {results.Count}";
        }
    }
}
