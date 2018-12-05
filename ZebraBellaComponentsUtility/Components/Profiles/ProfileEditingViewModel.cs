using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.Components.Profiles
{
    public class ProfileEditingViewModel : BindableBase
    {
        private string _name;
        public event Action<ProfileViewModel> ProcessFinished;



        public ProfileEditingViewModel
            (
            IProfileService profileService,
            IPathService pathService,
            Func<string, IEnumerable<string>, ProfileViewModel> profileViewModelFunc,
            ProfileViewModel originalProfileViewModel,
            Func<string, bool, ComponentNameViewModel> componentViewModelFunc
            )
        {
            IsCreation = string.IsNullOrEmpty(originalProfileViewModel.Name);

            Name = originalProfileViewModel.Name;

            ComponentNameViewModels = pathService.EnumerateComponents()
                .Select
                (
                componentName =>
                    componentViewModelFunc
                        (
                        componentName, 
                        originalProfileViewModel.ComponentNames
                            .Contains
                            (
                            componentName
                            )
                        )
                )
                .ToArray();

            OkCommand = new DelegateCommand(() =>
            {
                var selectedComponentNames = ComponentNameViewModels.Where
                    (
                    componentNameViewModel =>
                        componentNameViewModel.IsSelected
                    )
                    .Select
                    (
                    componentNameViewModel =>
                        componentNameViewModel.Value
                    )
                    .ToArray();

                if (string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("You should specify name");
                }
                else if (IsCreation && profileService.Profiles.Any(profile => profile.Name == Name))
                {
                    MessageBox.Show("You should specify unique name");
                }
                else if (!selectedComponentNames.Any())
                {
                    MessageBox.Show("Select at least one component");
                }
                else
                {
                    ProcessFinished?.Invoke(profileViewModelFunc(Name, selectedComponentNames));
                }
            });

            CancelCommand = new DelegateCommand
                (
                () => 
                    ProcessFinished?.Invoke(null)
                );
        }



        public string Name
        {
            get =>
                _name;
            set =>
                SetProperty(ref _name, value);
        }

        public ComponentNameViewModel[] ComponentNameViewModels
        {
            get;
        }

        public bool IsCreation
        {
            get;
        }

        public DelegateCommand OkCommand
        {
            get;
        }

        public DelegateCommand CancelCommand
        {
            get;
        }
    }
}
