using System;
using System.Collections.Generic;

namespace Base
{
    public class Road
    {
        public Road(List<long> nodeIds,
            int? lanes,
            int? lanesFwd,
            int? lanesBwd,
            bool isOneway)
        {
            NodeIds = nodeIds;
            Lanes = lanes;
            LanesForward = lanesFwd;
            LanesBackward = lanesBwd;
            IsOneway = isOneway;
        }
        
        public List<long> NodeIds { get; }
        public int? Lanes { get; }                // тег lanes
        public int? LanesForward { get; }         // lanes:forward
        public int? LanesBackward { get; }        // lanes:backward
        public bool IsOneway { get; }             // oneway=yes/no/-1
    }
}