using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BingHousingMVC.Models
{
    public class MailModel
    {
        private string _from;
        private string _to;
        private string _sub;
        private string _message;

        private string _attachment;

        private MailType _mailtype;
        private string _payee;


        public string From
        {
            get { return _from; }
            set { _from = value; }
        }
        public string Payee
        {
            get { return _payee; }
            set { _payee = value; }
        }


        public string To
        {
            get { return _to; }
            set { _to = value; }
        }

        public string Sub
        {
            get { return _sub; }
            set { _sub = value; }
        }


        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }


        public MailType Type
        {
            get { return _mailtype; }
            set { _mailtype = value; }
        }

        public string Attachment { get { return _attachment; } set { _attachment = value; } }
    }


    public class InvoiceMailModel : MailModel
    {
        private string _amount;
        private string _name;
        private string _projectNumber;
        private string _note;
        private int _billid;
        private bool _isReminder;
      
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ProjectNumber
        {
            get { return _projectNumber; }
            set { _projectNumber = value; }
        }
        public int BillId
        {
            get { return _billid; }
            set { _billid = value; }
        }
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }
        public bool IsReminder
        {
            get { return _isReminder; }
            set { _isReminder = value; }
        }
        
    }

    public class PaymentMailModel : InvoiceMailModel
    {
        public int OrderId { get; set; }
        public string BillingId { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Tax { get; set; }
        public string ShippingMethod { get; set; }
        public string PaymentType { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public DateTime BillingDate { get; set; }
        public string BillDescription { get; set; }
        public DateTime PaymentDate { get; set; }


    }
    public enum MailType
    {
        EmailPaymentUser,
        EmailPamentCustomer,
        EmailInvoice,
        EmailVerification

    }
}