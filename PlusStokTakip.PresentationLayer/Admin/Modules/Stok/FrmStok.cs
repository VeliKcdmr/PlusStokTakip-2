using PlusStokTakip.BusinessLayer.Concrete;
using PlusStokTakip.DataAccessLayer.EntityFramework;
using PlusStokTakip.EntityLayer.EntityModel;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace PlusStokTakip.PresentationLayer.Admin.Modules.Stok
{
    public partial class FrmStok : DevExpress.XtraEditors.XtraForm
    {
        private readonly ProductsManager _productsManager = new ProductsManager(new EfProductsDal());
        private readonly StockMovementsManager _stockMovementsManager = new StockMovementsManager(new EfStockMovementsDal());
        public FrmStok()
        {
            InitializeComponent();
        }
        private void FrmStok_Load(object sender, EventArgs e)
        {
            cleanForm();
        }

        private void cleanForm()
        {
            lkpUrunSecimi.EditValue = null;
            lkpUrunSecimi.Properties.NullText = "Ürün Seçiniz"; // Reset the placeholder text
            txtMevcutStok.Text = string.Empty;
            spnMiktar.Value = 0;
            cmbHareketTuru.SelectedIndex = -1; // Reset the selected index of the combo box
            cmbHareketTuru.Properties.NullText = "Hareket Türü Seçiniz"; // Reset the placeholder text for the combo box
            dateHareketTarihi.DateTime = DateTime.Now.Date; // Set to current date
            memoAciklama.Text = string.Empty;
            lblKritikUyari.Text = string.Empty;
            lblKIcon.ImageOptions.Image = null; // Clear the icon image
            btnKaydet.Enabled = true;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;
            loadProduct();
            loadStockHistory();
        }

        private void loadProduct()
        {
            var products = _productsManager.TGetAll()
                .Select(p => new { p.ProductID, p.ProductName, p.Barcode })
                .ToList();

            lkpUrunSecimi.Properties.DataSource = products;
            lkpUrunSecimi.Properties.ValueMember = "ProductID";
            lkpUrunSecimi.Properties.DisplayMember = "ProductName";
            lkpUrunSecimi.Properties.NullText = "Ürün Seçiniz"; // Placeholder text for the lookup editor
            lkpUrunSecimi.Properties.PopulateColumns();
            lkpUrunSecimi.Properties.Columns["ProductID"].Visible = false; // Hide ProductID column
            lkpUrunSecimi.Properties.Columns["Barcode"].Caption = "Barkod"; // Rename Barcode column caption
            lkpUrunSecimi.Properties.Columns["ProductName"].Caption = "Ürün Adı"; // Rename ProductName column caption
        }
        private void loadStockHistory()
        {
            var products = _productsManager.TGetAll()
                .Select(p => new { p.ProductID, p.ProductName, p.Barcode }).ToList();
            var stockMovements = _stockMovementsManager.TGetAll().Select(sm => new
            {
                sm.MovementID,
                sm.ProductID,
                ProductName = products.FirstOrDefault(p => p.ProductID == sm.ProductID)?.ProductName ?? "Bulunamadı",
                Barcode = products.FirstOrDefault(p => p.ProductID == sm.ProductID)?.Barcode ?? "Bulunamadı",
                sm.Quantity,
                sm.MovementType,
                sm.MovementDate,
                sm.Description,
            }).ToList();
            gridControl1.DataSource = stockMovements;
            gridView1.Columns["MovementID"].Visible = false; // Hide StockMovementID column
            gridView1.Columns["ProductID"].Visible = false; // Hide ProductID column
            gridView1.Columns["ProductName"].Caption = "Ürün Adı"; // Rename ProductName column caption
            gridView1.Columns["Barcode"].Caption = "Barkod"; // Rename Barcode column caption
            gridView1.Columns["Quantity"].Caption = "Miktar"; // Rename Quantity column caption
            gridView1.Columns["MovementType"].Caption = "Hareket Türü"; // Rename MovementType column caption
            gridView1.Columns["MovementDate"].Caption = "Tarih"; // Rename Date column caption
            gridView1.Columns["Description"].Caption = "Açıklama"; // Rename Description column caption
        }
        private void lkpUrunSecimi_EditValueChanged(object sender, EventArgs e)
        {
            if (lkpUrunSecimi.EditValue == null) return;
            int selectedProductId = Convert.ToInt32(lkpUrunSecimi.EditValue);
            var selectedProduct = _productsManager.TGetById(selectedProductId);
            var mevcutStok = selectedProduct.StockQuantity;
            if (selectedProduct != null)
            {
                txtMevcutStok.Text = mevcutStok.ToString();
                if (mevcutStok == 0)
                {
                    lblKIcon.ImageOptions.Image = imageCollection1.Images[1];
                    lblKritikUyari.Text = "Stok Yok";
                    lblKritikUyari.ForeColor = Color.Red;
                }
                else if (mevcutStok < 10)
                {
                    lblKIcon.ImageOptions.Image = imageCollection1.Images[2];
                    lblKritikUyari.Text = "Kritik Stok!";
                    lblKritikUyari.ForeColor = Color.Orange;
                }
                else if (mevcutStok >= 10)
                {
                    lblKIcon.ImageOptions.Image = imageCollection1.Images[0];
                    lblKritikUyari.Text = "Stok İyi";
                    lblKritikUyari.ForeColor = Color.Green;
                }

            }
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

                {
                    var movement = new StockMovements
                    {
                        ProductID = Convert.ToInt32(lkpUrunSecimi.EditValue),
                        Quantity = Convert.ToInt32(spnMiktar.Value),
                        MovementType = cmbHareketTuru.Text,
                        MovementDate = dateHareketTarihi.DateTime.Date,
                        Description = memoAciklama.Text,
                        IsActive = true
                    };

                    _stockMovementsManager.TInsert(movement);

                    // Stok miktarını güncelle
                    var productID = Convert.ToInt32(lkpUrunSecimi.EditValue);
                    var selectedProduct = _productsManager.TGetById(productID);
                    if (selectedProduct != null)
                    {
                        switch (movement.MovementType.ToString())
                        {
                            case "Satın Alma":
                                selectedProduct.StockQuantity += movement.Quantity;
                                break;
                            case "İade":
                                selectedProduct.StockQuantity += movement.Quantity;
                                break;
                            case "Satış":
                                if (selectedProduct.StockQuantity < movement.Quantity)
                                {
                                    MessageBox.Show("Hata: Stok miktarı yetersiz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                selectedProduct.StockQuantity -= movement.Quantity;
                                break;
                            case "Stok Düzeltme":
                                selectedProduct.StockQuantity = movement.Quantity;
                                break;
                        }

                        _productsManager.TUpdate(selectedProduct);
                    }
                    MessageBox.Show("Stok hareketi başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cleanForm();
                }
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
                    MessageBox.Show("Lütfen güncellemek için bir stok hareketi seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int movementId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("MovementID"));
                var movement = _stockMovementsManager.TGetById(movementId);

                if (movement != null)
                {
                    movement.ProductID = Convert.ToInt32(lkpUrunSecimi.EditValue);
                    movement.Quantity = Convert.ToInt32(spnMiktar.Value);
                    movement.MovementType = cmbHareketTuru.Text;
                    movement.MovementDate = dateHareketTarihi.DateTime;
                    movement.Description = memoAciklama.Text;

                    _stockMovementsManager.TUpdate(movement);

                    var productID = Convert.ToInt32(lkpUrunSecimi.EditValue);
                    var selectedProduct = _productsManager.TGetById(productID);
                    if (selectedProduct != null)
                    {
                        switch (movement.MovementType.ToString())
                        {
                            case "Satın Alma":
                                selectedProduct.StockQuantity += movement.Quantity;
                                break;
                            case "İade":
                                selectedProduct.StockQuantity += movement.Quantity;
                                break;
                            case "Satış":
                                if (selectedProduct.StockQuantity < movement.Quantity)
                                {
                                    MessageBox.Show("Hata: Stok miktarı yetersiz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                selectedProduct.StockQuantity -= movement.Quantity;
                                break;
                            case "Stok Düzeltme":
                                selectedProduct.StockQuantity = movement.Quantity;
                                break;
                        }

                        _productsManager.TUpdate(selectedProduct);
                    }

                    MessageBox.Show("Stok hareketi başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cleanForm();
                }
                else
                {
                    MessageBox.Show("Hata: Güncellenecek stok hareketi bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (lkpUrunSecimi.EditValue == null)
                {
                    MessageBox.Show("Lütfen silinecek ürünü seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var selectedProductId = Convert.ToInt32(lkpUrunSecimi.EditValue);
                var selectedProduct = _productsManager.TGetById(selectedProductId);
                if (selectedProduct != null)
                {
                    selectedProduct.IsActive = false; // Soft delete
                    _productsManager.TUpdate(selectedProduct);
                    MessageBox.Show("Ürün başarıyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cleanForm();
                }
                else
                {
                    MessageBox.Show("Ürün bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (gridView1.FocusedRowHandle >= 0)
                {
                    var selectedRow = gridView1.GetFocusedRow();
                    if (selectedRow != null)
                    {
                        int movementID = Convert.ToInt32(gridView1.GetFocusedRowCellValue("MovementID"));
                        var selectedMovement = _stockMovementsManager.TGetById(movementID);
                        if (selectedMovement != null)
                        {
                            lkpUrunSecimi.EditValue = selectedMovement.ProductID;
                            spnMiktar.EditValue = selectedMovement.Quantity;
                            cmbHareketTuru.Text = selectedMovement.MovementType;
                            dateHareketTarihi.DateTime =(DateTime)selectedMovement.MovementDate;  
                            memoAciklama.Text = selectedMovement.Description;
                            btnKaydet.Enabled = false;
                            btnGuncelle.Enabled = true;
                            btnSil.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            if (gridView1.RowCount == 0)
            {
                string message = "Veri Bulunamadı";
                Font messageFont = new Font("Tahoma", 12, FontStyle.Bold);
                Rectangle rect = e.Bounds;
                e.Graphics.DrawString(message, messageFont, Brushes.Gray, rect, new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                });
            }
        }
    }
}