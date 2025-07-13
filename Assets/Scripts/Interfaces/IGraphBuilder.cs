using System.Collections.Generic;
using Base;
using Base.OSM;

namespace Interfaces
{
    public interface IGraphBuilder
    {
        HashSet<long> BuildGraph(Dictionary<long, Node> nodes);
    }
}