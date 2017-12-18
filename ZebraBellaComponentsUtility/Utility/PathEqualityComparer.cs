namespace ZebraBellaComponentsUtility.Utility
{
    public class PathEqualityComparer : IPathEqualityComparer
    {
        private readonly IPathService _pathService;

        public PathEqualityComparer(IPathService pathService)
        {
            _pathService = pathService;
        }

        public bool Equals(string x, string y)
        {
            return string.Equals(_pathService.Normalize(x), _pathService.Normalize(y));
        }

        public int GetHashCode(string obj)
        {
            return _pathService.Normalize(obj).GetHashCode();
        }
    }
}