namespace CourierManager.Forms
{
    partial class ShipmentForm
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
            lblCustomer = new Label();
            cmbCustomer = new ComboBox();
            lblDriver = new Label();
            cmbDriver = new ComboBox();
            lblTracking = new Label();
            txtTrackingNo = new TextBox();
            lblOrigin = new Label();
            txtOrigin = new TextBox();
            lblDestination = new Label();
            txtDestination = new TextBox();
            lblWeight = new Label();
            numWeight = new NumericUpDown();
            lblCost = new Label();
            numCost = new NumericUpDown();
            lblPickup = new Label();
            dtpPickup = new DateTimePicker();
            lblStatus = new Label();
            cmbStatus = new ComboBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)numWeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCost).BeginInit();
            SuspendLayout();

            int lx = 20, tx = 145, w = 270, lh = 22, th = 26, gap = 44;
            int y = 20;

            SetLabel(lblCustomer, "Customer *", lx, y, lh);
            cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCustomer.Font = new Font("Segoe UI", 9.5F);
            cmbCustomer.Location = new Point(tx, y);
            cmbCustomer.Size = new Size(w, th);
            y += gap;

            SetLabel(lblDriver, "Assign Driver", lx, y, lh);
            cmbDriver.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDriver.Font = new Font("Segoe UI", 9.5F);
            cmbDriver.Location = new Point(tx, y);
            cmbDriver.Size = new Size(w, th);
            y += gap;

            SetLabel(lblTracking, "Tracking No", lx, y, lh);
            SetText(txtTrackingNo, tx, y, w, th);
            txtTrackingNo.ReadOnly = true;
            txtTrackingNo.BackColor = Color.FromArgb(245, 245, 245);
            y += gap;

            SetLabel(lblOrigin, "Origin *", lx, y, lh);
            SetText(txtOrigin, tx, y, w, th);
            y += gap;

            SetLabel(lblDestination, "Destination *", lx, y, lh);
            SetText(txtDestination, tx, y, w, th);
            y += gap;

            SetLabel(lblWeight, "Weight (kg) *", lx, y, lh);
            numWeight.DecimalPlaces = 2;
            numWeight.Font = new Font("Segoe UI", 9.5F);
            numWeight.Location = new Point(tx, y);
            numWeight.Maximum = 1000;
            numWeight.Minimum = 0;
            numWeight.Size = new Size(130, th);
            y += gap;

            SetLabel(lblCost, "Cost (Rs) *", lx, y, lh);
            numCost.DecimalPlaces = 0;
            numCost.Font = new Font("Segoe UI", 9.5F);
            numCost.Location = new Point(tx, y);
            numCost.Maximum = 999999;
            numCost.Minimum = 0;
            numCost.Size = new Size(130, th);
            y += gap;

            SetLabel(lblPickup, "Pickup Date *", lx, y, lh);
            dtpPickup.Font = new Font("Segoe UI", 9.5F);
            dtpPickup.Format = DateTimePickerFormat.Short;
            dtpPickup.Location = new Point(tx, y);
            dtpPickup.Size = new Size(w, th);
            y += gap;

            SetLabel(lblStatus, "Status", lx, y, lh);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Font = new Font("Segoe UI", 9.5F);
            cmbStatus.Items.AddRange(["Pending", "In Transit", "Delivered", "Cancelled"]);
            cmbStatus.Location = new Point(tx, y);
            cmbStatus.Size = new Size(w, th);
            cmbStatus.SelectedIndex = 0;
            y += gap;

            SetLabel(lblNotes, "Notes", lx, y, lh);
            txtNotes.Font = new Font("Segoe UI", 9.5F);
            txtNotes.Location = new Point(tx, y);
            txtNotes.Multiline = true;
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(w, 64);
            y += 80;

            btnSave.BackColor = Color.FromArgb(230, 126, 34);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(tx, y);
            btnSave.Size = new Size(120, 34);
            btnSave.Text = "Save";
            btnSave.Click += new EventHandler(btnSave_Click);

            btnCancel.BackColor = Color.FromArgb(200, 210, 220);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9.5F);
            btnCancel.Location = new Point(tx + 130, y);
            btnCancel.Size = new Size(120, 34);
            btnCancel.Text = "Cancel";
            btnCancel.Click += new EventHandler(btnCancel_Click);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(450, y + 70);
            Controls.Add(lblCustomer); Controls.Add(cmbCustomer);
            Controls.Add(lblDriver); Controls.Add(cmbDriver);
            Controls.Add(lblTracking); Controls.Add(txtTrackingNo);
            Controls.Add(lblOrigin); Controls.Add(txtOrigin);
            Controls.Add(lblDestination); Controls.Add(txtDestination);
            Controls.Add(lblWeight); Controls.Add(numWeight);
            Controls.Add(lblCost); Controls.Add(numCost);
            Controls.Add(lblPickup); Controls.Add(dtpPickup);
            Controls.Add(lblStatus); Controls.Add(cmbStatus);
            Controls.Add(lblNotes); Controls.Add(txtNotes);
            Controls.Add(btnSave); Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ShipmentForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Shipment";

            ((System.ComponentModel.ISupportInitialize)numWeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCost).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private static void SetLabel(Label lbl, string text, int x, int y, int h)
        {
            lbl.Font = new Font("Segoe UI", 9F);
            lbl.ForeColor = Color.FromArgb(60, 60, 60);
            lbl.Location = new Point(x, y + 3);
            lbl.Size = new Size(120, h);
            lbl.Text = text;
        }

        private static void SetText(TextBox txt, int x, int y, int w, int h)
        {
            txt.Font = new Font("Segoe UI", 9.5F);
            txt.Location = new Point(x, y);
            txt.Size = new Size(w, h);
        }

        private Label lblCustomer, lblDriver, lblTracking, lblOrigin, lblDestination;
        private Label lblWeight, lblCost, lblPickup, lblStatus, lblNotes;
        private ComboBox cmbCustomer, cmbDriver, cmbStatus;
        private TextBox txtTrackingNo, txtOrigin, txtDestination, txtNotes;
        private NumericUpDown numWeight, numCost;
        private DateTimePicker dtpPickup;
        private Button btnSave, btnCancel;
    }
}
