using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BingHousing_BO;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BingHousingMVC.Models
{
    
    public class UserModel:User
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
        private string _mobile;

        [Required]
        [Display (Name="First Name")]
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
        [Display(Name = "Zip Code")]
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

        
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^[0-9|-|+|(|)]{10,15}$", ErrorMessage = "Enter only numbers and min length must be 10 max length must be 15")]
        public string Mobile
        {
            get
            {
                return _mobile;
            }

            set
            {
                _mobile = value;
            }
        }

    }

   

    public class PayeeModel : User
    {
        private int _payeeid;
        private string _payee;
        private string _paddress;
        private string _paddress2;
        private string _pcity;
        private string _pstate;
        private string _pzipcode;
        private string _pcountry;
        private string _pemail;
        private string _pcontactperson;
        private string _pcomments;
        private string _pwebsite;

        public int PayeeId
        {
            get
            {
                return _payeeid;
            }

            set
            {
                _payeeid = value;
            }
        }

        [Required]
        [Display(Name = "Payee")]
        public string Payee
        {
            get
            {
                return _payee;
            }

            set
            {
                _payee = value;
            }
        }

        [Required]
        [Display(Name = "Payee Address")]
        public string PayeeAddress
        {
            get
            {
                return _paddress;
            }

            set
            {
                _paddress = value;
            }
        }


        [Display(Name = "Payee Address2")]
        public string PayeeAddress2
        {
            get
            {
                return _paddress2;
            }

            set
            {
                _paddress2 = value;
            }
        }

        [Required]
        [Display(Name = "Payee City")]
        public string PayeeCity
        {
            get
            {
                return _pcity;
            }

            set
            {
                _pcity = value;
            }
        }

        [Required]
        [Display(Name = "Payee State")]
        public string PayeeState
        {
            get
            {
                return _pstate;
            }

            set
            {
                _pstate = value;
            }
        }

        [Required]
        [Display(Name = "Payee Zip Code")]
        public string PayeeZipCode
        {
            get
            {
                return _pzipcode;
            }

            set
            {
                _pzipcode = value;
            }
        }

        [Required]
        [Display(Name = "Payee Country")]
        public string PayeeCountry
        {
            get
            {
                return _pcountry;
            }

            set
            {
                _pcountry = value;
            }
        }

        [Required]
        [Display(Name = "Payee Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.+-]+\.(?:[a-zA-Z]{2}|COM|com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|edu|academy|biz|college|education|int)$", ErrorMessage = "Invalid Email Address")]
        public string PayeeEmail
        {
            get
            {
                return _pemail;
            }

            set
            {
                _pemail = value.Trim();
            }
        }


        [Display(Name = "Payee Contact Person")]
        public string PayeeContactPerson
        {
            get
            {
                return _pcontactperson;
            }

            set
            {
                _pcontactperson = value;
            }
        }


        [Display(Name = "Payee Comments")]
        public string PayeeComments
        {
            get
            {
                return _pcomments;
            }

            set
            {
                _pcomments = value;
            }
        }


        [Display(Name = "Payee Website")]
        public string PayeeWebsite
        {
            get
            {
                return _pwebsite;
            }

            set
            {
                _pwebsite = value;
            }
        }


    }

    public class PayPalAccountModel
    {
        private int accid;
        private string clientId;
        private string clientSecretId;
        private int _payeeid;
        private int _paypalSurcharge;

          [Required]
        [Display(Name="PayPal Surcharge (%)")]
        public int PayPalSurCharge
        {
            get { return _paypalSurcharge; }
            set { _paypalSurcharge = value; }
        }
        public int PayeeId
        {
            get
            {
                return _payeeid;
            }

            set
            {
                _payeeid = value;
            }
        }

        public int PayPalAccountId
        {
            get { return accid; }
            set { accid = value; }
        }
        [Required]
        [Display(Name="Client Id")]
        public string ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }
        [Required]
        [Display(Name = "Client Secret Id")]
        public string ClientSecretId
        {
            get { return clientSecretId; }
            set { clientSecretId = value; }
        }
    }
    public class ACHDepositAccountModel
    {
        private int accid;
        private string stripetestkey;
        private string stripeproductionkey;
        private int _payeeid;



        public int PayeeId
        {
            get
            {
                return _payeeid;
            }

            set
            {
                _payeeid = value;
            }
        }

        public int ACHDepositAccountId
        {
            get { return accid; }
            set { accid = value; }
        }
        [Required]
        [Display(Name = "Stripe TestMode Key")]
        public string StripeTestKey
        {
            get { return stripetestkey; }
            set { stripetestkey = value; }
        }
        [Required]
        [Display(Name = "Stripe Production Key")]
        public string StripeProductionKey
        {
            get { return stripeproductionkey; }
            set { stripeproductionkey = value; }
        }
    }


    public class DurationModel
    {
        private DateTime from;
        private DateTime to;
        [Required]
        [Display(Name = "From Date")]
        public DateTime From
        {
            get { return from; }
            set { from = value; }
        }
        [Required]
        [Display(Name = "To Date")]
        public DateTime To
        {
            get { return to; }
            set { to = value; }
        }

    }

    public class ReportDurationModel
    {
        private int month;
        private int year;
        private int payee;
        private string payeeIds;

        [Required]
        public int Month
        {
            get { return month; }
            set { month = value; }
        }
        [Required]
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        [Required]
        public int Payee
        {
            get { return payee; }
            set { payee = value; }
        }

        public string PayeeIds
        {
            get { return payeeIds; }
            set { payeeIds = value; }
        }

    }
   
    //public static class ExtensionMethods
    //{
    //    public static Customer ToCustomer(this CustomerModel model)
    //    {
    //        Customer newmodel = new Customer();
    //        if (model != null)
    //        {
    //            newmodel.PayeeId = model.PayeeId;
    //            newmodel.CustomerId = model.CustomerId;
    //            newmodel.CustomerFirstName = model.CustomerFirstName;
    //            newmodel.CustomerLastName = model.CustomerLastName;
    //            newmodel.CustomerAddress = model.CustomerAddress;
    //            newmodel.CustomerAddress2 = model.CustomerAddress2;
    //            newmodel.CustomerCity = model.CustomerCity;
    //            newmodel.CustomerState = model.CustomerState;
    //            newmodel.CustomerCountry = model.CustomerCountry;
    //            newmodel.CustomerZipCode = model.CustomerZipCode;
    //            newmodel.CustomerEmail = model.CustomerEmail;
    //            newmodel.AmountDue = model.AmountDue;
    //            newmodel.BillDescription = model.BillDescription;
    //            newmodel.NextBillDate = Convert.ToDateTime(model.NextBillDate).Date;
    //            newmodel.Intervals = model.Intervals;
    //            newmodel.IntervalTypeId = Convert.ToInt32(model.IntervalType);
    //            newmodel.LateCharges = model.LateCharges;
    //            newmodel.LateChargesStartday = model.LateChargesStartday;
    //            newmodel.LateChargesStartdayAmount = model.LateChargesStartdayAmount;
    //            newmodel.LateChargesThereAfterday = model.LateChargesThereAfterday;
    //            newmodel.LateChargesThereAfterdayAmount = model.LateChargesThereAfterdayAmount;
    //            newmodel.CCEmail = model.CCEmail;
    //            newmodel.BCCEmail = model.BCCEmail;
    //            newmodel.CustomerPhoneNo = model.CustomerPhoneNo;
    //            newmodel.EmergencyPhoneNo = model.EmergencyPhoneNo;
    //            return newmodel;

    //        }
    //        return newmodel;

    //    }

    //    public static CustomerModel ToCustomerModel(this Customer model)
    //    {
    //        CustomerModel newmodel = new CustomerModel();
    //        if (model != null)
    //        {
    //            newmodel.PayeeId = model.PayeeId;
    //            newmodel.CustomerId = model.CustomerId;
    //            newmodel.CustomerFirstName = model.CustomerFirstName;
    //            newmodel.CustomerLastName = model.CustomerLastName;
    //            newmodel.CustomerAddress = model.CustomerAddress;
    //            newmodel.CustomerAddress2 = model.CustomerAddress2;
    //            newmodel.CustomerCity = model.CustomerCity;
    //            newmodel.CustomerState = model.CustomerState;
    //            newmodel.CustomerCountry = model.CustomerCountry;
    //            newmodel.CustomerZipCode = model.CustomerZipCode;
    //            newmodel.CustomerEmail = model.CustomerEmail;
    //            newmodel.AmountDue = Convert.ToDecimal(model.AmountDue);
    //            newmodel.BillDescription = model.BillDescription;
    //            newmodel.NextBillDate = Convert.ToDateTime(model.NextBillDate).ToString("MM-dd-yyyy");
    //            newmodel.Intervals = model.Intervals;
    //            newmodel.IntervalType = model.IntervalTypeId.ToString();
    //            newmodel.LateCharges = model.LateCharges;
    //            newmodel.LateChargesStartday = model.LateChargesStartday;
    //            newmodel.LateChargesStartdayAmount = model.LateChargesStartdayAmount;
    //            newmodel.LateChargesThereAfterday = model.LateChargesThereAfterday;
    //            newmodel.LateChargesThereAfterdayAmount = model.LateChargesThereAfterdayAmount;
    //            newmodel.CCEmail = model.CCEmail;
    //            newmodel.BCCEmail = model.BCCEmail;
    //            newmodel.CustomerPhoneNo = model.CustomerPhoneNo;
    //            newmodel.EmergencyPhoneNo = model.EmergencyPhoneNo;
    //            return newmodel;

    //        }
    //        return newmodel;

    //    }

    //    public static Payee ToPayee(this PayeeModel model)
    //    {
    //        Payee newmodel = new Payee();
    //        newmodel.PayeeId = model.PayeeId;
    //        newmodel.UserId = model.UserId;
    //        newmodel.Payee1 = model.Payee;
    //        newmodel.PayeeAddress = model.PayeeAddress;
    //        newmodel.PayeeAddress2 = model.PayeeAddress2;
    //        newmodel.PayeeCity = model.PayeeCity;
    //        newmodel.PayeeState = model.PayeeState;
    //        newmodel.PayeeZipCode = model.PayeeZipCode;
    //        newmodel.PayeeComments = model.PayeeComments;
    //        newmodel.PayeeCountry = model.PayeeCountry;
    //        newmodel.PayeeEmail = model.PayeeEmail;
    //        newmodel.PayeeContactPerson = model.PayeeContactPerson;
    //        newmodel.PayeeWebsite = model.PayeeWebsite;
            
           
    //        return newmodel;

    //    }

    //    public static PayeeModel ToPayeeModel(this Payee model)
    //    {
    //        PayeeModel newmodel = new PayeeModel();
    //        newmodel.PayeeId = model.PayeeId;
    //        newmodel.UserId = model.UserId;
    //        newmodel.Payee = model.Payee1;
    //        newmodel.PayeeAddress = model.PayeeAddress;
    //        newmodel.PayeeAddress2 = model.PayeeAddress2;
    //        newmodel.PayeeCity = model.PayeeCity;
    //        newmodel.PayeeState = model.PayeeState;
    //        newmodel.PayeeZipCode = model.PayeeZipCode;
    //        newmodel.PayeeComments = model.PayeeComments;
    //        newmodel.PayeeCountry = model.PayeeCountry;
    //        newmodel.PayeeEmail = model.PayeeEmail;
    //        newmodel.PayeeContactPerson = model.PayeeContactPerson;
    //        newmodel.PayeeWebsite = model.PayeeWebsite;
    //        return newmodel;

    //    }

    //    public static UserDetail ToUserDetail(this UserModel model)
    //    {
    //        UserDetail newmodel = new UserDetail();

    //        newmodel.UserId = model.UserId;
    //        newmodel.FirstName = model.FirstName;
    //        newmodel.LastName = model.LastName;
    //        newmodel.Address = model.Address;
    //        newmodel.Address2 = model.Address2;
    //        newmodel.City = model.City;
    //        newmodel.State = model.State;
    //        newmodel.Country = model.Country;
    //        newmodel.ZipCode = model.ZipCode;
    //        newmodel.PhoneNumber = model.PhoneNumber;
    //        newmodel.Mobile = model.Mobile;

    //        return newmodel;
    //    }

    //    public static UserModel ToUserModel(this UserDetail model)
    //    {
    //        UserModel newmodel = new UserModel();

    //        newmodel.UserId = model.UserId;
    //        newmodel.FirstName = model.FirstName;
    //        newmodel.LastName = model.LastName;
    //        newmodel.Address = model.Address;
    //        newmodel.Address2 = model.Address2;
    //        newmodel.City = model.City;
    //        newmodel.State = model.State;
    //        newmodel.Country = model.Country;
    //        newmodel.ZipCode = model.ZipCode;
    //        newmodel.PhoneNumber = model.PhoneNumber;
    //        newmodel.Mobile = model.Mobile;
            
    //        return newmodel;
    //    }
    //}

}