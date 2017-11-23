using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using ZebraBellaComponentsUtility.DryIoc;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility
{
    public partial class App
    {
        private TaskbarIcon _taskbarIcon;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Ioc.SetStaticResources(Resources);
            ;
            _taskbarIcon = (TaskbarIcon)FindResource("TaskbarIcon");
            _taskbarIcon.DataContext = Ioc.Container.Resolve<TaskbarIconViewModel>();
        }
    }
}
