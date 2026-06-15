namespace CourierManager.Forms
{
    partial class MainForm
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
            pnlSidebar = new Panel();
            lblAppName = new Label();
            lblAppSub = new Label();
            btnDashboard = new Button();
            btnCustomers = new Button();
            btnShipments = new Button();
            btnDrivers = new Button();
            btnPayments = new Button();
            pnlContent = new Panel();
            pnlStatusBar = new Panel();
            lblStatus = new Label();

            pnlSidebar.SuspendLayout();
            pnlStatusBar.SuspendLayout();
            SuspendLayout();

            // pnlSidebar
            pnlSidebar.BackColor = Color.FromArgb(33, 33, 40);
            pnlSidebar.Controls.Add(lblAppName);
            pnlSidebar.Controls.Add(lblAppSub);
            pnlSidebar.Controls.Add(btnDashboard);
            pnlSidebar.Controls.Add(btnCustomers);
            pnlSidebar.Controls.Add(btnShipments);
            pnlSidebar.Controls.Add(btnDrivers);
            pnlSidebar.Controls.Add(btnPayments);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 190;

            // lblAppName
            lblAppName.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblAppName.ForeColor = Color.White;
            lblAppName.Location = new Point(0, 20);
            lblAppName.Size = new Size(190, 30);
            lblAppName.Text = "CourierManager";
            lblAppName.TextAlign = ContentAlignment.MiddleCenter;

            // lblAppSub
            lblAppSub.Font = new Font("Segoe UI", 8F);
            lblAppSub.ForeColor = Color.FromArgb(235, 175, 120);
            lblAppSub.Location = new Point(0, 50);
            lblAppSub.Size = new Size(190, 20);
            lblAppSub.Text = "Delivery & Shipment System";
            lblAppSub.TextAlign = ContentAlignment.MiddleCenter;

            // NavButton helper setup
            SetupNavButton(btnDashboard, "  Dashboard", 100);
            SetupNavButton(btnCustomers, "  Customers", 152);
            SetupNavButton(btnShipments, "  Shipments", 204);
            SetupNavButton(btnDrivers, "  Drivers", 256);
            SetupNavButton(btnPayments, "  Payments", 308);

            btnDashboard.Click += new EventHandler(btnDashboard_Click);
            btnCustomers.Click += new EventHandler(btnCustomers_Click);
            btnShipments.Click += new EventHandler(btnShipments_Click);
            btnDrivers.Click += new EventHandler(btnDrivers_Click);
            btnPayments.Click += new EventHandler(btnPayments_Click);

            // pnlContent
            pnlContent.BackColor = Color.FromArgb(52, 52, 60);
            pnlContent.Dock = DockStyle.Fill;

            // pnlStatusBar
            pnlStatusBar.BackColor = Color.FromArgb(38, 38, 46);
            pnlStatusBar.Controls.Add(lblStatus);
            pnlStatusBar.Dock = DockStyle.Bottom;
            pnlStatusBar.Height = 28;

            // lblStatus
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Font = new Font("Segoe UI", 8.5F);
            lblStatus.ForeColor = Color.FromArgb(235, 235, 240);
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;

            // MainForm
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 680);
            Controls.Add(pnlContent);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlStatusBar);
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CourierManager — Delivery & Shipment System";

            pnlSidebar.ResumeLayout(false);
            pnlStatusBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        private static void SetupNavButton(Button btn, string text, int top)
        {
            btn.BackColor = Color.FromArgb(33, 33, 40);
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 126, 34);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 10F);
            btn.ForeColor = Color.White;
            btn.Location = new Point(0, top);
            btn.Size = new Size(190, 46);
            btn.Text = text;
            btn.TextAlign = ContentAlignment.MiddleLeft;
        }

        private Panel pnlSidebar;
        private Label lblAppName;
        private Label lblAppSub;
        private Button btnDashboard;
        private Button btnCustomers;
        private Button btnShipments;
        private Button btnDrivers;
        private Button btnPayments;
        private Panel pnlContent;
        private Panel pnlStatusBar;
        private Label lblStatus;
    }
}
