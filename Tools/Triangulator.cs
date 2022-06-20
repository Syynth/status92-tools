using System.Collections.Generic;
using UnityEngine;

namespace Status92.Tools
{
    ///Credit where credit is due
    ///https://wiki.unity3d.com/index.php?title=Triangulator 
    [ExecuteInEditMode]
    public class Triangulator
    {
        private List<Vector2> points;

        public Triangulator(IEnumerable<Vector2> points)
        {
            this.points = new List<Vector2>(points);
        }

        public int[] Triangulate()
        {
            var indices = new List<int>();

            var n = points.Count;
            if (n < 3)
                return indices.ToArray();

            var V = new int[n];
            if (Area() > 0)
            {
                for (var v = 0; v < n; v++)
                    V[v] = v;
            }
            else
            {
                for (var v = 0; v < n; v++)
                    V[v] = (n - 1) - v;
            }

            var nv = n;
            var count = 2 * nv;
            for (var v = nv - 1; nv > 2;)
            {
                if (count-- <= 0)
                    return indices.ToArray();

                var u = v;
                if (nv <= u)
                    u = 0;
                v = u + 1;
                if (nv <= v)
                    v = 0;
                var w = v + 1;
                if (nv <= w)
                    w = 0;

                if (!Snip(u, v, w, nv, V)) continue;
                
                int s, t;
                var a = V[u];
                var b = V[v];
                var c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);

                for (s = v, t = v + 1; t < nv; s++, t++)
                {
                    V[s] = V[t];
                }
                nv--;
                count = 2 * nv;
            }

            indices.Reverse();
            return indices.ToArray();
        }

        private float Area()
        {
            int n = points.Count;
            float A = 0.0f;
            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Vector2 pval = points[p];
                Vector2 qval = points[q];
                A += pval.x * qval.y - qval.x * pval.y;
            }

            return (A * 0.5f);
        }

        private bool Snip(int u, int v, int w, int n, int[] V)
        {
            int p;
            Vector2 A = points[V[u]];
            Vector2 B = points[V[v]];
            Vector2 C = points[V[w]];
            if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
                return false;
            for (p = 0; p < n; p++)
            {
                if ((p == u) || (p == v) || (p == w))
                    continue;
                Vector2 P = points[V[p]];
                if (InsideTriangle(A, B, C, P))
                    return false;
            }

            return true;
        }

        private static bool InsideTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
        {
            var ax = c.x - b.x;
            var ay = c.y - b.y;
            var bx = a.x - c.x;
            var by = a.y - c.y;
            var cx = b.x - a.x;
            var cy = b.y - a.y;
            var apx = p.x - a.x;
            var apy = p.y - a.y;
            var bpx = p.x - b.x;
            var bpy = p.y - b.y;
            var cpx = p.x - c.x;
            var cpy = p.y - c.y;

            var aCROSSbp = ax * bpy - ay * bpx;
            var cCROSSap = cx * apy - cy * apx;
            var bCROSScp = bx * cpy - by * cpx;

            return aCROSSbp >= 0.0f && bCROSScp >= 0.0f && cCROSSap >= 0.0f;
        }
    }
}