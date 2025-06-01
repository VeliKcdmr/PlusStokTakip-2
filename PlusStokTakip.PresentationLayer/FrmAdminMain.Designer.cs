namespace PlusStokTakip.PresentationLayer
{
    partial class FrmAdminMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAdminMain));
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnRaf = new DevExpress.XtraBars.BarButtonItem();
            this.btnKategori = new DevExpress.XtraBars.BarButtonItem();
            this.btnMarka = new DevExpress.XtraBars.BarButtonItem();
            this.btnModel = new DevExpress.XtraBars.BarButtonItem();
            this.btnUrun = new DevExpress.XtraBars.BarButtonItem();
            this.btnUpdate = new DevExpress.XtraBars.BarButtonItem();
            this.btnUser = new DevExpress.XtraBars.BarButtonItem();
            this.btnDataBase = new DevExpress.XtraBars.BarButtonItem();
            this.btnCustomer = new DevExpress.XtraBars.BarButtonItem();
            this.BtnTedarikci = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage4 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup9 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup10 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage3 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup6 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup7 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup8 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.MdiParent = this;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.btnRaf,
            this.btnKategori,
            this.btnMarka,
            this.btnModel,
            this.btnUrun,
            this.btnUpdate,
            this.btnUser,
            this.btnDataBase,
            this.btnCustomer,
            this.BtnTedarikci});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 11;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage2,
            this.ribbonPage4,
            this.ribbonPage1,
            this.ribbonPage3});
            this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.MacOffice;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Show;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1022, 133);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Below;
            // 
            // btnRaf
            // 
            this.btnRaf.Caption = "Raf İşlemleri";
            this.btnRaf.Id = 1;
            this.btnRaf.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRaf.ImageOptions.Image")));
            this.btnRaf.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnRaf.ImageOptions.LargeImage")));
            this.btnRaf.Name = "btnRaf";
            this.btnRaf.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRaf_ItemClick);
            // 
            // btnKategori
            // 
            this.btnKategori.Caption = "Kategori İşlemleri";
            this.btnKategori.Id = 2;
            this.btnKategori.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnKategori.ImageOptions.Image")));
            this.btnKategori.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnKategori.ImageOptions.LargeImage")));
            this.btnKategori.Name = "btnKategori";
            this.btnKategori.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnKategori_ItemClick);
            // 
            // btnMarka
            // 
            this.btnMarka.Caption = "Marka İşlemleri";
            this.btnMarka.Id = 3;
            this.btnMarka.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnMarka.ImageOptions.Image")));
            this.btnMarka.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnMarka.ImageOptions.LargeImage")));
            this.btnMarka.Name = "btnMarka";
            this.btnMarka.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMarka_ItemClick);
            // 
            // btnModel
            // 
            this.btnModel.Caption = "Model İşlemleri";
            this.btnModel.Id = 4;
            this.btnModel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnModel.ImageOptions.Image")));
            this.btnModel.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnModel.ImageOptions.LargeImage")));
            this.btnModel.Name = "btnModel";
            this.btnModel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnModel_ItemClick);
            // 
            // btnUrun
            // 
            this.btnUrun.Caption = "Ürün İşlemleri";
            this.btnUrun.Id = 5;
            this.btnUrun.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUrun.ImageOptions.Image")));
            this.btnUrun.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUrun.ImageOptions.LargeImage")));
            this.btnUrun.Name = "btnUrun";
            this.btnUrun.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUrun_ItemClick);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Caption = "Güncelleme";
            this.btnUpdate.Id = 6;
            this.btnUpdate.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.ImageOptions.Image")));
            this.btnUpdate.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUpdate.ImageOptions.LargeImage")));
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUpdate_ItemClick);
            // 
            // btnUser
            // 
            this.btnUser.Caption = "Kullanıcılar";
            this.btnUser.Id = 7;
            this.btnUser.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnUser.ImageOptions.Image")));
            this.btnUser.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUser.ImageOptions.LargeImage")));
            this.btnUser.Name = "btnUser";
            this.btnUser.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUser_ItemClick);
            // 
            // btnDataBase
            // 
            this.btnDataBase.Caption = "Veritabanı Ayarları";
            this.btnDataBase.Id = 8;
            this.btnDataBase.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDataBase.ImageOptions.Image")));
            this.btnDataBase.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnDataBase.ImageOptions.LargeImage")));
            this.btnDataBase.Name = "btnDataBase";
            this.btnDataBase.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDataBase_ItemClick);
            // 
            // btnCustomer
            // 
            this.btnCustomer.Caption = "Müşteri & Firma";
            this.btnCustomer.Id = 9;
            this.btnCustomer.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCustomer.ImageOptions.Image")));
            this.btnCustomer.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnCustomer.ImageOptions.LargeImage")));
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCustomer_ItemClick);
            // 
            // BtnTedarikci
            // 
            this.BtnTedarikci.Caption = "Tedarikçi";
            this.BtnTedarikci.Id = 10;
            this.BtnTedarikci.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnTedarikci.ImageOptions.Image")));
            this.BtnTedarikci.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BtnTedarikci.ImageOptions.LargeImage")));
            this.BtnTedarikci.Name = "BtnTedarikci";
            this.BtnTedarikci.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtnTedarikci_ItemClick);
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup5});
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "Ürünler";
            // 
            // ribbonPageGroup5
            // 
            this.ribbonPageGroup5.ItemLinks.Add(this.btnUrun);
            this.ribbonPageGroup5.Name = "ribbonPageGroup5";
            // 
            // ribbonPage4
            // 
            this.ribbonPage4.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup9,
            this.ribbonPageGroup10});
            this.ribbonPage4.Name = "ribbonPage4";
            this.ribbonPage4.Text = "Cari Yönetimi";
            // 
            // ribbonPageGroup9
            // 
            this.ribbonPageGroup9.ItemLinks.Add(this.btnCustomer);
            this.ribbonPageGroup9.Name = "ribbonPageGroup9";
            this.ribbonPageGroup9.Text = "Müşteri Yönetimi";
            // 
            // ribbonPageGroup10
            // 
            this.ribbonPageGroup10.ItemLinks.Add(this.BtnTedarikci);
            this.ribbonPageGroup10.Name = "ribbonPageGroup10";
            this.ribbonPageGroup10.Text = "Tedarikçi Yönetimi";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4,
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Tanımlar";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnKategori);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.btnMarka);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.btnModel);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnRaf);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            // 
            // ribbonPage3
            // 
            this.ribbonPage3.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup6,
            this.ribbonPageGroup7,
            this.ribbonPageGroup8});
            this.ribbonPage3.Name = "ribbonPage3";
            this.ribbonPage3.Text = "Ayarlar";
            // 
            // ribbonPageGroup6
            // 
            this.ribbonPageGroup6.ItemLinks.Add(this.btnUpdate);
            this.ribbonPageGroup6.Name = "ribbonPageGroup6";
            this.ribbonPageGroup6.Text = "Uygulama Güncelleme";
            // 
            // ribbonPageGroup7
            // 
            this.ribbonPageGroup7.ItemLinks.Add(this.btnUser);
            this.ribbonPageGroup7.Name = "ribbonPageGroup7";
            this.ribbonPageGroup7.Text = "Kullanıcı İşlemleri";
            // 
            // ribbonPageGroup8
            // 
            this.ribbonPageGroup8.ItemLinks.Add(this.btnDataBase);
            this.ribbonPageGroup8.Name = "ribbonPageGroup8";
            this.ribbonPageGroup8.Text = "Veri Tabanı İşlemleri";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 541);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1022, 27);
            // 
            // FrmAdminMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 568);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Name = "FrmAdminMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAdminMain_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem btnRaf;
        private DevExpress.XtraBars.BarButtonItem btnKategori;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem btnMarka;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem btnModel;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarButtonItem btnUrun;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraBars.BarButtonItem btnUpdate;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup6;
        private DevExpress.XtraBars.BarButtonItem btnUser;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup7;
        private DevExpress.XtraBars.BarButtonItem btnDataBase;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup8;
        private DevExpress.XtraBars.BarButtonItem btnCustomer;
        private DevExpress.XtraBars.BarButtonItem BtnTedarikci;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage4;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup9;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup10;
    }
}