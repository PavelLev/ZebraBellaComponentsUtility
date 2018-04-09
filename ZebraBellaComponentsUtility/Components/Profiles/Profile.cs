namespace ZebraBellaComponentsUtility.Components.Profiles
{
    public class Profile
    {
        public string Name
        {
            get;
        }

        public string[] ComponentNames
        {
            get;
        }



        public Profile(string name, string[] componentNames)
        {
            Name = name;
            ComponentNames = componentNames;
        }
    }
}
