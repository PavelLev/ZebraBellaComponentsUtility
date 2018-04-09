using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ZebraBellaComponentsUtility.Utility.Extensions
{
    public static class WpfEnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }
    }
}

