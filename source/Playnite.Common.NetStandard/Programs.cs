using Microsoft.Win32;
using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Playnite.Common
{
    public class Program
    {
        public string Path { get; set; }
        public string Arguments { get; set; }
        public string Icon { get; set; }
        public string WorkDir { get; set; }
        public string Name { get; set; }
        public string AppId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class UninstallProgram
    {
        public string DisplayIcon { get; set; }
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public string InstallLocation { get; set; }
        public string Publisher { get; set; }
        public string UninstallString { get; set; }
        public string URLInfoAbout { get; set; }
        public string RegistryKeyName { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return DisplayName ?? RegistryKeyName;
        }
    }

    public class Programs
    {
        private static ILogger logger = LogManager.GetLogger();

        public static void CreateShortcut(string executablePath, string arguments, string iconPath, string shortcutPath)
        {
            throw new NotImplementedException();
        }

        public static async Task<List<Program>> GetExecutablesFromFolder(string path, SearchOption searchOption, CancellationTokenSource cancelToken = null)
        {
            return await Task.Run(() =>
            {
                var execs = new List<Program>();
                var files = new SafeFileEnumerator(path, "*.exe", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    if (cancelToken?.IsCancellationRequested == true)
                    {
                        return null;
                    }

                    if (file.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        continue;
                    }

                    var versionInfo = FileVersionInfo.GetVersionInfo(file.FullName);
                    var programName = !string.IsNullOrEmpty(versionInfo.ProductName?.Trim()) ? versionInfo.ProductName : new DirectoryInfo(Path.GetDirectoryName(file.FullName)).Name;

                    execs.Add(new Program()
                    {
                        Path = file.FullName,
                        Icon = file.FullName,
                        WorkDir = Path.GetDirectoryName(file.FullName),
                        Name = programName
                    });
                }

                return execs;
            });
        }

        public static async Task<List<Program>> GetShortcutProgramsFromFolder(string path, CancellationTokenSource cancelToken = null)
        {
            return await Task.Run(() =>
            {
                throw new NotImplementedException();
#pragma warning disable CS0162 // Unreachable code detected
                // The Task.Run method needs this return to infer the return type
                return new List<Program>();
#pragma warning restore CS0162 // Unreachable code detected
            });
        }

        public static async Task<List<Program>> GetInstalledPrograms(CancellationTokenSource cancelToken = null)
        {
            var apps = new List<Program>();

            // Get apps from All Users
            var allPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs");
            var allApps = await GetShortcutProgramsFromFolder(allPath);
            if (cancelToken?.IsCancellationRequested == true)
            {
                return null;
            }
            else
            {
                apps.AddRange(allApps);
            }

            // Get current user apps
            var userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs");
            var userApps = await GetShortcutProgramsFromFolder(userPath);
            if (cancelToken?.IsCancellationRequested == true)
            {
                return null;
            }
            else
            {
                apps.AddRange(userApps);
            }

            return apps;
        }

        private static string GetUWPGameIcon(string defPath)
        {
            if (File.Exists(defPath))
            {
                return defPath;
            }

            var folder = Path.GetDirectoryName(defPath);
            var fileMask = Path.GetFileNameWithoutExtension(defPath) + ".scale*.png";
            var files = Directory.GetFiles(folder, fileMask);

            if (files == null || files.Count() == 0)
            {
                return string.Empty;
            }
            else
            {
                var icons = files.Where(a => Regex.IsMatch(a, @"\.scale-\d+\.png"));
                if (icons.Any())
                {
                    return icons.OrderBy(a => a).Last();
                }

                return string.Empty;
            }
        }

        public static List<Program> GetUWPApps()
        {
            throw new NotImplementedException();
        }

        private static List<UninstallProgram> GetUninstallProgsFromView(RegistryView view)
        {
            var rootString = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
            void SearchRoot(RegistryHive hive, List<UninstallProgram> programs)
            {
                var root = RegistryKey.OpenBaseKey(hive, view);
                var keyList = root.OpenSubKey(rootString);
                if (keyList == null)
                {
                    return;
                }

                foreach (var key in keyList.GetSubKeyNames())
                {
                    var prog = root.OpenSubKey(rootString + key);
                    var program = new UninstallProgram()
                    {
                        DisplayIcon = prog.GetValue("DisplayIcon")?.ToString(),
                        DisplayVersion = prog.GetValue("DisplayVersion")?.ToString(),
                        DisplayName = prog.GetValue("DisplayName")?.ToString(),
                        InstallLocation = prog.GetValue("InstallLocation")?.ToString().Replace("\"", string.Empty),
                        Publisher = prog.GetValue("Publisher")?.ToString(),
                        UninstallString = prog.GetValue("UninstallString")?.ToString(),
                        URLInfoAbout = prog.GetValue("URLInfoAbout")?.ToString(),
                        Path = prog.GetValue("Path")?.ToString(),
                        RegistryKeyName = key
                    };

                    programs.Add(program);
                }
            }

            var progs = new List<UninstallProgram>();
            SearchRoot(RegistryHive.LocalMachine, progs);
            SearchRoot(RegistryHive.CurrentUser, progs);
            return progs;
        }

        public static List<UninstallProgram> GetUnistallProgramsList()
        {
            var progs = new List<UninstallProgram>();

            if (Environment.Is64BitOperatingSystem)
            {
                progs.AddRange(GetUninstallProgsFromView(RegistryView.Registry64));
            }

            progs.AddRange(GetUninstallProgsFromView(RegistryView.Registry32));
            return progs;
        }

        public static Program ParseShortcut(string path)
        {
            throw new NotImplementedException();
        }
    }
}
