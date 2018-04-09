using System.Configuration;

namespace ZebraBellaComponentsUtility.Utility.IO
{
    public class DomainRelativePathConfiguration
    {
        public string ComponentsFolder
        {
            get;
            set;
        }
        
        public string AlternativeFileTreeFolder
        {
            get;
            set;
        }
    }
}