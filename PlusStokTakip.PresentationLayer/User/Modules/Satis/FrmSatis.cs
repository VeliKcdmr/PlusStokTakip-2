using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Satis
{
    public partial class FrmSatis : DevExpress.XtraEditors.XtraForm
    {
        private readonly SalesManager _salesManager = new SalesManager(new EfSalesDal());
        private readonly ProductsManager _productsManager = new ProductsManager(new EfProductsDal());
        private readonly CustomersManager _customersManager = new CustomersManager(new EfCustomersDal());
        public FrmSatis()
        {
            InitializeComponent();
        }
        private void FrmSatis_Load(object sender, EventArgs e)
        {
            cleanForm();
            loadSales();
            loadProducts();
            loadCustomers();

        }
        private void cleanForm()
        {
            lkpUrunSecimi.EditValue = null;
            lkpUrunSecimi.Properties.NullText = "Seçiniz";
            lkpCariSecim.EditValue = null;
            lkpCariSecim.Properties.NullText = "Seçiniz";
            txtSFiyat.Text = "0";
            dateSTarihi.EditValue = DateTime.Now.Date;
            memoAciklama.EditValue = null;
            btnKaydet.Enabled = Enabled;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
        }
        private void loadSales()
        {
            var products = _productsManager.TGetAll().Where(p => p.IsActive == true).Select(p => new
            {
                p.ProductID,
                p.ProductName,
                p.Barcode
            }).ToList();
            var customers = _customersManager.TGetAll().Where(c => c.IsActive == true).Select(c => new
            {
                c.CustomerID,
                c.CustomerName,
                c.CompanyName
            }).ToList();
            var sales = _salesManager.TGetAll().Where(s => s.IsActive == true).Select(s => new
            {
                s.SaleID,
                CustomerName = customers.FirstOrDefault(c => c.CustomerID == s.CustomerID)?.CustomerName ?? "Bulunamadı",
                CompanyName = customers.FirstOrDefault(c => c.CustomerID == s.CustomerID)?.CompanyName ?? "Bulunamadı",
                ProductName = products.FirstOrDefault(p => p.ProductID == s.ProductID)?.ProductName ?? "Bulunamadı",
                Barcode = products.FirstOrDefault(p => p.ProductID == s.ProductID)?.Barcode ?? "Bulunamadı",
                s.Quantity,
                s.Price,
                s.TotalRevenue,
                s.SaleDate
            }).ToList();
            gridControl1.DataSource = sales;
            gridView1.Columns["SaleID"].Visible = false; // Hide SaleID column
            gridView1.Columns["CustomerName"].Caption = "Müşteri Adı";
            gridView1.Columns["CompanyName"].Caption = "Şirket Adı";
            gridView1.Columns["ProductName"].Caption = "Ürün Adı";
            gridView1.Columns["Barcode"].Caption = "Barkod";
            gridView1.Columns["Quantity"].Caption = "Adet";
            gridView1.Columns["Price"].Caption = "Fiyat";
            gridView1.Columns["TotalRevenue"].Caption = "Toplam Tutar";
            gridView1.Columns["SaleDate"].Caption = "Satış Tarihi";
        }
        private void loadProducts()
        {
            var products = _productsManager.TGetAll().Where(p => p.IsActive == true).Select(p => new
            {
                p.ProductID,
                p.ProductName,
                p.Barcode
            }).ToList();
            lkpUrunSecimi.Properties.DataSource = products;
            lkpUrunSecimi.Properties.DisplayMember = "ProductName";
            lkpUrunSecimi.Properties.ValueMember = "ProductID";
            lkpUrunSecimi.Properties.NullText = "Seçiniz";
            lkpUrunSecimi.Properties.PopulateColumns();
            lkpUrunSecimi.Properties.Columns["ProductID"].Visible = false; // Hide ProductID column
            lkpUrunSecimi.Properties.Columns["ProductName"].Caption = "Ürün Adı";
            lkpUrunSecimi.Properties.Columns["Barcode"].Caption = "Barkod";
        }
        private void loadCustomers()
        {
            var customers = _customersManager.TGetAll().Where(c => c.IsActive == true).Select(c => new
            {
                c.CustomerID,
                c.CustomerName,
                c.CompanyName
            }).ToList();
            lkpCariSecim.Properties.DataSource = customers;
            lkpCariSecim.Properties.DisplayMember = "CustomerName";
            lkpCariSecim.Properties.ValueMember = "CustomerID";
            lkpCariSecim.Properties.NullText = "Seçiniz";
            lkpCariSecim.Properties.PopulateColumns();
            lkpCariSecim.Properties.Columns["CustomerID"].Visible = false; // Hide CustomerID column
            lkpCariSecim.Properties.Columns["CustomerName"].Caption = "Müşteri";
            lkpCariSecim.Properties.Columns["CompanyName"].Caption = "Şirket Adı";
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (lkpUrunSecimi.EditValue == null || lkpCariSecim.EditValue == null)
                {
                    MessageBox.Show("Lütfen ürün ve müşteri seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var sale = new Sales
                {
                    ProductID = Convert.ToInt32(lkpUrunSecimi.EditValue),
                    CustomerID = Convert.ToInt32(lkpCariSecim.EditValue),
                    Quantity = Convert.ToInt32(spnMiktar.Value),
                    Price = Convert.ToDecimal(txtSFiyat.Text),
                    TotalRevenue = Convert.ToDecimal(spnMiktar.Value) * Convert.ToDecimal(txtSFiyat.Text),
                    SaleDate = dateSTarihi.DateTime,
                    IsActive = true
                };
                _salesManager.TInsert(sale);
                // Update product stock after sale
                var selectedProductId = Convert.ToInt32(lkpUrunSecimi.EditValue);
                var product = _productsManager.TGetById(selectedProductId);
                if (product != null)
                {
                    product.StockQuantity -= sale.Quantity; // Assuming Stock is a property in Products
                    _productsManager.TUpdate(product);
                }
                loadSales();
                cleanForm();
                MessageBox.Show("Satış kaydı başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show("Lütfen güncellemek için bir satış işlemi seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedProductId = Convert.ToInt32(lkpUrunSecimi.EditValue);
                var sale = _salesManager.TGetById(selectedProductId);

                if (sale != null)
                {
                    sale.ProductID = Convert.ToInt32(lkpUrunSecimi.EditValue);
                    sale.CustomerID = Convert.ToInt32(lkpCariSecim.EditValue);
                    sale.Quantity = Convert.ToInt32(txtSFiyat.Text);
                    sale.Price = Convert.ToDecimal(txtSFiyat.Text);
                    sale.TotalRevenue = sale.Price * sale.Quantity;
                    sale.SaleDate = dateSTarihi.DateTime;
                    _salesManager.TUpdate(sale);
                    MessageBox.Show("Satış işlemi başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadSales();
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
                    MessageBox.Show("Lütfen silmek için bir satış işlemi seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedProductId = Convert.ToInt32(lkpUrunSecimi.EditValue);
                var sale = _salesManager.TGetById(selectedProductId);
                var result = MessageBox.Show("Bu satış işlemini silmek istediğinizden emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    sale.IsActive = false; // Soft delete
                    _salesManager.TUpdate(selectedProductId);
                    MessageBox.Show("Satış işlemi başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadSales();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lkpUrunSecimi_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkpUrunSecimi.EditValue == null) return;

                int selectedProductId = Convert.ToInt32(lkpUrunSecimi.EditValue);
                var selectedProduct = _productsManager.TGetById(selectedProductId);

                if (selectedProduct != null)
                {
                    txtSFiyat.Text = selectedProduct.Price.ToString(); // Satış fiyatını otomatik getir
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
               int salesId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("SaleID"));
                var sale = _salesManager.TGetById(salesId);
                if (sale != null)
                {
                    lkpUrunSecimi.EditValue = sale.ProductID;
                    lkpCariSecim.EditValue = sale.CustomerID;
                    txtSFiyat.Text = sale.Price.ToString();
                    dateSTarihi.EditValue = sale.SaleDate;
                    memoAciklama.EditValue = sale.Description; // Assuming you have a Description field in Sales
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