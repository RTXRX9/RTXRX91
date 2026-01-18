namespace RobloxToolkit.Models
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public class ClickerSettings
    {
        public int Cps { get; set; }
        public int MinDelay { get; set; }
        public int MaxDelay { get; set; }
        public bool UseRandomDelay { get; set; }
        public MouseButton MouseButton { get; set; }
        public bool HoldMode { get; set; }
        public bool RobloxFocusOnly { get; set; }
    }

    public class ClickerStats
    {
        public int TotalClicks { get; set; }
        public double CurrentCps { get; set; }
    }
}
