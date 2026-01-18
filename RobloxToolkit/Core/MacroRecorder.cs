using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RobloxToolkit.Models;

namespace RobloxToolkit.Core
{
    public class MacroRecorder
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private List<MacroAction> recordedActions = new List<MacroAction>();
        private bool isRecording = false;
        private CancellationTokenSource? recordingCancellation;
        private Task? recordingTask;
        private Stopwatch? recordingStopwatch;

        public void StartRecording()
        {
            recordedActions.Clear();
            isRecording = true;
            recordingCancellation = new CancellationTokenSource();
            recordingStopwatch = Stopwatch.StartNew();

            recordingTask = Task.Run(() => RecordingLoop(recordingCancellation.Token));
        }

        public List<MacroAction> StopRecording()
        {
            isRecording = false;
            recordingCancellation?.Cancel();
            recordingTask?.Wait(1000);
            recordingStopwatch?.Stop();
            recordingCancellation?.Dispose();

            return new List<MacroAction>(recordedActions);
        }

        public bool HasRecordedActions()
        {
            return recordedActions.Count > 0;
        }

        public List<MacroAction> GetCurrentActions()
        {
            return new List<MacroAction>(recordedActions);
        }

        private async Task RecordingLoop(CancellationToken token)
        {
            var lastMouseState = new Dictionary<int, bool>();
            var lastKeyState = new Dictionary<int, bool>();
            POINT lastMousePos = new POINT();

            for (int i = 0; i < 256; i++)
            {
                lastMouseState[i] = false;
                lastKeyState[i] = false;
            }

            while (!token.IsCancellationRequested && isRecording)
            {
                try
                {
                    GetCursorPos(out POINT currentPos);

                    if (currentPos.X != lastMousePos.X || currentPos.Y != lastMousePos.Y)
                    {
                        recordedActions.Add(new MacroAction
                        {
                            Type = ActionType.MouseMove,
                            X = currentPos.X,
                            Y = currentPos.Y,
                            Timestamp = recordingStopwatch?.ElapsedMilliseconds ?? 0
                        });
                        lastMousePos = currentPos;
                    }

                    CheckMouseButton(0x01, "Left", lastMouseState);
                    CheckMouseButton(0x02, "Right", lastMouseState);
                    CheckMouseButton(0x04, "Middle", lastMouseState);

                    for (int key = 0x08; key <= 0xFE; key++)
                    {
                        if (key == 0x5B || key == 0x5C) continue;

                        bool isPressed = (GetAsyncKeyState(key) & 0x8000) != 0;
                        if (isPressed != lastKeyState[key])
                        {
                            lastKeyState[key] = isPressed;

                            if (isPressed)
                            {
                                recordedActions.Add(new MacroAction
                                {
                                    Type = ActionType.KeyDown,
                                    KeyCode = key,
                                    Timestamp = recordingStopwatch?.ElapsedMilliseconds ?? 0
                                });
                            }
                            else
                            {
                                recordedActions.Add(new MacroAction
                                {
                                    Type = ActionType.KeyUp,
                                    KeyCode = key,
                                    Timestamp = recordingStopwatch?.ElapsedMilliseconds ?? 0
                                });
                            }
                        }
                    }

                    await Task.Delay(10, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch { }
            }
        }

        private void CheckMouseButton(int vKey, string buttonName, Dictionary<int, bool> lastState)
        {
            bool isPressed = (GetAsyncKeyState(vKey) & 0x8000) != 0;
            if (isPressed != lastState[vKey])
            {
                lastState[vKey] = isPressed;
                GetCursorPos(out POINT pos);

                recordedActions.Add(new MacroAction
                {
                    Type = isPressed ? ActionType.MouseDown : ActionType.MouseUp,
                    Button = buttonName,
                    X = pos.X,
                    Y = pos.Y,
                    Timestamp = recordingStopwatch?.ElapsedMilliseconds ?? 0
                });
            }
        }

        public async Task PlayMacro(int loopCount, int speedPercent, bool robloxFocusOnly)
        {
            if (recordedActions.Count == 0) return;

            int loops = loopCount == 0 ? int.MaxValue : loopCount;

            for (int i = 0; i < loops; i++)
            {
                if (robloxFocusOnly && !IsRobloxFocused())
                {
                    await Task.Delay(100);
                    continue;
                }

                await PlayMacroOnce(speedPercent);
            }
        }

        private async Task PlayMacroOnce(int speedPercent)
        {
            long lastTimestamp = 0;

            foreach (var action in recordedActions)
            {
                long delay = action.Timestamp - lastTimestamp;
                if (delay > 0)
                {
                    int adjustedDelay = (int)(delay * 100 / speedPercent);
                    await Task.Delay(adjustedDelay);
                }

                ExecuteAction(action);
                lastTimestamp = action.Timestamp;
            }
        }

        private void ExecuteAction(MacroAction action)
        {
            switch (action.Type)
            {
                case ActionType.MouseMove:
                    SetCursorPosition(action.X, action.Y);
                    break;

                case ActionType.MouseDown:
                    PerformMouseAction(action.Button, true);
                    break;

                case ActionType.MouseUp:
                    PerformMouseAction(action.Button, false);
                    break;

                case ActionType.KeyDown:
                    keybd_event((byte)action.KeyCode, 0, 0, 0);
                    break;

                case ActionType.KeyUp:
                    keybd_event((byte)action.KeyCode, 0, KEYEVENTF_KEYUP, 0);
                    break;
            }
        }

        private void SetCursorPosition(int x, int y)
        {
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            uint normalizedX = (uint)((x * 65535) / screenWidth);
            uint normalizedY = (uint)((y * 65535) / screenHeight);

            mouse_event(MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE, normalizedX, normalizedY, 0, 0);
        }

        private void PerformMouseAction(string button, bool down)
        {
            uint flag = button switch
            {
                "Left" => down ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP,
                "Right" => down ? MOUSEEVENTF_RIGHTDOWN : MOUSEEVENTF_RIGHTUP,
                _ => down ? MOUSEEVENTF_LEFTDOWN : MOUSEEVENTF_LEFTUP
            };

            mouse_event(flag, 0, 0, 0, 0);
        }

        public void SaveMacro(string filePath)
        {
            var json = JsonConvert.SerializeObject(recordedActions, Formatting.Indented);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.WriteAllText(filePath, json);
        }

        public void LoadMacro(string filePath)
        {
            var json = File.ReadAllText(filePath);
            recordedActions = JsonConvert.DeserializeObject<List<MacroAction>>(json) ?? new List<MacroAction>();
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
