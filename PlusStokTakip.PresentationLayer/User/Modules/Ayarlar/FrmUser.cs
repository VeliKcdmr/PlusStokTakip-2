using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
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

        private void FrmUser_Load(object sender, System.EventArgs e)
        {
            LoadUserData();
        }
        private void LoadUserData()
        {
            var user = _usersManager.TGetById(_userId);
            txtUserName.Text = user.UserName;
            txtRol.Text = user.Role;
            txtPassword.Text = user.Password;
        }
        private void btnKaydet_Click(object sender, System.EventArgs e)
        {
            var user = _usersManager.TGetById(_userId);
            user.Password = txtPassword.Text;
            _usersManager.TUpdate(user);
            MessageBox.Show("Kullanıcı şifresi güncellendi.");
            LoadUserData();
        }
    }
}