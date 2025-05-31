using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Controls
{

    public partial class FancyListBox : ListBox
    {
        public FancyListBox() : base()
        {
            DoubleBuffered = true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            PInvoke.SetWindowTheme(Handle, Application.IsDarkModeEnabled ? "DarkMode_Explorer" : "Explorer", null);
        }
    }
}
