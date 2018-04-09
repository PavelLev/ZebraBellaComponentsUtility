using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ZebraBellaComponentsUtility.Utility.Extensions;

namespace ZebraBellaComponentsUtility.Components.Profiles
{
    public class ProfileService : IProfileService
    {
        private readonly JsonSerializer _jsonSerializer;
        private readonly string _profileConfigurationFilePath;
        private readonly List<Profile> _profiles;
        private Profile _currentProfile;



        public ProfileService
            (
            JsonSerializer jsonSerializer,
            MiscellaneousConfiguration miscellaneousConfiguration
            )
        {
            _jsonSerializer = jsonSerializer;
            _profileConfigurationFilePath = miscellaneousConfiguration.ProfileConfigurationFilePath;

            _profiles = LoadProfiles();
        }



        public IEnumerable<string> FilterComponents(IEnumerable<string> source)
        {
            if (!_currentProfile.ComponentNames.Any())
            {
                return source;
            }

            var filteredComponents = source.Intersect(_currentProfile.ComponentNames);

            return filteredComponents;
        }



        public IEnumerable<Profile> Profiles =>
            _profiles.Select(x => x);



        public void AddProfile(string name, IEnumerable<string> componentNames)
        {
            if (componentNames.Any() && _profiles.Any(profile => profile.Name == name))
            {
                return;
            }


            var newProfile = new Profile(name, componentNames.ToArray());

            _profiles.Add(newProfile);


            SaveProfiles();
        }



        public void UpdateProfile(string name, IEnumerable<string> componentNames)
        {
            var index = _profiles.FindIndex(profile => profile.Name == name);

            _profiles[index] = new Profile(name, componentNames.ToArray());


            SaveProfiles();
        }



        public void RemoveProfile(string name)
        {
            _profiles.RemoveAll(profile => profile.Name == name);


            SaveProfiles();
        }



        public void SetCurrentProfile(string name)
        {
            var newCurrentProfile = _profiles.FirstOrDefault(profile => profile.Name == name);

            if (newCurrentProfile != null)
            {
                _currentProfile = newCurrentProfile;
            }
        }



        private List<Profile> LoadProfiles()
        {
            var profiles = new[] {
                new Profile("Default", new string[]{})
                }
                .Concat
                    (
                    _jsonSerializer.DeserializeFromFile<IEnumerable<Profile>>
                        (
                        _profileConfigurationFilePath
                        )
                    )
                .ToList();

            return profiles;
        }



        private void SaveProfiles()
        {
            _jsonSerializer.SerializeToFile(_profileConfigurationFilePath, _profiles.Skip(1));
        }
    }
}