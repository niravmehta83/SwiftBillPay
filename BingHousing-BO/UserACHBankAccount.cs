using System;
namespace BingHousing_BO
{
    public partial class UserACHBankAccount
    {
        public Nullable<int> UserId { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public string BankToken { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CustomerDefaultSourceId { get; set; }
        public string CustomerId { get; set; }
    }

}
