using PlusStokTakip.PresentationLayer.Admin.Modules.Ayarlar;
using PlusStokTakip.PresentationLayer.Admin.Modules.Defines;
using PlusStokTakip.PresentationLayer.Admin.Modules.Urunler;
using System.Windows.Forms;

namespace PlusStokTakip.PresentationLayer
{
    public partial class FrmAdminMain : DevExpress.XtraEditors.XtraForm
    {
        FrmRaf frmRaf;
        FrmKategori frmKategori;
        FrmMarka frmMarka;
        FrmModel frmModel;
        FrmUrun frmUrun;
        FrmUpdate frmUpdate;
        FrmUser frmUser;
        FrmDataBase frmDataBase;
        public FrmAdminMain()
        {
            InitializeComponent();
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
    }
}