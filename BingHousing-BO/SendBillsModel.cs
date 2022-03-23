using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{
    public class SendBillsModel
    {
        public List<CustomerDetail> CustomerDetail { get; set; }
        public int PrevGroupId { get; set; }
        public Group Group { get; set; }
        public int NextGroupId { get; set; }

    }
}
