namespace JK.ImageViewer.ValueListProviders
{
    public interface IValueListProvider
    {
        public object?[] GetValues();
        public string DisplayMember { get; }
        public string ValueMember { get; }
    }
}
