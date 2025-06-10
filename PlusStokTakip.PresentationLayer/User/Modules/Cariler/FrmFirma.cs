using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Cariler
{
    public partial class FrmFirma : DevExpress.XtraEditors.XtraForm
    {
        private readonly CustomersManager _customersManager = new CustomersManager(new EfCustomersDal());
        public FrmFirma()
        {
            InitializeComponent();
        }

        private void FrmFirma_Load(object sender, System.EventArgs e)
        {
            cleanForm();
        }
        private void cleanForm()
        {
            txtAdSoyad.Text = string.Empty;
            txtFirmaAdi.Text = string.Empty;
            txtVergiNo.Text = string.Empty;
            txtTelefon.Text = string.Empty;
            txtEposta.Text = string.Empty;
            txtAdres.Text = string.Empty;
            loadCustomers(); // Müşteri listesini yükle
            btnKaydet.Enabled = true; // Kaydet butonunu etkinleştir
            btnGuncelle.Enabled = false; // Güncelle butonunu devre dışı bırak
            btnSil.Enabled = false; // Sil butonunu devre dışı bırak
        }
        private void loadCustomers()
        {
            try
            {
                var customers = _customersManager.TGetAll().Where(c => c.CustomerType == "Firma" && c.IsActive == true).Select(c => new
                {
                    c.CustomerID,
                    c.CompanyName,
                    c.TaxNumber,
                    c.CustomerName,
                    c.Phone,
                    c.Email,
                    c.Address
                }).ToList();
                gridControl1.DataSource = customers;
                gridView1.Columns["CustomerID"].Visible = false; // CustomerID sütununu gizle
                gridView1.Columns["CustomerName"].Caption = "Ad Soyad"; // Sütun başlıklarını düzenle
                gridView1.Columns["CompanyName"].Caption = "Firma Adı";
                gridView1.Columns["TaxNumber"].Caption = "Vergi No";
                gridView1.Columns["Phone"].Caption = "Telefon";
                gridView1.Columns["Email"].Caption = "E-posta";
                gridView1.Columns["Address"].Caption = "Adres";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Müşteri verileri yüklenirken bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnKaydet_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtFirmaAdi, "Firma Adı boş bırakılamaz!")) return;
                var newCustomer = new Customers
                {
                    CustomerName = txtAdSoyad.Text.Trim(),
                    CustomerType = "Firma",
                    CompanyName = txtFirmaAdi.Text.Trim(),
                    TaxNumber = txtVergiNo.Text.Trim(),
                    Phone = txtTelefon.Text.Trim(),
                    Email = txtEposta.Text.Trim(),
                    Address = txtAdres.Text.Trim(),
                    IsActive = true // Yeni müşteri aktif olarak ekleniyor
                };

                _customersManager.TInsert(newCustomer);
                MessageBox.Show("Müşteri başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cleanForm(); // Listeyi güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuncelle_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtFirmaAdi, "Firma Adı boş bırakılamaz!")) return;
                int customerId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CustomerID"));
                var customerToUpdate = _customersManager.TGetById(customerId);
                if (customerToUpdate == null)
                {
                    MessageBox.Show("Lütfen güncellemek istediğiniz müşteriyi seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                customerToUpdate.CustomerName = txtAdSoyad.Text.Trim();
                customerToUpdate.CompanyName = txtFirmaAdi.Text.Trim();
                customerToUpdate.TaxNumber = txtVergiNo.Text.Trim();
                customerToUpdate.Phone = txtTelefon.Text.Trim();
                customerToUpdate.Email = txtEposta.Text.Trim();
                customerToUpdate.Address = txtAdres.Text.Trim();
                _customersManager.TUpdate(customerToUpdate);
                MessageBox.Show("Müşteri başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cleanForm(); // Listeyi güncelle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtFirmaAdi, "Firma Adı boş bırakılamaz!")) return;
                int customerId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CustomerID"));
                var customerToDelete = _customersManager.TGetById(customerId);

                if (customerToDelete == null)
                {
                    MessageBox.Show("Silinecek Firma bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmation = MessageBox.Show("Bu Firmayı silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmation == DialogResult.Yes)
                {
                    customerToDelete.IsActive = false;
                    _customersManager.TUpdate(customerToDelete);
                    MessageBox.Show("Müşteri başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cleanForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int customerId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CustomerID"));
                var customer = _customersManager.TGetById(customerId);
                if (customer != null)
                {
                    txtAdSoyad.Text = customer.CustomerName;
                    txtFirmaAdi.Text = customer.CompanyName;
                    txtVergiNo.Text = customer.TaxNumber;
                    txtTelefon.Text = customer.Phone;
                    txtEposta.Text = customer.Email;
                    txtAdres.Text = customer.Address;
                    btnKaydet.Enabled = false; // Kaydet butonunu devre dışı bırak
                    btnGuncelle.Enabled = true; // Güncelle butonunu etkinleştir
                    btnSil.Enabled = true; // Sil butonunu etkinleştir
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}