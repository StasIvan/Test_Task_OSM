using System.Collections.Generic;
using System.Linq;
using Base.Items;
using Base.Utilities;
using Interfaces;
using UnityEngine;
using UnityEngine.Splines;

namespace Base.Builders
{
    public class TransitionBuilder : ITransitionBuilder
    {
        private const int TransitionSegments = 20;
        private const string LaneName = "TransitionSpline_";
        
        public void CreateTransitionCurveWithSpline(Road road, Dictionary<long, Node> nodes, Transform parent)
        {
            var roadNodes = road.NodeIds.Select(id => nodes[id]).ToList();
            int lanes = road.Lanes ?? 1;

            for (int laneIndex = 0; laneIndex < lanes; laneIndex++)
            {
                var splineContainer = CreateSplineContainer(LaneName + $"{laneIndex}", parent);
                var spline = splineContainer.Spline;
                spline.Clear();

                bool isIntersection = false;

                foreach (var nodeA in roadNodes)
                {
                    if (nodeA.IsIntersection)
                    {
                        if (nodeA.ConnectionPoints.Count < 3)
                            isIntersection = true;
                        continue;
                    }

                    if (isIntersection)
                    {
                        splineContainer = CreateSplineContainer(LaneName + $"{laneIndex}", parent);
                        spline = splineContainer.Spline;
                        spline.Clear();
                        isIntersection = false;
                    }

                    if (laneIndex < nodeA.Nodes.Count)
                    {
                        var position = nodeA.Nodes[laneIndex].Position;
                        spline.Add(new BezierKnot(position) { TangentIn = Vector3.zero, TangentOut = Vector3.zero });
                    }
                }
            }
        }

        public void CreateTransitionCurveWithSpline(Node center, LineNode from, LineNode to, Transform parent)
        {
            var start = from.Position;
            var end = to.Position;
            float centerBias = 1f;

            var c1 = Vector3.Lerp(start, center.Position, centerBias);
            var c2 = Vector3.Lerp(end, center.Position, centerBias);

            var splineContainer = CreateSplineContainer(LaneName + $"{from.Id}_{to.Id}", parent);
            var spline = splineContainer.Spline;
            spline.Clear();

            var positions = CalculateBezierPositions(start, c1, c2, end, TransitionSegments);

            foreach (var p in positions)
            {
                spline.Add(new BezierKnot(p)
                {
                    TangentIn = Vector3.zero,
                    TangentOut = Vector3.zero,
                    Rotation = Quaternion.identity
                });
            }
        }

        private Vector3[] CalculateBezierPositions(Vector3 start, Vector3 c1, Vector3 c2, Vector3 end, int segments)
        {
            var pts = new Vector3[segments + 1];
            float inv = 1f / segments;
            for (int i = 0; i <= segments; i++)
                pts[i] = BezierUtility.CubicBezier(start, c1, c2, end, i * inv);
            return pts;
        }

        private SplineContainer CreateSplineContainer(string name, Transform parent)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var splineContainer = go.AddComponent<SplineContainer>();
            return splineContainer;
        }
    }

}