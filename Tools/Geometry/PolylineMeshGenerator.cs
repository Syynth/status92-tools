using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Status92.Tools.Geometry
{
    
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Polyline2D))]
    public class PolylineMeshGenerator : MonoBehaviour, OnSceneSavedHandler
    {

        public bool RegenerateOnSave = true;
        
        public MeshCollider MeshCollider;
        public Polyline2D Polyline2D;
        
        private void AssignReferencesFromSelf()
        {
            MeshCollider ??= GetComponent<MeshCollider>();
            Polyline2D ??= GetComponent<Polyline2D>();
        }

        private void RegenerateMesh()
        {
            var points = Polyline2D.Nodes.ToList();
            if (Polyline2D.IsClosed)
            {
                points.Add(points.FirstOrDefault());
            }
            var mesh = MeshTools.MeshFromLine(points.ToArray(), -20, 20);
            mesh.OffsetVertices(transform.position);
            MeshCollider.WriteMeshes(mesh);
        }
        
        public void OnSceneSaved(SceneInfo info)
        {
            if (RegenerateOnSave)
            {
                RegenerateMesh();
            }
        }
    }
}