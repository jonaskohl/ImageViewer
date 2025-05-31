using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JK.ImageViewer
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            LoadSettingsStructure();
        }

        private void LoadSettingsStructure()
        {
            var doc = XDocument.Load(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Settings",
                "SettingsMenu.xml"
            ));

            // TODO Assert root tag

            foreach (var category in doc.Root!.Elements("Category"))
            {
                listBox1.Items.Add(category.Attribute("Which")!.Value);
            }
        }
    }
}
