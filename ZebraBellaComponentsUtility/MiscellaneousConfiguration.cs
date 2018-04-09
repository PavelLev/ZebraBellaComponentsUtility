using System.Configuration;

namespace ZebraBellaComponentsUtility
{
    public class MiscellaneousConfiguration
    {
        public int BellaCloseDelay
        {
            get;
            set;
        }
        
        public int AlarmDelay
        {
            get;
            set;
        }

        public string ProfileConfigurationFilePath
        {
            get;
            set;
        }
    }
}