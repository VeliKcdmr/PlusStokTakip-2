using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.Admin.Modules.Finans
{
    public partial class FrmKasa : DevExpress.XtraEditors.XtraForm
    {
        private readonly CashRegisterManager _cashRegisterManager = new CashRegisterManager(new EfCashRegisterDal());
        public FrmKasa()
        {
            InitializeComponent();
        }

        private void FrmKasa_Load(object sender, EventArgs e)
        {
            LoadCashTransactions();
        }
        private void LoadCashTransactions()
        {
            var cashTransactions = _cashRegisterManager.TGetAll().Select(c => new
            {
                c.CashID,
                c.TransactionType,
                c.Amount,
                TransactionDate = c.TransactionDate.ToString("dd/MM/yyyy"),
                c.Description,
                IsActive = c.IsActive ? "Aktif" : "Pasif"
            }).ToList();

            gridControl1.DataSource = cashTransactions;

            gridView1.Columns["CashID"].Visible = false;
            gridView1.Columns["TransactionType"].Caption = "İşlem Türü";
            gridView1.Columns["Amount"].Caption = "Tutar";
            gridView1.Columns["TransactionDate"].Caption = "Tarih";
            gridView1.Columns["Description"].Caption = "Açıklama";
            gridView1.Columns["IsActive"].Caption = "Durum";
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var cashTransaction = new CashRegister
                {
                    TransactionType = cmbIslemTuru.Text,  // Gelir mi, gider mi?
                    Amount = Convert.ToDecimal(txtTutar.Text),
                    TransactionDate = dateTarih.DateTime,
                    Description = memoAciklama.Text,
                    IsActive = true // Varsayılan olarak aktif
                };

                _cashRegisterManager.TInsert(cashTransaction);
                MessageBox.Show("Kasa işlemi başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCashTransactions();
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
                    MessageBox.Show("Lütfen güncellemek için bir kayıt seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int cashId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CashID"));
                var cashTransaction = _cashRegisterManager.TGetById(cashId);

                if (cashTransaction != null)
                {
                    cashTransaction.TransactionType = cmbIslemTuru.Text;
                    cashTransaction.Amount = Convert.ToDecimal(txtTutar.Text);
                    cashTransaction.TransactionDate = dateTarih.DateTime;
                    cashTransaction.Description = memoAciklama.Text;
                    _cashRegisterManager.TUpdate(cashTransaction);
                    MessageBox.Show("Kasa işlemi başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCashTransactions();
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
                    MessageBox.Show("Lütfen silmek için bir kayıt seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int cashId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CashID"));
                var cashTransaction = _cashRegisterManager.TGetById(cashId);
                var result = MessageBox.Show("Bu kasa işlemini silmek istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    cashTransaction.IsActive= false; // Silme işlemi yerine pasifleştiriyoruz
                    _cashRegisterManager.TUpdate(cashTransaction);
                    MessageBox.Show("Kasa işlemi başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCashTransactions();
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
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen düzenlemek için bir kayıt seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int cashId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CashID"));
                var cashTransaction = _cashRegisterManager.TGetById(cashId);
                if (cashTransaction != null)
                {
                    cmbIslemTuru.Text = cashTransaction.TransactionType;
                    txtTutar.Text = cashTransaction.Amount.ToString();
                    dateTarih.DateTime = cashTransaction.TransactionDate;
                    memoAciklama.Text = cashTransaction.Description;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}