using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    public interface IPathNode<TSelf>
    {
        TSelf[] Neighbors { get; }

        float DistanceTo(TSelf node);

        bool IsPassable { get; }
    }
}
