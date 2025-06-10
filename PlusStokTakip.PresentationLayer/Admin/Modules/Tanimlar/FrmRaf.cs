using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using StokTakipApp.PresentationLayer.Helpers;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer.Admin.Modules.Tanimlar
{
    public partial class FrmRaf : DevExpress.XtraEditors.XtraForm
    {
        ShelvesManager _shelvesManager = new ShelvesManager(new EfShelvesDal());
        public FrmRaf()
        {
            InitializeComponent();
        }
        private void LoadShelves()
        {
            var shelvesList = _shelvesManager.TGetAll().Select(s => new
            {
                s.ShelfID,
                s.ShelfName,
                s.ShelfDescription,
                IsActive = s.IsActive ? "Aktif" : "Pasif"
            });
            gridControl1.DataSource = shelvesList; // GridControl'e bağlar
            gridView1.Columns["ShelfName"].Caption = "Raf Adı"; // Sütun başlıklarını değiştir
            gridView1.Columns["ShelfDescription"].Caption = "Açıklama";
            gridView1.Columns["IsActive"].Caption = "Durum";
            gridView1.Columns["ShelfID"].Caption = "Raf ID";
            // gridView1.Columns["ShelfID"].Visible = false; // "Id" sütununu gizle
        }
        private void FrmRaf_Load(object sender, EventArgs e)
        {
            freshForm();
        }
        private void CleanFields()
        {
            txtAd.Text = string.Empty;
            txtAciklama.Text = string.Empty;
            tsDurum.IsOn = true;
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
            btnRafYazdir.Enabled = false;
        }
        private void freshForm()
        {
            LoadShelves();
            CleanFields();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidationHelper.ValidateControl(txtAd, "Raf adı boş bırakılamaz!")) return;
                var existingShelf = _shelvesManager.TGetAll()
                    .FirstOrDefault(s => s.ShelfName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && s.IsActive);
                if (existingShelf != null)
                {
                    MessageBox.Show($"'{txtAd.Text.Trim()}' adlı raf zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Yeni raf oluştur
                Shelves newShelf = new Shelves
                {
                    ShelfName = txtAd.Text.Trim(),
                    ShelfDescription = txtAciklama.Text.Trim(),
                    IsActive = tsDurum.IsOn // Durumu aktif olarak ayarla
                };

                // Kaydetme işlemi
                _shelvesManager.TInsert(newShelf);
                MessageBox.Show("Raf başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                freshForm();

            }
            catch (Exception ex)
            {
                // Hata yönetimi
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var existingShelf = _shelvesManager.TGetAll()
                    .FirstOrDefault(s => s.ShelfName.Equals(txtAd.Text.Trim(), StringComparison.OrdinalIgnoreCase) && s.IsActive);
                if (existingShelf != null)
                {
                    MessageBox.Show($"'{txtAd.Text.Trim()}' adlı raf zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedShelfId = (int)gridView1.GetFocusedRowCellValue("ShelfID");
                var selectedShelf = _shelvesManager.TGetById(selectedShelfId);

                if (selectedShelf != null)
                {
                    // Kullanıcı girdilerini raf objesine uygula
                    selectedShelf.ShelfName = txtAd.Text.Trim();
                    selectedShelf.ShelfDescription = txtAciklama.Text.Trim();
                    selectedShelf.IsActive = tsDurum.IsOn;

                    // Rafı güncelle
                    _shelvesManager.TUpdate(selectedShelf);

                    MessageBox.Show("Raf başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    freshForm();
                }
                else
                {
                    MessageBox.Show("Seçilen raf bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var dialogResult = MessageBox.Show("Rafı silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    var selectedShelfId = (int)gridView1.GetFocusedRowCellValue("ShelfID");
                    var selectedShelf = _shelvesManager.TGetById(selectedShelfId);

                    if (selectedShelf != null)
                    {
                        selectedShelf.IsActive = false; // Rafın durumunu pasif yap
                        _shelvesManager.TUpdate(selectedShelf); // Rafı güncelle

                        MessageBox.Show("Raf başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        freshForm();
                    }
                    else
                    {
                        MessageBox.Show("Seçilen raf bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                return; // Kullanıcı silme işlemini iptal etti

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                MessageBox.Show("Lütfen bir satır seçiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedShelfId = (int)gridView1.GetFocusedRowCellValue("ShelfID");
            var shelf = _shelvesManager.TGetById(selectedShelfId);

            if (shelf == null)
            {
                MessageBox.Show("Seçilen raf bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtAd.Text = shelf.ShelfName;
            txtAciklama.Text = shelf.ShelfDescription;
            tsDurum.IsOn = shelf.IsActive;

            btnKaydet.Enabled = false;
            btnGuncelle.Enabled = true;
            btnSil.Enabled = true;
            btnRafYazdir.Enabled = true;
        }
        private void btnRafYazdir_Click(object sender, EventArgs e)
        {
            try
            {
                // PrintDocument oluştur
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += printDocument_PrintPage; // Yazdırma olayını bağla

                // Yazıcı seçim penceresi oluştur
                PrintDialog printDialog = new PrintDialog
                {
                    Document = printDocument // PrintDocument bağlanır
                };

                // Yazıcı seçim penceresini göster
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    // Seçilen yazıcı ayarlarını uygula
                    printDocument.PrinterSettings = printDialog.PrinterSettings;

                    // Yazdırma işlemini başlat
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yazdırma sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Yazdırılacak bilgiler
            string shelfName = txtAd.Text.Trim();
            string shelfDescription = txtAciklama.Text.Trim();

            // Font tanımlamaları
            Font boldFont = new Font("Arial", 14, FontStyle.Bold); // Raf adı için kalın font
            Font regularFont = new Font("Arial", 10, FontStyle.Regular); // Açıklama için küçük ve normal font

            // Pozisyon tanımları
            float x = 100;
            float y = 100;

            // Yazdırma işlemi
            e.Graphics.DrawString($"{shelfName}", boldFont, Brushes.Black, x, y); // Raf adı kalın yazdırılır
            e.Graphics.DrawString($"{shelfDescription}", regularFont, Brushes.Black, x, y + 30); // Açıklama küçük yazdırılır

        }
    }
}