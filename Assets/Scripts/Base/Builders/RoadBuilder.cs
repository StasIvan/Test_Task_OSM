using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Base.Builders
{
    public class RoadBuilder : IRoadBuilder
    {
        private readonly bool _isGenerateLineRenderers;
        private readonly float _laneWidth = 4f;

        public RoadBuilder(bool isGenerateLineRenderers)
        {
            _isGenerateLineRenderers = isGenerateLineRenderers;
        }

        public void CreateRoadSpline(List<Node> nodes, Transform parent, Material material, Road road)
        {
            if (nodes == null || nodes.Count < 2)
                return;

            int lanes = road.Lanes ?? 1;

            for (int i = 0; i < lanes; i++)
            {
                float offset = OffsetFromCenter(i, lanes);
                List<LineNode> polyline = OffsetPolyline(nodes, offset, lanes);
                
                if (i >= lanes / 2f) polyline.Reverse();
                
                if(_isGenerateLineRenderers)
                    CreateLineRenderer(parent, material, i, polyline);
            }

        }
        
        private void CreateLineRenderer(Transform parent, Material material, int index, List<LineNode> polyline)
        {
            var laneGO = new GameObject($"Lane_{index}_Road", typeof(LineRenderer));
            laneGO.transform.SetParent(parent, false);

            var lr = laneGO.GetComponent<LineRenderer>();
            lr.positionCount = polyline.Count;
            lr.SetPositions(polyline.Select(n => n.Position).ToArray());
            lr.widthMultiplier = _laneWidth;
            lr.material = new Material(material);
            lr.startColor = Color.yellow;
            lr.endColor = Color.yellow;
            lr.useWorldSpace = true;
        }

        private float OffsetFromCenter(int laneIndex, int totalLanes)
        {
            return (-((totalLanes - 1) / 2f) + laneIndex) * _laneWidth;
        }

        private List<LineNode> OffsetPolyline(List<Node> nodes, float offset, int lineNumber)
        {
            var result = new List<LineNode>(nodes.Count);

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                
                Vector3 a = node.Position;
                Vector3 b = nodes[Mathf.Min(i + 1, nodes.Count - 1)].Position;
                
                Vector3 dir = (b - a).normalized; 
                Vector3 normal = new Vector3(-dir.z, 0f, dir.x);
                Vector3 aOff = a + normal * offset;

                LineNode lineNode = new LineNode
                {
                    LineNumber = lineNumber,
                    Id = node.Id,
                    Position = aOff
                };

                if (node.Nodes.Count < lineNumber)
                    node.Nodes.Add(lineNode);
                
                result.Add(lineNode);
                
            }

            return result;
        }
    }

}