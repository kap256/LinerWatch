using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinerWatch
{
    public partial class Form1 : Form
    {
        #region 初期化-----------------------------------------
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.icon.ToBitmap();
            label_build.Text = "Build : " + GetBuildDateStr();
#if !DEBUG
            HideForm();
#endif
        }

        public String GetBuildDateStr()
        {
            var asm = GetType().Assembly;
            var ver = asm.GetName().Version;

            var build = ver.Build;
            var revision = ver.Revision;
            var baseDate = new DateTime(2000, 1, 1);

            return baseDate.AddDays(build).AddSeconds(revision * 2).ToString("yyyy/MM/dd HH:mm");
        }

        #endregion

        #region 起動・終了-----------------------------------------
        private void Notify_Click(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left) {
                ShowForm();
            }
        }
        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button_ok_Click(object sender, EventArgs e)
        {
            HideForm();
        }

        private void ShowForm()
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Opacity = 100;
        }

        private void HideForm()
        {
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Opacity = 0;
        }
        #endregion
    }
}
