using UnityEngine;

namespace Status92.Tools
{
    public static class GameObjectExtensions
    {

        public static T GetComponentInParentExclusive<T, S>(this S obj, bool includeInactive = false)
            where T : Component
            where S : Component
        {
            var selfComp = obj.GetComponent<T>();
            if (!selfComp)
            {
                return obj.GetComponentInParent<T>();
            }
            return obj.transform.parent == null
                ? null
                : selfComp.transform.parent.GetComponentInParent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            return self.HasComponent<T>(out var component)
                ? component
                : self.AddComponent<T>();
        }
        
        public static bool HasComponent<T>(this GameObject self)
        {
            return self.GetComponent<T>() != null;
        }

        public static bool HasComponent<T>(this GameObject self, out T component)
        {
            component = self.GetComponent<T>();
            return component != null;
        }

        public static bool HasComponent<T>(this Component self)
        {
            var component = self.GetComponent<T>();
            return component != null;
        }
        
        public static bool HasComponent<T>(this Component self, out T component)
        {
            component = self.GetComponent<T>();
            return component != null;
        }
        
    }
}