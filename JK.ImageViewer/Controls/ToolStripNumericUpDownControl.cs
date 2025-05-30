using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace JK.ImageViewer.Controls
{
    public partial class ToolStripNumericUpDown
    {
        internal class ToolStripNumericUpDownControl : NumericUpDown
        {
            private ToolStripNumericUpDown? _ownerItem;

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public ToolStripNumericUpDown? Owner
            {
                get { return _ownerItem; }
                set { _ownerItem = value; }
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public override bool AutoSize { get => false; }

            public ToolStripNumericUpDownControl() : base()
            {
                base.AutoSize = false;
            }
        }
    }
}
