# Visual Studio 2022 Step-by-Step Guide

## Complete Walkthrough for Building Roblox Toolkit

### Part 1: Installing Visual Studio 2022

#### Step 1.1: Download Visual Studio
1. Open your web browser
2. Go to: https://visualstudio.microsoft.com/downloads/
3. Click "Free download" under "Community 2022"
4. Save the installer file

#### Step 1.2: Run the Installer
1. Double-click the downloaded file (VisualStudioSetup.exe)
2. Click "Continue" on the installer splash screen
3. Wait for the installer to download components (1-2 minutes)

#### Step 1.3: Select Workloads
1. When the workload selection screen appears:
   - Find ".NET desktop development"
   - Check the box next to it
2. On the right side panel, verify these are included:
   - .NET 8.0 Runtime
   - .NET Framework 4.8 development tools
3. Click "Install" button (bottom right)
4. Wait for installation (15-30 minutes depending on internet speed)
5. Click "Launch" when complete

### Part 2: Opening the Project

#### Step 2.1: Launch Visual Studio 2022
1. Open Visual Studio 2022 from Start Menu
2. On the start screen, you'll see:
   - "Create a new project"
   - "Open a project or solution" ← Click this
   - "Clone a repository"

#### Step 2.2: Navigate to Project
1. In the file dialog:
   - Browse to where you extracted the project
   - Example: `C:\Projects\RobloxToolkit`
2. Look for `RobloxToolkit.sln` (the solution file)
3. Click on it
4. Click "Open"

#### Step 2.3: Wait for Project to Load
1. Visual Studio will load the project (5-10 seconds)
2. You'll see "Solution Explorer" panel on the right showing:
   ```
   Solution 'RobloxToolkit' (1 of 1 project)
   └── RobloxToolkit
       ├── Dependencies
       ├── Core
       ├── Models
       ├── Styles
       ├── App.xaml
       └── MainWindow.xaml
   ```

### Part 3: Restoring NuGet Packages

#### Step 3.1: Automatic Restoration
1. Visual Studio should automatically restore packages
2. Look at the bottom status bar for "Restoring NuGet packages..."
3. Wait for "Package restore complete" message

#### Step 3.2: Manual Restoration (if needed)
1. Right-click on "Solution 'RobloxToolkit'" in Solution Explorer
2. Click "Restore NuGet Packages"
3. Wait for completion (shown in Output window)

#### Step 3.3: Verify Package Installation
1. Expand "Dependencies" in Solution Explorer
2. Expand "Packages"
3. You should see:
   - Newtonsoft.Json (13.0.3)

### Part 4: Building the Project

#### Step 4.1: Select Build Configuration
1. Look at the top toolbar
2. Find the dropdown that says "Debug"
3. Click it and select "Release" (for final builds)
   - Use "Debug" for development/testing
   - Use "Release" for final executable

#### Step 4.2: Build the Solution
**Method 1: Menu**
1. Click "Build" in the top menu
2. Click "Build Solution"

**Method 2: Keyboard Shortcut**
1. Press `Ctrl + Shift + B`

**Method 3: Toolbar**
1. Right-click on "Solution 'RobloxToolkit'" in Solution Explorer
2. Click "Build Solution"

#### Step 4.3: Monitor Build Progress
1. Watch the "Output" window at the bottom
2. You'll see compilation messages scrolling
3. Wait for the final message:
   ```
   ========== Build: 1 succeeded, 0 failed, 0 up-to-date, 0 skipped ==========
   ```

#### Step 4.4: Check for Errors
If build fails:
1. Look at the "Error List" window (bottom panel)
2. Double-click any error to jump to the problem
3. Common issues:
   - Missing .NET 8.0 SDK → Install from Microsoft
   - Missing NuGet packages → Restore packages again
   - Corrupted files → Re-download project

### Part 5: Running the Application

#### Step 5.1: Run from Visual Studio (Debug Mode)
1. Press `F5` key
   - Or click green "▶ Start" button on toolbar
2. Application will launch
3. Visual Studio enters debug mode (can set breakpoints, inspect variables)
4. Close the app to return to Visual Studio

#### Step 5.2: Run Without Debugging
1. Press `Ctrl + F5`
   - Or click "Debug" menu → "Start Without Debugging"
2. Application runs without debugger attached (faster)

#### Step 5.3: Find the Built Executable

**Debug Build Location:**
```
RobloxToolkit\bin\Debug\net8.0-windows\RobloxToolkit.exe
```

**Release Build Location:**
```
RobloxToolkit\bin\Release\net8.0-windows\RobloxToolkit.exe
```

