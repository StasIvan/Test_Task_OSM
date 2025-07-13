using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Base.Utilities;
using Interfaces;
using UnityEngine;

namespace Base.OSM
{
    public class OSMImport : IOSMImportable
    {
        private const string OsmFilePath = "Assets/Resources/";
        private readonly GeoConverter _geoConverter = new(47.3769, 8.5417);
        private readonly string _osmFileName;

        public OSMImport(string osmFileName)
        {
            _osmFileName = osmFileName;
        }
        
        public (Dictionary<long, Node>, List<Road>) Import()
        {
            var roads = new List<Road>();

            var doc = XDocument.Load(OsmFilePath + _osmFileName + ".osm");

            var nodeParser = new OSMNodeParser(_geoConverter);
            nodeParser.ParseNodes(doc, out var nodes);

            var wayParser = new OSMWayParser();
            wayParser.ParseWays(doc, nodes, roads);

            return (nodes, roads);
        }
    }
}