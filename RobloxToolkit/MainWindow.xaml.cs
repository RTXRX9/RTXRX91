using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using RobloxToolkit.Core;
using RobloxToolkit.Models;

namespace RobloxToolkit
{
    public partial class MainWindow : Window
    {
        private AutoClicker? autoClicker;
        private MacroRecorder? macroRecorder;
        private RobloxMonitor? robloxMonitor;
        private DispatcherTimer? statsTimer;
        private DateTime clickerStartTime;
        private Key currentHotkey = Key.F6;

        public MainWindow()
        {
            InitializeComponent();
            InitializeComponents();
            RegisterGlobalHotkey();
            StartRobloxMonitoring();
        }

        private void InitializeComponents()
        {
            autoClicker = new AutoClicker();
            macroRecorder = new MacroRecorder();
            robloxMonitor = new RobloxMonitor();

            statsTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            statsTimer.Tick += StatsTimer_Tick;

            robloxMonitor.RobloxDetected += RobloxMonitor_RobloxDetected;
            robloxMonitor.RobloxClosed += RobloxMonitor_RobloxClosed;

            LoadSavedMacros();
        }

        #region Auto Clicker

        private void CpsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CpsTextBox != null)
            {
                CpsTextBox.Text = ((int)CpsSlider.Value).ToString();
            }
        }

        private void CpsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(CpsTextBox.Text, out int value))
            {
                if (value >= 1 && value <= 500)
                {
                    CpsSlider.Value = value;
                }
            }
        }

        private void RandomDelayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MinDelayTextBox.IsEnabled = true;
            MaxDelayTextBox.IsEnabled = true;
        }

        private void RandomDelayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MinDelayTextBox.IsEnabled = false;
            MaxDelayTextBox.IsEnabled = false;
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            if (e.Key == Key.Escape)
                return;

            currentHotkey = e.Key;
            HotkeyTextBox.Text = e.Key.ToString();
            StartClickerButton.Content = $"Start Clicker ({e.Key})";
        }

        private void StartClickerButton_Click(object sender, RoutedEventArgs e)
        {
            StartAutoClicker();
        }

        private void StopClickerButton_Click(object sender, RoutedEventArgs e)
        {
            StopAutoClicker();
        }

        private void StartAutoClicker()
        {
            if (autoClicker == null) return;

            var settings = new ClickerSettings
            {
                Cps = (int)CpsSlider.Value,
                MinDelay = int.TryParse(MinDelayTextBox.Text, out int min) ? min : 0,
                MaxDelay = int.TryParse(MaxDelayTextBox.Text, out int max) ? max : 0,
                UseRandomDelay = RandomDelayCheckBox.IsChecked ?? false,
                MouseButton = (MouseButton)MouseButtonComboBox.SelectedIndex,
                HoldMode = HoldModeCheckBox.IsChecked ?? false,
                RobloxFocusOnly = RobloxFocusModeCheckBox?.IsChecked ?? false
            };

            autoClicker.Start(settings);
            clickerStartTime = DateTime.Now;

            StartClickerButton.IsEnabled = false;
            StopClickerButton.IsEnabled = true;
            StatusText.Text = "Clicker Active";
            StatusIndicator.Fill = (System.Windows.Media.Brush)FindResource("AccentBrush");

            statsTimer?.Start();
        }

        private void StopAutoClicker()
        {
            autoClicker?.Stop();

            StartClickerButton.IsEnabled = true;
            StopClickerButton.IsEnabled = false;
            StatusText.Text = "Ready";
            StatusIndicator.Fill = (System.Windows.Media.Brush)FindResource("SuccessBrush");

            statsTimer?.Stop();
        }

        private void StatsTimer_Tick(object? sender, EventArgs e)
        {
            if (autoClicker == null) return;

            var stats = autoClicker.GetStats();
            CurrentCpsText.Text = stats.CurrentCps.ToString("F1");
            TotalClicksText.Text = stats.TotalClicks.ToString();

            var elapsed = DateTime.Now - clickerStartTime;
            RunningTimeText.Text = $"{elapsed:hh\\:mm\\:ss}";
        }

        #endregion

        #region Macro Recorder

        private void RecordMacroButton_Click(object sender, RoutedEventArgs e)
        {
            if (macroRecorder == null) return;

            MacroActionsListBox.Items.Clear();
            macroRecorder.StartRecording();

            RecordMacroButton.IsEnabled = false;
            StopRecordingButton.IsEnabled = true;
            PlayMacroButton.IsEnabled = false;
            StatusText.Text = "Recording Macro";
            StatusIndicator.Fill = (System.Windows.Media.Brush)FindResource("ErrorBrush");
        }

        private void StopRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            if (macroRecorder == null) return;

            var actions = macroRecorder.StopRecording();

            foreach (var action in actions)
            {
                MacroActionsListBox.Items.Add(action.ToString());
            }

            RecordMacroButton.IsEnabled = true;
            StopRecordingButton.IsEnabled = false;
            PlayMacroButton.IsEnabled = true;
            StatusText.Text = "Recording Stopped";
            StatusIndicator.Fill = (System.Windows.Media.Brush)FindResource("SuccessBrush");
        }

        private async void PlayMacroButton_Click(object sender, RoutedEventArgs e)
        {
            if (macroRecorder == null || !macroRecorder.HasRecordedActions()) return;

            int loopCount = int.TryParse(LoopCountTextBox.Text, out int count) ? count : 1;
            int speed = int.TryParse(PlaybackSpeedTextBox.Text, out int spd) ? spd : 100;

            PlayMacroButton.IsEnabled = false;
            RecordMacroButton.IsEnabled = false;
            StatusText.Text = "Playing Macro";
            StatusIndicator.Fill = (System.Windows.Media.Brush)FindResource("AccentBrush");

            bool robloxFocusOnly = RobloxFocusModeCheckBox?.IsChecked ?? false;
            await macroRecorder.PlayMacro(loopCount, speed, robloxFocusOnly);

            PlayMacroButton.IsEnabled = true;
            RecordMacroButton.IsEnabled = true;
            StatusText.Text = "Macro Completed";
            StatusIndicator.Fill = (System.Windows.Media.Brush)FindResource("SuccessBrush");
        }

        private void SaveMacroButton_Click(object sender, RoutedEventArgs e)
        {
            if (macroRecorder == null || !macroRecorder.HasRecordedActions()) return;

            var dialog = new SaveFileDialog
            {
                Filter = "Macro Files (*.macro)|*.macro",
                DefaultExt = ".macro"
            };

            if (dialog.ShowDialog() == true)
            {
                macroRecorder.SaveMacro(dialog.FileName);
                LoadSavedMacros();
                MessageBox.Show("Macro saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadMacroButton_Click(object sender, RoutedEventArgs e)
        {
            if (macroRecorder == null) return;

            var dialog = new OpenFileDialog
            {
                Filter = "Macro Files (*.macro)|*.macro",
                DefaultExt = ".macro"
            };

            if (dialog.ShowDialog() == true)
            {
                macroRecorder.LoadMacro(dialog.FileName);

                MacroActionsListBox.Items.Clear();
                var actions = macroRecorder.GetCurrentActions();
                foreach (var action in actions)
                {
                    MacroActionsListBox.Items.Add(action.ToString());
                }

                PlayMacroButton.IsEnabled = true;
                MessageBox.Show("Macro loaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadSavedMacros()
        {
            DefaultMacroComboBox.Items.Clear();
            DefaultMacroComboBox.Items.Add(new System.Windows.Controls.ComboBoxItem { Content = "None", IsSelected = true });

            var macrosDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RobloxToolkit", "Macros");
            if (System.IO.Directory.Exists(macrosDir))
            {
                var macroFiles = System.IO.Directory.GetFiles(macrosDir, "*.macro");
                foreach (var file in macroFiles)
                {
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                    DefaultMacroComboBox.Items.Add(new System.Windows.Controls.ComboBoxItem { Content = fileName, Tag = file });
                }
            }
        }

        #endregion

        #region Roblox Integration

        private void StartRobloxMonitoring()
        {
            robloxMonitor?.Start();
        }

        private void RobloxMonitor_RobloxDetected(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                RobloxStatusText.Text = "Running";
                RobloxStatusText.Foreground = (System.Windows.Media.Brush)FindResource("SuccessBrush");

                if (ShowNotificationCheckBox?.IsChecked ?? false)
                {
                    MessageBox.Show("Roblox detected and is now running!", "Roblox Toolkit", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (AutoActivateClickerCheckBox?.IsChecked ?? false)
                {
                    StartAutoClicker();
                }

                if (AutoActivateMacroCheckBox?.IsChecked ?? false)
                {
                    AutoPlayMacro();
                }
            });
        }

        private void RobloxMonitor_RobloxClosed(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                RobloxStatusText.Text = "Not Running";
                RobloxStatusText.Foreground = (System.Windows.Media.Brush)FindResource("ErrorBrush");

                if (autoClicker?.IsRunning ?? false)
                {
                    StopAutoClicker();
                }
            });
        }

        private void AutoPlayMacro()
        {
            if (DefaultMacroComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem item && item.Tag is string filePath)
            {
                macroRecorder?.LoadMacro(filePath);
                PlayMacroButton_Click(this, new RoutedEventArgs());
            }
        }

        #endregion

        #region Optimizer

        private void SetHighPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");
                if (robloxProcesses.Length == 0)
                {
                    MessageBox.Show("Roblox is not running!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach (var process in robloxProcesses)
                {
                    process.PriorityClass = ProcessPriorityClass.High;
                }

                MessageBox.Show("Roblox process priority set to High!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to set priority: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearCacheButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long totalFreed = 0;
                var tempPath = System.IO.Path.GetTempPath();

                var tempFiles = System.IO.Directory.GetFiles(tempPath);
                foreach (var file in tempFiles)
                {
                    try
                    {
                        var fileInfo = new System.IO.FileInfo(file);
                        totalFreed += fileInfo.Length;
                        System.IO.File.Delete(file);
                    }
                    catch { }
                }

                double mbFreed = totalFreed / (1024.0 * 1024.0);
                MessageBox.Show($"Cache cleared! Freed {mbFreed:F2} MB", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to clear cache: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ScanAppsButton_Click(object sender, RoutedEventArgs e)
        {
            HighRamAppsListBox.Items.Clear();

            var processes = Process.GetProcesses()
                .Where(p => p.WorkingSet64 > 100 * 1024 * 1024)
                .OrderByDescending(p => p.WorkingSet64)
                .Take(20);

            foreach (var process in processes)
            {
                try
                {
                    double ramMB = process.WorkingSet64 / (1024.0 * 1024.0);
                    var item = new System.Windows.Controls.CheckBox
                    {
                        Content = $"{process.ProcessName} - {ramMB:F0} MB",
                        Tag = process.Id,
                        Foreground = (System.Windows.Media.Brush)FindResource("TextPrimaryBrush")
                    };
                    HighRamAppsListBox.Items.Add(item);
                }
                catch { }
            }

            KillSelectedAppsButton.IsEnabled = HighRamAppsListBox.Items.Count > 0;
        }

        private void KillSelectedAppsButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to kill the selected processes?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            int killedCount = 0;
            foreach (var item in HighRamAppsListBox.Items)
            {
                if (item is System.Windows.Controls.CheckBox checkBox && checkBox.IsChecked == true && checkBox.Tag is int processId)
                {
                    try
                    {
                        var process = Process.GetProcessById(processId);
                        process.Kill();
                        killedCount++;
                    }
                    catch { }
                }
            }

            MessageBox.Show($"Killed {killedCount} processes", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ScanAppsButton_Click(sender, e);
        }

        private void ApplyNetworkTweaksButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DisableNagleCheckBox.IsChecked == true)
                {
                    using var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", true);
                    if (key != null)
                    {
                        key.SetValue("TcpAckFrequency", 1, RegistryValueKind.DWord);
                        key.SetValue("TCPNoDelay", 1, RegistryValueKind.DWord);
                    }
                }

                MessageBox.Show("Network tweaks applied! Restart required.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Please run as Administrator to apply network tweaks!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to apply tweaks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Helpers

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string text)
        {
            return Regex.IsMatch(text, "^[0-9]+$");
        }

        private void RegisterGlobalHotkey()
        {
            ComponentDispatcher.ThreadPreprocessMessage += (ref MSG msg, ref bool handled) =>
            {
                if (msg.message == 0x0100 && (Key)msg.wParam == currentHotkey)
                {
                    if (autoClicker?.IsRunning ?? false)
                    {
                        StopAutoClicker();
                    }
                    else
                    {
                        StartAutoClicker();
                    }
                    handled = true;
                }
            };
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            autoClicker?.Stop();
            macroRecorder?.StopRecording();
            robloxMonitor?.Stop();
            statsTimer?.Stop();
        }

        #endregion
    }
}
