using System.Collections.Generic;
using Base;
using Base.OSM;

namespace Interfaces
{
    public interface IOSMImportable
    {
        (Dictionary<long, Node>, List<Road>) Import();
    }
}