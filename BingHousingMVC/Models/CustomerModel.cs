using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BingHousingMVC.Models
{
    public class CustomerModel
    {
        private string type;
        private int _payeeid;
        private int _customerid;
        private string _cfirstname;
        private string _clastname;
        private string _caddress;
        private string _caddress2;
        private string _ccity;
        private string _cstate;
        private string _czipcode;
        private string _ccountry;
        private string _cemail;
        private decimal _amountdue;
        private string _billdescription;

        private string _nextbilldate;
        private int _intervals;
        private string _intervalTypeId;

        private bool _lateCharges;
        private int _lateChargesStartDay;
        private decimal _lateChargesStartDayAmount;
        private int _lateChargesAfter;
        private decimal _lateChargesAfterAmount;
        private string _customerphoneno;

        private string _emergencyphoneno;

        private string _ccemail;

        private string _bccemail;



        public int CustomerId
        {
            get
            {
                return _customerid;
            }

            set
            {
                _customerid = value;
            }
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

        [Required]
        [Display(Name = "Type")]
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "First Name")]
        public string CustomerFirstName
        {
            get
            {
                return _cfirstname;
            }

            set
            {
                _cfirstname = value;
            }
        }

        [Required]
        [Display(Name = "Last Name")]
        public string CustomerLastName
        {
            get
            {
                return _clastname;
            }

            set
            {
                _clastname = value;
            }
        }

        [Required]
        [Display(Name = "Address")]
        public string CustomerAddress
        {
            get
            {
                return _caddress;
            }

            set
            {
                _caddress = value;
            }
        }

        [Display(Name = "Customer Phone No")]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Phone number")]
        public string CustomerPhoneNo
        {
            get
            {
                return this._customerphoneno;
            }
            set
            {
                this._customerphoneno = value;
            }
        }
        [Display(Name = "Emergency Phone No")]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Phone number")]
        public string EmergencyPhoneNo
        {
            get
            {
                return this._emergencyphoneno;
            }
            set
            {
                this._emergencyphoneno = value;
            }
        }
        [Display(Name = "Bcc Email")]
        public string BCCEmail
        {
            get
            {
                return this._bccemail;
            }
            set
            {
                this._bccemail = value;
            }
        }

        [Display(Name = "Cc Email")]
        public string CCEmail
        {
            get
            {
                return this._ccemail;
            }
            set
            {
                this._ccemail = value;
            }
        }

        [Display(Name = "Address2")]
        public string CustomerAddress2
        {
            get
            {
                return _caddress2;
            }

            set
            {
                _caddress2 = value;
            }
        }

        [Required]
        [Display(Name = "City")]
        public string CustomerCity
        {
            get
            {
                return _ccity;
            }

            set
            {
                _ccity = value;
            }
        }

        [Required]
        [Display(Name = "State")]
        public string CustomerState
        {
            get
            {
                return _cstate;
            }

            set
            {
                _cstate = value;
            }
        }

        [Required]
        [Display(Name = "Zip Code")]
        public string CustomerZipCode
        {
            get
            {
                return _czipcode;
            }

            set
            {
                _czipcode = value;
            }
        }

        [Required]
        [Display(Name = "Country")]
        public string CustomerCountry
        {
            get
            {
                return _ccountry;
            }

            set
            {
                _ccountry = value;
            }
        }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.+-]+\.(?:[a-zA-Z]{2}|COM|com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|edu|academy|biz|college|education|int)$", ErrorMessage = "Invalid Email Address")]
        public string CustomerEmail
        {
            get
            {
                return _cemail;
            }

            set
            {
                _cemail = value.Trim();
            }
        }

        [Required]
        [Display(Name = "Amount Due")]
        [RegularExpression(@"^[0-9|.]{0,50}$", ErrorMessage = "Enter only numbers")]
        public decimal AmountDue
        {
            get
            {
                return _amountdue;
            }

            set
            {
                _amountdue = value;
            }
        }

        [Required]
        [Display(Name = "Bill Description")]
        public string BillDescription
        {
            get
            {
                return _billdescription;
            }

            set
            {
                _billdescription = value;
            }
        }
        [Required]
        [Display(Name = "Next Bill Date")]
        public string NextBillDate
        {
            get
            {
                return _nextbilldate;
            }

            set
            {
                _nextbilldate = value;
            }
        }

        [Required]
        [Display(Name = "Intervals")]
        public int Intervals
        {
            get
            {
                return _intervals;
            }

            set
            {
                _intervals = value;
            }
        }

        [Required]
        [Display(Name = "Interval Type")]
        public string IntervalTypeID
        {
            get
            {
                return _intervalTypeId;
            }

            set
            {
                _intervalTypeId = value;
            }
        }

        [Required]
        [Display(Name = "Late Charges")]
        public bool LateCharges
        {
            get
            {
                return _lateCharges;
            }

            set
            {
                _lateCharges = value;
            }
        }

        [Required]
        [Display(Name = "Late Charges Start Day")]
        public int LateChargesStartday
        {
            get
            {
                return _lateChargesStartDay;
            }

            set
            {
                _lateChargesStartDay = value;
            }
        }

        [Required]
        [Display(Name = "Late Charges Start Day Amount")]
        [RegularExpression(@"^[0-9|.]{0,50}$", ErrorMessage = "Enter only numbers")]
        public decimal LateChargesStartdayAmount
        {
            get
            {
                return _lateChargesStartDayAmount;
            }

            set
            {
                _lateChargesStartDayAmount = value;
            }
        }

        [Required]
        [Display(Name = "Late Charges There After")]
        public int LateChargesThereAfterday
        {
            get
            {
                return _lateChargesAfter;
            }

            set
            {
                _lateChargesAfter = value;
            }
        }

        [Required]
        [Display(Name = "Late Charges There After Amount")]
        [RegularExpression(@"^[0-9|.]{0,50}$", ErrorMessage = "Enter only numbers")]
        public decimal LateChargesThereAfterdayAmount
        {
            get
            {
                return _lateChargesAfterAmount;
            }

            set
            {
                _lateChargesAfterAmount = value;
            }
        }

    }
}