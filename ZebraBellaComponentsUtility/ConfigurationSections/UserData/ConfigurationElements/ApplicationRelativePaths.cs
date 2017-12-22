using System.Configuration;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class ApplicationRelativePaths : ConfigurationElement
    {
        [ConfigurationProperty(nameof(DomainRoot), IsRequired = true)]
        public string DomainRoot
        {
            get => (string)this[nameof(DomainRoot)];
            set => this[nameof(DomainRoot)] = value;
        }
    }
}