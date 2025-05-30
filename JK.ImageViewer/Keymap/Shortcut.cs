
namespace JK.ImageViewer.Keymap
{
    public class Shortcut
    {
        public bool CtrlKey { get; init; }
        public bool ShiftKey { get; init; }
        public bool AltKey { get; init; }
        public Keys Key { get; init; }

        public Keys ToKeys()
        {
            var key = Key;
            if (CtrlKey) key |= Keys.Control;
            if (ShiftKey) key |= Keys.Shift;
            if (AltKey) key |= Keys.Alt;
            return key;
        }
    }
}
