using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements
{
    public class DirectoryJunctionCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DirectoryJunction();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DirectoryJunction) element).Name;
        }
    }
}