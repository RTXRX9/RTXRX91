# Quick Start Guide - Roblox Toolkit

## For Visual Studio 2022

### 🚀 Fast Setup (5 Steps)

1. **Install Visual Studio 2022**
   - Download from: https://visualstudio.microsoft.com/downloads/
   - Select ".NET desktop development" workload during installation

2. **Open the Project**
   - Launch Visual Studio 2022
   - File → Open → Project/Solution
   - Select `RobloxToolkit.sln`

3. **Restore Packages**
   - Right-click solution in Solution Explorer
   - Click "Restore NuGet Packages"

4. **Build**
   - Press `Ctrl + Shift + B`
   - Or: Build menu → Build Solution

5. **Run**
   - Press `F5` to run
   - Or: Click green "Start" button

### 📁 Project Structure

```
RobloxToolkit/
├── RobloxToolkit.sln          ← Open this in Visual Studio
├── RobloxToolkit/
│   ├── Core/                  ← Backend logic
│   ├── Models/                ← Data models
│   ├── Styles/                ← Dark theme
│   ├── MainWindow.xaml        ← Main UI
│   └── MainWindow.xaml.cs     ← UI logic
└── README.md                  ← Full documentation
```

### 🎯 First Run

After building, the app will be at:
```
RobloxToolkit\bin\Debug\net8.0-windows\RobloxToolkit.exe
```

For release version:
1. Change dropdown to "Release"
2. Build again
3. Find at: `RobloxToolkit\bin\Release\net8.0-windows\RobloxToolkit.exe`

### ⚡ Quick Test

1. Run the app (F5)
2. Go to "Auto Clicker" tab
3. Set CPS to 10
4. Click "Start Clicker" or press F6
5. See statistics update in real-time

### 🛠️ Common Issues

**Build fails?**
- Make sure .NET 8.0 SDK is installed
- Tools → NuGet Package Manager → Clear NuGet Caches

**Missing references?**
- Right-click solution → Restore NuGet Packages

**Can't find executable?**
- Check: `RobloxToolkit\bin\Debug\net8.0-windows\`

### 📚 Full Documentation

See `README.md` for:
- Complete feature list
- Detailed usage guide
- Troubleshooting
- Advanced customization

---

**Need help?** Check the README.md file for detailed instructions.
