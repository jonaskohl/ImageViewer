using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JK.ImageViewer
{
    public partial class SplashScreen : Form
    {
        bool preventClose = true;

        public SplashScreen()
        {
            InitializeComponent();
            Application.DoEvents();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing && preventClose)
                e.Cancel = true;
        }

        public void ForceClose()
        {
            preventClose = false;
            Close();
        }
    }
}
