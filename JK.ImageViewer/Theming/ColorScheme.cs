namespace JK.ImageViewer.Theming
{
    public class ColorScheme(string scheme)
    {
        public ColorSchemeKind Kind { get; protected set; } = scheme switch
        {
            var s when s.ToLowerInvariant() == "light" => ColorSchemeKind.Light,
            var s when s.ToLowerInvariant() == "dark" => ColorSchemeKind.Dark,
            var s when s.ToLowerInvariant() == "any" || s == "*" => ColorSchemeKind.Any,
            _ => throw new Exception($"Invalid color scheme {scheme}")
        };

        public bool IsActive()
        {
            switch (Kind)
            {
                case ColorSchemeKind.Light:
                    return !Application.IsDarkModeEnabled;
                case ColorSchemeKind.Dark:
                    return Application.IsDarkModeEnabled;
                case ColorSchemeKind.Any:
                    return true;
                default:
                    return false;
            }
        }
    }
}
