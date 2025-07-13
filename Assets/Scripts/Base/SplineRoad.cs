using System.Collections.Generic;
using System.Linq;
using Base.Items;
using Interfaces;
using UnityEngine;

namespace Base
{
    public class SplineRoad
    {
        private readonly IOSMImportable _osmImporter;
        private readonly IGraphBuilder _graphBuilder;
        private readonly IRoadBuilder _roadBuilder;
        private readonly ITransitionBuilder _transitionBuilder;
        
        public SplineRoad(IOSMImportable osmImporter, IGraphBuilder graphBuilder, ITransitionBuilder transitionBuilder, IRoadBuilder roadBuilder)
        {
            _osmImporter = osmImporter;
            _graphBuilder = graphBuilder;
            _transitionBuilder = transitionBuilder;
            _roadBuilder = roadBuilder;
        }

        public void Build(Transform parent, Material material)
        {
            foreach (Transform child in parent)
            {
                Object.DestroyImmediate(child.gameObject);
            }
            
            var (nodes, roads) = _osmImporter.Import();
            var intersections = _graphBuilder.BuildGraph(nodes);

            foreach (var road in roads)
            {
                var pts = road.NodeIds.Select(id => nodes[id]).ToList();
                
                _roadBuilder.CreateRoadSpline(pts, parent, material, road);
                
                _transitionBuilder.CreateTransitionCurveWithSpline(road, nodes, parent);
            }

            BuildIntersectionTransitions(parent, intersections, nodes);
        }

        private void BuildIntersectionTransitions(Transform parent, HashSet<long> intersections, Dictionary<long, Node> nodes)
        {
            var lineNodes = new List<LineNode>();

            foreach (var centerId in intersections)
            {
                lineNodes.Clear();

                var centerNode = nodes[centerId];
                var dots = centerNode.ConnectionPoints.Select(id => nodes[id]);

                foreach (var dot in dots)
                    lineNodes.AddRange(dot.Nodes);

                SortLineNodesByAngle(lineNodes, centerNode.Position);

                foreach (var node in lineNodes)
                {
                    if (nodes[node.Id].Nodes.Count > 1)
                        continue;

                    foreach (var nextNode in lineNodes)
                    {
                        if (nextNode.Id == node.Id)
                            continue;

                        _transitionBuilder.CreateTransitionCurveWithSpline(centerNode, node, nextNode, parent);
                    }
                }
            }
        }

        private void SortLineNodesByAngle(List<LineNode> lineNodes, Vector3 center)
        {
            lineNodes.Sort((a, b) =>
            {
                var dirA = (Vector2)(a.Position - center);
                var dirB = (Vector2)(b.Position - center);

                float angleA = Mathf.Atan2(dirA.y, dirA.x);
                float angleB = Mathf.Atan2(dirB.y, dirB.x);

                return angleA.CompareTo(angleB);
            });
        }
    }
}
