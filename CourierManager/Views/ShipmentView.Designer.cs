namespace CourierManager.Views
{
    partial class ShipmentView
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
            tsToolbar = new ToolStrip();
            tsbAdd = new ToolStripButton();
            tsbEdit = new ToolStripButton();
            tsbView = new ToolStripButton();
            tsbDelete = new ToolStripButton();
            sep1 = new ToolStripSeparator();
            tsbInTransit = new ToolStripButton();
            tsbDeliver = new ToolStripButton();
            tsbCancel = new ToolStripButton();
            sep2 = new ToolStripSeparator();
            tsbRefresh = new ToolStripButton();
            pnlTop = new Panel();
            lblSearchHint = new Label();
            txtSearch = new TextBox();
            lblStatusFilter = new Label();
            cmbStatusFilter = new ComboBox();
            lblCount = new Label();
            dgv = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colCustomer = new DataGridViewTextBoxColumn();
            colDriver = new DataGridViewTextBoxColumn();
            colTracking = new DataGridViewTextBoxColumn();
            colDest = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colWeight = new DataGridViewTextBoxColumn();
            colCost = new DataGridViewTextBoxColumn();
            colPickup = new DataGridViewTextBoxColumn();

            tsToolbar.SuspendLayout();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();

            // tsToolbar
            tsToolbar.BackColor = Color.FromArgb(52, 52, 60);
            tsToolbar.GripStyle = ToolStripGripStyle.Hidden;
            tsToolbar.Items.AddRange([tsbAdd, tsbEdit, tsbView, tsbDelete, sep1,
                tsbInTransit, tsbDeliver, tsbCancel, sep2, tsbRefresh]);
            tsToolbar.Padding = new Padding(5, 2, 0, 2);
            tsToolbar.Size = new Size(900, 36);

            SetTsb(tsbAdd, "Add Shipment", Color.FromArgb(27, 140, 60));
            SetTsb(tsbEdit, "Edit", Color.FromArgb(180, 120, 0));
            SetTsb(tsbView, "View", Color.FromArgb(60, 60, 60));
            SetTsb(tsbDelete, "Delete", Color.FromArgb(180, 40, 40));
            SetTsb(tsbInTransit, "Mark In Transit", Color.FromArgb(0, 100, 160));
            SetTsb(tsbDeliver, "Mark Delivered", Color.FromArgb(27, 140, 60));
            SetTsb(tsbCancel, "Cancel Shipment", Color.FromArgb(140, 50, 50));
            SetTsb(tsbRefresh, "Refresh", Color.FromArgb(230, 126, 34));

            tsbAdd.Click += new EventHandler(tsbAdd_Click);
            tsbEdit.Click += new EventHandler(tsbEdit_Click);
            tsbView.Click += new EventHandler(tsbView_Click);
            tsbDelete.Click += new EventHandler(tsbDelete_Click);
            tsbInTransit.Click += new EventHandler(tsbInTransit_Click);
            tsbDeliver.Click += new EventHandler(tsbDeliver_Click);
            tsbCancel.Click += new EventHandler(tsbCancel_Click);
            tsbRefresh.Click += new EventHandler(tsbRefresh_Click);

            // pnlTop
            pnlTop.BackColor = Color.FromArgb(52, 52, 60);
            pnlTop.Controls.Add(lblSearchHint);
            pnlTop.Controls.Add(txtSearch);
            pnlTop.Controls.Add(lblStatusFilter);
            pnlTop.Controls.Add(cmbStatusFilter);
            pnlTop.Controls.Add(lblCount);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 44;
            pnlTop.Location = new Point(0, 36);

            lblSearchHint.Font = new Font("Segoe UI", 9F);
            lblSearchHint.ForeColor = Color.FromArgb(180, 183, 193);
            lblSearchHint.Location = new Point(12, 12);
            lblSearchHint.Size = new Size(55, 20);
            lblSearchHint.Text = "Search:";

            txtSearch.Font = new Font("Segoe UI", 9.5F);
            txtSearch.Location = new Point(70, 10);
            txtSearch.Size = new Size(200, 24);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);

            lblStatusFilter.Font = new Font("Segoe UI", 9F);
            lblStatusFilter.ForeColor = Color.FromArgb(180, 183, 193);
            lblStatusFilter.Location = new Point(285, 12);
            lblStatusFilter.Size = new Size(45, 20);
            lblStatusFilter.Text = "Status:";

            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusFilter.Font = new Font("Segoe UI", 9F);
            cmbStatusFilter.Items.AddRange(["All", "Pending", "In Transit", "Delivered", "Cancelled"]);
            cmbStatusFilter.Location = new Point(334, 9);
            cmbStatusFilter.Size = new Size(130, 24);
            cmbStatusFilter.SelectedIndex = 0;
            cmbStatusFilter.SelectedIndexChanged += new EventHandler(cmbStatusFilter_SelectedIndexChanged);

            lblCount.Dock = DockStyle.Right;
            lblCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCount.ForeColor = Color.FromArgb(230, 126, 34);
            lblCount.Padding = new Padding(0, 0, 12, 0);
            lblCount.Size = new Size(140, 44);
            lblCount.TextAlign = ContentAlignment.MiddleRight;

            // dgv
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.BackgroundColor = Color.FromArgb(64, 64, 73);
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange([colId, colCustomer, colDriver, colTracking, colDest, colStatus, colWeight, colCost, colPickup]);
            dgv.Dock = DockStyle.Fill;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 33, 40);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 126, 34);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(64, 64, 73);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(228, 228, 234);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(250, 224, 200);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.GridColor = Color.FromArgb(82, 82, 92);
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            SetCol(colId, "Id", "Id", 45);
            SetCol(colCustomer, "Customer", "CustomerName", 150);
            SetCol(colDriver, "Driver", "DriverName", 130);
            SetCol(colTracking, "Tracking No", "TrackingNo", 130);
            SetCol(colDest, "Destination", "Destination", 130);
            SetCol(colStatus, "Status", "Status", 100);
            SetCol(colWeight, "Weight (kg)", "WeightKg", 90);
            SetCol(colCost, "Cost (Rs)", "Cost", 90);
            SetCol(colPickup, "Pickup Date", "PickupDate", 110);
            colWeight.DefaultCellStyle.Format = "N2";
            colCost.DefaultCellStyle.Format = "N0";
            colPickup.DefaultCellStyle.Format = "dd MMM yyyy";

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgv);
            Controls.Add(pnlTop);
            Controls.Add(tsToolbar);
            Name = "ShipmentView";
            Size = new Size(900, 600);

            tsToolbar.ResumeLayout(false);
            tsToolbar.PerformLayout();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private static void SetTsb(ToolStripButton btn, string text, Color color)
        {
            btn.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.ForeColor = color;
            btn.Text = text;
            btn.Margin = new Padding(2, 0, 2, 0);
        }

        private static void SetCol(DataGridViewTextBoxColumn col, string header, string member, int width)
        {
            col.DataPropertyName = member;
            col.HeaderText = header;
            col.ReadOnly = true;
            col.Width = width;
        }

        private ToolStrip tsToolbar;
        private ToolStripButton tsbAdd, tsbEdit, tsbView, tsbDelete;
        private ToolStripButton tsbInTransit, tsbDeliver, tsbCancel, tsbRefresh;
        private ToolStripSeparator sep1, sep2;
        private Panel pnlTop;
        private Label lblSearchHint, lblStatusFilter, lblCount;
        private TextBox txtSearch;
        private ComboBox cmbStatusFilter;
        private DataGridView dgv;
        private DataGridViewTextBoxColumn colId, colCustomer, colDriver, colTracking, colDest, colStatus, colWeight, colCost, colPickup;
    }
}
