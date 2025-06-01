namespace JK.ImageViewer.Theming
{
    public class AlwaysTrueCondition : ThemePropertyCondition
    {
        public override bool Evaluate(Control _)
        {
            return true;
        }

        private AlwaysTrueCondition() { }

        private static AlwaysTrueCondition? _instance;
        public static AlwaysTrueCondition Instance => _instance ?? new();
    }
}
