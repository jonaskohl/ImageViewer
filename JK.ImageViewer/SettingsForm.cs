using JK.ImageViewer.Controls;
using JK.ImageViewer.Theming;
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
            Text = this._("Global.DialogTitles.Settings");
            okButton.Text = this._("Global.Buttons.GenericOkay");
            cancelButton.Text = this._("Global.Buttons.GenericCancel");
            applyButton.Text = this._("Global.Buttons.GenericApply");

            LoadSettingsStructure();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToParent();
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
                var cat = category.Attribute("Which")!.Value;
                iconListBox1.Items.Add(new IconListBoxItem()
                {
                    Label = this._($"SettingsCategory.{cat}"),
                    Image = ThemeManager.CurrentTheme.GetImage($"SettingsCategory.{cat}"),
                });
            }
            iconListBox1.SetSelected(0, true);
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
