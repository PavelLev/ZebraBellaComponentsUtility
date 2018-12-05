using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using ZebraBellaComponentsUtility.Utility;
using ZebraBellaComponentsUtility.Utility.Extensions;

namespace ZebraBellaComponentsUtility.Components.Profiles
{
    public class ProfileService : IProfileService
    {
        private readonly JsonSerializer _jsonSerializer;
        private readonly IFileService _fileService;
        private readonly string _profileConfigurationFilePath;
        private readonly List<Profile> _profiles;
        private Profile _currentProfile;
        private readonly string _defaultProfileName = "Default";



        public ProfileService
            (
            JsonSerializer jsonSerializer,
            MiscellaneousConfiguration miscellaneousConfiguration,
            IFileService fileService
            )
        {
            _jsonSerializer = jsonSerializer;
            _fileService = fileService;
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

            var updatedProfile = new Profile(name, componentNames.ToArray());

            if (_profiles[index] == CurrentProfile)
            {
                _currentProfile = updatedProfile;
            }

            _profiles[index] = updatedProfile;


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


            SaveProfiles();
        }



        private List<Profile> LoadProfiles()
        {
            if (!_fileService.Exists(_profileConfigurationFilePath))
            {
                _fileService.WriteAllText(_profileConfigurationFilePath, $"{{\"Item1\":[],\"Item2\":\"{_defaultProfileName}\"}}");
            }

            var (deserializedProfiles, currentProfileName) = _jsonSerializer.DeserializeFromFile<(IEnumerable<Profile>, string)>
                (
                _profileConfigurationFilePath
                );
            

            var profiles = new[] {
                new Profile(_defaultProfileName, new string[]{})
                }
                .Concat
                    (
                    deserializedProfiles
                    )
                .ToList();


            _currentProfile = profiles.FirstOrDefault(profile => profile.Name == currentProfileName) ?? profiles.First();


            return profiles;
        }



        private void SaveProfiles()
        {
            _jsonSerializer.SerializeToFile(_profileConfigurationFilePath, (_profiles.Skip(1), _currentProfile.Name));
        }



        public Profile CurrentProfile =>
            _currentProfile;
    }
}