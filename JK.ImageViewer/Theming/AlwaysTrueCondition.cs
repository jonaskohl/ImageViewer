namespace JK.ImageViewer.Theming
{
    public class AlwaysTrueCondition : ThemePropertyCondition
    {
        public override bool Evaluate()
        {
            return true;
        }

        private AlwaysTrueCondition() { }

        private static AlwaysTrueCondition? _instance;
        public static AlwaysTrueCondition Instance => _instance ?? new();
    }
}
