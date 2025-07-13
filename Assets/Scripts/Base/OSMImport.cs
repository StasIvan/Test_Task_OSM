using System.Collections.Generic;
using System.Xml.Linq;
using Base.Items;
using Base.Parsers;
using Base.Utilities;
using Interfaces;

namespace Base
{
    public class OSMImport : IOSMImportable
    {
        private const string OsmFilePath = "Assets/Resources/";
        private const string ExtensionFile = ".osm";
        private readonly GeoConverter _geoConverter = new(47.3769, 8.5417);
        private readonly string _osmFileName;

        public OSMImport(string osmFileName)
        {
            _osmFileName = osmFileName;
        }
        
        public (Dictionary<long, Node>, List<Road>) Import()
        {
            var roads = new List<Road>();

            var doc = XDocument.Load(OsmFilePath + _osmFileName + ExtensionFile);

            var nodeParser = new OSMNodeParser(_geoConverter);
            nodeParser.ParseNodes(doc, out var nodes);

            var wayParser = new OSMWayParser();
            wayParser.ParseWays(doc, nodes, roads);

            return (nodes, roads);
        }
    }
}