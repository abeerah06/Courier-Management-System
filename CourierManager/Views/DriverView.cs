using CourierApp.Core.Contracts;
using CourierApp.Core.Models;
using CourierApp.Core.Utilities;
using CourierManager.Forms;

namespace CourierManager.Views
{
    public partial class DriverView : UserControl
    {
        private readonly IDriverService _service;
        private readonly BindingSource _bindingSource = new();

        public DriverView(IDriverService service)
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
            var list = (_bindingSource.DataSource as List<Driver>) ?? [];
            _bindingSource.DataSource = colIndex switch
            {
                0 => [.. list.OrderBy(d => d.Id)],
                1 => [.. list.OrderBy(d => d.Name)],
                2 => [.. list.OrderBy(d => d.Phone)],
                3 => [.. list.OrderBy(d => d.VehicleType)],
                5 => [.. list.OrderBy(d => d.City)],
                _ => list
            };
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            using var form = new DriverForm(_service, DriverFormModeEnum.Add);
            if (form.ShowDialog() == DialogResult.OK) LoadAsync();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Driver d) return;
            using var form = new DriverForm(_service, DriverFormModeEnum.Edit, d);
            if (form.ShowDialog() == DialogResult.OK) LoadAsync();
        }

        private void tsbView_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Driver d) return;
            using var form = new DriverForm(_service, DriverFormModeEnum.View, d);
            form.ShowDialog();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Driver d) return;
            var confirm = MessageBox.Show($"Delete driver '{d.Name}'?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _service.Delete(d.Id);
                    LoadAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot delete: this driver may be assigned to shipments.\n\n" + ex.Message,
                        "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
