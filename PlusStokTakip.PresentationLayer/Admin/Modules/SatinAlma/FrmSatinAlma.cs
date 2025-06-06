using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Linq;
using System.Windows.Forms;


namespace PlusStokTakip.PresentationLayer.Admin.Modules.SatinAlma
{
    public partial class FrmSatinAlma : DevExpress.XtraEditors.XtraForm
    {
        private readonly ProductsManager _productsManager = new ProductsManager(new EfProductsDal());
        private readonly SuppliersManager _suppliersManager = new SuppliersManager(new EfSuppliersDal());
        private readonly PurchasesManager _purchaseManager = new PurchasesManager(new EfPurchasesDal());
        public FrmSatinAlma()
        {
            InitializeComponent();
        }

        private void FrmSatinAlma_Load(object sender, EventArgs e)
        {
            cleanForm();
        }
        private void cleanForm()
        {
            lkpUrunSecimi.EditValue = null;
            lkpUrunSecimi.Properties.NullText = "Seçiniz";
            lkpTedarikciSecim.EditValue = null;
            lkpTedarikciSecim.Properties.NullText = "Seçiniz";
            txtSAFiyat.EditValue = null;
            dateSATarihi.DateTime = DateTime.Now.Date;
            spnMiktar.EditValue = 0;
            memoAciklama.EditValue = null;
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
            loadProducts();
            loadSuppliers();
            loadPurchases();
        }
        private void loadPurchases()
        {
            var products = _productsManager.TGetAll().Select(pr => new
            {
                pr.ProductID,
                pr.ProductName,
                pr.Barcode
            }).ToList();
            var suppliers = _suppliersManager.TGetAll().Select(s => new
            {
                s.SupplierID,
                s.SupplierName,
                s.ContactPerson
            }).ToList();
            var purchases = _purchaseManager.TGetAll().Select(pu => new
            {
                pu.PurchaseID,
                ProductName =products.FirstOrDefault(pr=>pr.ProductID==pu.ProductID)?.ProductName ?? "Bulunamadı",
                Barcode = products.FirstOrDefault(pr => pr.ProductID == pu.ProductID)?.Barcode ?? "Bulunamadı",
                SupplierName =suppliers.FirstOrDefault(s => s.SupplierID == pu.SupplierID)?.SupplierName ?? "Bulunamadı",
                ContactPerson = suppliers.FirstOrDefault(s => s.SupplierID == pu.SupplierID)?.ContactPerson ?? "Bulunamadı",
                pu.Quantity,
                pu.Cost,
                pu.TotalCost,
                pu.PurchaseDate,
                pu.Description,
                pu.IsActive
            }).ToList();
            gridControl1.DataSource = purchases;
            gridView1.Columns["PurchaseID"].Visible = false;
            gridView1.Columns["ProductName"].Caption = "Ürün Adı";
            gridView1.Columns["Barcode"].Caption = "Barkod";
            gridView1.Columns["SupplierName"].Caption = "Tedarikçi Adı";
            gridView1.Columns["ContactPerson"].Caption = "Yetkili Kişi";
            gridView1.Columns["Quantity"].Caption = "Miktar";
            gridView1.Columns["Cost"].Caption = "Birim Fiyatı";
            gridView1.Columns["TotalCost"].Caption = "Toplam Tutar";
            gridView1.Columns["PurchaseDate"].Caption = "Satın Alma Tarihi";
            gridView1.Columns["Description"].Caption = "Açıklama";
            gridView1.Columns["IsActive"].Caption = "Aktif Durum";
        }

