using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Base.Builders
{
    public class RoadGraphBuilder : IGraphBuilder
    {
        public HashSet<long> BuildGraph(Dictionary<long, Node> nodes)
        {
            return nodes.Where(kv =>
            {
                bool isIntersection = IsCorner(kv.Value, nodes);
                SetIsIntersection(kv.Value, isIntersection);
                return isIntersection;
            }).Select(kv => kv.Key).ToHashSet();
        }
        
        private bool IsCorner(Node node, Dictionary<long, Node> nodes, float angleThreshold = 10f)
        {
            if (node.ConnectionPoints.Count > 2) return true;
            if (node.ConnectionPoints.Count <= 1) return false;

            var current = node.Position;
            var prev = nodes[node.ConnectionPoints.FirstOrDefault()].Position;
            var next = nodes[node.ConnectionPoints.LastOrDefault()].Position;
        
            Vector3 dir1 = (current - prev).normalized;
            Vector3 dir2 = (next - current).normalized;
            float angle = Vector3.Angle(dir1, dir2);
            
            return angle > angleThreshold;
        }

        private void SetIsIntersection(Node node, bool isIntersection)
        {
            node.IsIntersection = isIntersection;
        }
    }
}