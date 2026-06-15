namespace CourierManager.Views
{
    partial class PaymentView
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
            tsbPaid = new ToolStripButton();
            tsbUnpaid = new ToolStripButton();
            sep1 = new ToolStripSeparator();
            tsbDelete = new ToolStripButton();
            sep2 = new ToolStripSeparator();
            tsbRefresh = new ToolStripButton();
            pnlTop = new Panel();
            lblSearchHint = new Label();
            txtSearch = new TextBox();
            lblCount = new Label();
            dgv = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colCustomer = new DataGridViewTextBoxColumn();
            colTracking = new DataGridViewTextBoxColumn();
            colAmount = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colDate = new DataGridViewTextBoxColumn();

            tsToolbar.SuspendLayout();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();

            tsToolbar.BackColor = Color.FromArgb(52, 52, 60);
            tsToolbar.GripStyle = ToolStripGripStyle.Hidden;
            tsToolbar.Items.AddRange([tsbPaid, tsbUnpaid, sep1, tsbDelete, sep2, tsbRefresh]);
            tsToolbar.Padding = new Padding(5, 2, 0, 2);
            tsToolbar.Size = new Size(900, 36);

            SetTsb(tsbPaid, "Mark Paid", Color.FromArgb(27, 140, 60));
            SetTsb(tsbUnpaid, "Mark Unpaid", Color.FromArgb(180, 120, 0));
            SetTsb(tsbDelete, "Delete", Color.FromArgb(180, 40, 40));
            SetTsb(tsbRefresh, "Refresh", Color.FromArgb(230, 126, 34));

            tsbPaid.Click += new EventHandler(tsbPaid_Click);
            tsbUnpaid.Click += new EventHandler(tsbUnpaid_Click);
            tsbDelete.Click += new EventHandler(tsbDelete_Click);
            tsbRefresh.Click += new EventHandler(tsbRefresh_Click);

            pnlTop.BackColor = Color.FromArgb(52, 52, 60);
            pnlTop.Controls.Add(lblSearchHint);
            pnlTop.Controls.Add(txtSearch);
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
            txtSearch.Size = new Size(250, 24);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);

            lblCount.Dock = DockStyle.Right;
            lblCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCount.ForeColor = Color.FromArgb(230, 126, 34);
            lblCount.Padding = new Padding(0, 0, 12, 0);
            lblCount.Size = new Size(140, 44);
            lblCount.TextAlign = ContentAlignment.MiddleRight;

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.BackgroundColor = Color.FromArgb(64, 64, 73);
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Columns.AddRange([colId, colCustomer, colTracking, colAmount, colStatus, colDate]);
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

            SetCol(colId, "Id", "Id", 50);
            SetCol(colCustomer, "Customer", "CustomerName", 180);
            SetCol(colTracking, "Tracking No", "TrackingNo", 140);
            SetCol(colAmount, "Amount (Rs)", "Amount", 110);
            SetCol(colStatus, "Status", "Status", 100);
            SetCol(colDate, "Payment Date", "PaymentDate", 130);
            colAmount.DefaultCellStyle.Format = "N0";
            colDate.DefaultCellStyle.Format = "dd MMM yyyy";

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgv);
            Controls.Add(pnlTop);
            Controls.Add(tsToolbar);
            Name = "PaymentView";
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
        private ToolStripButton tsbPaid, tsbUnpaid, tsbDelete, tsbRefresh;
        private ToolStripSeparator sep1, sep2;
        private Panel pnlTop;
        private Label lblSearchHint, lblCount;
        private TextBox txtSearch;
        private DataGridView dgv;
        private DataGridViewTextBoxColumn colId, colCustomer, colTracking, colAmount, colStatus, colDate;
    }
}
