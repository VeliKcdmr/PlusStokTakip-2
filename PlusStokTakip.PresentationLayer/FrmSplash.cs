using DevExpress.XtraSplashScreen;
using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using System;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer
{
    public partial class FrmSplash : SplashScreen
    {
        private Timer splashTimer; // Timer nesnesi
        ProductsManager _productsManager = new ProductsManager(new EfProductsDal());
        UsersManager _usersManager = new UsersManager(new EfUsersDal());

        public FrmSplash()
        {
            InitializeComponent();
            this.labelCopyright.Text = $"Copyright © 2024-{DateTime.Now.Year}";
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        private void FrmSplash_Load(object sender, EventArgs e)
        {
            try
            {
                // Timer oluştur ve 5 saniyede bir tetiklenmesi için ayarla
                splashTimer = new Timer();
                splashTimer.Interval = 5000; // 5 saniye
                splashTimer.Tick += SplashTimer_Tick; // Tick olayını bağla
                splashTimer.Start(); // Timer'ı başlat

                // Kullanıcı ve ürün listelerini al
                var usersList = _usersManager.TGetAll();
                var productsList = _productsManager.TGetAll();

                // Toplam adım sayısı
                int totalSteps = usersList.Count + productsList.Count;
                int currentStep = 0;

                // Kullanıcı listesini yükle ve ilerleme yüzdesini yazdır
                labelStatus.Text = "Kullanıcı listesi yükleniyor...";
                foreach (var user in usersList)
                {
                    currentStep++;
                    UpdateProgress(currentStep, totalSteps);
                    Application.DoEvents(); // UI güncellemesi için
                    System.Threading.Thread.Sleep(100); // Simülasyon için kısa bir bekleme
                }

                // Ürün listesini yükle ve ilerleme yüzdesini yazdır
                labelStatus.Text = "Ürün listesi yükleniyor...";
                foreach (var product in productsList)
                {
                    currentStep++;
                    UpdateProgress(currentStep, totalSteps);
                    Application.DoEvents(); // UI güncellemesi için
                    System.Threading.Thread.Sleep(100); // Simülasyon için kısa bir bekleme
                }

                // Yükleme tamamlandı
                UpdateProgress(totalSteps, totalSteps); // %100 için güncelle

                // Yükleme tamamlandıktan sonra biraz bekleyin
                System.Threading.Thread.Sleep(1000); // 1 saniye bekle
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yükleme sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Hide(); // Hata durumunda Splash ekranını kapat
            }
        }

        private void UpdateProgress(int currentStep, int totalSteps)
        {
            int percentage = (currentStep * 100) / totalSteps;
            labelStatus.Text = $"Yükleniyor: {percentage}%";
        }

        private void SplashTimer_Tick(object sender, EventArgs e)
        {
            // Timer durdur ve bellekten temizle
            splashTimer.Stop();
            splashTimer.Dispose();

            // Login ekranını aç ve Splash ekranını gizle
            FrmLogin frmLogin = new FrmLogin();
            this.Hide(); // Splash ekranını kapat
            frmLogin.Show();
        }
    }
}
