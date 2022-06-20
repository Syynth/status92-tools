using System.Linq;
using UnityEngine;

namespace Status92.Tools
{
    public static class MeshExtensions
    {
        public static void OffsetVertices(this Mesh mesh, Vector3 offset)
        {
            mesh.vertices = mesh.vertices
                .Select(v => new Vector3(
                    v.x + offset.x,
                    v.y + offset.y,
                    v.z + offset.z))
                .ToArray();
        }
    }
    
    public static class MeshTools
    {
        private static bool IsCCW(Vector2[] shape)
        {
            var sum = 0f;
            for (var i = 0; i < shape.Length; i++)
            {
                var v1 = shape[i];
                var v2 = shape[(i + 1) % shape.Length];
                sum += (v2.x - v1.x) * (v2.y + v1.y);
            }

            return sum <= 0f;
        }

        public static Mesh MeshFromLine(Vector2[] initial, float zFront, float zBack)
        {
            var pCount = initial.Length;
            var vCount = pCount * 2;
            var mesh = new Mesh();
            var vertices = new Vector3[vCount];
            var tris = new int[vCount * 3];

            for (var i = 0; i < pCount; ++i)
            {
                vertices[i].x = initial[i].x;
                vertices[i].y = initial[i].y;
                vertices[i].z = zFront;
                vertices[i + pCount].x = initial[i].x;
                vertices[i + pCount].y = initial[i].y;
                vertices[i + pCount].z = zBack;
            }

            for (int i = 0, t = 0; i < pCount - 1; ++i, t += 6)
            {
                tris[t + 0] = i + pCount;
                tris[t + 1] = i + 1;
                tris[t + 2] = i;

                tris[t + 3] = i + 1 + pCount;
                tris[t + 4] = i + 1;
                tris[t + 5] = i + pCount;
            }

            mesh.vertices = vertices;
            mesh.triangles = tris;
            
            return mesh;
        }

        public static Mesh MeshFromPolygon(Vector2[] initial, float zFront, float zBack)
        {
            var shape = initial;
            var ccw = IsCCW(shape);
            if (!ccw)
            {
                var list = shape.ToList();
                list.Reverse();
                shape = list.ToArray();
            }

            // convert polygon to triangles
            var triangulator = new Triangulator(shape);
            var tris = triangulator.Triangulate();
            var mesh = new Mesh();
            var vertices = new Vector3[shape.Length * 2];

            for (var i = 0; i < shape.Length; i++)
            {
                vertices[i].x = shape[i].x;
                vertices[i].y = shape[i].y;
                vertices[i].z = zFront; // front vertex
                vertices[i + shape.Length].x = shape[i].x;
                vertices[i + shape.Length].y = shape[i].y;
                vertices[i + shape.Length].z = zBack; // back vertex    
            }

            var triangles = new int[tris.Length * 2 + shape.Length * 6];
            var countTris = 0;
            for (var i = 0; i < tris.Length; i += 3)
            {
                triangles[i] = tris[i];
                triangles[i + 1] = tris[i + 1];
                triangles[i + 2] = tris[i + 2];
            } // front vertices

            countTris += tris.Length;
            for (var i = 0; i < tris.Length; i += 3)
            {
                triangles[countTris + i] = tris[i + 2] + shape.Length;
                triangles[countTris + i + 1] = tris[i + 1] + shape.Length;
                triangles[countTris + i + 2] = tris[i] + shape.Length;
            } // back vertices

            countTris += tris.Length;
            var L = shape.Length;
            for (var i = 0; i < shape.Length; i++)
            {
                var n = (i + 1) % L;

                triangles[countTris + 0] = i;
                triangles[countTris + 1] = n;
                triangles[countTris + 2] = n + L;
                triangles[countTris + 3] = i;
                triangles[countTris + 4] = n + L;
                triangles[countTris + 5] = i + L;

                countTris += 6;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            return mesh;
        }
    }

    public static class MeshFilterExtensions
    {
        public static void WriteMeshes(this MeshFilter filter, params Mesh[] meshes)
        {
            var t = filter.transform;
            var mesh = new Mesh();
            var combine = new CombineInstance[meshes.Length];
            for (var i = 0; i < meshes.Length; ++i)
            {
                combine[i].mesh = meshes[i];
                combine[i].transform = t.worldToLocalMatrix *
                                       Matrix4x4.Translate(new Vector3(0, 0, t.position.z));
            }

            mesh.CombineMeshes(combine, true);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            filter.sharedMesh = mesh;
        }
    }

    public static class MeshColliderExtensions
    {
        public static void WriteMeshes(this MeshCollider filter, params Mesh[] meshes)
        {
            var t = filter.transform;
            var mesh = new Mesh();
            var combine = new CombineInstance[meshes.Length];
            for (var i = 0; i < meshes.Length; ++i)
            {
                combine[i].mesh = meshes[i];
                combine[i].transform = t.worldToLocalMatrix *
                                       Matrix4x4.Translate(new Vector3(0, 0, t.position.z));
            }

            mesh.CombineMeshes(combine, true);
            filter.sharedMesh = mesh;
        }
    }
}