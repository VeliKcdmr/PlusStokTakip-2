using DevExpress.XtraLayout.Customization;
using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.Admin.Modules.Defines
{
    public partial class FrmKategori : DevExpress.XtraEditors.XtraForm
    {
        CategoriesManager _categoryManager = new CategoriesManager(new EfCategoriesDal());

        public FrmKategori()
        {
            InitializeComponent();
        }

        // Form yüklenirken kategorileri yükle
        private void FrmKategori_Load(object sender, EventArgs e)
        {
            FreshForm();
        }

        // Kategori listesini yükler
        private void LoadCategories()
        {
            var categoriesList = _categoryManager.TGetAll().Select(c => new
            {
                c.CategoryID,
                c.CategoryName               
            }).ToList();

            gridControl1.DataSource = categoriesList;
            gridView1.Columns["CategoryID"].Caption = "Kategori ID";
            gridView1.Columns["CategoryName"].Caption = "Kategori Adı";            
        }

        // Alanları temizler
        private void CleanFields()
        {
            txtAd.Text = string.Empty;            
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
        }

        // Formu yeniler
        private void FreshForm()
        {
            LoadCategories();
            CleanFields();
        }

        // Yeni kategori kaydetme
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Kategori adı boş bırakılamaz!")) return;
                var existingCategory = _categoryManager.TGetAll()
                    .FirstOrDefault(c => c.CategoryName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && c.IsActive);
                if (existingCategory != null)
                {
                    MessageBox.Show($"{txtAd.Text.Trim()} kategorisi zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Categories newCategory = new Categories
                {
                    CategoryName = txtAd.Text.Trim(),                    
                    IsActive = true // Varsayılan olarak aktif
                };

                _categoryManager.TInsert(newCategory);
                MessageBox.Show("Kategori başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FreshForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kategori güncelleme
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                var existingCategory = _categoryManager.TGetAll()
                    .FirstOrDefault(c => c.CategoryName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && c.IsActive);
                if (existingCategory != null)
                {
                    MessageBox.Show($"{txtAd.Text.Trim()} kategorisi zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedCategoryId = (int)gridView1.GetFocusedRowCellValue("CategoryID");
                var selectedCategory = _categoryManager.TGetById(selectedCategoryId);

                if (selectedCategory != null)
                {
                    selectedCategory.CategoryName = txtAd.Text.Trim();                    

                    _categoryManager.TUpdate(selectedCategory);
                    MessageBox.Show("Kategori başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FreshForm();
                }
                else
                {
                    MessageBox.Show("Seçilen kategori bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Kategori silme
        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dialogResult = MessageBox.Show("Kategoriyi silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    var selectedCategoryId = (int)gridView1.GetFocusedRowCellValue("CategoryID");
                    var selectedCategory = _categoryManager.TGetById(selectedCategoryId);

                    if (selectedCategory != null)
                    {
                        _categoryManager.TDelete(selectedCategory);
                        MessageBox.Show("Kategori başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FreshForm();
                    }
                    else
                    {
                        MessageBox.Show("Seçilen kategori bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Seçilen kategoriyi form alanlarına doldur
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                MessageBox.Show("Lütfen bir satır seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedCategoryId = (int)gridView1.GetFocusedRowCellValue("CategoryID");
            var category = _categoryManager.TGetById(selectedCategoryId);

            if (category != null)
            {
                txtAd.Text = category.CategoryName;                

                btnKaydet.Enabled = false;
                btnGuncelle.Enabled = true;
                btnSil.Enabled = true;
            }
            else
            {
                MessageBox.Show("Seçilen kategori bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}