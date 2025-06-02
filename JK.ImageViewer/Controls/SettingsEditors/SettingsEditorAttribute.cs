namespace JK.ImageViewer.Controls.SettingsEditors
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingsEditorAttribute(Type propertyType) : Attribute
    {
        public Type PropertyType => propertyType;
    }
}
