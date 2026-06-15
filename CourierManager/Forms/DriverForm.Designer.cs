namespace CourierManager.Forms
{
    partial class DriverForm
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
            lblName = new Label();
            txtName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblVehicleType = new Label();
            cmbVehicleType = new ComboBox();
            lblVehicleNo = new Label();
            txtVehicleNo = new TextBox();
            lblCity = new Label();
            txtCity = new TextBox();
            chkActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();

            int lx = 20, tx = 140, w = 250, lh = 22, th = 26, gap = 44;
            int y = 20;

            SetLabel(lblName, "Name *", lx, y, lh);
            SetText(txtName, tx, y, w, th);
            y += gap;

            SetLabel(lblPhone, "Phone *", lx, y, lh);
            SetText(txtPhone, tx, y, w, th);
            y += gap;

            SetLabel(lblVehicleType, "Vehicle Type *", lx, y, lh);
            cmbVehicleType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVehicleType.Font = new Font("Segoe UI", 9.5F);
            cmbVehicleType.Items.AddRange(["Bike", "Car", "Van", "Truck"]);
            cmbVehicleType.Location = new Point(tx, y);
            cmbVehicleType.Size = new Size(w, th);
            cmbVehicleType.SelectedIndex = 0;
            y += gap;

            SetLabel(lblVehicleNo, "Vehicle No *", lx, y, lh);
            SetText(txtVehicleNo, tx, y, w, th);
            y += gap;

            SetLabel(lblCity, "City *", lx, y, lh);
            SetText(txtCity, tx, y, w, th);
            y += gap;

            chkActive.Font = new Font("Segoe UI", 9.5F);
            chkActive.Location = new Point(tx, y);
            chkActive.Size = new Size(200, 24);
            chkActive.Text = "Active (available for delivery)";
            chkActive.Checked = true;
            y += gap + 6;

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
            ClientSize = new Size(410, y + 70);
            Controls.Add(lblName); Controls.Add(txtName);
            Controls.Add(lblPhone); Controls.Add(txtPhone);
            Controls.Add(lblVehicleType); Controls.Add(cmbVehicleType);
            Controls.Add(lblVehicleNo); Controls.Add(txtVehicleNo);
            Controls.Add(lblCity); Controls.Add(txtCity);
            Controls.Add(chkActive);
            Controls.Add(btnSave); Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DriverForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Driver";
            ResumeLayout(false);
            PerformLayout();
        }

        private static void SetLabel(Label lbl, string text, int x, int y, int h)
        {
            lbl.Font = new Font("Segoe UI", 9F);
            lbl.ForeColor = Color.FromArgb(60, 60, 60);
            lbl.Location = new Point(x, y + 3);
            lbl.Size = new Size(115, h);
            lbl.Text = text;
        }

        private static void SetText(TextBox txt, int x, int y, int w, int h)
        {
            txt.Font = new Font("Segoe UI", 9.5F);
            txt.Location = new Point(x, y);
            txt.Size = new Size(w, h);
        }

        private Label lblName, lblPhone, lblVehicleType, lblVehicleNo, lblCity;
        private TextBox txtName, txtPhone, txtVehicleNo, txtCity;
        private ComboBox cmbVehicleType;
        private CheckBox chkActive;
        private Button btnSave, btnCancel;
    }
}
