using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.Admin.Modules.Ayarlar
{
    public partial class FrmUser : DevExpress.XtraEditors.XtraForm
    {
        private readonly UsersManager _usersManager = new UsersManager(new EfUsersDal());
        public FrmUser()
        {
            InitializeComponent();
        }

        private void userList()
        {
            var userList = _usersManager.TGetAll().Select(u => new
            {
                u.UserID,
                u.UserName,
                Password = "*********",
                u.Role,
                IsActive = u.IsActive ? "Aktif" : "Pasif"
            }).ToList();
            gridControl1.DataSource = userList;
            gridView1.Columns["UserID"].Caption = "Kullanıcı ID";
            gridView1.Columns["UserName"].Caption = "Kullanıcı Adı";
            gridView1.Columns["Password"].Caption = "Şifre";
            gridView1.Columns["Role"].Caption = "Rol";
            gridView1.Columns["IsActive"].Caption = "Durum";
        }

        private void RolList()
        {
            var roles = new[]
            {
                new {RolID=0,RoleName = "Rol Seçiniz" },
                new {RolID=1,RoleName = "Admin" },
                new {RolID=2,RoleName = "Kullanıcı" }
            };
            CmbRol.Properties.DataSource = roles;
            CmbRol.Properties.DisplayMember = "RoleName";
            CmbRol.Properties.ValueMember = "RolID";
            CmbRol.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("RoleName", "Rol Adı"));
            CmbRol.EditValue = 0;
        }


        private void FrmUser_Load(object sender, EventArgs e)
        {
            gridView1.RefreshData();
            userList();
            CleanFields();
            RolList();
        }
        private void CleanFields()
        {
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            CmbRol.EditValue = null;
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
        }

        private void btnKaydet_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validasyon kontrolleri
                if (!ValidationHelper.ValidateControl(txtUserName, "Kullanıcı adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtPassword, "Şifre boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(CmbRol, "Lütfen kullanıcı rolü seçiniz!")) return;

                // Aynı kategoride aynı isim kontrolü
                var existingUser = _usersManager.TGetAll().FirstOrDefault(u =>
                    u.UserName.Equals(txtUserName.Text.Trim(), StringComparison.OrdinalIgnoreCase) && u.IsActive);

                if (existingUser != null)
                {
                    MessageBox.Show($"'{txtUserName.Text.Trim()}' Kullanıcı adı mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Yeni marka ekleme
                Users newUser = new Users
                {
                    UserName = txtUserName.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                    Role = CmbRol.GetColumnValue("RoleName").ToString(),
                    IsActive = true
                };

                _usersManager.TInsert(newUser);
                MessageBox.Show("Kullanıcı başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Form yenileme
                userList();
                gridView1.RefreshData();
                CleanFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtUserName, "Kullanıcı adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtPassword, "Şifre boş bırakılamaz!")) return;

                var selectedUserId = (int)gridView1.GetFocusedRowCellValue("UserID");
                var selectedUser = _usersManager.TGetById(selectedUserId);

                if (selectedUser == null)
                {
                    MessageBox.Show("Güncellenecek kullanıcı bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                selectedUser.UserName = txtUserName.Text.Trim();
                selectedUser.Password = txtPassword.Text.Trim();
                selectedUser.Role = CmbRol.GetColumnValue("RoleName").ToString();
                selectedUser.IsActive = tsDurum.IsOn;

                _usersManager.TUpdate(selectedUser);
                MessageBox.Show("Kullanıcı başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Form yenileme
                userList();
                gridView1.RefreshData();
                CleanFields();
                RolList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedUserId = (int)gridView1.GetFocusedRowCellValue("UserID");
                var selectedUser = _usersManager.TGetById(selectedUserId);

                if (selectedUser == null)
                {
                    MessageBox.Show("Silinecek kullanıcı bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show($"'{selectedUser.UserName}' kullanıcısını silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    selectedUser.IsActive = false; // Kullanıcıyı pasif yap
                    _usersManager.TUpdate(selectedUser);
                    MessageBox.Show("Kullanıcı başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Form yenileme
                    userList();
                    gridView1.RefreshData();
                    CleanFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    return;
                }
                var selectedUserId = (int)gridView1.GetFocusedRowCellValue("UserID");
                var selectedUser = _usersManager.TGetById(selectedUserId);
                if (selectedUser != null)
                {
                    txtUserName.Text = selectedUser.UserName;
                    txtPassword.Text = selectedUser.Password;
                    var roleId = new[] { 0, 1, 2 } // Rol ID'lerini temsil ediyor
               .FirstOrDefault(id =>
                   id == (selectedUser.Role == "Admin" ? 1 : selectedUser.Role == "Kullanıcı" ? 2 : 0));

                    CmbRol.EditValue = roleId; // CmbRol'e uygun Rol ID'sini atama
                    btnKaydet.Enabled = false;
                    btnGuncelle.Enabled = true;
                    btnSil.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}