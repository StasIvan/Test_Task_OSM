using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Base.Items;
using Base.Utilities;

namespace Base.Parsers
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