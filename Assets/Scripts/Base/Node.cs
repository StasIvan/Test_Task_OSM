using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base
{
    public class Node
    {
        public long Id;
        public Vector3 Position;
        public readonly HashSet<long> ConnectionPoints = new();
        public List<LineNode> Nodes = new();
        public bool IsIntersection;
    }
}