using UnityEngine;

namespace Status92.Tools
{
    public static class TransformExtensions
    {

        public static void FlipLocalScaleX(this Transform t, bool flipped)
        {
            t.SetLocalScale(t.localScale.WithX(Mathf.Abs(t.localScale.x) * (flipped ? -1 : 1)));
        }

        public static void SetLocalScale(this Transform t, Vector3 scale)
        {
            t.localScale = scale;
        }

        public static Transform FindChildByName(this Transform t, string name)
        {
            for (var i = 0; i < t.childCount; ++i)
            {
                var child = t.GetChild(i);
                if (child.name == name)
                {
                    return child;
                }
            }
            return null;
        }
        
        public static void DestroyAllChildren(this Transform transform)
        {
            for (var t = transform.childCount - 1; t >= 0; t--)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(transform.GetChild(t).gameObject);
                }
                else
                {
                    Object.DestroyImmediate(transform.GetChild(t).gameObject);
                }
            }
        }
    }
}