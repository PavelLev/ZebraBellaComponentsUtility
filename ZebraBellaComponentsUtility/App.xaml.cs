using System;
using System.Collections.Generic;
using System.Windows;
using DryIoc;
using Hardcodet.Wpf.TaskbarNotification;
using ZebraBellaComponentsUtility.Components.Profiles;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility
{
    public partial class App
    {
        private TaskbarIcon _taskbarIcon;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var container = new Container();

            container.UseInstance(Resources);

            
            container.Register<CompositionRoot>(Reuse.Singleton);

            container.Resolve<CompositionRoot>();


            _taskbarIcon = (TaskbarIcon)FindResource("TaskbarIcon");

            _taskbarIcon.DataContext = container.Resolve<TaskbarIconViewModel>();
        }


    }
}
