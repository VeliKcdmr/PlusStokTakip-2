using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace PlusStokTakip.PresentationLayer.Admin.Modules.Cariler
{
    public partial class FrmMusteriFirma : DevExpress.XtraEditors.XtraForm
    {
        private readonly CustomersManager _customersManager = new CustomersManager(new EfCustomersDal());
        public FrmMusteriFirma()
        {
            InitializeComponent();
        }
        private void customerLoad()
        {
            var customers = _customersManager.TGetAll().Select(c => new
            {
                c.CustomerID,
                c.CustomerType,
                c.CustomerName,
                c.CompanyName,
                c.TaxNumber,
                c.Phone,
                c.Email,
                c.Address
            }).ToList();
            gridControl1.DataSource = customers;
            gridView1.Columns["CustomerID"].Visible = false; // CustomerID sütununu gizle
            gridView1.Columns["CustomerType"].Caption = "Müşteri Tipi"; // Müşteri tipi sütun başlığını düzenle
            gridView1.Columns["CustomerName"].Caption = "Ad Soyad"; // Sütun başlıklarını düzenle
            gridView1.Columns["CompanyName"].Caption = "Firma Adı"; // Firma adı sütun başlığını düzenle
            gridView1.Columns["TaxNumber"].Caption = "Vergi No"; // Vergi numarası sütun başlığını düzenle
            gridView1.Columns["Phone"].Caption = "Telefon"; // Telefon sütun başlığını düzenle
            gridView1.Columns["Email"].Caption = "E-posta";// E-posta sütun başlığını düzenle
            gridView1.Columns["Address"].Caption = "Adres"; // Adres sütun başlığını düzenle            
        }
        private void Musteri_Firma_Load(object sender, EventArgs e)
        {
            cleanForm();
        }

        private void cleanForm()
        {
            cmbMusteriTipi.SelectedIndexChanged -= cmbMusteriTipi_SelectedIndexChanged; // Event'i kaldır
            txtAdres.EditValue = string.Empty;
            txtAdSoyad.EditValue = string.Empty;
            txtEposta.EditValue = string.Empty;
            txtFirmaAdi.EditValue = string.Empty;
            txtTelefon.EditValue = string.Empty;
            txtVergiNo.EditValue = string.Empty;
            txtFirmaAdi.Enabled = false;
            txtVergiNo.Enabled = false;           
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;                   
            cmbMusteriTipi.EditValue = null; // Müşteri tipi combobox'ını temizle
            cmbMusteriTipi.Properties.NullText = "Müşteri Tipi Seçiniz"; // Combobox için varsayılan metin ayarla
            customerLoad();
            cmbMusteriTipi.SelectedIndexChanged += cmbMusteriTipi_SelectedIndexChanged; // Event'i tekrar ekle
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAdSoyad, "Ad Soyad boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMusteriTipi, "Müşteri Tipi Seçilmedi!")) return;
                if (cmbMusteriTipi.SelectedItem.ToString() == "Firma")
                {
                    if (!ValidationHelper.ValidateControl(txtFirmaAdi, "Firma adı boş bırakılamaz!")) return;
                }
                var newCustomer = new Customers
                {
                    CustomerName = txtAdSoyad.Text,
                    CustomerType = cmbMusteriTipi.SelectedItem.ToString(),
                    CompanyName = txtFirmaAdi.Text,
                    TaxNumber = txtVergiNo.Text,
                    Phone = txtTelefon.Text,
                    Email = txtEposta.Text,
                    Address = txtAdres.Text,
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

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAdSoyad, "Ad Soyad boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMusteriTipi, "Müşteri Tipi Seçilmedi!")) return;
                if (cmbMusteriTipi.SelectedItem.ToString() == "Firma")
                {
                    if (!ValidationHelper.ValidateControl(txtFirmaAdi, "Firma adı boş bırakılamaz!")) return;
                }
                int customerId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CustomerID"));
                var customerToUpdate = _customersManager.TGetById(customerId);

                if (customerToUpdate == null)
                {
                    MessageBox.Show("Güncellenecek müşteri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                customerToUpdate.CustomerName = txtAdSoyad.Text;
                customerToUpdate.CustomerType = cmbMusteriTipi.SelectedItem.ToString();
                customerToUpdate.CompanyName = txtFirmaAdi.Text;
                customerToUpdate.TaxNumber = txtVergiNo.Text;
                customerToUpdate.Phone = txtTelefon.Text;
                customerToUpdate.Email = txtEposta.Text;
                customerToUpdate.Address = txtAdres.Text;

                _customersManager.TUpdate(customerToUpdate);
                MessageBox.Show("Müşteri başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cleanForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAdSoyad, "Ad Soyad boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMusteriTipi, "Müşteri Tipi Seçilmedi!")) return;
                if (cmbMusteriTipi.SelectedItem.ToString() == "Firma")
                {
                    if (!ValidationHelper.ValidateControl(txtFirmaAdi, "Firma adı boş bırakılamaz!")) return;
                }
                int customerId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CustomerID"));
                var customerToDelete = _customersManager.TGetById(customerId);

                if (customerToDelete == null)
                {
                    MessageBox.Show("Silinecek müşteri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmation = MessageBox.Show("Bu müşteriyi silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
            int customerId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("CustomerID"));
            var customer = _customersManager.TGetById(customerId);

            if (customer != null)
            {
                txtAdSoyad.Text = customer.CustomerName;
                cmbMusteriTipi.SelectedItem = customer.CustomerType;
                txtFirmaAdi.Text = customer.CompanyName;
                txtVergiNo.Text = customer.TaxNumber;
                txtTelefon.Text = customer.Phone;
                txtEposta.Text = customer.Email;
                txtAdres.Text = customer.Address;
                tsDurum.IsOn = customer.IsActive;
                btnKaydet.Enabled = false; // Güncelleme yapabilmek için kaydet butonunu devre dışı bırak
                btnGuncelle.Enabled = true; // Güncelleme butonunu etkinleştir
                btnSil.Enabled = true; // Silme butonunu etkinleştir

            }

        }

        private void gridView1_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            if (gridView1.RowCount == 0)
            {
                string message = "Veri Bulunamadı";
                Font messageFont = new Font("Tahoma", 12, FontStyle.Bold);
                Rectangle rect = e.Bounds;
                e.Graphics.DrawString(message, messageFont, Brushes.Gray, rect, new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
            }


        }

        private void cmbMusteriTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMusteriTipi.SelectedItem != null && cmbMusteriTipi.SelectedItem.ToString() == "Bireysel")
            {
                txtFirmaAdi.Enabled = false;
                txtVergiNo.Enabled = false;
                lblFirma.Enabled = false; // Eğer bir Label varsa, onu da gizle
                lblVergi.Enabled = false;
            }
            else // Firma seçiliyse
            {
                txtFirmaAdi.Enabled = true;
                txtVergiNo.Enabled = true;
                lblFirma.Enabled = true;
                lblVergi.Enabled = true;
            }


        }
    }
}