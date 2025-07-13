using System.Collections.Generic;
using Base;
using Base.Items;
using UnityEngine;

namespace Interfaces
{
    public interface IRoadBuilder
    {
        void CreateRoadSpline(List<Node> nodes, Transform parent, Material material, Road road);
    }
}