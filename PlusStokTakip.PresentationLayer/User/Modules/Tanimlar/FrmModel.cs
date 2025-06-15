using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Tanimlar
{
    public partial class FrmModel : DevExpress.XtraEditors.XtraForm
    {
        private readonly ModelsManager _modelsManager = new ModelsManager(new EfModelsDal());
        private readonly BrandsManager _brandManager = new BrandsManager(new EfBrandsDal());

        public FrmModel()
        {
            InitializeComponent();
        }

        // Form yüklendiğinde
        private void FrmModel_Load(object sender, EventArgs e)
        {
            FreshForm();
        }

        // Formu yeniler
        private void FreshForm()
        {
            gridControl1.Refresh();
            modelsLoad();
            brandLoad();
            CleanFields();
        }

        // Modelleri yükler
        private void modelsLoad()
        {
            var modelsList = _modelsManager.TGetAll().Where(m => m.IsActive == true).Select(m => new
            {
                m.ModelID,
                m.ModelName,
                m.ModelYear,
                BrandName = m.Brands?.BrandName ?? "Bulunamadı"                
            }).ToList();
            gridControl1.DataSource = modelsList;
            gridView1.Columns["ModelID"].Visible=false;
            gridView1.Columns["ModelName"].Caption = "Model Adı";
            gridView1.Columns["ModelYear"].Caption = "Model Yılı";
            gridView1.Columns["BrandName"].Caption = "Marka";
            

        }

        // Markaları yükler
        private void brandLoad()
        {
            var brands = _brandManager.TGetAll().Where(b => b.IsActive == true).Select(b => new
            {
                b.BrandID,
                b.BrandName
            }).ToList();
            cmbMarka.Properties.DataSource = brands;
            cmbMarka.Properties.DisplayMember = "BrandName";
            cmbMarka.Properties.ValueMember = "BrandID";
            cmbMarka.Properties.PopulateColumns();
            cmbMarka.Properties.NullText = "Marka Seçiniz";
            cmbMarka.Properties.Columns["BrandName"].Caption = "Marka Adı";
            cmbMarka.Properties.Columns["BrandID"].Visible = false; // ID'yi gizle

        }

        // Alanları temizler
        private void CleanFields()
        {
            txtAd.Text = string.Empty;
            cmbMarka.EditValue = null;
            cmbModelYil.EditValue = null;
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
        }

        // Yeni model kaydetme
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Model adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModelYil, "Lütfen bir Model Yılı seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Lütfen bir Marka seçiniz!")) return;

                var selectedYear = ((DateTimeOffset)cmbModelYil.EditValue).Year; // DateTimeOffset'ten yıl bilgisi
                var selectedBrandId = (int)cmbMarka.EditValue; // LookUpEdit'ten seçilen marka ID'si
                if (_modelsManager.TGetAll().Any(c => c.ModelName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && c.ModelYear == selectedYear && c.BrandID == selectedBrandId && c.IsActive))
                {
                    MessageBox.Show($"'{txtAd.Text}' adlı model zaten {selectedYear} yılı için seçilen marka ile kayıtlı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Models newModel = new Models
                {
                    ModelName = txtAd.Text.Trim(),
                    ModelYear = DateTimeOffset.Parse(cmbModelYil.EditValue.ToString()).Year,
                    BrandID = (int)cmbMarka.EditValue,
                    IsActive = true

                };

                _modelsManager.TInsert(newModel);
                MessageBox.Show("Model başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FreshForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Model güncelleme
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Model adı boş bırakılamaz!")) return;
                if (!ValidationHelper.ValidateControl(cmbModelYil, "Lütfen bir Model Yılı seçiniz!")) return;
                if (!ValidationHelper.ValidateControl(cmbMarka, "Lütfen bir Marka seçiniz!")) return;

                var selectedYear = ((DateTimeOffset)cmbModelYil.EditValue).Year; // DateTimeOffset'ten yıl bilgisi
                var selectedBrandId = (int)cmbMarka.EditValue; // LookUpEdit'ten seçilen marka ID'si
                if (_modelsManager.TGetAll().Any(c => c.ModelName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && c.ModelYear == selectedYear && c.BrandID == selectedBrandId && c.IsActive))
                {
                    MessageBox.Show($"'{txtAd.Text}' adlı model zaten {selectedYear} yılı için seçilen marka ile kayıtlı!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedModelId = (int)gridView1.GetFocusedRowCellValue("ModelID");
                var selectedModel = _modelsManager.TGetById(selectedModelId);

                if (selectedModel != null)
                {
                    selectedModel.ModelName = txtAd.Text.Trim();
                    selectedModel.ModelYear = DateTimeOffset.Parse(cmbModelYil.EditValue.ToString()).Year;
                    selectedModel.BrandID = (int)cmbMarka.EditValue;                    

                    _modelsManager.TUpdate(selectedModel);
                    MessageBox.Show("Model başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FreshForm();
                }
                else
                {
                    MessageBox.Show("Seçilen model bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Model silme
        private void btnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dialogResult = MessageBox.Show("Modeli silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    var selectedModelId = (int)gridView1.GetFocusedRowCellValue("ModelID");
                    var selectedModel = _modelsManager.TGetById(selectedModelId);

                    if (selectedModel != null)
                    {
                        selectedModel.IsActive = false; // Modeli pasif yap
                        _modelsManager.TUpdate(selectedModel);
                        MessageBox.Show("Model başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FreshForm();
                    }
                    else
                    {
                        MessageBox.Show("Seçilen model bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Seçilen modelin bilgilerini form alanlarına doldur
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                MessageBox.Show("Lütfen bir satır seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedModelId = (int)gridView1.GetFocusedRowCellValue("ModelID");
            var model = _modelsManager.TGetById(selectedModelId);

            if (model != null)
            {
                txtAd.Text = model.ModelName;
                cmbModelYil.EditValue = new DateTimeOffset(new DateTime(model.ModelYear, 1, 1));
                cmbMarka.EditValue = model.BrandID;              
                btnKaydet.Enabled = false;
                btnGuncelle.Enabled = true;
                btnSil.Enabled = true;
            }
            else
            {
                MessageBox.Show("Seçilen model bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}