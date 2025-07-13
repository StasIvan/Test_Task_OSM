using System.Collections.Generic;
using Base;
using Base.Items;

namespace Interfaces
{
    public interface IOSMImportable
    {
        (Dictionary<long, Node>, List<Road>) Import();
    }
}