﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using ZebraBellaComponentsUtility.Utility.Extensions;

namespace ZebraBellaComponentsUtility.Components.Profiles
{
    public class ProfileViewModel : BindableBase
    {
        private string _name;
        private ObservableCollection<string> _componentNames;



        public ProfileViewModel
            (
            string name,
            IEnumerable<string> componentNames
            )
        {
            _name = name;
            _componentNames = componentNames.ToObservableCollection();
        }

        public string Name
        {
            get =>
                _name;
            set =>
               SetProperty(ref _name, value);
        }

        public ObservableCollection<string> ComponentNames  
        {
            get =>
                _componentNames;
            set =>
                SetProperty(ref _componentNames, value);
        }
    }
}
