using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{
    public partial class ACHAccountDepositDetail
    {
        public int ACHDepositAccountId { get; set; }
        public int PayeeId { get; set; }
        public string StripeProductionKey { get; set; }
        public string StripeTestKey { get; set; }
        public System.DateTime InsertedOn { get; set; }
        public System.DateTime UpdatedOn { get; set; }
    }

}
