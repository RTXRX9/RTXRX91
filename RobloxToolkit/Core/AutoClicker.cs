using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using RobloxToolkit.Models;

namespace RobloxToolkit.Core
{
    public class AutoClicker
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;

        private CancellationTokenSource? cancellationTokenSource;
        private Task? clickerTask;
        private ClickerSettings? currentSettings;
        private ClickerStats stats;
        private readonly object statsLock = new object();
        private DateTime lastClickTime;
        private readonly Random random = new Random();

        public bool IsRunning => clickerTask != null && !clickerTask.IsCompleted;

        public void Start(ClickerSettings settings)
        {
            if (IsRunning)
                Stop();

            currentSettings = settings;
            stats = new ClickerStats();
            cancellationTokenSource = new CancellationTokenSource();

            clickerTask = Task.Run(() => ClickerLoop(cancellationTokenSource.Token));
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
            clickerTask?.Wait(1000);
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            clickerTask = null;
        }

        public ClickerStats GetStats()
        {
            lock (statsLock)
            {
                return new ClickerStats
                {
                    TotalClicks = stats.TotalClicks,
                    CurrentCps = stats.CurrentCps
                };
            }
        }

        private async Task ClickerLoop(CancellationToken token)
        {
            if (currentSettings == null) return;

            int delayMs = 1000 / currentSettings.Cps;
            DateTime cpsStartTime = DateTime.Now;
            int clicksInLastSecond = 0;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (currentSettings.RobloxFocusOnly && !IsRobloxFocused())
                    {
                        await Task.Delay(100, token);
                        continue;
                    }

                    PerformClick(currentSettings.MouseButton, currentSettings.HoldMode);

                    lock (statsLock)
                    {
                        stats.TotalClicks++;
                        clicksInLastSecond++;

                        if ((DateTime.Now - cpsStartTime).TotalSeconds >= 1)
                        {
                            stats.CurrentCps = clicksInLastSecond;
                            clicksInLastSecond = 0;
                            cpsStartTime = DateTime.Now;
                        }
                    }

                    int actualDelay = delayMs;
                    if (currentSettings.UseRandomDelay)
                    {
                        actualDelay += random.Next(currentSettings.MinDelay, currentSettings.MaxDelay + 1);
                    }

                    await Task.Delay(actualDelay, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception)
                {
                }
            }
        }

        private void PerformClick(MouseButton button, bool holdMode)
        {
            uint downFlag = 0, upFlag = 0;

            switch (button)
            {
                case MouseButton.Left:
                    downFlag = MOUSEEVENTF_LEFTDOWN;
                    upFlag = MOUSEEVENTF_LEFTUP;
                    break;
                case MouseButton.Right:
                    downFlag = MOUSEEVENTF_RIGHTDOWN;
                    upFlag = MOUSEEVENTF_RIGHTUP;
                    break;
                case MouseButton.Middle:
                    downFlag = MOUSEEVENTF_MIDDLEDOWN;
                    upFlag = MOUSEEVENTF_MIDDLEUP;
                    break;
            }

            mouse_event(downFlag, 0, 0, 0, 0);

            if (!holdMode)
            {
                Thread.Sleep(10);
                mouse_event(upFlag, 0, 0, 0, 0);
            }
        }

        private bool IsRobloxFocused()
        {
            try
            {
                IntPtr foregroundWindow = GetForegroundWindow();
                GetWindowThreadProcessId(foregroundWindow, out uint processId);

                var process = System.Diagnostics.Process.GetProcessById((int)processId);
                return process.ProcessName.Contains("RobloxPlayerBeta", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
