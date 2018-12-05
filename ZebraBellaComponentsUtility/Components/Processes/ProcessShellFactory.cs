using System.Diagnostics;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.Components.Processes
{
    public class ProcessShellFactory : IProcessShellFactory
    {
        private readonly IPathService _pathService;
        private readonly MiscellaneousConfiguration _miscellaneousConfiguration;

        public ProcessShellFactory(IPathService pathService, MiscellaneousConfiguration miscellaneousConfiguration)
        {
            _pathService = pathService;
            _miscellaneousConfiguration = miscellaneousConfiguration;
        }

        public ProcessShell Create(string componentName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments =
                        $"-Command \"$host.ui.RawUI.WindowTitle = '{componentName}'; ./{_pathService.GetExecutableFileName()}\" {_miscellaneousConfiguration.ComponentCommandLineArguments}",
                    WorkingDirectory = _pathService.GetExecutableDirectoryPath(componentName)
                },
                EnableRaisingEvents = true
            };

            return new ProcessShell(componentName, process);
        }
    }
}