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
    
    public partial class GroupMember
    {
        public int GroupMemberId { get; set; }
        public int GroupId { get; set; }
        public int CustomerId { get; set; }
        public System.DateTime AddedOn { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime LastModified { get; set; }
        public string Billdescription { get; set; }
        public Nullable<System.DateTime> Nextbilldate { get; set; }
        public Nullable<decimal> BillAmount { get; set; }
    
        public virtual Group Group { get; set; }
    }
}