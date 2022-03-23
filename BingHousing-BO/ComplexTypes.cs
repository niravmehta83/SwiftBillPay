using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{
    public class EmailSummaryReport
    {
        public DateTime Date { get; set; }
        public List<EmailedReport> list { get; set; }
        public decimal TotalAmount { get; set; }

        public int Payee { get; set; }
    }

    public class PaymentSummaryReport
    {
        public DateTime Date { get; set; }
        public List<Payments> list { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
