using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{
    public partial class Payee
    {
        protected readonly IBHDbase dbase;

        public Payee()
        {

        }
        public Payee(int UserId, string Payee, string PayeeAddress, string PayeeAddress2, string PayeeCity, string PayeeComments, string PayeeContactPerson, string PayeeCountry, string PayeeEmail,string PayeeState,string PayeeZipCode, string PayeeWebsite, IBHDbase dbase)
            : this()
        {
            this.UserId = UserId;
            this.Payee1 = Payee;
            this.PayeeAddress = PayeeAddress;
            this.PayeeAddress2 = PayeeAddress2;
            this.PayeeCity = PayeeCity;
            this.PayeeComments = PayeeComments;
            this.PayeeContactPerson = PayeeContactPerson;
            this.PayeeCountry = PayeeCountry;
            this.PayeeEmail = PayeeEmail;
            this.PayeeState = PayeeState;
            this.PayeeWebsite = PayeeWebsite;
            this.PayeeZipCode = PayeeZipCode;
            this.dbase = dbase;
        }
        ~Payee()
        {

        }
        public int Insert()
        {
            return dbase.InsertPayee(this);
        }

        public void Update(int Id)
        {
            this.PayeeId = Id;
            dbase.UpdatePayee(this);
        }

        public static void Delete(IBHDbase dbase, int Id)
        {
            dbase.DeletePayee(Id);
        }

        public static List<Payee> GetAllPayees(IBHDbase dbase,int UserId)
        {
            return dbase.GetAllPayee(UserId);
        }

        public static Payee GetPayeeByCustomerId(IBHDbase dbase, int CustomerId)
        {
            return dbase.GetPayeeByCustomerId(CustomerId);
        }

        public static Payee GetPayee(IBHDbase dbase, int PayeeId)
        {
            return dbase.GetPayee(PayeeId);
        }
    }
}
