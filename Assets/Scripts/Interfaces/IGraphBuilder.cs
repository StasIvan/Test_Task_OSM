using System.Collections.Generic;
using Base;
using Base.Items;

namespace Interfaces
{
    public interface IGraphBuilder
    {
        HashSet<long> BuildGraph(Dictionary<long, Node> nodes);
    }
}