using Prism.Mvvm;

namespace ZebraBellaComponentsUtility.Components.Profiles
{
    public class ComponentNameViewModel : BindableBase
    {
        private bool _isSelected;



        public ComponentNameViewModel
            (
            string value,
            bool isSelected
            )
        {
            _isSelected = isSelected;
            Value = value;
        }



        public string Value
        {
            get;
        }

        public bool IsSelected
        {
            get =>
                _isSelected;
            set =>
                SetProperty(ref _isSelected, value);
        }
    }
}
