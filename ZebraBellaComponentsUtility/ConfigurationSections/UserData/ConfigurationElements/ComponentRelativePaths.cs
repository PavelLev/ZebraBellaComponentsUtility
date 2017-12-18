using System.Configuration;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class ComponentRelativePaths : ConfigurationElement
    {
        [ConfigurationProperty(nameof(ExecutableDirectory), IsRequired = true)]
        public string ExecutableDirectory
        {
            get => (string)this[nameof(ExecutableDirectory)];
            set => this[nameof(ExecutableDirectory)] = value;
        }

        [ConfigurationProperty(nameof(ExecutableFile), IsRequired = true)]
        public string ExecutableFile
        {
            get => (string)this[nameof(ExecutableFile)];
            set => this[nameof(ExecutableFile)] = value;
        }

        [ConfigurationProperty(nameof(StorageDirectory), IsRequired = true)]
        public string StorageDirectory
        {
            get => (string)this[nameof(StorageDirectory)];
            set => this[nameof(StorageDirectory)] = value;
        }

        [ConfigurationProperty(nameof(LogsDirectory), IsRequired = true)]
        public string LogsDirectory
        {
            get => (string)this[nameof(LogsDirectory)];
            set => this[nameof(LogsDirectory)] = value;
        }
    }
}