using System.Configuration;
using ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData
{
    public class UserData : ConfigurationSection
    {
        [ConfigurationProperty(nameof(AlternativeFileTree), IsRequired = false)]
        public AlternativeFileTree AlternativeFileTree =>
            (AlternativeFileTree) this[nameof(AlternativeFileTree)];

        [ConfigurationProperty(nameof(ApplicationRelativePaths), IsRequired = false)]
        public ApplicationRelativePaths ApplicationRelativePaths =>
            (ApplicationRelativePaths) this[nameof(ApplicationRelativePaths)];

        [ConfigurationProperty(nameof(ComponentRelativePaths), IsRequired = false)]
        public ComponentRelativePaths ComponentRelativePaths =>
            (ComponentRelativePaths) this[nameof(ComponentRelativePaths)];

        [ConfigurationProperty(nameof(Miscellaneous), IsRequired = false)]
        public Miscellaneous Miscellaneous =>
            (Miscellaneous) this[nameof(Miscellaneous)];

        [ConfigurationProperty(nameof(RepositoryRelativePaths), IsRequired = false)]
        public RepositoryRelativePaths RepositoryRelativePaths =>
            (RepositoryRelativePaths) this[nameof(RepositoryRelativePaths)];
    }
}