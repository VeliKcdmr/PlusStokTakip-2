using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Native;
using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Tanimlar
{
    public partial class FrmMarka : DevExpress.XtraEditors.XtraForm
    {
        private readonly BrandsManager _brandsManager = new BrandsManager(new EfBrandsDal());
        private readonly CategoriesManager _categoriesManager = new CategoriesManager(new EfCategoriesDal());

        public FrmMarka()
        {
            InitializeComponent();
        }

        private void FrmMarka_Load(object sender, EventArgs e)
        {
            FreshForm();
        }

        private void FreshForm()
        {
            BrandsLoad();
            CategoriesLoad();
            CleanFields();
        }

        private void BrandsLoad()
        {
            var categories = _categoriesManager.TGetAll().Where(c => c.IsActive); // Sadece aktif kategorileri getir
            var brandsList = _brandsManager.TGetAll().Where(b => b.IsActive) // Sadece aktif markaları getir
                .Select(b => new
                {
                    b.BrandID,
                    b.BrandName,
                    categories.FirstOrDefault(c => c.CategoryID == b.CategoryID)?.CategoryName
                }).ToList();

            gridControl1.DataSource = brandsList;
            gridView1.Columns["BrandID"].Visible = false; // BrandID sütununu gizle           
            gridView1.Columns["BrandName"].Caption = "Marka Adı";
            gridView1.Columns["CategoryName"].Caption = "Kategori Adı"; // Kategori adını göster           
        }

        private void CategoriesLoad()
        {
            var categories = _categoriesManager.TGetAll().Where(c=>c.IsActive==true).Select(c => new
            {
                c.CategoryID,
                c.CategoryName
            });
            cmbKategori.Properties.DataSource = categories;
            cmbKategori.Properties.DisplayMember = "CategoryName";
            cmbKategori.Properties.ValueMember = "CategoryID";
            cmbKategori.Properties.NullText = "Kategori Seçiniz";
            cmbKategori.Properties.PopulateColumns();
            cmbKategori.Properties.Columns["CategoryID"].Visible = false; // CategoryID sütununu gizle
            cmbKategori.Properties.Columns["CategoryName"].Caption = "Kategori Adı";
        }

        private void CleanFields()
        {
            txtAd.Text = string.Empty;
            cmbKategori.EditValue = null;
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // Validasyon kontrolleri
                if (!ValidationHelper.ValidateControl(txtAd, "Model adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbKategori, "Lütfen bir kategori seçiniz!")) return;

                // Aynı kategoride aynı isim kontrolü
                var existingBrand = _brandsManager.TGetAll().FirstOrDefault(b =>
                    b.BrandName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    b.CategoryID == (int)cmbKategori.EditValue && b.IsActive);

                if (existingBrand != null)
                {
                    MessageBox.Show($"'{txtAd.Text.Trim()}' Markası zaten seçilen kategori altında mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Yeni marka ekleme
                Brands newBrand = new Brands
                {
                    BrandName = txtAd.Text.Trim(),
                    CategoryID = (int)cmbKategori.EditValue,
                    IsActive = true
                };

                _brandsManager.TInsert(newBrand);
                MessageBox.Show("Marka başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Form yenileme
                FreshForm();
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
                // Aynı kategoride aynı isim kontrolü
                var existingBrand = _brandsManager.TGetAll().FirstOrDefault(b =>
                    b.BrandName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    b.CategoryID == (int)cmbKategori.EditValue && b.IsActive);

                if (existingBrand != null)
                {
                    MessageBox.Show($"'{txtAd.Text.Trim()}' Markası zaten seçilen kategori altında mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedBrandId = (int)gridView1.GetFocusedRowCellValue("BrandID");
                var selectedBrand = _brandsManager.TGetById(selectedBrandId);

                if (selectedBrand != null)
                {
                    selectedBrand.BrandName = txtAd.Text.Trim();
                    selectedBrand.CategoryID = (int)cmbKategori.EditValue;                   

                    _brandsManager.TUpdate(selectedBrand);
                    MessageBox.Show("Marka başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FreshForm();
                }
                else
                {
                    MessageBox.Show("Seçilen marka bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dialogResult = MessageBox.Show("Markayı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    var selectedBrandId = (int)gridView1.GetFocusedRowCellValue("BrandID");
                    var selectedBrand = _brandsManager.TGetById(selectedBrandId);

                    if (selectedBrand != null)
                    {
                        selectedBrand.IsActive = false; // Silme işlemi yerine durumu pasif yapıyoruz
                        _brandsManager.TUpdate(selectedBrand);
                        MessageBox.Show("Marka başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FreshForm();
                    }
                    else
                    {
                        MessageBox.Show("Seçilen marka bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                MessageBox.Show("Lütfen bir satır seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedBrandId = (int)gridView1.GetFocusedRowCellValue("BrandID");
            var brand = _brandsManager.TGetById(selectedBrandId);

            if (brand != null)
            {
                txtAd.Text = brand.BrandName;
                cmbKategori.EditValue = brand.CategoryID;              

                btnKaydet.Enabled = false;
                btnGuncelle.Enabled = true;
                btnSil.Enabled = true;
            }
            else
            {
                MessageBox.Show("Seçilen marka bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}