        private void loadProducts()
        {
            var products = _productsManager.TGetAll().Select(p => new
            {
                p.ProductID,
                p.ProductName,
                p.Barcode
            }).ToList();
            lkpUrunSecimi.Properties.DataSource = products;
            lkpUrunSecimi.Properties.DisplayMember = "ProductName";
            lkpUrunSecimi.Properties.ValueMember = "ProductID";
            lkpUrunSecimi.Properties.PopulateColumns();
            lkpUrunSecimi.Properties.Columns["ProductID"].Visible = false;
            lkpUrunSecimi.Properties.Columns["Barcode"].Caption = "Barkod";
            lkpUrunSecimi.Properties.Columns["ProductName"].Caption = "Ürün Adı";
        }
        private void loadSuppliers()
        {
            var suppliers = _suppliersManager.TGetAll().Select(s => new
            {
                s.SupplierID,
                s.SupplierName,
                s.TaxNumber,
                s.ContactPerson
            }).ToList();
            lkpTedarikciSecim.Properties.DataSource = suppliers;
            lkpTedarikciSecim.Properties.DisplayMember = "SupplierName";
            lkpTedarikciSecim.Properties.ValueMember = "SupplierID";
            lkpTedarikciSecim.Properties.PopulateColumns();
            lkpTedarikciSecim.Properties.Columns["SupplierID"].Visible = false;
            lkpTedarikciSecim.Properties.Columns["TaxNumber"].Caption = "Vergi Numarası";
            lkpTedarikciSecim.Properties.Columns["ContactPerson"].Caption = "Yetkili Kişi";
            lkpTedarikciSecim.Properties.Columns["SupplierName"].Caption = "Firma Adı";
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (lkpUrunSecimi.EditValue == null || spnMiktar.Value <= 0)
                {
                    MessageBox.Show("Lütfen geçerli bir ürün ve miktar girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var purchase = new Purchases
                {
                    ProductID = Convert.ToInt32(lkpUrunSecimi.EditValue),
                    Quantity = Convert.ToInt32(spnMiktar.Value),
                    Cost = Convert.ToDecimal(txtSAFiyat.Text),
                    TotalCost = Convert.ToDecimal(spnMiktar.Value) * Convert.ToDecimal(txtSAFiyat.Text),
                    SupplierID = Convert.ToInt32(lkpTedarikciSecim.EditValue),
                    PurchaseDate = dateSATarihi.DateTime.Date,
                    Description = memoAciklama.Text,
                    IsActive = true

                };

                _purchaseManager.TInsert(purchase);
                var productId = Convert.ToInt32(lkpUrunSecimi.EditValue);
                var selectedProduct = _productsManager.TGetById(productId);
                if (selectedProduct != null)
                {
                    selectedProduct.StockQuantity += purchase.Quantity;
                    _productsManager.TUpdate(selectedProduct);
                }

                MessageBox.Show("Satın alma işlemi başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cleanForm();
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
                    MessageBox.Show("Lütfen güncellemek için bir satın alma işlemi seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int purchaseId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("PurchaseID"));
                var purchase = _purchaseManager.TGetById(purchaseId);

                if (purchase != null)
                {
                    purchase.Quantity = Convert.ToInt32(spnMiktar.Value);
                    purchase.Cost = Convert.ToDecimal(txtSAFiyat.Text);
                    purchase.TotalCost = Convert.ToDecimal(spnMiktar.Value) * Convert.ToDecimal(txtSAFiyat.Text);
                    purchase.SupplierID = Convert.ToInt32(lkpTedarikciSecim.EditValue);
                    purchase.PurchaseDate = dateSATarihi.DateTime.Date;
                    purchase.Description = memoAciklama.Text;

                    _purchaseManager.TUpdate(purchase);
                    MessageBox.Show("Satın alma işlemi başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cleanForm();
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
                    MessageBox.Show("Lütfen bir satın alma işlemi seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int purchaseId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("PurchaseID"));
                var purchase = _purchaseManager.TGetById(purchaseId);
                var result = MessageBox.Show("Bu satın alma işlemini silmek istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    purchase.IsActive = false; // Silme yerine pasifleştiriyoruz
                    _purchaseManager.TUpdate(purchase);
                    MessageBox.Show("Satın alma işlemi başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cleanForm();
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

                int purchaseId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("PurchaseID"));
                var purchase = _purchaseManager.TGetById(purchaseId);

                if (purchase != null)
                {
                    lkpUrunSecimi.EditValue = purchase.ProductID;
                    spnMiktar.Value = purchase.Quantity;
                    txtSAFiyat.Text = purchase.Cost.ToString();
                    lkpTedarikciSecim.EditValue = purchase.SupplierID;
                    dateSATarihi.DateTime = (DateTime)purchase.PurchaseDate;
                    memoAciklama.Text = purchase.Description;
                    tsDurum.IsOn = purchase.IsActive;
                    btnKaydet.Enabled = false;
                    btnGuncelle.Enabled = true;
                    btnSil.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}