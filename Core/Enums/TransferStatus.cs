using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{

    public enum TransferStatus
    {
        Nothing = 0,
        Requested = 1,
        Approved = 2,
        Reject = 3,
        Returned = 4,
    }
}
