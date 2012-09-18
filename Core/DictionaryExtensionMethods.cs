using System.Collections.Generic;

namespace Core
{
    public static class DictionaryExtensionMethods
    {
        public static Dictionary<TKey, TValue> AppendIf<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
                                                                      bool condition, TKey key, TValue value)
        {
            if (!condition)
                return dictionary;

            dictionary.Add(key, value);
            return dictionary;
        }
    }
}