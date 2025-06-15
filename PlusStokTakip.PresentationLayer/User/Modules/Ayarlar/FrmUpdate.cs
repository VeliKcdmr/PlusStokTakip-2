using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.User.Modules.Ayarlar
{
    public partial class FrmUpdate : DevExpress.XtraEditors.XtraForm
    {
        private UpdateInfo updateInfo; // Güncelleme bilgilerini saklamak için
        private static readonly HttpClient client = new HttpClient(); // Tek HttpClient nesnesi

        public FrmUpdate()
        {
            InitializeComponent();
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            btnGuncelle.Enabled = false;
            try
            {
                string currentVersion = Application.ProductVersion;
                lblVDurum.Text = $"{currentVersion}";               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Versiyon bilgisi alınırken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }      


        private async void btnDenetle_Click(object sender, EventArgs e)
        {
            try
            {
                updateInfo = await GetUpdateInfoAsync(); // Güncelleme bilgilerini getir
                lblSDurum.Text = updateInfo?.Version ?? "Bilinmiyor";

                if (updateInfo != null && IsNewVersionAvailable(Application.ProductVersion, updateInfo.Version))
                {
                    btnGuncelle.Enabled = true;
                    lblDurum.Text = "Yeni güncelleme mevcut!";
                }
                else
                {
                    lblDurum.Text = "Güncel versiyon kullanıyorsunuz.";
                    btnGuncelle.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Güncelleme kontrolü sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<UpdateInfo> GetUpdateInfoAsync()
        {
            try
            {
                string updateInfoUrl = "http://95.70.200.104/update-info.json";
                HttpResponseMessage response = await client.GetAsync(updateInfoUrl);

                response.EnsureSuccessStatusCode(); // HTTP yanıt kodunu kontrol et

                string responseData = await response.Content.ReadAsStringAsync(); // JSON verisini al
                return JsonConvert.DeserializeObject<UpdateInfo>(responseData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sunucu versiyon bilgisi alınırken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private bool IsNewVersionAvailable(string currentVersion, string serverVersion)
        {
            try
            {
                return new Version(currentVersion).CompareTo(new Version(serverVersion)) < 0;
            }
            catch
            {
                return false;
            }
        }

        private async void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (updateInfo?.Files == null || updateInfo.Files.Count == 0)
                {
                    MessageBox.Show("Güncelleme dosyası bilgisi alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string updateFileUrl = "http://95.70.200.104/" + updateInfo.Files[0].Path;
                string savePath = Path.Combine(Application.StartupPath, "PlusStokTakip-Update.msi");

                // Güncelleme başlatılmadan önce yeni versiyon bilgisini al ve sakla
                string newVersion = updateInfo.Version;

                lblDurum.Text = "Güncelleme dosyası indiriliyor...";
                await DownloadUpdateFileAsync(updateFileUrl, savePath);

                lblDurum.Text = "Güncelleme dosyası indirildi. Güncelleme başlatılıyor...";
                
                StartUpdate(savePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Güncelleme sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task DownloadUpdateFileAsync(string updateFileUrl, string savePath)
        {
            try
            {
                // Önce eski dosya var mı kontrol edelim, varsa silelim
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }

                HttpResponseMessage response = await client.GetAsync(updateFileUrl, HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode(); // HTTP yanıt kodunu kontrol et

                using (var contentStream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[8192];
                    long totalBytes = response.Content.Headers.ContentLength ?? 0;
                    long totalRead = 0;
                    int read;

                    while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, read);
                        totalRead += read;
                        int progress = totalBytes > 0 ? (int)((totalRead * 100) / totalBytes) : 0;

                        progressBar.Invoke((MethodInvoker)(() => progressBar.Value = progress));
                        lblDDurum.Invoke((MethodInvoker)(() => lblDDurum.Text = $"{progress}%"));
                    }
                }

                MessageBox.Show("Güncelleme dosyası başarıyla indirildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Güncelleme sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartUpdate(string updateFilePath)
        {
            try
            {
                if (!File.Exists(updateFilePath))
                {
                    MessageBox.Show("Güncelleme dosyası bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }               

                // Güncelleme dosyasını çalıştır
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "msiexec.exe",
                    Arguments = $"/i \"{updateFilePath}\" /qb",
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process.Start(psi);

                // Güncelleme başladıktan sonra uygulamayı kapat
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Güncelleme başlatılırken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }       
    }

    public class UpdateInfo
    {
        public string Version { get; set; }
        public List<UpdateFileInfo> Files { get; set; }
    }

    public class UpdateFileInfo
    {
        public string Path { get; set; }
        public string Size { get; set; } // Boyutu artık kontrol etmiyoruz ama sunucudan yine JSON'da bulunacak
    }
}