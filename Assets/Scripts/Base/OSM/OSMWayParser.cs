using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

namespace Base.OSM
{
    public class OSMWayParser
    {
        private readonly HashSet<string> IgnoredHighways = new()
        {
            "footway", "pedestrian", "cycleway", "path", "steps", "track"
        };
        
        public void ParseWays(XDocument doc, Dictionary<long, Node> nodes, List<Road> roads)
        {
            foreach (var way in doc.Descendants("way"))
            {
                var highway = way.Elements("tag").FirstOrDefault(t => (string)t.Attribute("k") == "highway")?.Attribute("v")?.Value;
                if (highway == null || IgnoredHighways.Contains(highway))
                    continue;

                var path = way.Elements("nd")
                    .Select(nd => (long)nd.Attribute("ref"))
                    .Where(nodes.ContainsKey)
                    .ToList();

                if (path.Count < 2)
                    continue;

                // Read lane and oneway tags
                int? lanes = ParseIntTag(way, "lanes");
                int? lanesF = ParseIntTag(way, "lanes:forward");
                int? lanesB = ParseIntTag(way, "lanes:backward");
                bool isOneway = way.Elements("tag").
                    Any(t => (string)t.Attribute("k") == "oneway" && new[] {"yes", "-1"}.Contains((string)t.Attribute("v")));

                // Normalize direction: from south→north or west→east
                var start = nodes[path.First()].Position;
                var end = nodes[path.Last()].Position;
                var delta = end - start;
                if ((Mathf.Abs(delta.z) >= Mathf.Abs(delta.x) && delta.z < 0)
                    || (Mathf.Abs(delta.x) > Mathf.Abs(delta.z) && delta.x < 0))
                {
                    path.Reverse();
                }

                roads.Add(new Road(path, lanes, lanesF, lanesB, isOneway));
                UpdateConnections(path, nodes);
            }
        }

        private int? ParseIntTag(XElement way, string key)
        {
            return int.TryParse(
                way.Elements("tag").FirstOrDefault(t => (string)t.Attribute("k") == key)?
                    .Attribute("v")?.Value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var v)
                ? v
                : null;
        }

        private void UpdateConnections(List<long> path, Dictionary<long, Node> nodes)
        {
            for (int i = 0; i < path.Count; i++)
            {
                var id = path[i];
                var node = nodes[id];
                long prev = path[Mathf.Max(0, i - 1)];
                long next = path[Mathf.Min(path.Count - 1, i + 1)];
                if (prev != id) node.ConnectionPoints.Add(prev);
                if (next != id) node.ConnectionPoints.Add(next);
            }
        }
    }
}