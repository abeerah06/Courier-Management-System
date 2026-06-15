using CourierApp.Core.Contracts;
using CourierApp.Core.Services;
using CourierManager.Views;
using System.Configuration;

namespace CourierManager.Forms
{
    public partial class MainForm : Form
    {
        private readonly ICustomerService _customerService;
        private readonly IShipmentService _shipmentService;
        private readonly IPaymentService _paymentService;
        private readonly IDashboardService _dashboardService;
        private readonly IDriverService _driverService;

        private readonly Dictionary<Type, UserControl> _views = [];

        public MainForm()
        {
            InitializeComponent();

            string conn = ConfigurationManager.ConnectionStrings["CourierDB"].ConnectionString;

            _customerService = new DBCustomerService(conn);
            _shipmentService = new DBShipmentService(conn);
            _paymentService = new DBPaymentService(conn);
            _dashboardService = new DBDashboardService(conn);
            _driverService = new DBDriverService(conn);

            ShowView(() => new DashboardView(_dashboardService));
            UpdateStatusBar("Dashboard");
        }

        private void ShowView<T>(Func<T> factory) where T : UserControl
        {
            var key = typeof(T);
            if (!_views.TryGetValue(key, out var view))
            {
                view = factory();
                view.Dock = DockStyle.Fill;
                _views[key] = view;
            }
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(view);
            view.BringToFront();
        }

        private void UpdateStatusBar(string section)
        {
            lblStatus.Text = $"  User: admin   |   Section: {section}   |   {DateTime.Now:dd MMM yyyy  HH:mm}";
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            _views.Remove(typeof(DashboardView));
            ShowView(() => new DashboardView(_dashboardService));
            UpdateStatusBar("Dashboard");
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            ShowView(() => new CustomerView(_customerService));
            UpdateStatusBar("Customers");
        }

        private void btnShipments_Click(object sender, EventArgs e)
        {
            _views.Remove(typeof(ShipmentView));
            ShowView(() => new ShipmentView(_shipmentService, _customerService, _paymentService, _driverService));
            UpdateStatusBar("Shipments");
        }

        private void btnDrivers_Click(object sender, EventArgs e)
        {
            _views.Remove(typeof(DriverView));
            ShowView(() => new DriverView(_driverService));
            UpdateStatusBar("Drivers");
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            _views.Remove(typeof(PaymentView));
            ShowView(() => new PaymentView(_paymentService));
            UpdateStatusBar("Payments");
        }
    }
}
