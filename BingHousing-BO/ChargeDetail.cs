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
    using System.Collections.Generic;
    
    public partial class ChargeDetail
    {
        public int ChargeId { get; set; }
        public string StripeChargeId { get; set; }
        public string InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> InsertedOn { get; set; }
        public string ChargeStatus { get; set; }
        public string ChargeFailureMessage { get; set; }
    }
}
