using CourierApp.Core.Contracts;
using CourierApp.Core.Models;

namespace CourierManager.Views
{
    public partial class PaymentView : UserControl
    {
        private readonly IPaymentService _service;
        private readonly BindingSource _bindingSource = new();

        public PaymentView(IPaymentService service)
        {
            InitializeComponent();
            _service = service;
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = _bindingSource;
            dgv.ColumnHeaderMouseClick += (s, e) => SortByColumn(e.ColumnIndex);
            LoadPayments();
        }

        private async void LoadPayments()
        {
            var list = await _service.GetAllAsync();
            _bindingSource.DataSource = list;
            lblCount.Text = $"Total: {list.Count}";
        }

        private void SortByColumn(int colIndex)
        {
            var list = (_bindingSource.DataSource as List<Payment>) ?? [];
            _bindingSource.DataSource = colIndex switch
            {
                0 => [.. list.OrderBy(p => p.Id)],
                1 => [.. list.OrderBy(p => p.CustomerName)],
                2 => [.. list.OrderBy(p => p.TrackingNo)],
                3 => [.. list.OrderBy(p => p.Amount)],
                4 => [.. list.OrderBy(p => p.Status)],
                _ => list
            };
        }

        private void tsbPaid_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Payment p) return;
            _service.UpdateStatus(p.Id, "Paid");
            LoadPayments();
        }

        private void tsbUnpaid_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Payment p) return;
            _service.UpdateStatus(p.Id, "Unpaid");
            LoadPayments();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is not Payment p) return;
            var confirm = MessageBox.Show("Delete this payment record?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _service.Delete(p.Id);
                    LoadPayments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not delete payment.\n\nDetails: " + ex.Message,
                        "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e) => LoadPayments();

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var results = await _service.SearchAsync(txtSearch.Text);
            _bindingSource.DataSource = results;
            lblCount.Text = $"Showing: {results.Count}";
        }
    }
}
