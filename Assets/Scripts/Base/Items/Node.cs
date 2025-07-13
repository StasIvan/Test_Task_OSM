using System.Collections.Generic;
using UnityEngine;

namespace Base.Items
{
    public class Node
    {
        public long Id;
        public Vector3 Position;
        public readonly HashSet<long> ConnectionPoints = new();
        public readonly List<LineNode> Nodes = new();
        public bool IsIntersection;
    }
}