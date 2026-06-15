using System.Windows.Forms.DataVisualization.Charting;

namespace CourierManager.Views
{
    partial class DashboardView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ChartArea barArea = new("ChartArea1");
            ChartArea pieArea = new("ChartArea1");
            Legend pieLegend = new("Default");

            pnlStats = new Panel();
            pnlTotal = new Panel();
            lblTotalTitle = new Label();
            lblTotalValue = new Label();
            pnlMonth = new Panel();
            lblMonthTitle = new Label();
            lblMonthValue = new Label();
            pnlWeek = new Panel();
            lblWeekTitle = new Label();
            lblWeekValue = new Label();
            pnlToday = new Panel();
            lblTodayTitle = new Label();
            lblTodayValue = new Label();
            pnlCharts = new Panel();
            chartBar = new Chart();
            chartPie = new Chart();

            pnlStats.SuspendLayout();
            pnlTotal.SuspendLayout();
            pnlMonth.SuspendLayout();
            pnlWeek.SuspendLayout();
            pnlToday.SuspendLayout();
            pnlCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartPie).BeginInit();
            SuspendLayout();

            // pnlStats
            pnlStats.BackColor = Color.FromArgb(52, 52, 60);
            pnlStats.Controls.Add(pnlTotal);
            pnlStats.Controls.Add(pnlMonth);
            pnlStats.Controls.Add(pnlWeek);
            pnlStats.Controls.Add(pnlToday);
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Height = 120;
            pnlStats.Padding = new Padding(10, 12, 10, 12);

            BuildStatCard(pnlTotal, lblTotalTitle, lblTotalValue, "Total Shipments", Color.FromArgb(230, 126, 34), 0);
            BuildStatCard(pnlMonth, lblMonthTitle, lblMonthValue, "This Month", Color.FromArgb(211, 84, 0), 1);
            BuildStatCard(pnlWeek, lblWeekTitle, lblWeekValue, "This Week", Color.FromArgb(243, 156, 18), 2);
            BuildStatCard(pnlToday, lblTodayTitle, lblTodayValue, "Today", Color.FromArgb(192, 57, 43), 3);

            // pnlCharts
            pnlCharts.BackColor = Color.FromArgb(52, 52, 60);
            pnlCharts.Controls.Add(chartBar);
            pnlCharts.Controls.Add(chartPie);
            pnlCharts.Dock = DockStyle.Fill;
            pnlCharts.AutoScroll = true;

            // chartBar
            barArea.BackColor = Color.FromArgb(64, 64, 73);
            chartBar.ChartAreas.Add(barArea);
            chartBar.BackColor = Color.FromArgb(64, 64, 73);
            chartBar.BorderlineColor = Color.FromArgb(82, 82, 92);
            chartBar.BorderlineDashStyle = ChartDashStyle.Solid;
            chartBar.BorderlineWidth = 1;
            chartBar.Location = new Point(12, 12);
            chartBar.Size = new Size(560, 420);

            // chartPie
            pieArea.BackColor = Color.FromArgb(64, 64, 73);
            chartPie.ChartAreas.Add(pieArea);
            chartPie.Legends.Add(pieLegend);
            chartPie.BackColor = Color.FromArgb(64, 64, 73);
            chartPie.BorderlineColor = Color.FromArgb(82, 82, 92);
            chartPie.BorderlineDashStyle = ChartDashStyle.Solid;
            chartPie.BorderlineWidth = 1;
            chartPie.Location = new Point(584, 12);
            chartPie.Size = new Size(400, 420);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(52, 52, 60);
            Controls.Add(pnlCharts);
            Controls.Add(pnlStats);
            Name = "DashboardView";
            Size = new Size(960, 560);

            pnlStats.ResumeLayout(false);
            pnlTotal.ResumeLayout(false);
            pnlMonth.ResumeLayout(false);
            pnlWeek.ResumeLayout(false);
            pnlToday.ResumeLayout(false);
            pnlCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartPie).EndInit();
            ResumeLayout(false);
        }

        private static void BuildStatCard(Panel card, Label title, Label value, string titleText, Color accent, int index)
        {
            card.BackColor = Color.FromArgb(64, 64, 73);
            card.BorderStyle = BorderStyle.FixedSingle;
            int cardW = 195, gap = 10, startX = 10 + index * (cardW + gap);
            card.Location = new Point(startX, 12);
            card.Size = new Size(cardW, 88);

            title.Font = new Font("Segoe UI", 8.5F);
            title.ForeColor = Color.FromArgb(180, 183, 193);
            title.Location = new Point(12, 12);
            title.Size = new Size(170, 20);
            title.Text = titleText;

            value.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            value.ForeColor = accent;
            value.Location = new Point(10, 32);
            value.Size = new Size(170, 46);
            value.Text = "—";
            value.TextAlign = ContentAlignment.MiddleLeft;

            card.Controls.Add(title);
            card.Controls.Add(value);
        }

        private Panel pnlStats, pnlTotal, pnlMonth, pnlWeek, pnlToday, pnlCharts;
        private Label lblTotalTitle, lblTotalValue;
        private Label lblMonthTitle, lblMonthValue;
        private Label lblWeekTitle, lblWeekValue;
        private Label lblTodayTitle, lblTodayValue;
        private Chart chartBar, chartPie;
    }
}
