using System.Configuration;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class Miscellaneous : ConfigurationElement
    {
        [ConfigurationProperty(nameof(BellaCloseDelay), IsRequired = true)]
        public int BellaCloseDelay
        {
            get => (int)this[nameof(BellaCloseDelay)];
            set => this[nameof(BellaCloseDelay)] = value;
        }

        [ConfigurationProperty(nameof(AlarmDelay), IsRequired = true)]
        public int AlarmDelay
        {
            get => (int)this[nameof(AlarmDelay)];
            set => this[nameof(AlarmDelay)] = value;
        }
    }
}