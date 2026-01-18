# Roblox Toolkit

A powerful, lightweight Windows desktop application for Roblox players with auto-clicking, macro recording, and performance optimization features.

## Features

### Auto Clicker
- **Unlimited CPS**: Adjustable from 1-500+ clicks per second using slider or manual input
- **Random Delay**: Human-like clicking with customizable delay ranges (min/max in milliseconds)
- **Multiple Mouse Buttons**: Support for left, right, and middle click
- **Hold Mode**: Hold button down instead of clicking
- **Customizable Hotkey**: Toggle clicker with any key (default: F6)
- **Real-time Statistics**: Live CPS counter, total clicks, and running time display

### Macro Recorder/Player
- **Advanced Recording**: Captures both mouse movements and keyboard actions
- **Save/Load System**: Store multiple macros for different tasks
- **Editable Timeline**: View all recorded actions with timestamps
- **Loop/Repeat Options**: Set loop count (0 = infinite) and playback speed percentage
- **Modern Timeline UI**: Clean display of all recorded actions

### Roblox Integration
- **Auto-Detection**: Automatically detects when RobloxPlayerBeta.exe starts
- **Auto-Activation**: Configure auto-start for clicker or macros when Roblox launches
- **Notification System**: Optional alerts when Roblox is detected
- **Roblox Focus Mode**: Only clicks/runs macros when Roblox window is active

### FPS/Ping Optimizer
- **Process Priority**: Set Roblox to High priority for better performance
- **Cache Cleanup**: Clear temporary files and cache to free disk space
- **Background Apps Manager**: Scan and kill high-RAM applications
- **Network Tweaks**: Disable Nagle's Algorithm to reduce latency (requires admin)

## System Requirements

- **OS**: Windows 10/11
- **Framework**: .NET 8.0 Runtime
- **IDE**: Visual Studio 2022 (for building)

## Installation & Setup Instructions

### Step 1: Install Prerequisites

1. **Download and Install Visual Studio 2022**
   - Go to https://visualstudio.microsoft.com/downloads/
   - Download "Visual Studio 2022 Community" (free version)
   - Run the installer

2. **Select Workloads** (in Visual Studio Installer)
   - Check ".NET desktop development"
   - Click "Install" (may take 15-30 minutes)

3. **Install .NET 8.0 SDK** (if not included)
   - Go to https://dotnet.microsoft.com/download/dotnet/8.0
   - Download ".NET 8.0 SDK" for Windows x64
   - Run the installer

### Step 2: Download the Project

1. Download the project files to your computer
2. Extract to a folder like `C:\Projects\RobloxToolkit`

### Step 3: Open in Visual Studio 2022

1. **Launch Visual Studio 2022**
2. Click "Open a project or solution"
3. Navigate to your project folder
4. Select `RobloxToolkit.sln`
5. Click "Open"

### Step 4: Restore NuGet Packages

Visual Studio should automatically restore packages. If not:

1. Right-click on the solution in Solution Explorer
2. Select "Restore NuGet Packages"
3. Wait for the process to complete

### Step 5: Build the Project

1. **Select Build Configuration**
   - At the top, change dropdown from "Debug" to "Release"

2. **Build the Solution**
   - Click "Build" menu → "Build Solution"
   - Or press `Ctrl + Shift + B`
   - Wait for "Build succeeded" message in the Output window

3. **Check for Errors**
   - If errors appear in the Error List, check:
     - .NET 8.0 SDK is properly installed
     - All NuGet packages are restored
     - Project files are not corrupted

### Step 6: Run the Application

#### Option A: Run from Visual Studio (for testing)
1. Press `F5` or click the green "Start" button
2. The application will launch

#### Option B: Run Standalone Executable
1. Navigate to the build output folder:
   - `RobloxToolkit\bin\Release\net8.0-windows\`
2. Double-click `RobloxToolkit.exe`
3. Create a shortcut to desktop if desired

### Step 7: First Run Configuration

1. **Auto Clicker Setup**
   - Set your desired CPS using the slider
   - Configure random delay if desired (more human-like)
   - Choose mouse button (left/right/middle)
   - Set your hotkey (default is F6)

2. **Roblox Integration**
   - Enable "Auto-detection of RobloxPlayerBeta.exe"
   - Choose auto-activation options if desired
   - Enable "Roblox window focus mode" for safety

3. **Create Your First Macro**
   - Go to "Macro Recorder" tab
   - Click "⏺ Record"
   - Perform your actions (mouse + keyboard)
   - Click "⏹ Stop"
   - Click "💾 Save" to save the macro

## Building for Distribution

### Create Release Build

1. Set configuration to "Release"
2. Build → Publish Selection
3. Or manually copy from `bin\Release\net8.0-windows\`

### Single-File Executable (Optional)

1. Edit `RobloxToolkit.csproj`
2. Add inside `<PropertyGroup>`:
   ```xml
   <PublishSingleFile>true</PublishSingleFile>
   <SelfContained>false</SelfContained>
   ```
3. Open terminal in project folder
4. Run:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true
   ```
