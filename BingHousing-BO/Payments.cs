//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BingHousing_BO
{
    using System;
    
    public partial class Payments
    {
        public int PaymentId { get; set; }
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PaymentMode { get; set; }
        public string Payee { get; set; }
        public int PayeeId { get; set; }
        public int InvoiceId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public string BillDescription { get; set; }
    }
}
