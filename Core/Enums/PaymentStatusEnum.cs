using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum PaymentStatus
    {

        Successs = 1,
        Pending = 2,
        Failed = 3,
        Refunded = 4,
    }
    public enum BookingStatus
    {
        Requested = 1,
        Confirm = 2,
        Cancelled = 3,
        Failed = 4,
        Invoiced = 5
    }
}
