using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using NCode.ReparsePoints;
using ZebraBellaComponentsUtility.Components;
using ZebraBellaComponentsUtility.Components.Alarms;
using ZebraBellaComponentsUtility.Components.Processes;
using ZebraBellaComponentsUtility.ConfigurationSections.UserData;
using ZebraBellaComponentsUtility.DryIoc;
using ZebraBellaComponentsUtility.Utility.CustomMessageBoxes;

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
            Container.Register<IUnexpectedStopAlarmService, UnexpectedStopAlarmService>(Reuse.Singleton);

            Container.Register<IProcessShellFactory, ProcessShellFactory>(Reuse.Singleton);


            Container.Register<IAlternativeFileTreeService, AlternativeFileTreeService>(Reuse.Singleton);

            Container.Register<IComponentsService, ComponentsService>(Reuse.Singleton);
            

            Container.Register<ICustomMessageBoxService, CustomMessageBoxService>(Reuse.Singleton);

            Container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);

            Container.Register<IFileService, FileService>(Reuse.Singleton);

            Container.Register<IPathService, PathService>(Reuse.Singleton);

            Container.Register<IPathEqualityComparer, PathEqualityComparer>(Reuse.Singleton);

            Container.UseInstance(ReparsePointFactory.Provider);

            Container.Register<IWinApi, WinApi>(Reuse.Singleton);


            UserData userData;

            try
            {
                userData = (UserData) ConfigurationManager.GetSection("UserData");
            }
            catch (Exception exception)
            {
                MessageBox.Show(new Window(), exception.Message, "Invalid configuration");

                Process.GetCurrentProcess().Kill();
                return;
            }

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