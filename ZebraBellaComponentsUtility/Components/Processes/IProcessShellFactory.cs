namespace ZebraBellaComponentsUtility.Components.Processes
{
    public interface IProcessShellFactory
    {
        ProcessShell Create(string componentName);
    }
}