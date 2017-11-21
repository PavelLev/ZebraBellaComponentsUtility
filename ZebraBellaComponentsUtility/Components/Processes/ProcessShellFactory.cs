using System.Diagnostics;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.Components.Processes
{
    public class ProcessShellFactory : IProcessShellFactory
    {
        private readonly IPathService _pathService;

        public ProcessShellFactory(IPathService pathService)
        {
            _pathService = pathService;
        }

        public ProcessShell Create(string componentName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments =
                        $"-Command \"$host.ui.RawUI.WindowTitle = '{componentName}'; ./{_pathService.GetExecutableFileName()}\"",
                    WorkingDirectory = _pathService.GetExecutableDirectoryPath(componentName)
                },
                EnableRaisingEvents = true
            };

            return new ProcessShell(componentName, process);
        }
    }
}