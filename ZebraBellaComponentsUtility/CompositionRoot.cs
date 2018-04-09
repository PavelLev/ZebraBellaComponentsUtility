using DryIoc;
using Microsoft.Extensions.Configuration;
using NCode.ReparsePoints;
using Newtonsoft.Json;
using ZebraBellaComponentsUtility.Components;
using ZebraBellaComponentsUtility.Components.Alarms;
using ZebraBellaComponentsUtility.Components.FileTreeAltering;
using ZebraBellaComponentsUtility.Components.Processes;
using ZebraBellaComponentsUtility.Components.Profiles;
using ZebraBellaComponentsUtility.Utility;
using ZebraBellaComponentsUtility.Utility.CustomMessageBoxes;
using ZebraBellaComponentsUtility.Utility.Extensions;
using ZebraBellaComponentsUtility.Utility.IO;

namespace ZebraBellaComponentsUtility
{
    public class CompositionRoot
    {
        public CompositionRoot(IContainer container)
        {
            RegisterConfiguration(container);

            RegisterComponents(container);

            RegisterUtility(container);

            container.Register<TaskbarIconViewModel>(Reuse.Singleton);
        }



        private void RegisterConfiguration(IContainer container)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("ZebraBellaComponentsUtility.Configuration.json");

            var configuration = builder.Build();


            container.Configure<AlternativeFileTree>(configuration.GetSection("AlternativeFileTree"));

            container.Configure<ApplicationRelativePathConfiguration>(configuration.GetSection("ApplicationRelativePaths"));

            container.Configure<ComponentRelativePathConfiguration>(configuration.GetSection("ComponentRelativePaths"));

            container.Configure<DomainRelativePathConfiguration>(configuration.GetSection("DomainRelativePaths"));

            container.Configure<MiscellaneousConfiguration>(configuration.GetSection("Miscellaneous"));
        }



        private void RegisterComponents(IContainer container)
        {
            container.Register<IUnexpectedStopAlarmService, UnexpectedStopAlarmService>(Reuse.Singleton);


            container.Register<IAlternativeFileTreeService, AlternativeFileTreeService>(Reuse.Singleton);


            container.Register<IProcessShellFactory, ProcessShellFactory>(Reuse.Singleton);


            container.Register<ComponentNameViewModel>();

            container.Register<IProfileService, ProfileService>(Reuse.Singleton);

            container.Register<Profile>();

            container.Register<ProfileEditingView>();

            container.Register<ProfileEditingViewModel>();

            container.Register<ProfileManagementView>();

            container.Register<ProfileManagementViewModel>();

            container.Register<ProfileViewModel>();


            container.Register<IComponentsService, ComponentsService>(Reuse.Singleton);
        }



        private void RegisterUtility(IContainer container)
        {
            container.Register<ICustomMessageBoxService, CustomMessageBoxService>(Reuse.Singleton);

            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);

            container.Register<IFileService, FileService>(Reuse.Singleton);

            container.Register<JsonSerializer>();

            container.Register<IPathService, PathService>(Reuse.Singleton);

            container.Register<IPathEqualityComparer, PathEqualityComparer>(Reuse.Singleton);

            container.UseInstance(ReparsePointFactory.Provider);

            container.Register<IWinApi, WinApi>(Reuse.Singleton);
        }
    }
}
