using Playnite;
using Playnite.Common;
using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace RiotLibraryCore
{
    public class RiotGameController : BaseGameController
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        ProcessMonitor procMon;
        Stopwatch stopWatch;
        private static ILogger logger = LogManager.GetLogger();

        public RiotGameController(Game game) : base(game)
        {
        }

        public override void Dispose()
        {
            ReleaseResources();
        }

        void ReleaseResources()
        {
            if (procMon != null)
            {
                procMon.TreeStarted -= ProcMon_TreeStarted;
                procMon.TreeDestroyed -= Monitor_TreeDestroyed;
                procMon.Dispose();
            }
        }

        public override void Install()
        {
            throw new NotImplementedException();
        }

        public override void Play()
        {
            ReleaseResources();
            OnStarting(this, new GameControllerEventArgs(this, 0));

            if (Directory.Exists(Game.InstallDirectory))
            {
                stopWatch = Stopwatch.StartNew();
                StartGame();
                procMon = new ProcessMonitor();
                procMon.TreeStarted += ProcMon_TreeStarted;
                procMon.TreeDestroyed += Monitor_TreeDestroyed;
                procMon.WatchDirectoryProcesses(Game.InstallDirectory, false);
            }
            else
            {
                OnStopped(this, new GameControllerEventArgs(this, 0));
            }
        }

        public override void Uninstall()
        {
            throw new NotImplementedException();
        }

        void StartGame()
        {
            // First know if the game is already running
            var process = ProcessAlreadyRunning();
            if (process != null)
            {
                ShowWindow(process.MainWindowHandle, 9);
                SetForegroundWindow(process.MainWindowHandle);
                return;
            }
            GameActionActivator.ActivateAction(Game.PlayAction.ExpandVariables(Game));
        }

        Process ProcessAlreadyRunning()
        {
            var realPath = Game.InstallDirectory;
            try
            {
                realPath = Paths.GetFinalPathName(Game.InstallDirectory);
            }
            catch (Exception e)
            {
                logger.Error(e, $"Failed to get target path for a directory {Game.InstallDirectory}");
            }

            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                if (process.TryGetMainModuleFileName(out var procPath))
                {
                    if (procPath.IndexOf(realPath, StringComparison.OrdinalIgnoreCase) >= 0 && process.MainWindowHandle != IntPtr.Zero)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        void ProcMon_TreeStarted(object sender, EventArgs args)
        {
            OnStarted(this, new GameControllerEventArgs(this, 0));
        }

        void Monitor_TreeDestroyed(object sender, EventArgs args)
        {
            stopWatch.Stop();
            OnStopped(this, new GameControllerEventArgs(this, stopWatch.Elapsed.TotalSeconds));
        }
    }
}
