using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BingHousing_BO;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace BingHousingMVC.Models
{

    public enum PaymentMode
    {
        PayPal,CheckOnline
    }
    public class CartLoginModel
    {
        private string _projectnumber;
        private string _cusemail;

        [Required]
        [Display (Name="Customer Id")]
        public string ProjectNumber
        {
            get { return _projectnumber; }
            set { _projectnumber = value; }
        }

        [Required]
        [Display(Name = "Customer Email")]
        public string CustomerEmail
        {
            get { return _cusemail; }
            set { _cusemail = value.Trim(); }
        }
    }

    public class CheckOutLoginModel
    {
        private string _password;
        private string _cusemail;

        [Required]
        [Display(Name = "Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [Required]
        [Display(Name = " Email")]
        public string Email
        {
            get { return _cusemail; }
            set { _cusemail = value.Trim(); }
        }
    }

    public class CheckOutModel : User
    {
        
        private string _firstname;
        private string _lastname;
        private string _address;
        private string _address2;
        private string _city;
        private string _state;
        private string _zipcode;
        private string _country;
        private string _phonenumber;        
        private string _company;
        private bool _joml;
        private string _paymentmode;
        private string _comments;
        private string _cusemail;
       
        [Required]
        [Display(Name = "First Name")]
        public string FirstName
        {
            get
            {
                return _firstname;
            }

            set
            {
                _firstname = value;
            }
        }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName
        {
            get
            {
                return _lastname;
            }

            set
            {
                _lastname = value;
            }
        }

        [Required]
        [Display(Name = "Address")]
        public string Address
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
            }
        }


        [Display(Name = "Address2")]
        public string Address2
        {
            get
            {
                return _address2;
            }

            set
            {
                _address2 = value;
            }
        }

        [Required]
        [Display(Name = "City")]
        public string City
        {
            get
            {
                return _city;
            }

            set
            {
                _city = value;
            }
        }

        [Required]
        [Display(Name = "State")]
        public string State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
            }
        }

        [Required]
        [Display(Name = "Post Code")]
        public string ZipCode
        {
            get
            {
                return _zipcode;
            }

            set
            {
                _zipcode = value;
            }
        }

        [Required]
        [Display(Name = "Country")]
        public string Country
        {
            get
            {
                return _country;
            }

            set
            {
                _country = value;
            }
        }

        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[0-9|-|+|(|)]{10,15}$", ErrorMessage = "Enter only numbers and min length must be 10 max length must be 15")]
        public string PhoneNumber
        {
            get
            {
                return _phonenumber;
            }

            set
            {
                _phonenumber = value;
            }
        }


     

        [Display(Name = "Company")]
        public string Company
        {
            get
            {
                return _company;
            }

            set
            {
                _company = value;
            }
        }

        [Display(Name = "Join Our Mailing List")]
        public bool JOML
        {
            get
            {
                return _joml;
            }

            set
            {
                _joml = value;
            }
        }
        [Required]
        [Display(Name = "How Will You Pay ?")]
        public string PaymentMode
        {
            get
            {
                return _paymentmode;
            }

            set
            {
                _paymentmode = value;
            }
        }

        
        [Display(Name = "Order Comments and Speacial Requests")]
        public string Comments
        {
            get
            {
                return _comments;
            }

            set
            {
                _comments = value;
            }
        }

        [Required]
        [Display(Name = " Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.+-]+\.(?:[a-zA-Z]{2}|COM|com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|edu|academy|biz|college|education|int)$", ErrorMessage = "Invalid Email Address")]
        public string Email
        {
            get { return _cusemail; }
            set { _cusemail = value.Trim(); }
        }


    }

    public class CheckOutRegisterModel : CheckOutModel
    {

        private string _password;
        private string _cpassword;
       

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword
        {
            get { return _cpassword; }
            set { _cpassword = value; }
        }

        
    }

    public class CheckModel
    {
        private int _checkid;
        private int _userid;
       
        private string _payee;
        private string _name;
        private string _add;
        private string _city;
        private string _state;
        private string _zip;
        private DateTime _date;
        private decimal _amount;
        private string _bName;
        private string _badd;
        private string _bcity;
        private string _bstate;
        private string _bzip;
        private string _checknumber;
        private string _rountingnumber;
        private string _accoutnumber;
        private string _checkmemo;
        private string _comments;
        private DateTime _idate;
        private string _invoicenumber;
        private int _invoiceId;

        public int CheckId
        {
            get { return _checkid; }
            set { _checkid = value; }
        }
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { _invoiceId = value; }
        }

        public int UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }

      
        [Required]
        public string Payee
        {
            get { return _payee; }
            set { _payee = value; }
        }

        [Required]
        public string NameOnCheck
        {
            get { return _name; }
            set { _name = value; }
        }

        [Required]
        public string AddressOnCheck
        {
            get { return _add; }

            set { _add = value; }
        }

        [Required]
        public string CityOnCheck
        {
            get { return _city; }
            set { _city = value; }
        }

        [Required]
        public string StateOnCheck
        {
            get { return _state; }
            set { _state = value; }
        }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter only numbers")]
        public string ZipOnCheck
        {
            get { return _zip; }
            set { _zip = value; }
        }

        [Required]
        [RegularExpression(@"^[0-9|.]*$", ErrorMessage = "Enter only numbers")]
        public decimal AmountOnCheck
        {
            get { return _amount; }
            set { _amount = value; }

        }

        [Required]
        
        public DateTime DateOnCheck
        {
            get { return _date; }
            set { _date = value; }
        }

        [Required]
        public string BankName
        {
            get { return _bName; }
            set { _bName = value; }

        }
        [Required]
        public string BankAddress
        {
            get { return _badd; }
            set { _badd = value; }
        }
        [Required]
        public string BankCity
        {
            get { return _bcity; }
            set { _bcity = value; }
        }
        [Required]
        public string BankState
        {
            get { return _bstate; }
            set { _bstate = value; }
        }
        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter only numbers")]
        public string BankZip
        {
            get { return _bzip; }
            set { _bzip = value; }
        }

        [Required]
        [MinLength(3,ErrorMessage = "the check number should be a minimum of 3 digits")]
        [RegularExpression(@"^[0-9]*$",ErrorMessage="Enter only numbers")]
        public string CheckNumber
        {
            get { return _checknumber; }
            set { _checknumber = value; }
        }

        [Required]
        //[MinLength(7, ErrorMessage = "the check Routing should be 9 digits")]
        [MaxLength(9, ErrorMessage = "the check Routing should be 9 digits")]
        //[RegularExpression(@"^[0-9]{9,9}$", ErrorMessage = "Enter only numbers the check Routing Number should be 9 digits")]
        public string RoutingNumber
        {
            get { return _rountingnumber ; }
            set { _rountingnumber = value; }
        }

        [Required]
        [MinLength(5, ErrorMessage = " min length must be 5 digits")]
        [MaxLength(18, ErrorMessage = " max length must be 18 digits")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter only numbers")]
        public string AccountNumber
        {
            get { return _accoutnumber; }
            set { _accoutnumber = value; }
        }

        public string CheckMemo
        {
            get { return _checkmemo; }
            set { _checkmemo = value; }
        }

        public string Comment
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public DateTime InsertedOn
        {
            get { return _idate; }
            set { _idate = value; }
        }

        [Required]
        public string InvoiceNumber
        {
            get { return _invoicenumber; }
            set { _invoicenumber = value; }
        }

    }

    public class ACHModel
    {
        private int _checkid;
        private int _userid;

        private string _payee;
        private string _name;
        private string _add;
        private string _city;
        private string _state;
        private string _zip;
        private DateTime _date;
        private decimal _amount;
        private string _bName;
        private string _badd;
        private string _bcity;
        private string _bstate;
        private string _bzip;
        private string _checknumber;
        private string _rountingnumber;
        private string _accoutnumber;
        private string _checkmemo;
        private string _comments;
        private DateTime _idate;
        private string _invoicenumber;
        private int _invoiceId;

        public int CheckId
        {
            get { return _checkid; }
            set { _checkid = value; }
        }
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { _invoiceId = value; }
        }

        public int UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }


        [Required]
        public string Payee
        {
            get { return _payee; }
            set { _payee = value; }
        }

        [Required]
        public string NameOnCheck
        {
            get { return _name; }
            set { _name = value; }
        }

        [Required]
        public string AddressOnCheck
        {
            get { return _add; }

            set { _add = value; }
        }

        [Required]
        public string CityOnCheck
        {
            get { return _city; }
            set { _city = value; }
        }

        [Required]
        public string StateOnCheck
        {
            get { return _state; }
            set { _state = value; }
        }

        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter only numbers")]
        public string ZipOnCheck
        {
            get { return _zip; }
            set { _zip = value; }
        }

        [Required]
        [RegularExpression(@"^[0-9|.]*$", ErrorMessage = "Enter only numbers")]
        public decimal AmountOnCheck
        {
            get { return _amount; }
            set { _amount = value; }

        }

        [Required]

        public DateTime DateOnCheck
        {
            get { return _date; }
            set { _date = value; }
        }

        [Required]
        public string BankName
        {
            get { return _bName; }
            set { _bName = value; }

        }
        [Required]
        public string BankAddress
        {
            get { return _badd; }
            set { _badd = value; }
        }
        [Required]
        public string BankCity
        {
            get { return _bcity; }
            set { _bcity = value; }
        }
        [Required]
        public string BankState
        {
            get { return _bstate; }
            set { _bstate = value; }
        }
        [Required]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter only numbers")]
        public string BankZip
        {
            get { return _bzip; }
            set { _bzip = value; }
        }

        [Required]
        [MinLength(3, ErrorMessage = "the check number should be a minimum of 3 digits")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter only numbers")]
        public string CheckNumber
        {
            get { return _checknumber; }
            set { _checknumber = value; }
        }

        [Required]
        //[MinLength(7, ErrorMessage = "the check Routing should be 9 digits")]
        [MaxLength(9, ErrorMessage = "the check Routing should be 9 digits")]
        //[RegularExpression(@"^[0-9]{9,9}$", ErrorMessage = "Enter only numbers the check Routing Number should be 9 digits")]
        public string RoutingNumber
        {
            get { return _rountingnumber; }
            set { _rountingnumber = value; }
        }

        [Required]
        [MinLength(5, ErrorMessage = " min length must be 5 digits")]
        [MaxLength(18, ErrorMessage = " max length must be 18 digits")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter only numbers")]
        public string AccountNumber
        {
            get { return _accoutnumber; }
            set { _accoutnumber = value; }
        }

        public string CheckMemo
        {
            get { return _checkmemo; }
            set { _checkmemo = value; }
        }

        public string Comment
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public DateTime InsertedOn
        {
            get { return _idate; }
            set { _idate = value; }
        }

        [Required]
        public string InvoiceNumber
        {
            get { return _invoicenumber; }
            set { _invoicenumber = value; }
        }

    }

    public class CartResetPasswordModel
    {
        [Required]
        public string ResetPasswordToken { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password1 and confirmation password1 do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class StripeChargeModel
    {
        private int _invoiceId;
        private int _userid;
        private int _payeeid;

        private string _payee;
        private long _amount;
        private string _comments;
        private DateTime _idate;
        private string _invoicenumber;
        private string _stripecustomerid;
        private string _stripecustomerDefaultsourceid;
        public string StripeCustomerDefaultSourceId
        {
            get { return _stripecustomerDefaultsourceid; }
            set { _stripecustomerDefaultsourceid = value; }
        }
        public int InvoiceId
        {
            get { return _invoiceId; }
            set { _invoiceId = value; }
        }
        public int UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }


        [Required]
        public int PayeeId
        {
            get { return _payeeid; }
            set { _payeeid = value; }
        }

        [Required]
        public string Payee
        {
            get { return _payee; }
            set { _payee = value; }
        }

        public string StripeCustomerId
        {
            get { return _stripecustomerid; }
            set { _stripecustomerid = value; }
        }

        [Required]
        [RegularExpression(@"^[0-9|.]*$", ErrorMessage = "Enter only numbers")]
        public long Amount
        {
            get { return _amount; }
            set { _amount = value; }

        }
        public string Comment
        {
            get { return _comments; }
            set { _comments = value; }
        }

        public DateTime InsertedOn
        {
            get { return _idate; }
            set { _idate = value; }
        }
        public string InvoiceNumber
        {
            get { return _invoicenumber; }
            set { _invoicenumber = value; }
        }

    }


}