using System;
using System.Windows;
using Prism.Commands;
using ZebraBellaComponentsUtility.Components;

namespace ZebraBellaComponentsUtility
{
    public class TaskbarIconViewModel
    {
        public TaskbarIconViewModel(IComponentsService componentsService, IAlternativeFileTreeService alternativeFileTreeService)
        {
            StartCommand = new DelegateCommand(componentsService.Start);

            RestartCommand = new DelegateCommand(componentsService.Restart);

            StopCommand = new DelegateCommand(componentsService.Stop);

            ClearStorageCommand = new DelegateCommand(componentsService.ClearStorage);

            ClearLogsCommand = new DelegateCommand(componentsService.ClearLogs);

            CreateAlternativeFileTree = new DelegateCommand(() =>
            {
                try
                {
                    alternativeFileTreeService.Create();
                }
                catch (Exception exception)
                {
                    System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form(), exception.Message, "Error while creating alternative file tree directory");
                }
            });

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

        public DelegateCommand CreateAlternativeFileTree { get; }

        public DelegateCommand ExitCommand { get; }
    }
}