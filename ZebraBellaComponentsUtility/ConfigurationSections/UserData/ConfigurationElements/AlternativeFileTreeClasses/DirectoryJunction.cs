using System.Configuration;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class DirectoryJunction : ConfigurationElement
    {
        [ConfigurationProperty(nameof(Name), IsRequired = true)]
        public string Name
        {
            get => (string) this[nameof(Name)];
            set => this[nameof(Name)] = value;
        }

        [ConfigurationProperty(nameof(RepositoryRelativePath), IsRequired = true)]
        public string RepositoryRelativePath
        {
            get => (string)this[nameof(RepositoryRelativePath)];
            set => this[nameof(RepositoryRelativePath)] = value;
        }
    }
}