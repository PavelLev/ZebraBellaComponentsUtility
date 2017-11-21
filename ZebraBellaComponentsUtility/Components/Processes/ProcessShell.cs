using System.Diagnostics;

namespace ZebraBellaComponentsUtility.Components.Processes
{
    public class ProcessShell
    {
        public ProcessShell(string componentName, Process process)
        {
            ComponentName = componentName;
            Process = process;
        }

        public string ComponentName { get; }

        public Process Process { get; }
    }
}