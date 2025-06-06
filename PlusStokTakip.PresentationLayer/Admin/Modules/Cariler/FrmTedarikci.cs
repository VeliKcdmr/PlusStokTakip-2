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
    public partial class FrmTedarikci : DevExpress.XtraEditors.XtraForm
    {
        private readonly SuppliersManager _suppliersManager = new SuppliersManager(new EfSuppliersDal());
        public FrmTedarikci()
        {
            InitializeComponent();
        }
        private void supplierLoad()
        {
            var suppliers = _suppliersManager.TGetAll().Select(c => new
            {
                c.SupplierID,
                c.SupplierName,
                c.TaxNumber,
                c.ContactPerson,                
                c.Phone,
                c.Email,
                c.Address
            }).ToList();
            gridControl1.DataSource = suppliers;
            gridView1.Columns["SupplierID"].Visible = false; // CustomerID sütununu gizle           
            gridView1.Columns["ContactPerson"].Caption = "Yetkili Ad Soyad"; // Sütun başlıklarını düzenle
            gridView1.Columns["SupplierName"].Caption = "Tedarikçi Adı"; // Firma adı sütun başlığını düzenle
            gridView1.Columns["TaxNumber"].Caption = "Vergi No"; // Vergi numarası sütun başlığını düzenle
            gridView1.Columns["Phone"].Caption = "Yetkili Telefon"; // Telefon sütun başlığını düzenle
            gridView1.Columns["Email"].Caption = "E-posta";// E-posta sütun başlığını düzenle
            gridView1.Columns["Address"].Caption = "Adres"; // Adres sütun başlığını düzenle            
        }
        private void Musteri_Firma_Load(object sender, EventArgs e)
        {
            cleanForm();
        }

        private void cleanForm()
        {

            txtAdres.EditValue = string.Empty;
            txtYAdSoyad.EditValue = string.Empty;
            txtEposta.EditValue = string.Empty;
            txtTedarikAd.EditValue = string.Empty;
            txtYTelefon.EditValue = string.Empty;
            txtVergiNo.EditValue = string.Empty;
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
            supplierLoad();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtTedarikAd, "Tedarikçi adı boş bırakılamaz!")) return;               
                var newSupplier = new Suppliers
                {
                    ContactPerson = txtYAdSoyad.Text,
                    SupplierName = txtTedarikAd.Text,
                    TaxNumber = txtVergiNo.Text,
                    Phone = txtYTelefon.Text,
                    Email = txtEposta.Text,
                    Address = txtAdres.Text,
                    IsActive = true
                };

                _suppliersManager.TInsert(newSupplier);
                MessageBox.Show("Tedarikçi başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (!ValidationHelper.ValidateControl(txtTedarikAd, "Tedarikçi adı boş bırakılamaz!")) return;
                int supplierId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("SupplierID"));
                var supplierToUpdate = _suppliersManager.TGetById(supplierId);

                if (supplierToUpdate == null)
                {
                    MessageBox.Show("Güncellenecek müşteri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                supplierToUpdate.ContactPerson = txtYAdSoyad.Text;
                supplierToUpdate.SupplierName = txtTedarikAd.Text;
                supplierToUpdate.TaxNumber = txtVergiNo.Text;
                supplierToUpdate.Phone = txtYTelefon.Text;
                supplierToUpdate.Email = txtEposta.Text;
                supplierToUpdate.Address = txtAdres.Text;

                _suppliersManager.TUpdate(supplierToUpdate);
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
                if (!ValidationHelper.ValidateControl(txtTedarikAd, "Tedarikçi adı boş bırakılamaz!")) return;
                int supplierId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("SupplierID"));
                var supplierToDelete = _suppliersManager.TGetById(supplierId);

                if (supplierToDelete == null)
                {
                    MessageBox.Show("Silinecek müşteri bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmation = MessageBox.Show("Bu müşteriyi silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmation == DialogResult.Yes)
                {
                    supplierToDelete.IsActive = false;
                    _suppliersManager.TUpdate(supplierToDelete);
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
            int supplierId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("SupplierID"));
            var supplier = _suppliersManager.TGetById(supplierId);

            if (supplier != null)
            {
                txtYAdSoyad.Text = supplier.ContactPerson;
                txtTedarikAd.Text = supplier.SupplierName;
                txtVergiNo.Text = supplier.TaxNumber;
                txtYTelefon.Text = supplier.Phone;
                txtEposta.Text = supplier.Email;
                txtAdres.Text = supplier.Address;
                tsDurum.IsOn = (bool)supplier.IsActive;

                btnKaydet.Enabled = false;
                btnGuncelle.Enabled = true;
                btnSil.Enabled = true;
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
    }
}