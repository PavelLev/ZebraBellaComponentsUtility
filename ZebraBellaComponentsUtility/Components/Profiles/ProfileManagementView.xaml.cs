namespace ZebraBellaComponentsUtility.Components.Profiles
{
    /// <summary>
    /// Interaction logic for ProfileManagementView.xaml
    /// </summary>
    public partial class ProfileManagementView
    {
        public ProfileManagementView(ProfileManagementViewModel profileManagementViewModel)
        {
            DataContext = profileManagementViewModel;

            InitializeComponent();
        }
    }
}
