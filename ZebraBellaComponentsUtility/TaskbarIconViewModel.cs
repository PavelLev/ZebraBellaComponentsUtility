using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using ZebraBellaComponentsUtility.Components;

namespace ZebraBellaComponentsUtility
{
    public class TaskbarIconViewModel
    {
        public TaskbarIconViewModel(IComponentsService componentsService)
        {
            StartCommand = new DelegateCommand(componentsService.Start);

            RestartCommand = new DelegateCommand(componentsService.Restart);

            StopCommand = new DelegateCommand(componentsService.Stop);

            ClearStorageCommand = new DelegateCommand(componentsService.ClearStorage);

            ClearLogsCommand = new DelegateCommand(componentsService.ClearLogs);

            ExitCommand = new DelegateCommand(() =>
            {
                componentsService.Stop();
                Application.Current.Shutdown();
            });
        }

        public DelegateCommand StartCommand { get; }

        public DelegateCommand RestartCommand { get; }

        public DelegateCommand StopCommand { get; }

        public DelegateCommand ClearStorageCommand { get; }

        public DelegateCommand ClearLogsCommand { get; }

        public DelegateCommand ExitCommand { get; }

        public DelegateCommand ExampleCommand { get; } = new DelegateCommand(() =>
        {
            var myWindow = new MyWindow();
            myWindow.ShowDialog();
            ;
        });
    }
}