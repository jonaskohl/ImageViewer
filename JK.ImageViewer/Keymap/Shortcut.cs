
using System.Text;

namespace JK.ImageViewer.Keymap
{
    public class Shortcut
    {
        public bool CtrlKey { get; init; }
        public bool ShiftKey { get; init; }
        public bool AltKey { get; init; }
        public Keys Key { get; init; }

        private static string GetKeycodeDisplayText(Keys key)
        {
            return key switch
            {
                Keys.Add => "+",
                Keys.Decimal => ".",
                Keys.Divide => "/",
                Keys.Multiply => "*",
                Keys.OemBackslash => "\\",
                Keys.OemCloseBrackets => "]",
                Keys.OemMinus => "-",
                Keys.OemOpenBrackets => "[",
                Keys.OemPeriod => ".",
                Keys.OemPipe => "|",
                Keys.OemQuestion => "/",
                Keys.OemQuotes => "\"",
                Keys.OemSemicolon => ",",
                Keys.Oemcomma => ",",
                Keys.Oemplus => "+",
                Keys.Oemtilde => "`",
                Keys.Separator => "-",
                Keys.Subtract => "-",
                Keys.D0 => "0",
                Keys.D1 => "1",
                Keys.D2 => "2",
                Keys.D3 => "3",
                Keys.D4 => "4",
                Keys.D5 => "5",
                Keys.D6 => "6",
                Keys.D7 => "7",
                Keys.D8 => "8",
                Keys.D9 => "9",
                Keys.Space => " ",
                _ => key.ToString(),
            };
        }

        public Keys ToKeys()
        {
            var key = Key;
            if (CtrlKey) key |= Keys.Control;
            if (ShiftKey) key |= Keys.Shift;
            if (AltKey) key |= Keys.Alt;
            return key;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (CtrlKey) sb.Append("Ctrl+");
            if (AltKey) sb.Append("Alt+");
            if (ShiftKey) sb.Append("Shift+");
            sb.Append(GetKeycodeDisplayText(Key));
            return sb.ToString();
        }
    }
}
