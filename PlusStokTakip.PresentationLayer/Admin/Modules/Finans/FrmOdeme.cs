using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.Admin.Modules.Finans
{
    public partial class FrmOdeme : DevExpress.XtraEditors.XtraForm
    {
        private readonly PaymentsManager _paymentsManager = new PaymentsManager(new EfPaymentsDal());
        private readonly CustomersManager _customersManager = new CustomersManager(new EfCustomersDal());
        private readonly SuppliersManager _suppliersManager = new SuppliersManager(new EfSuppliersDal());

        public FrmOdeme()
        {
            InitializeComponent();
        }

        private void FrmOdeme_Load(object sender, EventArgs e)
        {
            cleanFrom();
        }
        private void cleanFrom()
        {
            txtMiktar.Text = string.Empty;
            dateOdemeTarihi.DateTime = DateTime.Now.Date;
            cmbOdemeYontemi.SelectedIndex = -1;
            memoAciklama.Text = string.Empty;
            lkpMusteri.EditValue = null;
            lkpTedarikci.EditValue = null;
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
            loadPayments();
            loadCustomers();
            loadSuppliers();
        }
        private void loadPayments()
        {
            var customers = _customersManager.TGetAll().Select(c => new { c.CustomerID, c.CustomerName });
            var suppliers = _suppliersManager.TGetAll().Select(s => new { s.SupplierID, s.SupplierName });
            var payments = _paymentsManager.TGetAll().Select(p => new
            {
                p.PaymentID,
                CustomerName = customers.FirstOrDefault(c => c.CustomerID == p.CustomerID)?.CustomerName ?? "Bilinmeyen Müşteri",
                SupplierName = suppliers.FirstOrDefault(s => s.SupplierID == p.SupplierID)?.SupplierName ?? "Bilinmeyen Tedarikçi",
                p.PaymentMethod,
                p.Amount,
                PaymentDate = p.PaymentDate.ToString("dd/MM/yyyy"),                
                p.Description,
                IsActive = p.IsActive ? "Aktif" : "Pasif"
            }).ToList();

            gridControl1.DataSource = payments;

            gridView1.Columns["PaymentID"].Visible = false; // PaymentID sütununu gizle
            gridView1.Columns["CustomerName"].Caption = "Müşteri Adı";
            gridView1.Columns["SupplierName"].Caption = "Tedarikçi Adı";
            gridView1.Columns["Amount"].Caption = "Ödeme Tutarı";
            gridView1.Columns["PaymentDate"].Caption = "Ödeme Tarihi";
            gridView1.Columns["PaymentMethod"].Caption = "Ödeme Yöntemi";
            gridView1.Columns["Description"].Caption = "Açıklama";
            gridView1.Columns["IsActive"].Caption = "Durum";
        }
        private void loadCustomers()
        {
            lkpMusteri.Properties.DataSource = _customersManager.TGetAll().Select(c => new
            {
                c.CustomerID,
                c.CustomerName,
                c.CompanyName,
            }).ToList();
            lkpMusteri.Properties.DisplayMember = "CustomerName";
            lkpMusteri.Properties.ValueMember = "CustomerID";
            lkpMusteri.Properties.NullText = "Müşteri Seçiniz"; // NullText ekleyerek kullanıcıya bilgi veriyoruz
            lkpMusteri.Properties.PopulateColumns();
            lkpMusteri.Properties.Columns["CustomerID"].Visible = false; // CustomerID sütununu gizle
            lkpMusteri.Properties.Columns["CustomerName"].Caption = "Müşteri Adı"; // Sütun başlığını değiştiriyoruz
            lkpMusteri.Properties.Columns["CompanyName"].Caption = "Şirket Adı"; // Şirket Adı sütun başlığını değiştiriyoruz

        }
        private void loadSuppliers()
        {
            lkpTedarikci.Properties.DataSource = _suppliersManager.TGetAll().Select(s => new
            {
                s.SupplierID,
                s.SupplierName,
                s.ContactPerson,
                s.TaxNumber,
            }).ToList();
            lkpTedarikci.Properties.DisplayMember = "SupplierName";
            lkpTedarikci.Properties.ValueMember = "SupplierID";
            lkpTedarikci.Properties.NullText = "Tedarikçi Seçiniz"; // NullText ekleyerek kullanıcıya bilgi veriyoruz
            lkpTedarikci.Properties.PopulateColumns();
            lkpTedarikci.Properties.Columns["SupplierID"].Visible = false; // SupplierID sütununu gizle
            lkpTedarikci.Properties.Columns["SupplierName"].Caption = "Tedarikçi Adı"; // Sütun başlığını değiştiriyoruz
            lkpTedarikci.Properties.Columns["ContactPerson"].Caption = "Yetkili Kişi"; // İletişim Kişisi sütun başlığını değiştiriyoruz
            lkpTedarikci.Properties.Columns["TaxNumber"].Caption = "Vergi Numarası"; // Vergi Numarası sütun başlığını değiştiriyoruz
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var payment = new Payments
                {
                    CustomerID = lkpMusteri.EditValue != null ? Convert.ToInt32(lkpMusteri.EditValue) : (int?)null,
                    SupplierID = lkpTedarikci.EditValue != null ? Convert.ToInt32(lkpTedarikci.EditValue) : (int?)null,
                    Amount = Convert.ToDecimal(txtMiktar.Text),
                    PaymentDate = dateOdemeTarihi.DateTime.Date,
                    PaymentMethod = cmbOdemeYontemi.Text,
                    Description = memoAciklama.Text,
                    IsActive = true,
                };

                _paymentsManager.TInsert(payment);

                MessageBox.Show("Ödeme başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadPayments();
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
                    MessageBox.Show("Lütfen güncellemek için bir ödeme işlemi seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int paymentId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("PaymentID"));
                var payment = _paymentsManager.TGetById(paymentId);

                if (payment != null)
                {
                    payment.CustomerID = lkpMusteri.EditValue != null ? Convert.ToInt32(lkpMusteri.EditValue) : (int?)null;
                    payment.SupplierID = lkpTedarikci.EditValue != null ? Convert.ToInt32(lkpTedarikci.EditValue) : (int?)null;
                    payment.Amount = Convert.ToDecimal(txtMiktar.Text);
                    payment.PaymentDate = dateOdemeTarihi.DateTime;
                    payment.PaymentMethod = cmbOdemeYontemi.Text;
                    payment.Description = memoAciklama.Text;                   

                    _paymentsManager.TUpdate(payment);
                    MessageBox.Show("Ödeme işlemi başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadPayments();
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
                    MessageBox.Show("Lütfen silmek için bir ödeme işlemi seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int paymentId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("PaymentID"));
                var payment = _paymentsManager.TGetById(paymentId);
                var result = MessageBox.Show("Bu ödeme işlemini silmek istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    payment.IsActive = false; // Silme işlemi yerine pasifleştiriyoruz
                    _paymentsManager.TUpdate(payment);
                    MessageBox.Show("Ödeme işlemi başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadPayments();
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

                int paymentId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("PaymentID"));
                var payment = _paymentsManager.TGetById(paymentId);

                if (payment != null)
                {
                    lkpMusteri.EditValue = payment.CustomerID;
                    lkpTedarikci.EditValue = payment.SupplierID;
                    txtMiktar.Text = payment.Amount.ToString();
                    dateOdemeTarihi.DateTime = payment.PaymentDate;
                    cmbOdemeYontemi.Text = payment.PaymentMethod;
                    memoAciklama.Text = payment.Description;
                    tsDurum.IsOn = payment.IsActive;
                    btnKaydet.Enabled = false; // Kaydet butonunu devre dışı bırakıyoruz
                    btnGuncelle.Enabled = true; // Güncelle butonunu etkinleştiriyoruz
                    btnSil.Enabled = true; // Sil butonunu etkinleştiriyoruz
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}