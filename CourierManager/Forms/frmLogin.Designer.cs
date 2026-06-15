namespace CourierManager.Forms
{
    partial class frmLogin
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
            pnlCenter = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnLogin = new Button();
            pnlCenter.SuspendLayout();
            SuspendLayout();

            // pnlCenter
            pnlCenter.BackColor = Color.White;
            pnlCenter.BorderStyle = BorderStyle.FixedSingle;
            pnlCenter.Controls.Add(lblTitle);
            pnlCenter.Controls.Add(lblSubtitle);
            pnlCenter.Controls.Add(lblUsername);
            pnlCenter.Controls.Add(txtUsername);
            pnlCenter.Controls.Add(lblPassword);
            pnlCenter.Controls.Add(txtPassword);
            pnlCenter.Controls.Add(btnLogin);
            pnlCenter.Location = new Point(125, 80);
            pnlCenter.Size = new Size(350, 340);

            // lblTitle
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(230, 126, 34);
            lblTitle.Location = new Point(20, 25);
            lblTitle.Size = new Size(310, 38);
            lblTitle.Text = "CourierManager";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblSubtitle
            lblSubtitle.Font = new Font("Segoe UI", 9F);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(20, 60);
            lblSubtitle.Size = new Size(310, 20);
            lblSubtitle.Text = "Delivery & Shipment System";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblUsername
            lblUsername.Font = new Font("Segoe UI", 9F);
            lblUsername.Location = new Point(30, 105);
            lblUsername.Size = new Size(290, 20);
            lblUsername.Text = "Username";

            // txtUsername
            txtUsername.Font = new Font("Segoe UI", 10F);
            txtUsername.Location = new Point(30, 128);
            txtUsername.Size = new Size(290, 28);
            txtUsername.Text = "admin";

            // lblPassword
            lblPassword.Font = new Font("Segoe UI", 9F);
            lblPassword.Location = new Point(30, 168);
            lblPassword.Size = new Size(290, 20);
            lblPassword.Text = "Password";

            // txtPassword
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.Location = new Point(30, 191);
            txtPassword.PasswordChar = '●';
            txtPassword.Size = new Size(290, 28);
            txtPassword.KeyDown += new KeyEventHandler(txtPassword_KeyDown);

            // btnLogin
            btnLogin.BackColor = Color.FromArgb(230, 126, 34);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(30, 248);
            btnLogin.Size = new Size(290, 42);
            btnLogin.Text = "Login";
            btnLogin.Click += new EventHandler(btnLogin_Click);

            // frmLogin
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 243);
            ClientSize = new Size(600, 500);
            Controls.Add(pnlCenter);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CourierManager — Login";

            pnlCenter.ResumeLayout(false);
            pnlCenter.PerformLayout();
            ResumeLayout(false);
        }

        private Panel pnlCenter;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
    }
}
