using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingHousing_BO
{
    public partial class ChargeDetail
    {
        public int ChargeId { get; set; }
        public string ChargeResourceId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> InsertedOn { get; set; }
    }

}
