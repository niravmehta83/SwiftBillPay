using System.ComponentModel.DataAnnotations;
namespace BingHousingMVC.Models
{
    public class UserPaymentRegistrationModel
    {
        private string _accountholdername;
        private string _accounttype;
        private string _routingNumber;
        private string _accountNumber;
        private int _customerid;

        public int CustomerId
        {
            get { return _customerid; }
            set { _customerid = value; }
        }

        [Required]
        [Display(Name = "Account Holder Name")]
        public string AccountHolderName { get => _accountholdername; set => _accountholdername = value; }

        [Required]
        [Display(Name = "Accout Type")]
        public string Accounttype { get => _accounttype; set => _accounttype = value; }


        [Required]
        [Display(Name = "Routing Number")]
        public string RoutingNumber { get => _routingNumber; set => _routingNumber = value; }

        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get => _accountNumber; set => _accountNumber = value; }
    }

}