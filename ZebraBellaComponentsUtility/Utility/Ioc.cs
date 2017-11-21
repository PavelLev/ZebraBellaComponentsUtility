using System.Windows;
using ZebraBellaComponentsUtility.Components;
using ZebraBellaComponentsUtility.Components.Processes;
using ZebraBellaComponentsUtility.DryIoc;

namespace ZebraBellaComponentsUtility.Utility
{
    public static class Ioc
    {
        static Ioc()
        {
            RegisterAllDependencies();
        }
        public static void RegisterAllDependencies()
        {
            Container.Register<IProcessShellFactory, ProcessShellFactory>(Reuse.Singleton);

            Container.Register<IComponentsService, ComponentsService>(Reuse.Singleton);
            
            Container.Register<IPathService, PathService>(Reuse.Singleton);
            Container.Register<IWinApi, WinApi>();

            Container.UseInstance(Settings.Default);

            Container.Register<TaskbarIconViewModel>(Reuse.Singleton);
        }

        public static void SetStaticResources(ResourceDictionary resources)
        {

        }

        public static IContainer Container { get; } = new Container();
    }
}