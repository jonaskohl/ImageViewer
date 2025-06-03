using JK.ImageViewer.Controls;
using JK.ImageViewer.Controls.SettingsEditors;
using JK.ImageViewer.Theming;
using JK.ImageViewer.ValueListProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JK.ImageViewer
{
    public partial class SettingsForm : Form
    {
        XDocument structure;
        Dictionary<string, Type> editorTypeCache = new();
        Dictionary<string, Type> providerTypeCache = new();

        Dictionary<string, string> pendingSettings = new();

        public SettingsForm()
        {
            InitializeComponent();


            // TODO Assert root tag
            structure = XDocument.Load(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Settings",
                "SettingsMenu.xml"
            ));

            iconListBox1.SelectedIndexChanged += IconListBox1_SelectedIndexChanged;
            flowLayoutPanel2.Layout += FlowLayoutPanel2_Layout;

            ApplyStaticLocalization();
            LoadSettingsStructure();

            LocalizationManager.LanguageChanged += LocalizationManager_LanguageChanged;
        }

        private void ApplyStaticLocalization()
        {
            Text = this._("Global.DialogTitles.Settings");
            okButton.Text = this._("Global.Buttons.GenericOkay");
            cancelButton.Text = this._("Global.Buttons.GenericCancel");
            applyButton.Text = this._("Global.Buttons.GenericApply");
        }

        private void LocalizationManager_LanguageChanged(object? sender, EventArgs e)
        {
            ApplyStaticLocalization();
            LoadSettingsStructure(iconListBox1.SelectedIndex);
        }

        private void FlowLayoutPanel2_Layout(object? sender, LayoutEventArgs e)
        {
            flowLayoutPanel2.SuspendLayout();
            foreach (Control c in flowLayoutPanel2.Controls)
            {
                var w = flowLayoutPanel2.ClientSize.Width - 6;
                c.Width = w;
                c.MinimumSize = new Size(w, 0);
            }
            flowLayoutPanel2.ResumeLayout(false);
        }

        private void IconListBox1_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (iconListBox1.SelectedIndex < 0)
                return;
            BuildEditor(iconListBox1.SelectedIndex);
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            LoadIcons(iconListBox1.SelectedIndex);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CenterToParent();
        }

        private void LoadSettingsStructure(int index = 0)
        {
            LoadIcons(index);
            BuildEditor(index);
        }

        private void BuildEditor(int categoryIndex)
        {
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel2.Controls.Clear();
            var xCategory = structure.Root!.Elements("Category").ElementAt(categoryIndex);

            var tCurrentSettings = typeof(CurrentSettings);

            foreach (var xSetting in xCategory.Elements("Setting"))
            {
                var key = xSetting.Attribute("Key")!.Value;
                var editorName = xSetting.Attribute("Editor")!.Value;
                if (!editorTypeCache.ContainsKey(editorName))
                    CacheEditorType(editorName);
                var editorType = editorTypeCache[editorName];
                var editorInstance = Activator.CreateInstance(editorType)!;
                var ctrlEditorInstance = (Control)editorInstance;
                var propEditorInstance = (IPropertyEditor)editorInstance;
                ctrlEditorInstance.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                ctrlEditorInstance.Dock = DockStyle.Top;
                var w = flowLayoutPanel2.ClientSize.Width - 6;
                ctrlEditorInstance.Width = w;
                ctrlEditorInstance.MinimumSize = new Size(w, 0);
                propEditorInstance.PropertyName = key;

                if (editorInstance is IValueListPropertyEditor listEditorInstance)
                {
                    var providerName = xSetting.Attribute("ValueListProvider")!.Value;
                    if (!providerTypeCache.ContainsKey(providerName))
                        CacheProviderType(providerName);
                    var providerType = providerTypeCache[providerName];
                    var providerInstance = (IValueListProvider)Activator.CreateInstance(providerType)!;
                    var values = providerInstance.GetValues();
                    listEditorInstance.ValueListDisplayMember = providerInstance.DisplayMember;
                    listEditorInstance.ValueListValueMember = providerInstance.ValueMember;
                    listEditorInstance.ValueList = values;
                }

                propEditorInstance.SerializedValue = SettingsManager.Instance.SerializeValue(tCurrentSettings.GetProperty(key, BindingFlags.Static | BindingFlags.Public)?.GetValue(null));
                propEditorInstance.ValueChanged += (s, e) => ScheduleSettingChange(key, propEditorInstance.SerializedValue);

                flowLayoutPanel2.Controls.Add(ctrlEditorInstance);
            }
            flowLayoutPanel2.ResumeLayout(true);
        }

        private void ScheduleSettingChange(string key, string serializedValue)
        {
            pendingSettings[key] = serializedValue;
            applyButton.Enabled = true;
        }

        private void CacheEditorType(string editorName)
        {
            // TODO Generics
            var fullTypeName = "JK.ImageViewer.Controls.SettingsEditors." + editorName;

            var type = Type.GetType(fullTypeName);
            if (type is null)
                throw new ArgumentException($"Editor type {fullTypeName} not found", nameof(editorName));
            if (type.IsAbstract)
                throw new ArgumentException("Editor type cannot be abstract", nameof(editorName));
            if (!type.IsClass)
                throw new ArgumentException("Editor type must be a class", nameof(editorName));
            if (!type.IsAssignableTo(typeof(IPropertyEditor)))
                throw new ArgumentException($"Editor type must implement {nameof(IPropertyEditor)}", nameof(editorName));
            var attrib = type.GetCustomAttribute<SettingsEditorAttribute>();
            if (attrib is null)
                throw new ArgumentException($"Editor type must have an attribute of type {nameof(SettingsEditorAttribute)}", nameof(editorName));

            editorTypeCache[editorName] = type;
        }

        private void CacheProviderType(string providerName)
        {
            var fullTypeName = "JK.ImageViewer.ValueListProviders." + providerName;

            var type = Type.GetType(fullTypeName);
            if (type is null)
                throw new ArgumentException($"Value list provider type {fullTypeName} not found", nameof(providerName));
            if (type.IsAbstract)
                throw new ArgumentException("Value list provider type cannot be abstract", nameof(providerName));
            if (!type.IsClass)
                throw new ArgumentException("Value list provider type must be a class", nameof(providerName));
            if (!type.IsAssignableTo(typeof(IValueListProvider)))
                throw new ArgumentException($"Value list provider type must implement {nameof(IValueListProvider)}", nameof(providerName));

            providerTypeCache[providerName] = type;
        }

        private void LoadIcons(int selectedIndex)
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
                    Tag = cat,
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
            foreach (var pendingSetting in pendingSettings)
                SettingsManager.Instance.SetSerializedPreference(pendingSetting.Key, pendingSetting.Value);

            pendingSettings.Clear();
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
