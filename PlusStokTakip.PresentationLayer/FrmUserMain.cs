﻿using System.Windows.Forms;
using PlusStokTakip.PresentationLayer.User.Modules.Ayarlar;
using PlusStokTakip.PresentationLayer.User.Modules.Urunler;
using PlusStokTakip.PresentationLayer.User.Modules.Cariler;
using PlusStokTakip.PresentationLayer.User.Modules.Stok;
using PlusStokTakip.PresentationLayer.User.Modules.SatinAlma;
using PlusStokTakip.PresentationLayer.User.Modules.Satis;
using PlusStokTakip.PresentationLayer.User.Modules.Finans;
using PlusStokTakip.PresentationLayer.User.Modules.Tanimlar;
using PlusStokTakip.PresentationLayer.User.Modules.Dashboard;

namespace PlusStokTakip.PresentationLayer
{
    public partial class FrmUserMain : DevExpress.XtraEditors.XtraForm
    {
        FrmRaf frmRaf;
        FrmKategori frmKategori;
        FrmMarka frmMarka;
        FrmModel frmModel;
        FrmUrun frmUrun;
        FrmUpdate frmUpdate;
        FrmUser frmUser;
        FrmDataBase frmDataBase;
        FrmMusteriFirma frmMusteriFirma;
        FrmTedarikci frmTedarikci;
        FrmStok frmStok;
        FrmSatinAlma frmSatinAlma;
        FrmSatis FrmSatis;
        FrmBanka FrmBanka;
        FrmKasa FrmKasa;
        FrmOdeme FrmOdeme;
        FrmBankaHesap FrmBankaHesap;
        FrmFirma frmFirma;
        FrmDashboard frmDashboard;
        public string _userName;
        public int _userId;
        public FrmUserMain()
        {
            InitializeComponent();
        }
        private void FrmUserMain_Load(object sender, System.EventArgs e)
        {
            // Form yüklendiğinde Dashboard'u aç
            if (frmDashboard == null || frmDashboard.IsDisposed)
            {
                frmDashboard = new FrmDashboard();
                frmDashboard.MdiParent = this;
                frmDashboard.Show();
            }
            else
            {
                frmDashboard.BringToFront();
            }
        }

        private void btnRaf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmRaf == null || frmRaf.IsDisposed)
            {
                frmRaf = new FrmRaf();
                frmRaf.MdiParent = this;
                frmRaf.Show();
            }
            else
            {
                frmRaf.BringToFront();
            }
        }

        private void btnKategori_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmKategori == null || frmKategori.IsDisposed)
            {
                frmKategori = new FrmKategori();
                frmKategori.MdiParent = this;
                frmKategori.Show();
            }
            else
            {
                frmKategori.BringToFront();
            }
        }

        private void btnMarka_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmMarka == null || frmMarka.IsDisposed)
            {
                frmMarka = new FrmMarka();
                frmMarka.MdiParent = this;
                frmMarka.Show();
            }
            else
            {
                frmMarka.BringToFront();
            }
        }

        private void btnModel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmModel == null || frmModel.IsDisposed)
            {
                frmModel = new FrmModel();
                frmModel.MdiParent = this;
                frmModel.Show();
            }
            else
            {
                frmModel.BringToFront();
            }
        }

        private void btnUrun_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmUrun == null || frmUrun.IsDisposed)
            {
                frmUrun = new FrmUrun();
                frmUrun.MdiParent = this;
                frmUrun.Show();
            }
            else
            {
                frmUrun.BringToFront();
            }
        }

        private void FrmAdminMain_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmUpdate == null || frmUpdate.IsDisposed)
            {
                frmUpdate = new FrmUpdate();
                frmUpdate.MdiParent = this;
                frmUpdate.Show();
            }
            else
            {
                frmUpdate.BringToFront();
            }
        }

        private void btnUser_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmUser == null || frmUser.IsDisposed)
            {
                frmUser = new FrmUser();
                frmUser.MdiParent = this;
                frmUser._userId = _userId; // Kullanıcı ID'sini ayarla
                frmUser.Show();
            }
            else
            {
                frmUser.BringToFront();
            }
        }

        private void btnDataBase_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmDataBase == null || frmDataBase.IsDisposed)
            {
                frmDataBase = new FrmDataBase
                {
                    MdiParent = this
                };
                frmDataBase.Show();
            }
            else
            {
                frmDataBase.BringToFront();
            }
        }

        private void btnCustomer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmMusteriFirma == null || frmMusteriFirma.IsDisposed)
            {
                frmMusteriFirma = new FrmMusteriFirma();
                frmMusteriFirma.MdiParent = this;
                frmMusteriFirma.Show();
            }
            else
            {
                frmMusteriFirma.BringToFront();
            }
        }

        private void BtnTedarikci_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmTedarikci == null || frmTedarikci.IsDisposed)
            {
                frmTedarikci = new FrmTedarikci();
                frmTedarikci.MdiParent = this;
                frmTedarikci.Show();
            }
            else
            {
                frmTedarikci.BringToFront();
            }
        }

        private void btnStok_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmStok == null || frmStok.IsDisposed)
            {
                frmStok = new FrmStok();
                frmStok.MdiParent = this;
                frmStok.Show();
            }
            else
            {
                frmStok.BringToFront();
            }
        }

        private void btnSatinAlma_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmSatinAlma == null || frmSatinAlma.IsDisposed)
            {
                frmSatinAlma = new FrmSatinAlma();
                frmSatinAlma.MdiParent = this;
                frmSatinAlma.Show();
            }
            else
            {
                frmSatinAlma.BringToFront();
            }
        }

        private void btnUrunSatis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FrmSatis == null || FrmSatis.IsDisposed)
            {
                FrmSatis = new FrmSatis();
                FrmSatis.MdiParent = this;
                FrmSatis.Show();
            }
            else
            {
                FrmSatis.BringToFront();
            }
        }

        private void btnOdeme_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FrmOdeme == null || FrmOdeme.IsDisposed)
            {
                FrmOdeme = new FrmOdeme();
                FrmOdeme.MdiParent = this;
                FrmOdeme.Show();
            }
            else
            {
                FrmOdeme.BringToFront();
            }
        }

        private void btnKasa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FrmKasa == null || FrmKasa.IsDisposed)
            {
                FrmKasa = new FrmKasa();
                FrmKasa.MdiParent = this;
                FrmKasa.Show();
            }
            else
            {
                FrmKasa.BringToFront();
            }
        }

        private void btnBanka_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FrmBanka == null || FrmBanka.IsDisposed)
            {
                FrmBanka = new FrmBanka();
                FrmBanka.MdiParent = this;
                FrmBanka.Show();
            }
            else
            {
                FrmBanka.BringToFront();
            }
        }

        private void btnBankaHesap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FrmBankaHesap == null || FrmBankaHesap.IsDisposed)
            {
                FrmBankaHesap = new FrmBankaHesap();
                FrmBankaHesap.MdiParent = this;
                FrmBankaHesap.Show();
            }
            else
            {
                FrmBankaHesap.BringToFront();
            }
        }

        private void btnFirma_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmFirma == null || frmFirma.IsDisposed)
            {
                frmFirma = new FrmFirma();
                frmFirma.MdiParent = this;
                frmFirma.Show();
            }
            else
            {
                frmFirma.BringToFront();
            }
        }

        private void btnDashboard_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (frmDashboard == null || frmDashboard.IsDisposed)
            {
                frmDashboard = new FrmDashboard();
                frmDashboard.MdiParent = this;
                frmDashboard.Show();
            }
            else
            {
                frmDashboard.BringToFront();
            }
        }       
    }
}