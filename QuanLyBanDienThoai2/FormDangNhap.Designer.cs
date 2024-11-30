namespace QuanLyBanDienThoai2
{
    partial class FormDangNhap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label3 = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnCancel = new Button();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Book Antiqua", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(355, 86);
            label1.Name = "label1";
            label1.Size = new Size(72, 28);
            label1.TabIndex = 0;
            label1.Text = "Login";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Emoji", 10.2F);
            label3.Location = new Point(134, 223);
            label3.Name = "label3";
            label3.Size = new Size(81, 22);
            label3.TabIndex = 0;
            label3.Text = "Password";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(280, 150);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(125, 27);
            txtUsername.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(280, 223);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(125, 27);
            txtPassword.TabIndex = 2;
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("Segoe UI Emoji", 10.2F);
            btnLogin.Location = new Point(135, 292);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(94, 43);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI Emoji", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(337, 292);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 43);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Emoji", 10.2F);
            label2.Location = new Point(134, 157);
            label2.Name = "label2";
            label2.Size = new Size(87, 22);
            label2.TabIndex = 0;
            label2.Text = "Username";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.avatar_trang_1_cd729c335b;
            pictureBox1.Location = new Point(483, 150);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(125, 124);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.z6072823880692_c26cb436fab9a95ca1d710bbb75d56d7;
            pictureBox2.Location = new Point(-2, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(111, 62);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(665, 430);
            label4.Name = "label4";
            label4.Size = new Size(179, 20);
            label4.TabIndex = 7;
            label4.Text = "Design by Phan Van Hung";
            // 
            // FormDangNhap
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 192, 255);
            ClientSize = new Size(846, 450);
            Controls.Add(label4);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(btnCancel);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label1);
            Name = "FormDangNhap";
            Text = "FormDangNhap";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label3;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnCancel;
        private Label label2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label label4;
    }
}