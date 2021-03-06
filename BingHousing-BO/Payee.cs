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
    
    public partial class Payee
    {
        public Payee()
        {
            this.Customers = new HashSet<Customer>();
            this.PayPalAccountDetails = new HashSet<PayPalAccountDetail>();
        }
    
        public int PayeeId { get; set; }
        public int UserId { get; set; }
        public string Payee1 { get; set; }
        public string PayeeAddress { get; set; }
        public string PayeeAddress2 { get; set; }
        public string PayeeCity { get; set; }
        public string PayeeState { get; set; }
        public string PayeeZipCode { get; set; }
        public string PayeeCountry { get; set; }
        public string PayeeEmail { get; set; }
        public string PayeeComments { get; set; }
        public string PayeeContactPerson { get; set; }
        public string PayeeWebsite { get; set; }
    
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<PayPalAccountDetail> PayPalAccountDetails { get; set; }
    }
}
