using System.Configuration;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class AlternativeFileTree : ConfigurationElement
    {
        [ConfigurationProperty(nameof(DirectoryJunctions), IsRequired = true, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DirectoryJunctionCollection), AddItemName = "Add")]
        public DirectoryJunctionCollection DirectoryJunctions => 
            (DirectoryJunctionCollection) this[nameof(DirectoryJunctions)];
    }
}