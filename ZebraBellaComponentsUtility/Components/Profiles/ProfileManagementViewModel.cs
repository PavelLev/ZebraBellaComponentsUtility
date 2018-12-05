using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Prism.Commands;
using Prism.Mvvm;
using ZebraBellaComponentsUtility.Utility.Extensions;

namespace ZebraBellaComponentsUtility.Components.Profiles
{
    public class ProfileManagementViewModel : BindableBase
    {
        private ProfileViewModel _currentProfile;



        public ProfileManagementViewModel
            (
            Func<ProfileViewModel, ProfileEditingView> profileEditingViewFunc,
            Func<string, IEnumerable<string>, ProfileViewModel> profileViewModelFunc,
            IProfileService profileService
            )
        {
            Profiles = profileService.Profiles.Select
                    (
                    profile =>
                        profileViewModelFunc
                            (
                            profile.Name,
                            profile.ComponentNames
                            )
                    )
                .ToObservableCollection();

            CurrentProfile = Profiles.First(profileViewModel => profileViewModel.Name == profileService.CurrentProfile.Name);

   
            AddProfileCommand = new DelegateCommand
                (
                () =>
                {
                    var profileEditingView = profileEditingViewFunc
                        (
                        profileViewModelFunc
                            (
                            "",
                            Enumerable.Empty<string>()
                            )
                        );

                    profileEditingView.ShowDialog();

                    var processedProfileViewModel = profileEditingView.ProcessedProfileViewModel;

                    if (processedProfileViewModel != null)
                    {
                        Profiles.Add(processedProfileViewModel);
                        profileService.AddProfile
                            (
                            processedProfileViewModel.Name,
                            processedProfileViewModel.ComponentNames
                            );
                    }
                }
                );


            UpdateProfileCommand = new DelegateCommand<ProfileViewModel>
                (
                profileViewModel =>
                {
                    var profileEditingView = profileEditingViewFunc
                        (
                        profileViewModel
                        );

                    profileEditingView.ShowDialog();

                    var processedProfileViewModel = profileEditingView.ProcessedProfileViewModel;

                    if (processedProfileViewModel != null)
                    {
                        profileViewModel.ComponentNames = processedProfileViewModel.ComponentNames;

                        profileService.UpdateProfile
                            (
                            processedProfileViewModel.Name,
                            processedProfileViewModel.ComponentNames
                            );
                    }
                }
                );


            RemoveProfileCommand = new DelegateCommand<ProfileViewModel>
                (
                profileViewModel =>
                {
                    var messageBoxResult = MessageBox.Show(new Form(), "Are you sure?", "Confirm removal", MessageBoxButtons.YesNo);

                    if (messageBoxResult == DialogResult.Yes)
                    {
                        Profiles.Remove(profileViewModel);

                        profileService.RemoveProfile(profileViewModel.Name);
                    }
                }
                );


            SetCurrentProfileCommand = new DelegateCommand<ProfileViewModel>
                (
                profileViewModel =>
                {
                    CurrentProfile = profileViewModel;

                    profileService.SetCurrentProfile(profileViewModel.Name);
                }
                );
        }

        public ObservableCollection<ProfileViewModel> Profiles
        {
            get;
        }

        public ProfileViewModel CurrentProfile
        {
            get =>
                _currentProfile;
            set =>
                SetProperty(ref _currentProfile, value);
        }

        public DelegateCommand AddProfileCommand
        {
            get;
        }

        public DelegateCommand<ProfileViewModel> UpdateProfileCommand
        {
            get;
        }

        public DelegateCommand<ProfileViewModel> RemoveProfileCommand
        {
            get;
        }

        public DelegateCommand<ProfileViewModel> SetCurrentProfileCommand
        {
            get;
        }
    }
}