5. Find executable in `bin\Release\net8.0-windows\win-x64\publish\`

## Usage Guide

### Auto Clicker

1. **Start Clicking**
   - Configure CPS and options
   - Click "Start Clicker" or press your hotkey (default F6)
   - Statistics will update in real-time

2. **Stop Clicking**
   - Click "Stop Clicker" or press hotkey again

3. **Human-like Clicking**
   - Enable "Random delay"
   - Set Min/Max delay in milliseconds
   - Example: Min=5, Max=25 adds 5-25ms random delay

### Macro Recorder

1. **Recording**
   - Click "⏺ Record"
   - Perform your actions
   - Click "⏹ Stop"

2. **Playback**
   - Set loop count (0 = infinite)
   - Set playback speed (100 = normal, 200 = 2x speed)
   - Click "▶ Play"

3. **Save/Load**
   - Click "💾 Save" to save macro
   - Click "📁 Load" to load previously saved macro
   - Macros are saved in: `%AppData%\RobloxToolkit\Macros\`

### Roblox Integration

1. **Enable Auto-Detection**
   - Check "Enable auto-detection of RobloxPlayerBeta.exe"
   - Status will show "Running" when Roblox is detected

2. **Auto-Activation**
   - Check "Auto-start clicker when Roblox starts"
   - Or "Auto-play selected macro when Roblox starts"
   - Select default macro from dropdown

3. **Focus Mode**
   - Enable "Only click/run macros when Roblox window is active"
   - Prevents accidental clicks outside Roblox

### FPS/Ping Optimizer

1. **Process Priority**
   - Launch Roblox
   - Click "Set Roblox to High Priority"
   - Gives Roblox more CPU resources

2. **Cache Cleanup**
   - Click "Clear Temp Files & Cache"
   - Confirms how much space was freed

3. **Background Apps**
   - Click "Scan High-RAM Apps"
   - Select apps to close
   - Click "Kill Selected Apps"

4. **Network Tweaks**
   - Check "Disable Nagle's Algorithm"
   - Click "Apply Network Tweaks"
   - **Requires Administrator privileges**
   - Restart may be required

## Troubleshooting

### Application Won't Start

- **Error**: "Framework not found"
  - **Solution**: Install .NET 8.0 Runtime from https://dotnet.microsoft.com/download/dotnet/8.0

- **Error**: "Application can't run on your PC"
  - **Solution**: Make sure you have 64-bit Windows 10/11

### Build Errors

- **Error**: "SDK not found"
  - **Solution**: Install .NET 8.0 SDK

- **Error**: "NuGet package restore failed"
  - **Solution**:
    1. Tools → NuGet Package Manager → Package Manager Settings
    2. Click "Clear All NuGet Cache(s)"
    3. Rebuild solution

### Auto Clicker Not Working

- Check if Roblox focus mode is enabled but Roblox isn't active
- Try running as Administrator
- Check antivirus isn't blocking the application

### Macro Recording Issues

- **Mouse not moving to correct positions**
  - Make sure to stay on the same screen/resolution
  - Macros record absolute screen positions

- **Keyboard keys not working**
  - Some keys may be blocked by Windows
  - Try recording different key combinations

### Roblox Not Detected

- Make sure Roblox is actually running (check Task Manager)
- Look for "RobloxPlayerBeta.exe" process
- Restart the Toolkit application
- Check "Enable auto-detection" is checked

### Network Tweaks Fail

- **Error**: "Access denied"
  - **Solution**: Right-click RobloxToolkit.exe → "Run as Administrator"

## Important Notes

⚠️ **Use Responsibly**: This tool is for personal use and educational purposes. Using auto-clickers or macros may violate some games' terms of service.

⚠️ **Administrator Privileges**: Some features (network tweaks, process priority) require admin rights.

⚠️ **Antivirus Warnings**: Some antivirus software may flag auto-clickers as suspicious. This is a false positive, but you may need to add an exception.

⚠️ **Roblox Terms of Service**: Using automation tools may violate Roblox's Terms of Service and could result in account restrictions. Use at your own risk.

## Technical Details

### Architecture
- **Framework**: .NET 8.0 + WPF
- **UI**: Modern dark theme using XAML
- **Input Simulation**: Windows API (user32.dll)
- **Data Storage**: JSON format for macros

### File Structure
```
RobloxToolkit/
├── Core/
│   ├── AutoClicker.cs       # Auto-clicking logic
│   ├── MacroRecorder.cs     # Macro recording/playback
│   └── RobloxMonitor.cs     # Process detection
├── Models/
│   ├── ClickerSettings.cs   # Configuration models
│   └── MacroAction.cs       # Macro action data
├── Styles/
│   └── DarkTheme.xaml       # Dark theme styles
├── MainWindow.xaml          # UI layout
├── MainWindow.xaml.cs       # UI logic
├── App.xaml                 # Application config
└── App.xaml.cs              # Application startup

```

### Dependencies
- **Newtonsoft.Json**: JSON serialization for macro files
- **System.Windows.Forms**: Screen information for macros

## Advanced Customization

### Modify Hotkey Default
Edit `MainWindow.xaml.cs`, line with `private Key currentHotkey = Key.F6;`

### Change Theme Colors
Edit `Styles/DarkTheme.xaml`, modify color definitions at the top

### Adjust CPS Limits
Edit `MainWindow.xaml`, find `<Slider x:Name="CpsSlider"` and change `Maximum` value

### Add Custom Network Tweaks
Edit `MainWindow.xaml.cs`, find `ApplyNetworkTweaksButton_Click` method

## License

This project is for educational purposes. Use at your own risk and responsibility.

## Support

For issues or questions:
1. Check the Troubleshooting section
2. Verify all prerequisites are installed
3. Check that .NET 8.0 is properly installed
4. Make sure Visual Studio 2022 has the ".NET desktop development" workload

## Version History

**v1.0.0** - Initial Release
- Auto Clicker with unlimited CPS
- Macro Recorder/Player
- Roblox Integration
- FPS/Ping Optimizer
- Dark modern UI theme

---

**Happy Gaming! 🎮**
