using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Data;

namespace FreeLauncher.Forms.UpdateForm
{
    public partial class UpdaterForm : RadForm
    {
        public UpdaterForm()
        {
            InitializeComponent();
            DownloadProgressbar.ProgressBarElement.IndicatorElement1.BackColor = Color.Lime;
            DownloadLauncherUpdate();
        }

        private async void DownloadLauncherUpdate()
        {
            try
            {
                if (Program.UpdateLauncher)
                {
                    using (WebClient WebC = new WebClient())
                    {
                        Directory.CreateDirectory("tmp");
                        WebC.DownloadProgressChanged += WebC_DownloadProgressChanged;
                        WebC.DownloadDataCompleted += new DownloadDataCompletedEventHandler(WebC_DownloadUICompleted);
                        string ExeDownloadLocation = WebC.DownloadString("https://anycraft.github.io/Launcher/DownloadLocation.txt");
                        var data = await WebC.DownloadDataTaskAsync(new Uri(ExeDownloadLocation));
                        File.WriteAllBytes(@".\tmp\Launcher.exe", data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void WebC_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) => DownloadProgressbar.Value1 = e.ProgressPercentage;

        private void WebC_DownloadUICompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                MessageBox.Show("Download Completed\nUpdater Will now Close and Open Launcher", "Download Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new Thread(() =>
                {
                    Process.Start("cmd.exe",
                    "/C Del " + Application.ExecutablePath + @"& MOVE /Y .\tmp\Launcher.exe .\ & rmdir .\tmp & start Launcher.exe");
                    Environment.Exit(0);
                }).Start();
            }
        }
    }
}
