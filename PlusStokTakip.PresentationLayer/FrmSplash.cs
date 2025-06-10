using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;

namespace PlusStokTakip.PresentationLayer
{
    public partial class FrmSplash : SplashScreen
    {
        private readonly ProductsManager _productsManager = new ProductsManager(new EfProductsDal());
        private readonly PaymentsManager _paymentsManager = new PaymentsManager(new EfPaymentsDal());
        private readonly SalesManager _salesManager = new SalesManager(new EfSalesDal());
        private readonly CategoriesManager _categoriesManager = new CategoriesManager(new EfCategoriesDal());
        private readonly BrandsManager _brandsManager = new BrandsManager(new EfBrandsDal());
        private readonly SuppliersManager _suppliersManager = new SuppliersManager(new EfSuppliersDal());
        private readonly CustomersManager _customersManager = new CustomersManager(new EfCustomersDal());
        private readonly ModelsManager _modelsManager = new ModelsManager(new EfModelsDal());
        private readonly UsersManager _usersManager = new UsersManager(new EfUsersDal());

        public FrmSplash()
        {
            InitializeComponent();
            this.labelCopyright.Text = $"Copyright © 2024-{DateTime.Now.Year}";
        }

        private async void FrmSplash_Load(object sender, EventArgs e)
        {
            try
            {
                labelStatus.Text = "Yükleme başlatılıyor...";
                Application.DoEvents();

                var usersList = _usersManager.TGetAll();
                var productsList = _productsManager.TGetAll();
                var paymentsList = _paymentsManager.TGetAll();
                var suppliersList = _suppliersManager.TGetAll();
                var customersList = _customersManager.TGetAll();


                int totalSteps = usersList.Count + productsList.Count + paymentsList.Count +
                                 suppliersList.Count + customersList.Count;
                int currentStep = 0;

                currentStep = await LoadDataAsync(usersList, "Kullanıcı listesi yükleniyor...", currentStep, totalSteps);
                currentStep = await LoadDataAsync(productsList, "Ürün listesi yükleniyor...", currentStep, totalSteps);
                currentStep = await LoadDataAsync(paymentsList, "Ödeme listesi yükleniyor...", currentStep, totalSteps);
                currentStep = await LoadDataAsync(suppliersList, "Tedarikçi listesi yükleniyor...", currentStep, totalSteps);
                currentStep = await LoadDataAsync(customersList, "Müşteri listesi yükleniyor...", currentStep, totalSteps);
                labelStatus.Text = "Yükleme tamamlandı, uygulama başlatılıyor...";
                UpdateProgress(totalSteps, totalSteps);
                await Task.Delay(1000); // 1 saniye beklet

                OpenLoginScreen();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yükleme sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private async Task<int> LoadDataAsync<T>(IEnumerable<T> list, string message, int currentStep, int totalSteps)
        {
            labelStatus.Text = message;
            foreach (var item in list)
            {
                currentStep++; // Artırımı doğrudan burada yapıyoruz
                UpdateProgress(currentStep, totalSteps);
                Application.DoEvents();
                await Task.Delay(100); // UI güncellenirken donmayı önlemek için asenkron bekleme
            }
            return currentStep; // Güncellenmiş değeri geri döndür
        }

        private void UpdateProgress(int currentStep, int totalSteps)
        {
            int percentage = (currentStep * 100) / totalSteps;
            labelStatus.Text = $"Yükleniyor: {percentage}%";
        }

        private void OpenLoginScreen()
        {
            this.Hide();
            using (FrmLogin frmLogin = new FrmLogin())
            {
                frmLogin.ShowDialog();
            }
        }
    }
}