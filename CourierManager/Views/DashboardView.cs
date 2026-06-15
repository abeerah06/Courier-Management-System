using CourierApp.Core.Contracts;
using System.Windows.Forms.DataVisualization.Charting;

namespace CourierManager.Views
{
    public partial class DashboardView : UserControl
    {
        private readonly IDashboardService _service;

        public DashboardView(IDashboardService service)
        {
            InitializeComponent();
            _service = service;
            LoadDashboard();
        }

        private async void LoadDashboard()
        {
            await LoadStatCards();
            await LoadBarChart();
            LoadPieChart();
        }

        private async Task LoadStatCards()
        {
            lblTotalValue.Text = (await _service.GetTotalShipmentsAsync()).ToString();
            lblMonthValue.Text = _service.GetShipmentsThisMonth().ToString();
            lblWeekValue.Text = _service.GetShipmentsThisWeek().ToString();
            lblTodayValue.Text = _service.GetShipmentsToday().ToString();
        }

        private async Task LoadBarChart()
        {
            var data = await _service.GetMonthlyShipmentsAsync(6);

            chartBar.Series.Clear();
            var area = chartBar.ChartAreas[0];
            area.BackColor = Color.FromArgb(64, 64, 73);

            // X axis = months
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(220, 220, 228);
            area.AxisX.LineColor = Color.FromArgb(110, 110, 122);
            area.AxisX.Interval = 1;                       // show every month label
            area.AxisX.Title = "Month";
            area.AxisX.TitleFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            area.AxisX.TitleForeColor = Color.FromArgb(200, 203, 213);

            // Y axis = whole number of shipments
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(82, 82, 92);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(220, 220, 228);
            area.AxisY.LineColor = Color.FromArgb(110, 110, 122);
            area.AxisY.LabelStyle.Format = "0";            // no decimals
            area.AxisY.Minimum = 0;
            area.AxisY.Interval = 1;                        // tick every 1
            area.AxisY.Title = "Number of Shipments";
            area.AxisY.TitleFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            area.AxisY.TitleForeColor = Color.FromArgb(200, 203, 213);

            // If the largest month is big, let it auto-step in whole numbers instead of cramming
            int maxVal = data.Count > 0 ? data.Values.Max() : 0;
            if (maxVal > 10)
                area.AxisY.Interval = Math.Ceiling(maxVal / 8.0);

            var series = new Series("Shipments")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(230, 126, 34),
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                LabelForeColor = Color.FromArgb(235, 235, 240),
                BorderWidth = 0,
                IsXValueIndexed = true   // place each month at its own position (text X-values otherwise stack at 0)
            };

            foreach (var kvp in data)
                series.Points.AddXY(kvp.Key, kvp.Value);

            if (data.Count == 0)
                series.Points.AddXY("No Data", 0);

            chartBar.Series.Add(series);
            chartBar.Titles.Clear();
            chartBar.Titles.Add(new Title("Monthly Shipments (Last 6 Months)")
            {
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(230, 232, 238)
            });
            chartBar.BackColor = Color.FromArgb(64, 64, 73);
        }

        private void LoadPieChart()
        {
            var data = _service.GetStatusBreakdown();

            chartPie.Series.Clear();
            chartPie.ChartAreas[0].BackColor = Color.FromArgb(64, 64, 73);

            var series = new Series("Status")
            {
                ChartType = SeriesChartType.Pie,
                Font = new Font("Segoe UI", 8.5F),
                LabelForeColor = Color.FromArgb(228, 228, 234)
            };

            Color[] colors = [
                Color.FromArgb(230, 126, 34),
                Color.FromArgb(243, 156, 18),
                Color.FromArgb(211, 84, 0),
                Color.FromArgb(192, 57, 43)
            ];

            int i = 0;
            foreach (var kvp in data)
            {
                int idx = series.Points.AddXY(kvp.Key, kvp.Value);
                series.Points[idx].Color = colors[i % colors.Length];
                series.Points[idx].LegendText = $"{kvp.Key} ({kvp.Value})";
                i++;
            }

            if (data.Count == 0)
                series.Points.AddXY("No Data", 1);

            series["PieLabelStyle"] = "Outside";
            chartPie.Series.Add(series);
            chartPie.Titles.Clear();
            chartPie.Titles.Add(new Title("Shipment Status Breakdown")
            {
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(230, 232, 238)
            });
            chartPie.Legends[0].Font = new Font("Segoe UI", 8.5F);
            chartPie.Legends[0].ForeColor = Color.FromArgb(225, 225, 232);
            chartPie.Legends[0].BackColor = Color.Transparent;
            chartPie.BackColor = Color.FromArgb(64, 64, 73);
        }
    }
}
