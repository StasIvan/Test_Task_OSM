using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Base.Utilities;

namespace Base.OSM
{
    public class OSMNodeParser
    {
        private readonly GeoConverter _geoConverter;

        public OSMNodeParser(GeoConverter geoConverter)
        {
            _geoConverter = geoConverter;
        }

        public void ParseNodes(XDocument doc, out Dictionary<long, Node> nodes)
        {
            nodes = doc.Descendants("node")
                .ToDictionary(
                    x => (long)x.Attribute("id"),
                    x => new Node
                    {
                        Id = (long)x.Attribute("id"),
                        Position = _geoConverter.LatLonToXY(
                            (double)x.Attribute("lat"),
                            (double)x.Attribute("lon"))
                    });

        }
    }
}