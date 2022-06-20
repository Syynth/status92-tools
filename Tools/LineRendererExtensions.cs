using System.Collections.Generic;
using UnityEngine;

namespace Status92.Tools
{
    public static class LineRendererExtensions
    {

        public static void SetColor(this LineRenderer lr, Color c)
        {
            if (lr == null) return;
            lr.startColor = c;
            lr.endColor = c;
        }

        public static void WritePoints(this LineRenderer lr, List<Vector3> points)
        {
            lr.positionCount = points.Count;
            lr.SetPositions(points.ToArray());
        }
        
        public static void WritePoints(this LineRenderer lr, Vector3[] points)
        {
            lr.positionCount = points.Length;
            lr.SetPositions(points);
        }

        public static void TranslatePoints(this LineRenderer lr, Vector3 offset)
        {
            for (var i = 0; i < lr.positionCount; ++i)
            {
                var p = lr.GetPosition(i);
                p += offset;
                lr.SetPosition(i, p);
            }
        }
        
        public static void LerpColors(this LineRenderer lr, Color start, Color end, float t)
        {
            if (lr == null) return;
            lr.SetColor(Color.Lerp(start, end, t));
        }
        
    }
}