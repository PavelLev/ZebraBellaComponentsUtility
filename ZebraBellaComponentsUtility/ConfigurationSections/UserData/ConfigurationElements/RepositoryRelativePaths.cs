using System.Configuration;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class RepositoryRelativePaths : ConfigurationElement
    {
        [ConfigurationProperty(nameof(ComponentsFolder), IsRequired = true)]
        public string ComponentsFolder
        {
            get => (string)this[nameof(ComponentsFolder)];
            set => this[nameof(ComponentsFolder)] = value;
        }

        [ConfigurationProperty(nameof(AlternativeFileTreeFolder), IsRequired = true)]
        public string AlternativeFileTreeFolder
        {
            get => (string)this[nameof(AlternativeFileTreeFolder)];
            set => this[nameof(AlternativeFileTreeFolder)] = value;
        }
    }
}