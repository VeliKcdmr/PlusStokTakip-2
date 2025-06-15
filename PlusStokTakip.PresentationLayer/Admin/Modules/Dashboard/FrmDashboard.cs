using Newtonsoft.Json.Linq;
using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using System;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace PlusStokTakip.PresentationLayer.Admin.Modules.Dashboard
{
    public partial class FrmDashboard : DevExpress.XtraEditors.XtraForm
    {
        private readonly ProductsManager _productsManager = new ProductsManager(new EfProductsDal());
        private readonly CashRegisterManager _cashRegisterManager = new CashRegisterManager(new EfCashRegisterDal());
        private readonly SalesManager _salesManager = new SalesManager(new EfSalesDal());
        private readonly PurchasesManager _purchasesManager = new PurchasesManager(new EfPurchasesDal());
        private readonly SuppliersManager _suppliersManager = new SuppliersManager(new EfSuppliersDal());
        private readonly CustomersManager _customersManager = new CustomersManager(new EfCustomersDal());
        private readonly BrandsManager _brandsManager = new BrandsManager(new EfBrandsDal());
        private readonly CategoriesManager _categoriesManager = new CategoriesManager(new EfCategoriesDal());

        public FrmDashboard()
        {
            InitializeComponent();
        }

        private async void FrmDashboard_Load(object sender, EventArgs e)
        {
            LoadExchangeRates();
            await GetWeatherData();
            LoadFinancialData();
            loadLastSalesData(); // Son 10 satış verisini yükle
            loadLastPurchaseData(); // Son 10 alış verisini yükle
            loadLastProductsData(); // 
            loadlastCashRegisterData(); // Son 10 kasa işlemini yükle

            lblTarih.Text = DateTime.Now.ToString("d MMMM yyyy dddd", new System.Globalization.CultureInfo("tr-TR"));
            lblSaat.Text = DateTime.Now.ToString("HH:mm:ss");

            timer1.Interval = 1000; // Her saniye güncelle
            timer1.Tick += timer1_Tick;
            timer1.Start();

            timer2.Interval = 3600000; // Her 1 saat hava durumu güncellensin
            timer2.Tick += async (s, ev) => await GetWeatherData(); // `timer2` içine hava durumu çekme eklendi!
            timer2.Start();

            timer3.Interval = 300000; // Her 5 dakika bir güncelle
            timer3.Tick += (s, ev) => LoadFinancialData();
            timer3.Start();
        }
        private void loadlastCashRegisterData()
        {
            try
            {
                var transactions = _cashRegisterManager.TGetAll().Where(t => t.IsActive == true).OrderByDescending(t => t.TransactionDate).Take(10).Select(t => new
                {
                    t.CashID,
                    t.TransactionDate,
                    t.TransactionType,
                    Amount = $"{t.Amount:#,##0.00} ₺", // Formatlı yazım
                    t.Description
                }).ToList();
                gridControl2.DataSource = transactions;
                gridView2.Columns["CashID"].Visible = false; // Hide TransactionID column
                gridView2.Columns["TransactionDate"].Caption = "İşlem Tarihi";
                gridView2.Columns["TransactionType"].Caption = "İşlem Türü";
                gridView2.Columns["Amount"].Caption = "Tutar";
                gridView2.Columns["Description"].Caption = "Açıklama";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kasa verileri alınamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadLastProductsData()
        {
            try
            {
                var brands = _brandsManager.TGetAll().Where(b => b.IsActive == true).ToList(); // Aktif markaları al
                var categories = _categoriesManager.TGetAll().Where(c => c.IsActive == true).ToList(); // Aktif kategorileri al
                var products = _productsManager.TGetAll().Where(p => p.IsActive == true && p.StockQuantity > 0 && p.StockQuantity < 10).Select(p => new
                {
                    p.ProductID,
                    Categorie = categories.FirstOrDefault(c => c.CategoryID == p.ProductID)?.CategoryName ?? "Bulunamadı",
                    Brand = brands.FirstOrDefault(b => b.BrandID == p.BrandID)?.BrandName ?? "Bulunamadı",
                    p.ProductName,
                    p.Barcode,
                    p.StockQuantity,
                }).OrderBy(p=>p.StockQuantity).ToList();

                gridControl1.DataSource = products;
                gridView1.Columns["ProductID"].Visible = false; // Hide ProductID column
                gridView1.Columns["Categorie"].Caption = "Kategori";
                gridView1.Columns["Brand"].Caption = "Marka";
                gridView1.Columns["ProductName"].Caption = "Ürün Adı";
                gridView1.Columns["Barcode"].Caption = "Barkod";
                gridView1.Columns["StockQuantity"].Caption = "Adet";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ürün verileri alınamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadLastSalesData()
        {
            try
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
                var sales = _salesManager.TGetAll().Where(s => s.IsActive == true).OrderByDescending(s => s.SaleDate).Take(10).Select(s => new
                {
                    s.SaleID,
                    s.SaleDate,
                    CustomerName = customers.FirstOrDefault(c => c.CustomerID == s.CustomerID)?.CustomerName ?? "Bulunamadı",
                    CompanyName = customers.FirstOrDefault(c => c.CustomerID == s.CustomerID)?.CompanyName ?? "Bulunamadı",
                    ProductName = products.FirstOrDefault(p => p.ProductID == s.ProductID)?.ProductName ?? "Bulunamadı",
                    Barcode = products.FirstOrDefault(p => p.ProductID == s.ProductID)?.Barcode ?? "Bulunamadı",
                    s.Quantity,
                    s.Price,
                    s.TotalRevenue
                }).ToList();
                gridControl3.DataSource = sales;
                gridView3.Columns["SaleID"].Visible = false; // Hide SaleID column
                gridView3.Columns["SaleDate"].Caption = "Satış Tarihi";
                gridView3.Columns["CustomerName"].Caption = "Müşteri Adı";
                gridView3.Columns["CompanyName"].Caption = "Şirket Adı";
                gridView3.Columns["ProductName"].Caption = "Ürün Adı";
                gridView3.Columns["Barcode"].Caption = "Barkod";
                gridView3.Columns["Quantity"].Caption = "Adet";
                gridView3.Columns["Price"].Caption = "Fiyat";
                gridView3.Columns["TotalRevenue"].Caption = "Toplam Tutar";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Satış verileri alınamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadLastPurchaseData()
        {
            try
            {
                var products = _productsManager.TGetAll().Where(p => p.IsActive == true).Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Barcode
                }).ToList();
                var suppliers = _suppliersManager.TGetAll().Where(s => s.IsActive == true).Select(s => new
                {
                    s.SupplierID,
                    s.SupplierName,
                    s.ContactPerson
                }).ToList();
                var purchases = _purchasesManager.TGetAll().Where(p => p.IsActive == true).OrderByDescending(p => p.PurchaseDate).Take(10).Select(p => new
                {
                    p.PurchaseID,
                    p.PurchaseDate,
                    SupplierName = suppliers.FirstOrDefault(s => s.SupplierID == p.SupplierID)?.SupplierName ?? "Bulunamadı",
                    ContactPerson = suppliers.FirstOrDefault(s => s.SupplierID == p.SupplierID)?.ContactPerson ?? "Bulunamadı",
                    ProductName = products.FirstOrDefault(pr => pr.ProductID == p.ProductID)?.ProductName ?? "Bulunamadı",
                    Barcode = products.FirstOrDefault(pr => pr.ProductID == p.ProductID)?.Barcode ?? "Bulunamadı",
                    p.Quantity,
                    Cost = $"{p.Cost:#,##0.00} ₺",  // Formatlı yazım
                    TotalCost = $"{p.TotalCost:#,##0.00} ₺"
                }).ToList();
                gridControl4.DataSource = purchases;
                gridView4.Columns["PurchaseID"].Visible = false; // Hide PurchaseID column
                gridView4.Columns["PurchaseDate"].Caption = "Alış Tarihi";
                gridView4.Columns["SupplierName"].Caption = "Tedarikçi Adı";
                gridView4.Columns["ContactPerson"].Caption = "Yetkili Adı";
                gridView4.Columns["ProductName"].Caption = "Ürün Adı";
                gridView4.Columns["Barcode"].Caption = "Barkod";
                gridView4.Columns["Quantity"].Caption = "Adet";
                gridView4.Columns["Cost"].Caption = "Fiyat";
                gridView4.Columns["TotalCost"].Caption = "Toplam Tutar";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Alış verileri alınamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadExchangeRates()
        {
            try
            {
                string url = "https://www.tcmb.gov.tr/kurlar/today.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(url);

                XmlNode usdNode = xmlDoc.SelectSingleNode("//Currency[@Kod='USD']");
                XmlNode eurNode = xmlDoc.SelectSingleNode("//Currency[@Kod='EUR']");

                if (usdNode != null && eurNode != null)
                {
                    string usdBuy = usdNode.SelectSingleNode("BanknoteBuying")?.InnerText ?? "Veri Yok";
                    string usdSell = usdNode.SelectSingleNode("BanknoteSelling")?.InnerText ?? "Veri Yok";
                    string eurBuy = eurNode.SelectSingleNode("BanknoteBuying")?.InnerText ?? "Veri Yok";
                    string eurSell = eurNode.SelectSingleNode("BanknoteSelling")?.InnerText ?? "Veri Yok";

                    lblDolar.Text = $"Dolar: {usdBuy} ₺ / {usdSell} ₺";
                    lblEuro.Text = $"Euro: {eurBuy} ₺ / {eurSell} ₺";
                }
                else
                {
                    MessageBox.Show("Döviz bilgileri alınamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblSaat.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private async Task GetWeatherData()
        {
            try
            {
                string apiKey = "f372f3d8f6254ef396493004251406"; // WeatherAPI'den alınan API anahtarını buraya ekle
                string city = "Mersin";
                string url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}&lang=tr";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    JObject weatherData = JObject.Parse(responseBody);

                    lblSicaklik.Text = $"Sıcaklık: {weatherData["current"]["temp_c"]}°C";
                    lblDurum.Text = $"Durum: {weatherData["current"]["condition"]["text"].ToString()}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hava durumu verisi alınamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFinancialData()
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                var transactions = _cashRegisterManager.TGetAll();
                if (transactions == null || !transactions.Any())
                {
                    lblGunlukGelir.Text = "Günlük Gelir: Veri Yok";
                    lblGunlukGider.Text = "Günlük Gider: Veri Yok";
                    lblAylikGelir.Text = "Aylık Gelir: Veri Yok";
                    lblAylikGider.Text = "Aylık Gider: Veri Yok";
                    return;
                }


                // Günlük gelir ve giderleri hesapla
                decimal dailyIncome = _cashRegisterManager.TGetAll()
                    .Where(t => t.TransactionDate.Date == today && t.TransactionType == "Gelir" && t.IsActive == true)
                    .Sum(t => t.Amount);

                decimal dailyExpense = _cashRegisterManager.TGetAll()
                    .Where(t => t.TransactionDate.Date == today && t.TransactionType == "Gider" && t.IsActive == true)
                    .Sum(t => t.Amount);

                // Aylık gelir ve giderleri hesapla
                decimal monthlyIncome = _cashRegisterManager.TGetAll()
                    .Where(t => t.TransactionDate >= firstDayOfMonth && t.TransactionType == "Gelir" && t.IsActive == true)
                    .Sum(t => t.Amount);

                decimal monthlyExpense = _cashRegisterManager.TGetAll()
                    .Where(t => t.TransactionDate >= firstDayOfMonth && t.TransactionType == "Gider" && t.IsActive == true)
                    .Sum(t => t.Amount);

                // `Label`'lara yazdır
                lblGunlukGelir.Text = $"Günlük Gelir: {dailyIncome:#,##0.00} ₺";
                lblGunlukGider.Text = $"Günlük Gider: {dailyExpense:#,##0.00} ₺";
                lblAylikGelir.Text = $"Aylık Gelir: {monthlyIncome:#,##0.00} ₺";
                lblAylikGider.Text = $"Aylık Gider: {monthlyExpense:#,##0.00} ₺";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler alınamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}