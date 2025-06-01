namespace JK.ImageViewer.Theming
{
    public class ColorSchemeCondition(string colorSchemeProperty) : ThemePropertyCondition
    {
        public ColorScheme[] ColorSchemes { get; private set; } = colorSchemeProperty
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => new ColorScheme(s))
            .ToArray();

        public override bool Evaluate(Control _)
        {
            return ColorSchemes.Any(s => s.IsActive());
        }
    }
}
