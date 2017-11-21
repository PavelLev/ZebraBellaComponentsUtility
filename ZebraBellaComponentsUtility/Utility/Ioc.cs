using System.Windows;
using NCode.ReparsePoints;
using ZebraBellaComponentsUtility.Components;
using ZebraBellaComponentsUtility.Components.Processes;
using ZebraBellaComponentsUtility.DryIoc;
using ZebraBellaComponentsUtility.Settings;

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
            Container.UseInstance(ReparsePointFactory.Provider);
            Container.Register<IWinApi, WinApi>();
            
            Container.UseInstance(ApplicationRelativePathSettings.Default);
            Container.UseInstance(ComponentRelativePathSettings.Default);
            Container.UseInstance(MiscellaneousSettings.Default);
            Container.UseInstance(RepositoryRelativePathSettings.Default);

            Container.Register<TaskbarIconViewModel>(Reuse.Singleton);
        }

        public static void SetStaticResources(ResourceDictionary resources)
        {

        }

        public static IContainer Container { get; } = new Container();
    }
}