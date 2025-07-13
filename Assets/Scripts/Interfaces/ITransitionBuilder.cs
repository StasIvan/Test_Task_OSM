using System.Collections.Generic;
using Base;
using Base.Items;
using UnityEngine;

namespace Interfaces
{
    public interface ITransitionBuilder
    {
        void CreateTransitionCurveWithSpline(Road road, Dictionary<long, Node> nodes, Transform parent);
        void CreateTransitionCurveWithSpline(Node center, LineNode from, LineNode to, Transform parent);
    }
}