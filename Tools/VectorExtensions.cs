using UnityEngine;

namespace Status92.Tools
{
    public static class Vector2Extensions
    {
        public static Vector2 AlignTo(this Vector2 self, Vector2 align)
        {
            return Vector2.Dot(self, align) > 0 ? self : -self;
        }

        public static Vector2 PerpendicularClockwise(this Vector2 vector2)
        {
            return new Vector2(vector2.y, -vector2.x);
        }

        public static Vector2 PerpendicularCounterClockwise(this Vector2 vector2)
        {
            return new Vector2(-vector2.y, vector2.x);
        }

        public static bool Approximately(this Vector2 v1, Vector2 v2)
        {
            return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y);
        }
        
        public static bool Approximately(this Vector2 v1, Vector3 v2)
        {
            return Mathf.Approximately(v1.x, v2.x) &&
                   Mathf.Approximately(v1.y, v2.y);
        }

        public static Vector2 Divide(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x / v2.x, v1.y / v2.y);
        }

        public static Vector2 WithX(this Vector2 v, float x)
        {
            var rv = v;
            rv.x = x;
            return rv;
        }

        public static Vector2 WithY(this Vector2 v, float y)
        {
            var rv = v;
            rv.y = y;
            return rv;
        }
        
        public static Vector3 XY0(this Vector2 v)
        {
            return v;
        }

        public static Vector2 Round(this Vector2 v)
        {
            return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
        }

        public static float DistanceTo(this Vector2 v, Vector2 other)
        {
            return Vector2.Distance(v, other);
        }
        
        public static float DistanceTo(this Vector2 v, Vector3 other)
        {
            return Vector2.Distance(v, other);
        }
    }

    public static class Vector3Extensions
    {
        public static Vector3 Divide(this Vector3 v1, Vector3 v2)
        {
            v2 = v2.ReplaceZero(1f);
            return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
        }


        public static Vector3 Multiply(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static bool Approximately(this Vector3 v1, Vector3 v2)
        {
            return Mathf.Approximately(v1.x, v2.x) &&
                   Mathf.Approximately(v1.y, v2.y) &&
                   Mathf.Approximately(v1.z, v2.z);
        }
        
        public static bool Approximately(this Vector3 v1, Vector2 v2)
        {
            return Mathf.Approximately(v1.x, v2.x) &&
                   Mathf.Approximately(v1.y, v2.y);
        }

        public static Vector3 ReplaceZero(this Vector3 v, float f = 1f)
        {
            return new Vector3(
                Mathf.Approximately(v.x, 0f) ? f : v.x,
                Mathf.Approximately(v.y, 0f) ? f : v.y,
                Mathf.Approximately(v.z, 0f) ? f : v.z
            );
        }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            var rv = v;
            rv.x = x;
            return rv;
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            var rv = v;
            rv.y = y;
            return rv;
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            var rv = v;
            rv.z = z;
            return rv;
        }

        public static Vector3Int ToInt(this Vector3 v)
        {
            var i = new Vector3Int();
            i.x = Mathf.RoundToInt(v.x);
            i.y = Mathf.RoundToInt(v.y);
            i.z = Mathf.RoundToInt(v.z);
            return i;
        }
        
        public static Vector2 XY(this Vector3 v)
        {
            return v;
        }
        
        public static Vector3 Round(this Vector3 v)
        {
            return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }
        
        public static float DistanceTo(this Vector3 v, Vector3 other)
        {
            return Vector3.Distance(v, other);
        }
        
        public static float DistanceTo(this Vector3 v, Vector2 other)
        {
            return Vector3.Distance(v, other);
        }
    }
}