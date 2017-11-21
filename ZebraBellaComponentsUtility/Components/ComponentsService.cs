using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ZebraBellaComponentsUtility.Components.Processes;
using ZebraBellaComponentsUtility.Utility;
using ZebraBellaComponentsUtility.Utility.WinApiTypes;

namespace ZebraBellaComponentsUtility.Components
{
    public class ComponentsService : IComponentsService
    {
        private readonly IPathService _pathService;
        private readonly IWinApi _winApi;
        private readonly IProcessShellFactory _processShellFactory;
        private readonly Settings _settings;
        private readonly List<ProcessShell> _componentProcessShells = new List<ProcessShell>();
        private readonly List<Process> _deadProcesses = new List<Process>();
        private readonly ManualResetEvent _allProcessesExitedResetEvent = new ManualResetEvent(false);

        public ComponentsService(IPathService pathService, IWinApi winApi, IProcessShellFactory processShellFactory, Settings settings)
        {
            _pathService = pathService;
            _winApi = winApi;
            _processShellFactory = processShellFactory;
            _settings = settings;
        }

        public void Start()
        {
            lock (_componentProcessShells)
            {
                _allProcessesExitedResetEvent.Reset();

                var allComponents = _pathService.EnumerateComponents().ToArray();

                var componentsToCreate = allComponents
                    .Except(_componentProcessShells.Select(processShell => processShell.ComponentName))
                    .ToArray();

                _componentProcessShells.AddRange(componentsToCreate
                    .Select(componentName =>
                    {
                        var componentProcessShell = _processShellFactory.Create(componentName);

                        componentProcessShell.Process.EnableRaisingEvents = true;
                        componentProcessShell.Process.Exited += (sender, args) =>
                        {
                            lock (_componentProcessShells)
                            {
                                _componentProcessShells.Remove(componentProcessShell);

                                if (!_componentProcessShells.Any())
                                {
                                    _allProcessesExitedResetEvent.Set();
                                }
                            }
                        };

                        componentProcessShell.Process.Start();

                        return componentProcessShell;
                    }));
            }
        }

        public void Restart()
        {
            Stop();

            Start();
        }

        public void Stop()
        {
            if (!_componentProcessShells.Any())
            {
                return;
            }

            foreach (var processShell in _componentProcessShells.ToArray())
            {
                _winApi.PostMessage(processShell.Process.MainWindowHandle, WindowsMessageType.KeyDown, (int)KeyCode.Enter, 0);
            }

            if (!_allProcessesExitedResetEvent.WaitOne(_settings.BellaCloseDelay))
            {
                var result = MessageBox.Show(new Form(), "Do you want to kill processes anyway?", "BellaCloseDelay has been exceeded", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    KillPowerShellProcesses();
                }
            }
        }

        private void KillPowerShellProcesses()
        {
            lock (_componentProcessShells)
            {
                KillProcesses(_componentProcessShells.Select(processShell => processShell.Process));

                _componentProcessShells.Clear();
            }
        }

        private void KillProcesses(IEnumerable<Process> processes)
        {
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                    //Ignore
                }
            }
        }

        private IEnumerable<Process> FindProcessesWithChild()
        {
            var snapshotHandle = _winApi.CreateToolhelp32Snapshot(SnapshotType.AllProcesses, 0);

            var processEntry = new ProcessEntry
            {
                StructureSize = (uint)Marshal.SizeOf<ProcessEntry>()
            };

            _winApi.Process32First(snapshotHandle, ref processEntry);

            do
            {
                if (processEntry.ExecutableFileName == "BellaDomain.exe" && _componentProcessShells.Any(process => process.Process.Id == processEntry.ParentProcessId))
                {
                    foreach (var componentProcessShell in _componentProcessShells)
                    {
                        if (componentProcessShell.Process.Id == processEntry.ParentProcessId)
                        {
                            yield return componentProcessShell.Process;
                        }
                    }
                }
            } while (_winApi.Process32Next(snapshotHandle, ref processEntry));
        }

        public void ClearStorage()
        {
            Stop();

            var componentNames = _pathService.EnumerateComponents();

            var storageDirectoryPaths = componentNames.Select(_pathService.GetStorageDirectoryPath);

            ClearDirectories(storageDirectoryPaths);
        }

        public void ClearLogs()
        {
            var componentNames = _pathService.EnumerateComponents();

            var storageDirectoryPaths = componentNames.Select(_pathService.GetLogsDirectoryPath);

            ClearDirectories(storageDirectoryPaths, true);
        }

        private static void ClearDirectories(IEnumerable<string> directoryPaths, bool ignoreDeletionFail = false)
        {
            foreach (var directoryPath in directoryPaths)
            {
                if (!Directory.Exists(directoryPath))
                {
                    continue;
                }

                var files = Directory.EnumerateFiles(directoryPath);

                var failedFiles = new List<string>();

                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception exception)
                    {
                        if (!ignoreDeletionFail)
                        {
                            failedFiles.Add(exception.Message);
                        }
                    }
                }

                if (failedFiles.Count > 0)
                {
                    var joinedFailedFiles = string.Join("\n", failedFiles);
                    MessageBox.Show($"Unable to delete \n{joinedFailedFiles}");
                }
            }
        }
    }
}