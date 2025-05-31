using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace JK.ImageViewer.Controls
{
    public class SuffixableNumericUpDown : NumericUpDown
    {
        protected string _suffix = "%";
        [Category("Appearance")]
        [DefaultValue("%")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Suffix
        {
            get => _suffix;
            set
            {
                _suffix = value;
                UpdateEditText();
            }
        }

        protected override void UpdateEditText()
        {
            base.UpdateEditText();

            ChangingText = true;
            Text += Suffix;
        }
    }
}