To find it:
1. Right-click "RobloxToolkit" project in Solution Explorer
2. Click "Open Folder in File Explorer"
3. Navigate to `bin\Release\net8.0-windows\`
4. You'll see `RobloxToolkit.exe`

### Part 6: Using Solution Explorer

#### Understanding the Project Structure

```
Solution 'RobloxToolkit'
│
└── RobloxToolkit (Project)
    │
    ├── Dependencies
    │   ├── Analyzers
    │   ├── Frameworks
    │   └── Packages
    │       └── Newtonsoft.Json (13.0.3)
    │
    ├── Core (Folder)
    │   ├── AutoClicker.cs         ← Auto-clicking logic
    │   ├── MacroRecorder.cs       ← Macro recording system
    │   └── RobloxMonitor.cs       ← Roblox process detection
    │
    ├── Models (Folder)
    │   ├── ClickerSettings.cs     ← Configuration classes
    │   └── MacroAction.cs         ← Macro data structures
    │
    ├── Styles (Folder)
    │   └── DarkTheme.xaml         ← Dark theme styles
    │
    ├── App.xaml                   ← Application config
    ├── App.xaml.cs                ← Application startup code
    ├── MainWindow.xaml            ← Main UI layout (XAML)
    └── MainWindow.xaml.cs         ← Main UI logic (C#)
```

#### Navigating Files
- **Double-click** any file to open it
- **XAML files**: Visual designer + code view
- **CS files**: C# code editor
- **Ctrl + Tab**: Switch between open files

### Part 7: Editing and Customization

#### Edit CPS Maximum Limit
1. In Solution Explorer, expand "RobloxToolkit"
2. Double-click "MainWindow.xaml"
3. Press `Ctrl + F` to open Find
4. Search for: `CpsSlider`
5. Find the line with `Maximum="500"`
6. Change 500 to your desired max (e.g., 1000)
7. Save: `Ctrl + S`
8. Rebuild: `Ctrl + Shift + B`

#### Change Theme Colors
1. Expand "Styles" folder
2. Double-click "DarkTheme.xaml"
3. At the top, find `<Color x:Key="AccentColor">#007ACC</Color>`
4. Change the hex color (e.g., `#FF6B35` for orange)
5. Save and rebuild

#### Change Default Hotkey
1. Open "MainWindow.xaml.cs"
2. Find line: `private Key currentHotkey = Key.F6;`
3. Change `Key.F6` to `Key.F7` (or any key)
4. Save and rebuild

### Part 8: Troubleshooting Build Issues

#### Error: "The type or namespace name could not be found"
**Solution:**
1. Right-click Solution → Restore NuGet Packages
2. Build → Clean Solution
3. Build → Rebuild Solution

#### Error: "Could not find SDK 'Microsoft.NET.Sdk'"
**Solution:**
1. Install .NET 8.0 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0
2. Restart Visual Studio

#### Error: "Package restore failed"
**Solution:**
1. Tools → Options → NuGet Package Manager
2. Click "Clear All NuGet Cache(s)"
3. Restore packages again

#### Error: "Access to path denied" during build
**Solution:**
1. Close the RobloxToolkit.exe if it's running
2. Disable antivirus temporarily
3. Run Visual Studio as Administrator

#### Build succeeds but exe doesn't run
**Solution:**
1. Make sure .NET 8.0 Runtime is installed
2. Right-click exe → Properties → Unblock
3. Check Windows Defender isn't quarantining it

### Part 9: Publishing for Distribution

#### Create Release Build
1. Switch to "Release" configuration (top dropdown)
2. Build → Rebuild Solution
3. Exe located at: `bin\Release\net8.0-windows\RobloxToolkit.exe`

#### Copy Files for Distribution
Files needed to run (copy all from Release folder):
- `RobloxToolkit.exe`
- `RobloxToolkit.dll`
- `RobloxToolkit.runtimeconfig.json`
- `Newtonsoft.Json.dll`
- Any other .dll files in the folder

#### Create Single-File Executable (Advanced)
1. Right-click project → "Publish"
2. Click "New" profile
3. Target: Folder
4. Configuration: Release
5. Target runtime: win-x64
6. Deployment mode: Self-contained
7. Click "Publish"
8. Single exe will be in publish folder

### Part 10: Keyboard Shortcuts Reference

| Action | Shortcut |
|--------|----------|
| Build Solution | `Ctrl + Shift + B` |
| Start Debugging | `F5` |
| Start Without Debugging | `Ctrl + F5` |
| Save File | `Ctrl + S` |
| Save All | `Ctrl + Shift + S` |
| Find in File | `Ctrl + F` |
| Find in All Files | `Ctrl + Shift + F` |
| Go to Definition | `F12` |
| Comment/Uncomment | `Ctrl + K, Ctrl + C` / `Ctrl + K, Ctrl + U` |
| Format Document | `Ctrl + K, Ctrl + D` |
| IntelliSense | `Ctrl + Space` |
| Close Current File | `Ctrl + F4` |
| Switch Files | `Ctrl + Tab` |

### Part 11: Understanding the Build Output Window

When you build, you'll see messages like:

```
1>------ Build started: Project: RobloxToolkit, Configuration: Release Any CPU ------
1>RobloxToolkit -> C:\Projects\RobloxToolkit\bin\Release\net8.0-windows\RobloxToolkit.dll
========== Build: 1 succeeded, 0 failed, 0 up-to-date, 0 skipped ==========
========== Build started at 10:30 AM and took 5.2 seconds ==========
```

**What this means:**
- `1>` = Project number (if multiple projects)
- `succeeded` = Build completed successfully
- `failed` = Build had errors (check Error List)
- `up-to-date` = No changes, didn't need to rebuild
- Final line shows total time

### Tips for Success

✅ **DO:**
- Save often (`Ctrl + S`)
- Build before running (`Ctrl + Shift + B`)
- Check Error List for problems
- Use IntelliSense for code completion
- Read error messages carefully

❌ **DON'T:**
- Edit files while program is running
- Delete files from Solution Explorer unless sure
- Ignore build warnings (yellow triangles)
- Forget to restore NuGet packages after downloading project

### Next Steps

1. Follow this guide to build the project
2. Run the application (F5)
3. Test all features:
   - Auto Clicker tab
   - Macro Recorder tab
   - Roblox Integration
   - Optimizer tools
4. Customize as desired
5. Build Release version for distribution

---

**Congratulations!** You now know how to build and customize the Roblox Toolkit in Visual Studio 2022.

For more information, see:
- `README.md` - Full feature documentation
- `QUICKSTART.md` - Quick reference guide
