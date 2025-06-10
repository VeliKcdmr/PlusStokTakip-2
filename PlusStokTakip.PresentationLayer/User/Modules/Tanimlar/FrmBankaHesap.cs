using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Tanimlar
{
    public partial class FrmBankaHesap : DevExpress.XtraEditors.XtraForm
    {
        private readonly BankAccountsManager _bankAccountsManager = new BankAccountsManager(new EfBankAccountsDal());
        public FrmBankaHesap()
        {
            InitializeComponent();
        }

        private void FrmBankaHesap_Load(object sender, EventArgs e)
        {
            LoadBankAccounts();
        }
        private void LoadBankAccounts()
        {
            var bankAccounts = _bankAccountsManager.TGetAll().Where(a=>a.IsActive==true).Select(a => new
            {
                a.AccountID,
                a.BankName,
                a.Iban,
                a.AccountNumber,
                a.AccountType,
                a.Balance,
                a.Description,
                IsActive = a.IsActive ? "Aktif" : "Pasif"
            }).ToList();

            gridControl1.DataSource = bankAccounts;

            gridView1.Columns["AccountID"].Visible = false;
            gridView1.Columns["BankID"].Caption = "Banka ID";
            gridView1.Columns["AccountNumber"].Caption = "Hesap Numarası";
            gridView1.Columns["AccountType"].Caption = "Hesap Türü";
            gridView1.Columns["Balance"].Caption = "Bakiye";
            gridView1.Columns["Description"].Caption = "Açıklama";
            gridView1.Columns["IsActive"].Caption = "Durum";
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var account = new BankAccounts
                {
                    BankName = txtBankAdi.Text,
                    Iban = txtIBAN.Text,
                    AccountNumber = txtHesapNo.Text,
                    AccountType = cmbHesapTuru.Text,
                    Balance = Convert.ToDecimal(txtBakiye.Text),
                    Description = memoAciklama.Text,
                    IsActive = true
                };

                _bankAccountsManager.TInsert(account);
                MessageBox.Show("Banka hesabı başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadBankAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen güncellemek için bir hesap seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int accountId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("AccountID"));
                var account = _bankAccountsManager.TGetById(accountId);

                if (account != null)
                {
                    account.BankName = txtBankAdi.Text;
                    account.AccountNumber = txtHesapNo.Text;
                    account.Iban = txtIBAN.Text;
                    account.AccountType = cmbHesapTuru.Text;
                    account.Balance = Convert.ToDecimal(txtBakiye.Text);
                    account.Description = memoAciklama.Text;
                  
                    _bankAccountsManager.TUpdate(account);
                    MessageBox.Show("Banka hesabı başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBankAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen silmek için bir hesap seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int accountId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("AccountID"));
                var account = _bankAccountsManager.TGetById(accountId);
                var result = MessageBox.Show("Bu banka hesabını silmek istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    account.IsActive = false; // Soft delete
                    _bankAccountsManager.TUpdate(account);
                    MessageBox.Show("Banka hesabı başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBankAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0) return;

                int accountId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("AccountID"));
                var account = _bankAccountsManager.TGetById(accountId);

                if (account != null)
                {
                    txtBankAdi.Text = account.BankName;
                    txtIBAN.Text = account.Iban;
                    txtHesapNo.Text = account.AccountNumber;
                    cmbHesapTuru.Text = account.AccountType;
                    txtBakiye.Text = account.Balance.ToString();
                    memoAciklama.Text = account.Description;
                    btnKaydet.Enabled = false;
                    btnGuncelle.Enabled = true;
                    btnSil.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Seçilen banka hesabı bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}