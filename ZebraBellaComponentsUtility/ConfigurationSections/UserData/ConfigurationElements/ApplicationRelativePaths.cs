using System.Configuration;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class ApplicationRelativePaths : ConfigurationElement
    {
        [ConfigurationProperty(nameof(RepositoryRoot), IsRequired = true, DefaultValue = "/")]
        [RegexStringValidator(ConfigurationRegex.Directory)]
        public string RepositoryRoot
        {
            get => (string)this[nameof(RepositoryRoot)];
            set => this[nameof(RepositoryRoot)] = value;
        }
    }
}