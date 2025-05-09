using DevExpress.XtraEditors;
using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        private readonly UsersManager _usersManager = new UsersManager(new EfUsersDal());
        FrmAdminMain frmAdminMain = new FrmAdminMain();
        FrmUserMain frmUserMain = new FrmUserMain();
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            labelControl1.Text = "Plus Stok Takip " + Application.ProductVersion;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtPassword.Properties.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.Properties.UseSystemPasswordChar = true;
            }
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            // Kullanıcı adı ve şifre doğrulama
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            var user = _usersManager.TGetAll().FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null)
            {
                switch (user.Role)
                {
                    case "Admin":
                        // Admin ekranını aç
                        frmAdminMain.Show();
                        break;
                    case "Kullanıcı":
                        // Kullanıcı ekranını aç
                        frmUserMain.Show();
                        break;
                }

                this.Hide(); // Login ekranını gizle
            }
            else
            {
                // Hatalı giriş
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}