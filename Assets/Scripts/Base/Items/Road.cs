using System.Collections.Generic;

namespace Base.Items
{
    public class Road
    {
        public Road(List<long> nodeIds,
            int? lanes)
        {
            NodeIds = nodeIds;
            Lanes = lanes;
        }
        
        public List<long> NodeIds { get; }
        public int? Lanes { get; }                    
    }
}