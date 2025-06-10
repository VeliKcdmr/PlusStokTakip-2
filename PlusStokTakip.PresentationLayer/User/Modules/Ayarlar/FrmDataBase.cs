using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace PlusStokTakip.PresentationLayer.User.Modules.Ayarlar
{
    public partial class FrmDataBase : DevExpress.XtraEditors.XtraForm
    {
        private const string ConnectionStringName = "PlusStokTakipEntities";

        public FrmDataBase()
        {
            InitializeComponent();

            // Kimlik doğrulama türlerini ekle
            cmbAuthType.Properties.Items.AddRange(new string[] { "Windows", "SQL Server", "Azure", "LDAP" });

            // Olay dinleyicisi ekle
            cmbAuthType.SelectedIndexChanged += CmbAuthType_SelectedIndexChanged;

            // Form yüklenirken ayarları oku
            LoadSettings();
        }

        private void CmbAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isWindowsAuth = cmbAuthType.SelectedItem.ToString() == "Windows";
            txtUsername.Enabled = !isWindowsAuth;
            txtPassword.Enabled = !isWindowsAuth;

            if (isWindowsAuth)
            {
                txtUsername.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }
        }

        private void LoadSettings()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName]?.ConnectionString;

                if (!string.IsNullOrEmpty(connectionString))
                {
                    // "provider connection string=" kısmını ayıklayarak gerçek SQL bağlantı dizesini al
                    var providerConnectionString = connectionString.Split(new[] { "provider connection string=" }, StringSplitOptions.None).Last().Trim('"');

                    var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(providerConnectionString);

                    txtServerName.Text = builder.DataSource;
                    txtDatabaseName.Text = builder.InitialCatalog;
                    txtUsername.Text = builder.IntegratedSecurity ? "" : builder.UserID;
                    txtPassword.Text = builder.IntegratedSecurity ? "" : builder.Password;
                    cmbAuthType.SelectedItem = builder.IntegratedSecurity ? "Windows" : "SQL Server";

                    CmbAuthType_SelectedIndexChanged(null, null);
                    lblStatus.Text = "Bağlantı ayarları başarıyla yüklendi.";
                }
                else
                {
                    lblStatus.Text = "Bağlantı ayarları bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı ayarlarını yüklerken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateSettings_Click(object sender, EventArgs e)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(configFile.FilePath);

                var connectionStringsNode = xmlDoc.SelectSingleNode("//connectionStrings");
                if (connectionStringsNode == null)
                {
                    MessageBox.Show("Bağlantı ayarları yapılandırma dosyasında bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var newConnectionString = $"metadata=res://*/EntityModel.PSTModel.csdl|res://*/EntityModel.PSTModel.ssdl|res://*/EntityModel.PSTModel.msl;" +
                                          $"provider=System.Data.SqlClient;provider connection string=\"data source={txtServerName.Text};" +
                                          $"initial catalog={txtDatabaseName.Text};" +
                                          $"{(cmbAuthType.SelectedItem.ToString() == "Windows" ? "integrated security=True;" : $"user id={txtUsername.Text};password={txtPassword.Text};")}" +
                                          $"encrypt=False;MultipleActiveResultSets=True;App=EntityFramework\"";

                var connectionNode = connectionStringsNode.SelectSingleNode($"add[@name='{ConnectionStringName}']");
                if (connectionNode != null)
                {
                    connectionNode.Attributes["connectionString"].Value = newConnectionString;
                }
                else
                {
                    var newConnectionElement = xmlDoc.CreateElement("add");
                    newConnectionElement.SetAttribute("name", ConnectionStringName);
                    newConnectionElement.SetAttribute("connectionString", newConnectionString);
                    newConnectionElement.SetAttribute("providerName", "System.Data.EntityClient");
                    connectionStringsNode.AppendChild(newConnectionElement);
                }

                xmlDoc.Save(configFile.FilePath);
                ConfigurationManager.RefreshSection("connectionStrings");

                lblStatus.Text = "Bağlantı ayarları başarıyla güncellendi.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı ayarlarını güncellerken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}