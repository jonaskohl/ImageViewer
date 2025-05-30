using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JK.ImageViewer.Controls
{
    public partial class ToolStripNumericUpDown : ToolStripControlHost
    {
        private static ToolStripNumericUpDownControl CreateControlInstance() => new()
        {
            Size = new Size(100, 15)
        };

        public ToolStripNumericUpDown() : base(CreateControlInstance())
        {
            if (Control is ToolStripNumericUpDownControl toolStripNumericUpDownControl)
            {
                toolStripNumericUpDownControl.Owner = this;
            }
        }

        public ToolStripNumericUpDown(string name) : this()
        {
            Name = name;
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(100, 15);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NumericUpDown NumericUpDown
        {
            get
            {
                return (NumericUpDown)Control;
            }
        }

        [DefaultValue(100)]
        [Category("Behavior")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public decimal Maximum
        {
            get
            {
                return NumericUpDown.Maximum;
            }
            set
            {
                NumericUpDown.Maximum = value;
            }
        }

        [DefaultValue(0)]
        [Category("Behavior")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public decimal Minimum
        {
            get
            {
                return NumericUpDown.Minimum;
            }
            set
            {
                NumericUpDown.Minimum = value;
            }
        }

        [DefaultValue(0)]
        [Category("Data")]
        public int DecimalPlaces
        {
            get
            {
                return NumericUpDown.DecimalPlaces;
            }
            set
            {
                NumericUpDown.DecimalPlaces = value;
            }
        }

        [DefaultValue(0)]
        [Category("Data")]
        public decimal Value
        {
            get
            {
                return NumericUpDown.Value;
            }
            set
            {
                NumericUpDown.Value = value;
            }
        }

        [DefaultValue(1)]
        [Category("Data")]
        public decimal Increment
        {
            get
            {
                return NumericUpDown.Increment;
            }
            set
            {
                NumericUpDown.Increment = value;
            }
        }
    }
}
