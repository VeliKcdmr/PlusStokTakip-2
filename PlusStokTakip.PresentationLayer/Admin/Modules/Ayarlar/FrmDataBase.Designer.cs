namespace PlusStokTakip.PresentationLayer.Admin.Modules.Ayarlar
{
    partial class FrmDataBase
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnUpdateSettings = new DevExpress.XtraEditors.SimpleButton();
            this.lblStatus = new DevExpress.XtraEditors.LabelControl();
            this.btnReadSettings = new DevExpress.XtraEditors.SimpleButton();
            this.lblStatus1 = new DevExpress.XtraEditors.LabelControl();
            this.txtServerName = new DevExpress.XtraEditors.TextEdit();
            this.txtDatabaseName = new DevExpress.XtraEditors.TextEdit();
            this.txtUsername = new DevExpress.XtraEditors.TextEdit();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.lblPassword = new DevExpress.XtraEditors.LabelControl();
            this.lblUsername = new DevExpress.XtraEditors.LabelControl();
            this.lblDatabaseName = new DevExpress.XtraEditors.LabelControl();
            this.lblServerName = new DevExpress.XtraEditors.LabelControl();
            this.lblAuthType = new DevExpress.XtraEditors.LabelControl();
            this.cmbAuthType = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatabaseName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAuthType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnCancel);
            this.groupControl1.Controls.Add(this.btnUpdateSettings);
            this.groupControl1.Controls.Add(this.lblStatus);
            this.groupControl1.Controls.Add(this.btnReadSettings);
            this.groupControl1.Controls.Add(this.lblStatus1);
            this.groupControl1.Controls.Add(this.txtServerName);
            this.groupControl1.Controls.Add(this.txtDatabaseName);
            this.groupControl1.Controls.Add(this.txtUsername);
            this.groupControl1.Controls.Add(this.txtPassword);
            this.groupControl1.Controls.Add(this.lblPassword);
            this.groupControl1.Controls.Add(this.lblUsername);
            this.groupControl1.Controls.Add(this.lblDatabaseName);
            this.groupControl1.Controls.Add(this.lblServerName);
            this.groupControl1.Controls.Add(this.lblAuthType);
            this.groupControl1.Controls.Add(this.cmbAuthType);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(531, 278);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Veritabanı Bağlantısı";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(261, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "İptal";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdateSettings
            // 
            this.btnUpdateSettings.Location = new System.Drawing.Point(162, 201);
            this.btnUpdateSettings.Name = "btnUpdateSettings";
            this.btnUpdateSettings.Size = new System.Drawing.Size(93, 23);
            this.btnUpdateSettings.TabIndex = 13;
            this.btnUpdateSettings.Text = "Ayarları Güncelle";
            this.btnUpdateSettings.Click += new System.EventHandler(this.btnUpdateSettings_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(53, 32);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(17, 13);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Null";
            // 
            // btnReadSettings
            // 
            this.btnReadSettings.Location = new System.Drawing.Point(63, 201);
            this.btnReadSettings.Name = "btnReadSettings";
            this.btnReadSettings.Size = new System.Drawing.Size(93, 23);
            this.btnReadSettings.TabIndex = 11;
            this.btnReadSettings.Text = "Ayarları Oku";
            // 
            // lblStatus1
            // 
            this.lblStatus1.Location = new System.Drawing.Point(12, 32);
            this.lblStatus1.Name = "lblStatus1";
            this.lblStatus1.Size = new System.Drawing.Size(35, 13);
            this.lblStatus1.TabIndex = 10;
            this.lblStatus1.Text = "Durum:";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(126, 97);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(226, 20);
            this.txtServerName.TabIndex = 8;
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.Location = new System.Drawing.Point(126, 123);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(226, 20);
            this.txtDatabaseName.TabIndex = 7;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(126, 149);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(226, 20);
            this.txtUsername.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(126, 175);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(226, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(94, 178);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(26, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Şifre:";
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(61, 152);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(59, 13);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Kullanıcı Adı:";
            // 
            // lblDatabaseName
            // 
            this.lblDatabaseName.Location = new System.Drawing.Point(50, 126);
            this.lblDatabaseName.Name = "lblDatabaseName";
            this.lblDatabaseName.Size = new System.Drawing.Size(70, 13);
            this.lblDatabaseName.TabIndex = 2;
            this.lblDatabaseName.Text = "Veritabanı Adı:";
            // 
            // lblServerName
            // 
            this.lblServerName.Location = new System.Drawing.Point(63, 100);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(57, 13);
            this.lblServerName.TabIndex = 1;
            this.lblServerName.Text = "Sunucu Adı:";
            // 
            // lblAuthType
            // 
            this.lblAuthType.Location = new System.Drawing.Point(12, 74);
            this.lblAuthType.Name = "lblAuthType";
            this.lblAuthType.Size = new System.Drawing.Size(108, 13);
            this.lblAuthType.TabIndex = 0;
            this.lblAuthType.Text = "Kimlik Doğrulama Türü:";
            // 
            // cmbAuthType
            // 
            this.cmbAuthType.Location = new System.Drawing.Point(126, 71);
            this.cmbAuthType.Name = "cmbAuthType";
            this.cmbAuthType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbAuthType.Size = new System.Drawing.Size(226, 20);
            this.cmbAuthType.TabIndex = 15;
            // 
            // FrmDataBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 278);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmDataBase";
            this.Text = "Veritabanı İşlemleri";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatabaseName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAuthType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl lblPassword;
        private DevExpress.XtraEditors.LabelControl lblUsername;
        private DevExpress.XtraEditors.LabelControl lblDatabaseName;
        private DevExpress.XtraEditors.LabelControl lblServerName;
        private DevExpress.XtraEditors.LabelControl lblAuthType;
        private DevExpress.XtraEditors.TextEdit txtServerName;
        private DevExpress.XtraEditors.TextEdit txtDatabaseName;
        private DevExpress.XtraEditors.TextEdit txtUsername;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.LabelControl lblStatus1;
        private DevExpress.XtraEditors.LabelControl lblStatus;
        private DevExpress.XtraEditors.SimpleButton btnReadSettings;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnUpdateSettings;
        private DevExpress.XtraEditors.ComboBoxEdit cmbAuthType;
    }
}