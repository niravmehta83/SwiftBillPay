using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{
    public enum IdType
    {
        UserId,PaymentId,InvoiceId,CustomerId,PayeeId
    }
    public enum InvoiceType
    {
        Notpaid=0,
        Paid=1,
        All=2
    }
}
