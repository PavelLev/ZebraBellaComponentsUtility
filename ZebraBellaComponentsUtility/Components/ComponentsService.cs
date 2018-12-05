using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ZebraBellaComponentsUtility.Components.Alarms;
using ZebraBellaComponentsUtility.Components.Processes;
using ZebraBellaComponentsUtility.Components.Profiles;
using ZebraBellaComponentsUtility.Utility;
using ZebraBellaComponentsUtility.Utility.WinApiTypes;

namespace ZebraBellaComponentsUtility.Components
{
    public class ComponentsService : IComponentsService
    {
        private readonly ManualResetEvent _allProcessesExitedResetEvent = new ManualResetEvent(false);
        private readonly List<ProcessShell> _componentProcessShells = new List<ProcessShell>();
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly MiscellaneousConfiguration _miscellaneousConfiguration;
        private readonly IPathService _pathService;
        private readonly IProcessShellFactory _processShellFactory;
        private readonly IProfileService _profileService;

        private readonly object _syncRoot = new object();
        private readonly IUnexpectedStopAlarmService _unexpectedStopAlarmService;
        private readonly IWinApi _winApi;



        public ComponentsService
            (
            IPathService pathService,
            IWinApi winApi,
            IProcessShellFactory processShellFactory,
            MiscellaneousConfiguration miscellaneousConfiguration,
            IDirectoryService directoryService,
            IFileService fileService,
            IUnexpectedStopAlarmService unexpectedStopAlarmService,
            IProfileService profileService
            )
        {
            _pathService = pathService;
            _winApi = winApi;
            _processShellFactory = processShellFactory;
            _miscellaneousConfiguration = miscellaneousConfiguration;
            _directoryService = directoryService;
            _fileService = fileService;
            _unexpectedStopAlarmService = unexpectedStopAlarmService;
            _profileService = profileService;
        }



        private IEnumerable<string> ComponentNames =>
            _profileService.FilterComponents
                (
                _pathService.EnumerateComponents()
                );



        public void Start()
        {
            lock (_syncRoot)
            {
                _allProcessesExitedResetEvent.Reset();

                var componentsToCreate = ComponentNames
                    .Except(_componentProcessShells.Select(processShell => processShell.ComponentName))
                    .ToArray();

                _componentProcessShells.AddRange(componentsToCreate
                    .Select(componentName =>
                    {
                        var componentProcessShell = _processShellFactory.Create(componentName);

                        componentProcessShell.Process.EnableRaisingEvents = true;

                        componentProcessShell.Process.Exited += (sender, args) =>
                        {
                            lock (_syncRoot)
                            {
                                if (componentProcessShell.Process.ExitCode != 0 &&
                                    !componentProcessShell.ShouldDie)
                                {
                                    _unexpectedStopAlarmService.Alarm(componentProcessShell.ComponentName);
                                }

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
            lock (_syncRoot)
            {
                if (!_componentProcessShells.Any())
                {
                    return;
                }

                foreach (var processShell in _componentProcessShells.ToArray())
                {
                    _winApi.PostMessage(processShell.Process.MainWindowHandle, WindowsMessageType.KeyDown, (int) KeyCode.DownArrow, 0);

                    _winApi.PostMessage(processShell.Process.MainWindowHandle, WindowsMessageType.KeyDown, (int) KeyCode.Enter, 0);
                }
            }

            if (!_allProcessesExitedResetEvent.WaitOne(_miscellaneousConfiguration.BellaCloseDelay))
            {
                var result = MessageBox.Show(new Form(), "Do you want to kill processes anyway?", "BellaCloseDelay has been exceeded", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    KillPowerShellProcesses();
                }
            }
        }



        public void ClearStorage()
        {
            Stop();

            var storageDirectoryPaths = ComponentNames.Except(_miscellaneousConfiguration.PermanentStorageComponentNames)
                .Select(_pathService.GetStorageDirectoryPath);

            ClearDirectories(storageDirectoryPaths);
        }



        public void ClearLogs()
        {
            var storageDirectoryPaths = ComponentNames.Select(_pathService.GetLogsDirectoryPath);

            ClearDirectories(storageDirectoryPaths, true);
        }



        private void KillPowerShellProcesses()
        {
            lock (_syncRoot)
            {
                foreach (var componentProcessShell in _componentProcessShells)
                {
                    componentProcessShell.ShouldDie = true;

                    componentProcessShell.Process.CloseMainWindow();
                }

                _componentProcessShells.Clear();
            }
        }



        private IEnumerable<Process> FindProcessesWithChild()
        {
            var snapshotHandle = _winApi.CreateToolhelp32Snapshot(SnapshotType.AllProcesses, 0);

            var processEntry = new ProcessEntry
            {
                StructureSize = (uint) Marshal.SizeOf<ProcessEntry>()
            };

            _winApi.Process32First(snapshotHandle, ref processEntry);

            do
            {
                if (processEntry.ExecutableFileName == "BellaDomain.exe" &&
                    _componentProcessShells.Any(process => process.Process.Id == processEntry.ParentProcessId))
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



        private void ClearDirectories(IEnumerable<string> directoryPaths, bool ignoreDeletionFail = false)
        {
            foreach (var directoryPath in directoryPaths)
            {
                if (!_directoryService.Exists(directoryPath))
                {
                    continue;
                }

                var files = _directoryService.EnumerateFiles(directoryPath);

                var failedFiles = new List<string>();

                foreach (var file in files)
                {
                    try
                    {
                        _fileService.Delete(file);
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
