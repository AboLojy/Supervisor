
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supervisor.Trackers
{
    public interface ITracker
    {
        int Count();
        bool Add(int orderId, object oreder, string info);
    }
}
