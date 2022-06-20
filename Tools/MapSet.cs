using System.Collections.Generic;

namespace Status92.Tools
{
    public class MapSet<TKey, TVal> : Dictionary<TKey, HashSet<TVal>>
    {

        public HashSet<TVal> GetHashSet(TKey key)
        {
            HashSet<TVal> set = null;
            if (ContainsKey(key))
            {
                set = base[key];
            }
            else
            {
                set = new HashSet<TVal>();
                base[key] = set;
            }

            return set;
        }

        public new HashSet<TVal> this[TKey key] => GetHashSet(key);

        public void AddSet(TKey key, TVal value)
        {
            var set = GetHashSet(key);
            set.Add(value);
        }

        public void AddMany(TKey key, IEnumerable<TVal> values)
        {
            foreach (var val in values)
            {
                AddSet(key, val);
            }
        }

        public bool SetContains(TKey key, TVal value)
        {
            var set = GetHashSet(key);
            return set.Contains(value);
        }

    }
}