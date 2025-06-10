using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Urunler
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
        private void FrmUrun_Load(object sender, EventArgs e)
        {
            CleanForm();
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
                // **Tüm verileri tek seferde alın**
                var categories = _categoriesManager.TGetAll().Where(c => c.IsActive == true).ToDictionary(c => c.CategoryID, c => c.CategoryName);
                var brands = _brandsManager.TGetAll().Where(b => b.IsActive == true).ToDictionary(b => b.BrandID, b => b.BrandName);
                var models = _modelsManager.TGetAll().Where(m => m.IsActive == true).ToDictionary(m => m.ModelID, m => new { m.ModelName, m.ModelYear, m.BrandID });
                var shelves = _shelvesManager.TGetAll().Where(s => s.IsActive == true).ToDictionary(s => s.ShelfID, s => s.ShelfName);

                // **Ürünleri yükle**
                var products = _productsManager.TGetAll().Where(p => p.IsActive == true)
                    .Select(p => new
                    {
                        // **Resmi kare içine kırp ve yeniden boyutlandır**
                        ImageData = ResizeToSquare(p.ImageData, 128), // **128x128 px olarak kare içine ayarla**
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
                        Cost = string.Format("{0:#,##0.00} ₺", p.Cost),
                        Price = string.Format("{0:#,##0.00} ₺", p.Price)
                    }).ToList();

                // **GridControl'e bağla**
                gridControl1.DataSource = null;
                gridControl1.DataSource = products;

                // **Resim Kolonu İçin Repository Item Kullan**
                RepositoryItemPictureEdit riPictureEdit = new RepositoryItemPictureEdit
                {
                    SizeMode = PictureSizeMode.Clip, // **Kare içine kırp**
                    NullText = "Resim Yok",
                    AllowFocused = false,
                    PictureAlignment = ContentAlignment.MiddleCenter
                };
                gridControl1.RepositoryItems.Add(riPictureEdit);
                layoutView1.Columns["ImageData"].ColumnEdit = riPictureEdit;
                layoutView1.CardCaptionFormat = "{ProductName}"; // **Kart başlığı formatı**
                // **Kolon başlıklarını düzenle**
                layoutView1.Columns["ImageData"].Caption = "Ürün Resmi";
                layoutView1.Columns["ProductID"].Caption = "Ürün ID";
                layoutView1.Columns["ProductName"].Caption = "Ürün Adı";
                layoutView1.Columns["Barcode"].Caption = "Barkod";
                layoutView1.Columns["ShelfName"].Caption = "Raf Adı";
                layoutView1.Columns["CategoryName"].Caption = "Kategori Adı";
                layoutView1.Columns["BrandName"].Caption = "Marka Adı";
                layoutView1.Columns["ModelName"].Caption = "Model Adı";
                layoutView1.Columns["ModelYear"].Caption = "Model Yılı";
                layoutView1.Columns["StockQuantity"].Caption = "Stok Miktarı";
                layoutView1.Columns["Price"].Caption = "Satış Fiyat";
                layoutView1.Columns["Cost"].Caption = "Alış Fiyatı";                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürünler yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // **Resmi Kare İçine Yeniden Boyutlandırma Fonksiyonu**
        private Image ResizeToSquare(byte[] imageData, int size)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                Image originalImage = Image.FromStream(ms);

                Bitmap squareImage = new Bitmap(size, size);
                using (Graphics g = Graphics.FromImage(squareImage))
                {
                    g.Clear(Color.Transparent); // **Arka planı şeffaf yap**
                    g.DrawImage(originalImage, 0, 0, size, size);
                }

                return squareImage;
            }
        }
        private void LoadCategories()
        {
            var categories = _categoriesManager.TGetAll().Where(c => c.IsActive == true)
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
            pictureEdit1.Image = null;

            // Reset text fields
            txtAd.Text = string.Empty;
            txtBarkod.Text = string.Empty;
            spinAdet.Text = string.Empty;
            txtAFiyat.Text = string.Empty;
            txtSFiyat.Text = string.Empty;

            // Reset buttons
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;

            // Optionally reload data
            loadProducts();
            LoadCategories();
            CustomizeLookupEditAppearance();
            layoutView1.RefreshData();
            gridControl1.RefreshDataSource();
        }


        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // **Validation Kontrolü** (Daha Kısa ve Okunaklı)
                if (!ValidationHelper.ValidateControl(txtAd, "Ürün adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtBarkod, "Barkod boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbKategori, "Kategori seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Marka seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModel, "Model seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbYil, "Yıl seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbRaf, "Raf seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(spinAdet, "Stok miktarı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtAFiyat, "Fiyat boş bırakılamaz!")) return;

                var selectedCategoryId = Convert.ToInt32(cmbKategori.EditValue);
                var selectedBrandId = Convert.ToInt32(cmbMarka.EditValue);
                var selectedModelName = cmbModel.EditValue.ToString();
                var selectedYear = Convert.ToInt32(cmbYil.EditValue);
                var selectedShelfId = Convert.ToInt32(cmbRaf.EditValue);
                var stockQuantity = Convert.ToInt32(spinAdet.Value);
                var price = decimal.TryParse(txtAFiyat.Text, out decimal parsedPrice) ? parsedPrice : 0;
                var cost = decimal.TryParse(txtSFiyat.Text, out decimal parsedCost) ? parsedCost : 0;

                // **ModelName ve ModelYear eşleşmesine göre modeli al**
                var selectedModel = _modelsManager.TGetAll()
                    .FirstOrDefault(m => m.ModelName.Equals(selectedModelName, StringComparison.OrdinalIgnoreCase) && m.ModelYear == selectedYear);

                if (selectedModel == null)
                {
                    MessageBox.Show("Seçilen model bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // **Yeni Ürün Nesnesi Oluştur ve Doldur**
                Products newProduct = new Products
                {
                    ProductName = txtAd.Text,
                    Barcode = txtBarkod.Text,
                    CategoryID = selectedCategoryId,
                    BrandID = selectedBrandId,
                    ModelID = selectedModel.ModelID, // **Eğer `selectedModel` null ise hata vermeyecek**
                    ShelfID = selectedShelfId,
                    StockQuantity = stockQuantity,
                    Price = price,
                    Cost = cost,
                    ImageData = selectedImageBytes, // **Resim verisini ekle**
                    IsActive = true,
                    CDate = DateTime.Now.Date

                };

                // **Ürünü Kaydet**
                _productsManager.TInsert(newProduct);

                MessageBox.Show("Ürün başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // **Formu Yenile**
                CleanForm();
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
                if (!ValidationHelper.ValidateControl(txtAd, "Ürün adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtBarkod, "Barkod boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbKategori, "Kategori seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Marka seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModel, "Model seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbYil, "Yıl seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbRaf, "Raf seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(spinAdet, "Stok miktarı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtAFiyat, "Fiyat boş bırakılamaz!")) return;
                var productId = Convert.ToInt32(layoutView1.GetFocusedRowCellValue("ProductID"));
                var productToUpdate = _productsManager.TGetById(productId);

                if (productToUpdate == null)
                {
                    MessageBox.Show("Güncellenecek ürün bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                productToUpdate.ProductName = txtAd.Text;
                productToUpdate.Barcode = txtBarkod.Text;
                productToUpdate.StockQuantity = (int)spinAdet.Value;
                productToUpdate.Price = decimal.Parse(txtAFiyat.Text);
                productToUpdate.Cost = decimal.Parse(txtSFiyat.Text);
                productToUpdate.CategoryID = (int)cmbKategori.EditValue;
                productToUpdate.BrandID = (int)cmbMarka.EditValue;
                productToUpdate.ModelID = _modelsManager.TGetAll()
                    .FirstOrDefault(m => m.ModelName.Equals(cmbModel.EditValue.ToString(), StringComparison.OrdinalIgnoreCase) && m.ModelYear == (int)cmbYil.EditValue).ModelID;
                productToUpdate.ShelfID = (int)cmbRaf.EditValue;
                productToUpdate.UDate = DateTime.Now.Date;
                // **Resmi veritabanına kaydet**
                if (selectedImageBytes != null && selectedImageBytes.Length > 0)
                {
                    productToUpdate.ImageData = selectedImageBytes;

                }

                _productsManager.TUpdate(productToUpdate); // Güncelleme işlemi

                MessageBox.Show("Ürün başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CleanForm();
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
                if (!ValidationHelper.ValidateControl(txtAd, "Ürün adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtBarkod, "Barkod boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbKategori, "Kategori seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Marka seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModel, "Model seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbYil, "Yıl seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbRaf, "Raf seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(spinAdet, "Stok miktarı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(txtAFiyat, "Fiyat boş bırakılamaz!")) return;
                var productId = (int)layoutView1.GetFocusedRowCellValue("ProductID");
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
                var productId = (int)layoutView1.GetFocusedRowCellValue("ProductID");
                var selectedProduct = _productsManager.TGetById(productId);

                if (selectedProduct == null)
                {
                    MessageBox.Show("Seçilen ürün bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtAd.Text = selectedProduct.ProductName;
                txtBarkod.Text = selectedProduct.Barcode;
                spinAdet.Value = selectedProduct.StockQuantity.Value;
                txtAFiyat.Text = selectedProduct.Cost.ToString();
                txtSFiyat.Text = selectedProduct.Price.ToString();
                cmbKategori.EditValue = selectedProduct.CategoryID;
                cmbMarka.EditValue = selectedProduct.BrandID;
                cmbModel.EditValue = selectedProduct.Models.ModelName;
                cmbYil.EditValue = selectedProduct.Models.ModelYear;
                cmbRaf.EditValue = selectedProduct.ShelfID;
                pictureEdit1.Image = selectedProduct.ImageData != null && selectedProduct.ImageData.Length > 0
                    ? Image.FromStream(new MemoryStream(selectedProduct.ImageData))
                    : null; // Resim yoksa null ata

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
                cmbModel.Properties.Columns.Add(new LookUpColumnInfo("ModelName", "Model Adı"));
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
                cmbYil.Properties.Columns.Add(new LookUpColumnInfo("ModelYear", "Model Yılı"));
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
        private byte[] selectedImageBytes = null;
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Ürün Resmi Seç";
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // **Resmi byte dizisine çevir**
                        byte[] imageBytes = File.ReadAllBytes(ofd.FileName);

                        // **Resmi ekranda göster**
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            pictureEdit1.Image = Image.FromStream(ms);
                        }

                        // **Resmi geçici olarak bellekte sakla**
                        selectedImageBytes = imageBytes; // Bu değişkeni form seviyesinde tanımla
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Resim seçerken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

    }
}