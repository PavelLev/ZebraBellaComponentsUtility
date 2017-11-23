using System.Configuration;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class ApplicationRelativePaths : ConfigurationElement
    {
        [ConfigurationProperty(nameof(RepositoryRoot), IsRequired = true)]
        public string RepositoryRoot
        {
            get => (string)this[nameof(RepositoryRoot)];
            set => this[nameof(RepositoryRoot)] = value;
        }
    }
}