using System.Configuration;
using System.Windows;
using NCode.ReparsePoints;
using ZebraBellaComponentsUtility.Components;
using ZebraBellaComponentsUtility.Components.Processes;
using ZebraBellaComponentsUtility.ConfigurationSections.UserData;
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

            Container.UseInstance(ReparsePointFactory.Provider);

            Container.Register<IWinApi, WinApi>();


            var userData = (UserData) ConfigurationManager.GetSection("UserDataGroup/UserData");

            Container.UseInstance(userData.AlternativeFileTree);

            Container.UseInstance(userData.ApplicationRelativePaths);

            Container.UseInstance(userData.ComponentRelativePaths);

            Container.UseInstance(userData.Miscellaneous);

            Container.UseInstance(userData.RepositoryRelativePaths);


            Container.Register<TaskbarIconViewModel>(Reuse.Singleton);
        }

        public static void SetStaticResources(ResourceDictionary resources)
        {

        }

        public static IContainer Container { get; } = new Container();
    }
}