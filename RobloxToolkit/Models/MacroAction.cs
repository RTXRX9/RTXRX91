namespace RobloxToolkit.Models
{
    public enum ActionType
    {
        MouseMove,
        MouseDown,
        MouseUp,
        KeyDown,
        KeyUp
    }

    public class MacroAction
    {
        public ActionType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Button { get; set; } = string.Empty;
        public int KeyCode { get; set; }
        public long Timestamp { get; set; }

        public override string ToString()
        {
            return Type switch
            {
                ActionType.MouseMove => $"[{Timestamp}ms] Move to ({X}, {Y})",
                ActionType.MouseDown => $"[{Timestamp}ms] {Button} Click Down at ({X}, {Y})",
                ActionType.MouseUp => $"[{Timestamp}ms] {Button} Click Up at ({X}, {Y})",
                ActionType.KeyDown => $"[{Timestamp}ms] Key Down: {KeyCode}",
                ActionType.KeyUp => $"[{Timestamp}ms] Key Up: {KeyCode}",
                _ => $"[{Timestamp}ms] Unknown Action"
            };
        }
    }
}
