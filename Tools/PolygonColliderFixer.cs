using System.Linq;
using UnityEngine;

namespace Status92.Tools
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequiredByComponent(typeof(PolygonCollider2D))]
    public class PolygonColliderFixer : MonoBehaviour
    {
        public void FixPolygonCollider()
        {
            var pc2d = GetComponent<PolygonCollider2D>();
            pc2d.SetPath(
                0,
                pc2d.points.Select(p => new Vector2(Mathf.Round(p.x), Mathf.Round(p.y))).ToList()
            );
        }
    }
}