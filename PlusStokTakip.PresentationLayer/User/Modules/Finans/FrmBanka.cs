using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Finans
{
    public partial class FrmBanka : DevExpress.XtraEditors.XtraForm
    {
        private readonly BankTransactionsManager _bankTransactionsManager = new BankTransactionsManager(new EfBankTransactionsDal());
        private readonly BankAccountsManager _bankAccountsManager = new BankAccountsManager(new EfBankAccountsDal());
        public FrmBanka()
        {
            InitializeComponent();
        }

        private void FrmBanka_Load(object sender, EventArgs e)
        {
            LoadBankTransactions();
            LoadBankAccounts();
        }
        private void LoadBankTransactions()
        {
            var bankAccounts = _bankAccountsManager.TGetAll().Where(b=>b.IsActive==true).Select(b => new
            {
                b.AccountID,
                b.BankName,
                b.Iban,
                b.AccountNumber
            }).ToList();
            var bankTransactions = _bankTransactionsManager.TGetAll().Where(b=>b.IsActive==true).Select(b => new
            {
                b.TransactionID,
                b.AccountID,
                bankAccounts.FirstOrDefault(a => a.AccountID == b.AccountID)?.BankName,
                bankAccounts.FirstOrDefault(a => a.AccountID == b.AccountID)?.Iban,
                bankAccounts.FirstOrDefault(a => a.AccountID == b.AccountID)?.AccountNumber,
                b.TransactionType,
                b.Amount,
                TransactionDate = b.TransactionDate?.ToString("dd/MM/yyyy"),
                b.Description,
                IsActive = b.IsActive ? "Aktif" : "Pasif"
            });
            gridControl1.DataSource = bankTransactions;
            gridView1.Columns["TransactionID"].Visible = false;
            gridView1.Columns["AccountID"].Visible = false;
            gridView1.Columns["BankName"].Caption = "Banka Adı";
            gridView1.Columns["Iban"].Caption = "IBAN";
            gridView1.Columns["AccountNumber"].Caption = "Hesap Numarası";
            gridView1.Columns["TransactionType"].Caption = "İşlem Türü";
            gridView1.Columns["Amount"].Caption = "Tutar";
            gridView1.Columns["TransactionDate"].Caption = "Tarih";
            gridView1.Columns["Description"].Caption = "Açıklama";
            gridView1.Columns["IsActive"].Caption = "Durum";
        }
        private void LoadBankAccounts()
        {
            var bankAccounts = _bankAccountsManager.TGetAll().Where(b => b.IsActive == true).Select(b => new
            {
                b.AccountID,
                b.AccountNumber,
                b.BankName
            }).ToList();
            lkpHesapSecimi.Properties.DataSource = bankAccounts;
            lkpHesapSecimi.Properties.DisplayMember = "AccountNumber";
            lkpHesapSecimi.Properties.ValueMember = "AccountID";
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var transaction = new BankTransactions
                {
                    AccountID = Convert.ToInt32(lkpHesapSecimi.EditValue),
                    TransactionType = cmbIslemTuru.Text,
                    Amount = Convert.ToDecimal(txtTutar.Text),
                    TransactionDate = dateTarih.DateTime,
                    Description = memoAciklama.Text,
                    IsActive = true
                };

                _bankTransactionsManager.TInsert(transaction);
                MessageBox.Show("Banka işlemi başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadBankTransactions();
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
                    MessageBox.Show("Lütfen güncellemek için bir işlem seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int transactionId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("TransactionID"));
                var transaction = _bankTransactionsManager.TGetById(transactionId);
                if (transaction != null)
                {
                    transaction.AccountID = Convert.ToInt32(lkpHesapSecimi.EditValue);
                    transaction.TransactionType = cmbIslemTuru.Text;
                    transaction.Amount = Convert.ToDecimal(txtTutar.Text);
                    transaction.TransactionDate = dateTarih.DateTime.Date;
                    transaction.Description = memoAciklama.Text;
                    _bankTransactionsManager.TUpdate(transaction);
                    MessageBox.Show("Banka işlemi başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBankTransactions();
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
                    MessageBox.Show("Lütfen silmek için bir işlem seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int transactionId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("TransactionID"));
                var transaction = _bankTransactionsManager.TGetById(transactionId);
                var result = MessageBox.Show("Bu banka işlemini silmek istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    transaction.IsActive = false; // Silme yerine pasifleştirme işlemi yapılıyor
                    _bankTransactionsManager.TUpdate(transaction);
                    MessageBox.Show("Banka işlemi başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBankTransactions();
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
                int transactionId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("TransactionID"));
                var transaction = _bankTransactionsManager.TGetById(transactionId);
                if (transaction != null)
                {
                    lkpHesapSecimi.EditValue = transaction.AccountID;
                    cmbIslemTuru.Text = transaction.TransactionType;
                    txtTutar.Text = transaction.Amount.ToString();
                    dateTarih.DateTime = (DateTime)transaction.TransactionDate;
                    memoAciklama.Text = transaction.Description;                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}