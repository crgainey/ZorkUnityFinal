using System.Collections.Generic;

namespace Zork
{
    public static class DictonaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictonary, TKey key, TValue defaultValue = default(TValue))
        {
            return (dictonary != null && key != null && dictonary.TryGetValue(key, out TValue value)) ? value : defaultValue;
        }

    }
}
