namespace CourierManager.Forms
{
    partial class CustomerForm
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
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            lblCity = new Label();
            txtCity = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();

            int lx = 20, tx = 130, w = 260, lh = 22, th = 26, gap = 44;
            int y = 20;

            SetLabel(lblName, "Name *", lx, y, lh);
            SetText(txtName, tx, y, w, th);
            y += gap;
            SetLabel(lblPhone, "Phone *", lx, y, lh);
            SetText(txtPhone, tx, y, w, th);
            y += gap;
            SetLabel(lblEmail, "Email", lx, y, lh);
            SetText(txtEmail, tx, y, w, th);
            y += gap;
            SetLabel(lblAddress, "Address *", lx, y, lh);
            SetText(txtAddress, tx, y, w, th);
            y += gap;
            SetLabel(lblCity, "City *", lx, y, lh);
            SetText(txtCity, tx, y, w, th);
            y += gap + 10;

            // btnSave
            btnSave.BackColor = Color.FromArgb(230, 126, 34);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(tx, y);
            btnSave.Size = new Size(120, 34);
            btnSave.Text = "Save";
            btnSave.Click += new EventHandler(btnSave_Click);

            // btnCancel
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
            ClientSize = new Size(420, y + 70);
            Controls.Add(lblName); Controls.Add(txtName);
            Controls.Add(lblPhone); Controls.Add(txtPhone);
            Controls.Add(lblEmail); Controls.Add(txtEmail);
            Controls.Add(lblAddress); Controls.Add(txtAddress);
            Controls.Add(lblCity); Controls.Add(txtCity);
            Controls.Add(btnSave); Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CustomerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Customer";
            ResumeLayout(false);
            PerformLayout();
        }

        private static void SetLabel(Label lbl, string text, int x, int y, int h)
        {
            lbl.Font = new Font("Segoe UI", 9F);
            lbl.ForeColor = Color.FromArgb(60, 60, 60);
            lbl.Location = new Point(x, y + 3);
            lbl.Size = new Size(105, h);
            lbl.Text = text;
        }

        private static void SetText(TextBox txt, int x, int y, int w, int h)
        {
            txt.Font = new Font("Segoe UI", 9.5F);
            txt.Location = new Point(x, y);
            txt.Size = new Size(w, h);
        }

        private Label lblName, lblPhone, lblEmail, lblAddress, lblCity;
        private TextBox txtName, txtPhone, txtEmail, txtAddress, txtCity;
        private Button btnSave, btnCancel;
    }
}
