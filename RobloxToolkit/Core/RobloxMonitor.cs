using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RobloxToolkit.Core
{
    public class RobloxMonitor
    {
        public event EventHandler? RobloxDetected;
        public event EventHandler? RobloxClosed;

        private CancellationTokenSource? cancellationTokenSource;
        private Task? monitoringTask;
        private bool wasRunning = false;

        public void Start()
        {
            if (monitoringTask != null && !monitoringTask.IsCompleted)
                return;

            cancellationTokenSource = new CancellationTokenSource();
            monitoringTask = Task.Run(() => MonitorLoop(cancellationTokenSource.Token));
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
            monitoringTask?.Wait(1000);
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            monitoringTask = null;
        }

        private async Task MonitorLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    bool isRunning = IsRobloxRunning();

                    if (isRunning && !wasRunning)
                    {
                        RobloxDetected?.Invoke(this, EventArgs.Empty);
                        wasRunning = true;
                    }
                    else if (!isRunning && wasRunning)
                    {
                        RobloxClosed?.Invoke(this, EventArgs.Empty);
                        wasRunning = false;
                    }

                    await Task.Delay(2000, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch { }
            }
        }

        private bool IsRobloxRunning()
        {
            try
            {
                var processes = Process.GetProcessesByName("RobloxPlayerBeta");
                return processes.Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
