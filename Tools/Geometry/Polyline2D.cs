using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using System;
using UnityEngine;

namespace Status92.Tools.Geometry
{
    public class Polyline2D : MonoBehaviour
    {
        public enum PathTypes
        {
            Open,
            Closed,
        }

        public enum PathDirection
        {
            Forward,
            Reverse,
        }

        public PathTypes PathType = PathTypes.Open;

        public bool IsClosed => PathType == PathTypes.Closed;
        public Vector3 StartPosition { get; private set; }

        public List<Vector2> Nodes = new(new Vector2[] {new(-3, 0), new(3, 0)});

        public IEnumerable<Vector2> WorldPoints => Nodes.Select(NodeToWorldPoint);

        private void Start()
        {
            StartPosition = transform.position;
        }

        public Vector2 GetClosestPoint(Vector2 point)
        {
            var closestPoint = Vector2.positiveInfinity;
            Vector2 next;
            var points = WorldPoints.ToArray();
            for (var i = 0; i < points.Length - 1; ++i)
            {
                next = GetClosestPointOnLineSegment(points[i], points[i + 1], point);
                if (point.DistanceTo(next) < point.DistanceTo(closestPoint))
                {
                    closestPoint = next;
                }
            }

            if (IsClosed)
            {
                next = GetClosestPointOnLineSegment(points[points.Length - 1], points[0], point);
                if (point.DistanceTo(next) < point.DistanceTo(closestPoint))
                {
                    closestPoint = next;
                }
            }

            return closestPoint;
        }

        public (int, PathDirection) GetNextTarget(int current, PathDirection direction)
        {
            if (direction.IsForward())
            {
                if (IsClosed)
                {
                    return ((current + 1) % Nodes.Count, PathDirection.Forward);
                }

                return current + 1 == Nodes.Count
                    ? (current - 1, PathDirection.Reverse)
                    : (current + 1, PathDirection.Forward);
            }

            if (current == 0)
            {
                return IsClosed
                    ? (Nodes.Count - 1, PathDirection.Reverse)
                    : (1, PathDirection.Forward);
            }

            return (current - 1, PathDirection.Reverse);
        }

        public Vector3 IndexToWorldPoint(int index) => NodeToWorldPoint(Nodes[index]);
        
        private static Vector2 GetClosestPointOnLineSegment(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            var ap = point - lineStart;
            var lineSegment = lineEnd - lineStart;

            /* Given a line segment AB and a point P
             * Vector AP and Vector AB. Use the dot product to project AP onto AB
             * As an example:
             * Case 1: If P is to the left of A, then DOT(AB, AP) < 0
             * Case 2: If P is to the right of B, DOT(AB, AP) > 0
             * Case 3: If P is between A and B, then 0 < DOT(AB, AP) < 1
             *      P
             *     /
             *    /
             *  A -------- B
             * If Case 3, then start at A and scale AB by DOT(AB, AP)
             *
             * I don't understand why I'm doing this .sqrMagnitude business,
             * but maybe it's because AP is not part of a segment?
             */

            var dotProduct = Vector2.Dot(ap, lineSegment);
            var distance = dotProduct / lineSegment.sqrMagnitude;

            return distance switch
            {
                //Check if P projection is over vectorAB     
                < 0 => lineStart,
                > 1 => lineEnd,
                _ => lineStart + lineSegment * distance
            };
        }

        private Vector2 NodeToWorldPoint(Vector2 node)
        {
            var t = transform;
            return t.TransformPoint(node);
        }

        public override string ToString()
        {
            return string.Join(", ", Nodes.Select(node => $"({node.x}, {node.y})")) +
                   (IsClosed ? " - Closed" : " - Open");
        }

#if UNITY_EDITOR

        #region Editor functions

        [Button, ButtonGroup("Cycle Path")]
        private void MoveStartingPointForward()
        {
            var end = Nodes[^1];
            for (var i = Nodes.Count - 2; i >= 0; --i)
            {
                Nodes[i + 1] = Nodes[i];
            }

            Nodes[0] = end;
            Undo.RecordObject(this, "Move Starting Point Forward");
            AlignObjectToStartingPoint();
        }

        [Button, ButtonGroup("Cycle Path")]
        private void MoveStartingPointBackward()
        {
            var end = Nodes[0];
            for (var i = 1; i < Nodes.Count; ++i)
            {
                Nodes[i - 1] = Nodes[i];
            }

            Nodes[^1] = end;
            Undo.RecordObject(this, "Move Starting Point Backward");
            AlignObjectToStartingPoint();
        }

        private void AlignStartingPointToObject()
        {
            if (Nodes[0].Approximately(Vector2.zero)) return;

            var delta = Nodes[0];
            for (var i = 0; i < Nodes.Count; ++i)
            {
                Nodes[i] -= delta;
            }

            Undo.RecordObject(this, "Align Polyline Starting Point");
        }

        [Button(ButtonSizes.Large)]
        private void AlignAll()
        {
            SnapPointsToWorldGrid();
            AlignObjectToStartingPoint();
        }

        [Button, ButtonGroup("Alignment")]
        private void AlignObjectToStartingPoint()
        {
            if (Nodes[0].Approximately(Vector2.zero)) return;
            var t = transform;
            var delta = t.TransformPoint(Nodes[0]) - t.position;
            t.Translate(delta);
            Undo.RecordObject(t, "Align Polyline Starting Point");
            AlignStartingPointToObject();
        }

        [Button, ButtonGroup("Alignment")]
        private void SnapPointsToWorldGrid()
        {
            var t = transform;
            for (var i = 0; i < Nodes.Count; ++i)
            {
                Nodes[i] = t.InverseTransformPoint(NodeToWorldPoint(Nodes[i]).Round());
                Undo.RecordObject(this, "Snap Polyline To Grid");
            }
        }

        [Button, ButtonGroup("Alignment")]
        private void RoundPointsToIntegers()
        {
            for (var i = 0; i < Nodes.Count; ++i)
            {
                Nodes[i] = Nodes[i].Round();
            }

            Undo.RecordObject(this, "Round Polyline To Integers");
        }

        #endregion

#endif

    }

    public static class Polyline2DHelpers
    {
        public static bool IsForward(this Polyline2D.PathDirection direction) =>
            direction == Polyline2D.PathDirection.Forward;

        public static bool IsReverse(this Polyline2D.PathDirection direction) =>
            direction == Polyline2D.PathDirection.Reverse;

        public static Polyline2D.PathDirection Flip(this Polyline2D.PathDirection direction) =>
            direction.IsForward()
                ? Polyline2D.PathDirection.Reverse
                : Polyline2D.PathDirection.Forward;
    }
}