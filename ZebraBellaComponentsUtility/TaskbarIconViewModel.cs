using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Prism.Commands;
using ZebraBellaComponentsUtility.Components;
using ZebraBellaComponentsUtility.Components.FileTreeAltering;
using ZebraBellaComponentsUtility.Components.Profiles;
using ZebraBellaComponentsUtility.Utility.Extensions;
using Application = System.Windows.Application;

namespace ZebraBellaComponentsUtility
{
    public class TaskbarIconViewModel
    {
        private bool _isProfileManagementViewOpen = false;

        private readonly object _syncRoot = new object();
        private ProfileManagementView _currentProfileManagementView;



        public TaskbarIconViewModel
            (
            IComponentsService componentsService, 
            IAlternativeFileTreeService alternativeFileTreeService,
            Func<ProfileManagementView> profileManagementViewFunc)
        {
            StartCommand = new DelegateCommand(componentsService.Start);

            RestartCommand = new DelegateCommand(componentsService.Restart);

            StopCommand = new DelegateCommand(componentsService.Stop);

            ClearStorageCommand = new DelegateCommand(componentsService.ClearStorage);

            ClearLogsCommand = new DelegateCommand(componentsService.ClearLogs);

            CreateAlternativeFileTreeCommand = new DelegateCommand(() =>
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

            OpenProfileManagementCommand = new DelegateCommand(() =>
            {
                
                if (_currentProfileManagementView != null)
                {
                    _currentProfileManagementView.Activate();
                }
                else
                {
                    _currentProfileManagementView = profileManagementViewFunc();

                    _currentProfileManagementView.ShowDialog();

                    _currentProfileManagementView = null;
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

        public DelegateCommand CreateAlternativeFileTreeCommand { get; }

        public DelegateCommand OpenProfileManagementCommand { get; }

        public DelegateCommand ExitCommand { get; }
    }
}