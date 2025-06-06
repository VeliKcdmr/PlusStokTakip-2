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
                p.Amount,
                PaymentDate = p.PaymentDate.ToString("dd/MM/yyyy"),
                p.PaymentMethod,
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
            lkpMusteri.Properties.DataSource = _customersManager.TGetAll();
            lkpMusteri.Properties.DisplayMember = "CustomerName";
            lkpMusteri.Properties.ValueMember = "CustomerID";
        }
        private void loadSuppliers()
        {
            lkpTedarikci.Properties.DataSource = _suppliersManager.TGetAll();
            lkpTedarikci.Properties.DisplayMember = "SupplierName";
            lkpTedarikci.Properties.ValueMember = "SupplierID";
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

        }

        private void btnSil_Click(object sender, EventArgs e)
        {

        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}