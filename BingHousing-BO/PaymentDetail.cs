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
    
    public partial class PaymentDetail
    {
        public PaymentDetail()
        {
            this.Invoices = new HashSet<Invoice>();
            this.Subscriptions = new HashSet<Subscription>();
        }
    
        public int PaymentId { get; set; }
        public int PaymentModeId { get; set; }
        public Nullable<int> PayPalId { get; set; }
        public Nullable<int> CheckId { get; set; }
        public int Insertedby { get; set; }
        public System.DateTime InsertedOn { get; set; }
    
        public virtual CheckDetail CheckDetail { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual PaymentMode PaymentMode { get; set; }
        public virtual PayPalDetail PayPalDetail { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
