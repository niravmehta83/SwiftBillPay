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
    
    public partial class PaymentMode
    {
        public PaymentMode()
        {
            this.PaymentDetails = new HashSet<PaymentDetail>();
        }
    
        public int PaymentModeId { get; set; }
        public string PaymentMode1 { get; set; }
    
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}