using System;
using System.Windows;

namespace ZebraBellaComponentsUtility.Components.Profiles
{
    /// <summary>
    /// Interaction logic for ProfileEditingView.xaml
    /// </summary>
    public partial class ProfileEditingView
    {
        public ProfileEditingView
            (
            ProfileViewModel profileViewModel,
            Func<ProfileViewModel, ProfileEditingViewModel> profileEditingViewModelFunc
            )
        {
            var profileEditingViewModel = profileEditingViewModelFunc(profileViewModel);

            profileEditingViewModel.ProcessFinished += processedProfileViewModel =>
            {
                ProcessedProfileViewModel = processedProfileViewModel;
                Close();
            };

            DataContext = profileEditingViewModel;


            InitializeComponent();
        }



        public ProfileViewModel ProcessedProfileViewModel
        {
            get;
            private set;
        }
    }
}
