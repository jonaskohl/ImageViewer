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
                fancyTreeView1.Nodes.Add(category.Attribute("Which")!.Value);
            }
            fancyTreeView1.SelectedNode = fancyTreeView1.Nodes.Cast<TreeNode>().FirstOrDefault();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            ApplySettings();
        }

        private void ApplySettings()
        {
            applyButton.Enabled = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            ApplySettings();
        }
    }
}
