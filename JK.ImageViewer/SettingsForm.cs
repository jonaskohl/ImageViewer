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
        XDocument structure;

        public SettingsForm()
        {
            InitializeComponent();
            Text = this._("Global.DialogTitles.Settings");
            okButton.Text = this._("Global.Buttons.GenericOkay");
            cancelButton.Text = this._("Global.Buttons.GenericCancel");
            applyButton.Text = this._("Global.Buttons.GenericApply");


            // TODO Assert root tag
            structure = XDocument.Load(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Settings",
                "SettingsMenu.xml"
            ));

            LoadSettingsStructure(0);
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            LoadSettingsStructure(iconListBox1.SelectedIndex);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToParent();
        }

        private void LoadSettingsStructure(int selectedIndex)
        {
            iconListBox1.SuspendLayout();
            iconListBox1.Items.Clear();

            foreach (var category in structure.Root!.Elements("Category"))
            {
                var cat = category.Attribute("Which")!.Value;
                iconListBox1.Items.Add(new IconListBoxItem()
                {
                    Label = this._($"SettingsCategory.{cat}"),
                    Image = ThemeManager.CurrentTheme.GetImage($"SettingsCategory.{cat}", this),
                });
            }
            iconListBox1.SetSelected(selectedIndex, true);

            iconListBox1.ResumeLayout(true);
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
