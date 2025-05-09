using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.Admin.Modules.Urunler
{
    public partial class FrmUrun : DevExpress.XtraEditors.XtraForm
    {
        private readonly ProductsManager _productsManager = new ProductsManager(new EfProductsDal());
        private readonly CategoriesManager _categoriesManager = new CategoriesManager(new EfCategoriesDal());
        private readonly BrandsManager _brandsManager = new BrandsManager(new EfBrandsDal());
        private readonly ModelsManager _modelsManager = new ModelsManager(new EfModelsDal());
        private readonly ShelvesManager _shelvesManager = new ShelvesManager(new EfShelvesDal());
        public FrmUrun()
        {
            InitializeComponent();
        }
        private void CustomizeLookupEditAppearance()
        {
            var lookupEdits = new[] { cmbKategori, cmbMarka, cmbModel, cmbRaf, cmbYil };
            foreach (var edit in lookupEdits)
            {
                edit.Properties.AppearanceDropDown.Font = new Font("Tahoma", 11);
                edit.Properties.AppearanceDropDownHeader.Font = new Font("Tahoma", 11);
            }
        }
        private void loadProducts()
        {
            try
            {
                // Tüm verileri tek seferde alın
                var categories = _categoriesManager.TGetAll().ToDictionary(c => c.CategoryID, c => c.CategoryName);
                var brands = _brandsManager.TGetAll().ToDictionary(b => b.BrandID, b => b.BrandName);
                var models = _modelsManager.TGetAll().ToDictionary(m => m.ModelID, m => new { m.ModelName, m.ModelYear, m.BrandID });
                var shelves = _shelvesManager.TGetAll().ToDictionary(s => s.ShelfID, s => s.ShelfName);

                // Ürünleri yükle
                var products = _productsManager.TGetAll()
                    .Select(p => new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.Barcode,
                        ShelfName = shelves.ContainsKey(p.ShelfID ?? 0) ? shelves[p.ShelfID ?? 0] : "Bilinmiyor",
                        CategoryName = categories.ContainsKey(p.CategoryID ?? 0) ? categories[p.CategoryID ?? 0] : "Bilinmiyor",
                        BrandName = models.ContainsKey(p.ModelID ?? 0) && brands.ContainsKey(models[p.ModelID ?? 0].BrandID ?? 0)
                            ? brands[models[p.ModelID ?? 0].BrandID ?? 0]
                            : "Bilinmiyor",
                        ModelName = models.ContainsKey(p.ModelID ?? 0) ? models[p.ModelID ?? 0].ModelName : "Bilinmiyor",
                        ModelYear = models.ContainsKey(p.ModelID ?? 0) ? models[p.ModelID ?? 0].ModelYear : (int?)null,
                        p.StockQuantity,
                        p.Price,
                        IsActive = p.IsActive ? "Aktif" : "Pasif"
                    }).ToList();

                // GridControl'e bağla
                gridControl1.DataSource = null;
                gridControl1.DataSource = products;

                // Kolon başlıklarını düzenle
                gridView1.Columns["ProductID"].Caption = "Ürün ID";
                gridView1.Columns["ProductName"].Caption = "Ürün Adı";
                gridView1.Columns["Barcode"].Caption = "Barkod";
                gridView1.Columns["ShelfName"].Caption = "Raf Adı";
                gridView1.Columns["CategoryName"].Caption = "Kategori Adı";
                gridView1.Columns["BrandName"].Caption = "Marka Adı";
                gridView1.Columns["ModelName"].Caption = "Model Adı";
                gridView1.Columns["ModelYear"].Caption = "Model Yılı";
                gridView1.Columns["StockQuantity"].Caption = "Stok Miktarı";
                gridView1.Columns["Price"].Caption = "Fiyat";
                gridView1.Columns["IsActive"].Caption = "Durum";
            }
            catch (Exception ex)
            {
                // Hata mesajını göster
                MessageBox.Show($"Ürünler yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadCategories()
        {
            var categories = _categoriesManager.TGetAll()
                .Select(c => new
                {
                    c.CategoryID,
                    c.CategoryName,

                });
            cmbKategori.Properties.DataSource = categories.ToList();
            cmbKategori.Properties.DisplayMember = "CategoryName";
            cmbKategori.Properties.ValueMember = "CategoryID";
            cmbKategori.Properties.PopulateColumns();
            cmbKategori.Properties.NullText = "Kategori Seçiniz";
            cmbKategori.Properties.Columns["CategoryID"].Visible = false;
            cmbKategori.Properties.Columns["CategoryName"].Caption = "Kategori Adı";
        }

        private void CleanForm()
        {
            // Clear LookUpEdit values
            cmbKategori.EditValue = null;
            cmbMarka.EditValue = null;
            cmbModel.EditValue = null;
            cmbYil.EditValue = null;
            cmbRaf.EditValue = null;
            cmbKategori.Enabled = true;
            cmbMarka.Enabled = false;
            cmbModel.Enabled = false;
            cmbYil.Enabled = false;
            cmbRaf.Enabled = false;
            // Reset text fields
            txtAd.Text = string.Empty;
            txtBarkod.Text = string.Empty;
            spinAdet.Text = string.Empty;
            txtFiyat.Text = string.Empty;

            // Reset buttons
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;

            // Optionally reload data
            loadProducts();
            LoadCategories();
            CustomizeLookupEditAppearance();
        }
        private void FrmUrun_Load(object sender, EventArgs e)
        {
            CleanForm();

        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation kontrolü
                if (!ValidationHelper.ValidateControl(txtAd, "Ürün adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtBarkod, "Barkod boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbKategori, "Kategori seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Marka seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModel, "Model seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbYil, "Yıl seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbRaf, "Raf seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(spinAdet, "Stok miktarı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtFiyat, "Fiyat boş bırakılamaz!")) return;
                var selectedCategoryId = (int)cmbKategori.EditValue;
                var selectedBrandId = (int)cmbMarka.EditValue;
                var selectedModelName = cmbModel.EditValue.ToString();
                var selectedYear = (int)cmbYil.EditValue;
                var selectedShelfId = (int)cmbRaf.EditValue;
                var stockQuantity = (int)spinAdet.Value;
                var price = decimal.Parse(txtFiyat.Text);
                // ModelName ve ModelYear eşleşmesine göre modeli al
                var selectedModel = _modelsManager.TGetAll().FirstOrDefault(m => m.ModelName.Equals(selectedModelName, StringComparison.OrdinalIgnoreCase) && m.ModelYear == selectedYear);
                // Yeni ürün nesnesi oluştur ve doldur
                Products newProduct = new Products
                {
                    ProductName = txtAd.Text,
                    Barcode = txtBarkod.Text,
                    CategoryID = selectedCategoryId,
                    BrandID = selectedBrandId, // Doğrudan seçilen BrandID kullanılıyor
                    ModelID = selectedModel.ModelID, // ModelID'yi al                    
                    ShelfID = selectedShelfId,
                    StockQuantity = stockQuantity,
                    Price = price,
                    IsActive = true, // Varsayılan olarak aktif
                    CDate = DateTime.Now.Date
                };
                // Ürünü kaydet
                _productsManager.TInsert(newProduct);
                // Başarılı mesaj
                MessageBox.Show("Ürün başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Formu yenile
                CleanForm();
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
                if (!ValidationHelper.ValidateControl(txtAd, "Ürün adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtBarkod, "Barkod boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbKategori, "Kategori seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Marka seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModel, "Model seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbYil, "Yıl seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbRaf, "Raf seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(spinAdet, "Stok miktarı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtFiyat, "Fiyat boş bırakılamaz!")) return;

                var productId = (int)gridView1.GetFocusedRowCellValue("ProductID");
                var productToUpdate = _productsManager.TGetById(productId);

                if (productToUpdate == null)
                {
                    MessageBox.Show("Güncellenecek ürün bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                productToUpdate.ProductName = txtAd.Text;
                productToUpdate.Barcode = txtBarkod.Text;
                productToUpdate.StockQuantity = (int)spinAdet.Value;
                productToUpdate.Price = decimal.Parse(txtFiyat.Text);
                productToUpdate.CategoryID = (int)cmbKategori.EditValue;
                productToUpdate.BrandID = (int)cmbMarka.EditValue;
                productToUpdate.ModelID = _modelsManager.TGetAll()
                    .FirstOrDefault(m => m.ModelName.Equals(cmbModel.EditValue.ToString(), StringComparison.OrdinalIgnoreCase) && m.ModelYear == (int)cmbYil.EditValue).ModelID;
                productToUpdate.ShelfID = (int)cmbRaf.EditValue;
                productToUpdate.IsActive = tsDurum.IsOn;
                productToUpdate.UDate = DateTime.Now.Date;

                _productsManager.TUpdate(productToUpdate);
                MessageBox.Show("Ürün başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CleanForm();
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
                if (!ValidationHelper.ValidateControl(txtAd, "Ürün adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtBarkod, "Barkod boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbKategori, "Kategori seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Marka seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModel, "Model seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbYil, "Yıl seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbRaf, "Raf seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(spinAdet, "Stok miktarı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtFiyat, "Fiyat boş bırakılamaz!")) return;
                var productId = (int)gridView1.GetFocusedRowCellValue("ProductID");
                var productToDelete = _productsManager.TGetById(productId);

                if (productToDelete == null)
                {
                    MessageBox.Show("Silinecek ürün bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmation = MessageBox.Show("Ürünü silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmation == DialogResult.Yes)
                {
                    productToDelete.IsActive = false;
                    productToDelete.UDate = DateTime.Now.Date;
                    _productsManager.TUpdate(productToDelete);
                    MessageBox.Show("Ürün başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CleanForm();
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
                var productId = (int)gridView1.GetFocusedRowCellValue("ProductID");
                var selectedProduct = _productsManager.TGetById(productId);

                if (selectedProduct == null)
                {
                    MessageBox.Show("Seçilen ürün bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtAd.Text = selectedProduct.ProductName;
                txtBarkod.Text = selectedProduct.Barcode;
                spinAdet.Value = selectedProduct.StockQuantity.Value;
                txtFiyat.Text = selectedProduct.Price.ToString();
                cmbKategori.EditValue = selectedProduct.CategoryID;
                cmbMarka.EditValue = selectedProduct.BrandID;
                cmbModel.EditValue = selectedProduct.Models.ModelName;
                cmbYil.EditValue = selectedProduct.Models.ModelYear;
                cmbRaf.EditValue = selectedProduct.ShelfID;
                tsDurum.IsOn = selectedProduct.IsActive;

                btnKaydet.Enabled = false;
                btnGuncelle.Enabled = true;
                btnSil.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbKategori_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbKategori.EditValue == null)
            {
                cmbMarka.Properties.DataSource = null;
                cmbMarka.Properties.NullText = "";
                return;
            }

            try
            {
                cmbMarka.Enabled = true;
                var selectedCategoryId = (int)cmbKategori.EditValue;
                var brands = _brandsManager.TGetAll()
                    .Where(b => b.CategoryID == selectedCategoryId)
                    .Select(b => new
                    {
                        b.BrandID,
                        b.BrandName,
                    }).ToList();
                cmbMarka.Properties.DataSource = brands;
                cmbMarka.Properties.DisplayMember = "BrandName";
                cmbMarka.Properties.ValueMember = "BrandID";
                cmbMarka.Properties.PopulateColumns();
                cmbMarka.Properties.NullText = "Marka Seçiniz";
                cmbMarka.Properties.Columns["BrandID"].Visible = false;
                cmbMarka.Properties.Columns["BrandName"].Caption = "Marka Adı";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbMarka_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbMarka.EditValue == null)
            {
                cmbModel.Properties.DataSource = null;
                cmbModel.Properties.NullText = "";
                return;
            }
            try
            {
                cmbModel.Enabled = true;
                var selectedBrandId = (int)cmbMarka.EditValue;
                // Model adlarını gruplandır ve hazırlamak için gereken verileri al
                var models = _modelsManager.TGetAll()
                    .Where(m => m.BrandID == selectedBrandId)
                    .GroupBy(m => m.ModelName)
                    .Select(g => new
                    {
                        ModelName = g.Key // Grup anahtarı ModelName
                    }).ToList();
                cmbModel.Properties.DataSource = models;
                cmbModel.Properties.DisplayMember = "ModelName";
                cmbModel.Properties.ValueMember = "ModelName"; // ID yerine ad eşleşmesi
                cmbModel.Properties.PopulateColumns();
                cmbModel.Properties.NullText = "Model Seçiniz";

                // Sütun düzenlemesi
                cmbModel.Properties.Columns.Clear();
                cmbModel.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ModelName", "Model Adı"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Markalar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbModel_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbModel.EditValue == null)
            {
                cmbYil.Properties.DataSource = null;
                cmbYil.Properties.NullText = "";
                return;
            }
            try
            {
                cmbYil.Enabled = true;
                var selectedModelName = cmbModel.EditValue.ToString();
                // Seçilen model adına ait yılları getir
                var years = _modelsManager.TGetAll()
                    .Where(m => m.ModelName.Equals(selectedModelName, StringComparison.OrdinalIgnoreCase))
                    .Select(m => new
                    {
                        m.ModelYear // Yıllar
                    }).Distinct().ToList();
                cmbYil.Properties.DataSource = years;
                cmbYil.Properties.DisplayMember = "ModelYear";
                cmbYil.Properties.ValueMember = "ModelYear";
                cmbYil.Properties.NullText = "Yıl Seçiniz";

                // Kolon düzenlemesi
                cmbYil.Properties.Columns.Clear();
                cmbYil.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ModelYear", "Model Yılı"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Modeller yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbYil_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbYil.EditValue == null)
            {
                cmbRaf.Properties.DataSource = null;
                cmbRaf.Properties.NullText = "";
                return;
            }
            try
            {
                cmbRaf.Enabled = true;
                var shelves = _shelvesManager.TGetAll()
                    .Select(s => new
                    {
                        s.ShelfID,
                        s.ShelfName
                    }).ToList();
                cmbRaf.Properties.DataSource = shelves;
                cmbRaf.Properties.DisplayMember = "ShelfName";
                cmbRaf.Properties.ValueMember = "ShelfID";
                cmbRaf.Properties.PopulateColumns();
                cmbRaf.Properties.NullText = "Raf Seçiniz";
                cmbRaf.Properties.Columns["ShelfID"].Visible = false;
                cmbRaf.Properties.Columns["ShelfName"].Caption = "Raf Adı";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Raflar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}