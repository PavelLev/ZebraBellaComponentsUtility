using System;
using System.Collections.Generic;

namespace ZebraBellaComponentsUtility.Components.Profiles
{
    public interface IProfileService
    {
        IEnumerable<string> FilterComponents(IEnumerable<string> source);

        IEnumerable<Profile> Profiles
        {
            get;
        }

        Profile CurrentProfile
        {
            get;
        }

        void AddProfile(string name, IEnumerable<string> componentNames);

        void UpdateProfile(string name, IEnumerable<string> componentNames);

        void RemoveProfile(string name);

        void SetCurrentProfile(string name);
    }
}
