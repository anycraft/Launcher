using System;
using System.Net;
using System.Windows.Forms;
using FreeLauncher.Forms;
using Telerik.WinControls;

namespace FreeLauncher
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //Configuration configuration = new Configuration(args);
            ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LauncherForm(configuration));
            //configuration.SaveConfiguration();
            VersionCheck(args);
        }

        public static bool UpdateLauncher = false;

        private static void VersionCheck(string[] args)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string UIWebVersion = client.DownloadString("https://anycraft.github.io/Launcher/Version.txt");
                    string UICurrentVerion = Application.ProductVersion;
                    if (UIWebVersion != UICurrentVerion)
                    {
                        string UIUpdateFound = string.Format("An update is available\nYour Current version is: {0}\nNew Version is: {1}\n\nDo you want to update?", UICurrentVerion, UIWebVersion);
                        DialogResult UIUpdaterChecker = MessageBox.Show(UIUpdateFound, "New Update Found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        if (UIUpdaterChecker == DialogResult.Yes)
                        {
                            UpdateLauncher = true;
                            new Forms.UpdateForm.UpdaterForm().ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Configuration configuration = new Configuration(args);
            Application.Run(new LauncherForm(configuration));
            configuration.SaveConfiguration();
        }
    }
}
