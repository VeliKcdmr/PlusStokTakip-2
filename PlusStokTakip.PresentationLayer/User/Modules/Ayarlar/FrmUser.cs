using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Ayarlar
{
    public partial class FrmUser : DevExpress.XtraEditors.XtraForm
    {
        private readonly UsersManager _usersManager = new UsersManager(new EfUsersDal());
        public int _userId;

        public FrmUser()
        {
            InitializeComponent();
        }

        private void FrmUser_Load(object sender, EventArgs e)
        {
            var user = _usersManager.TGetAll().FirstOrDefault(x => x.UserID ==_userId);
            if (user != null)
            {
                txtUserName.Text = user.UserName;
                txtPassword.Text = user.Password;
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var user = _usersManager.TGetById(_userId);
                if (user != null)
                {                    
                    user.Password = txtPassword.Text;
                    _usersManager.TUpdate(user);
                    MessageBox.Show("Kullanıcı bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Kullanıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}