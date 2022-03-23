using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{
    public partial class PayPalAccountDetail 
    {
        protected readonly IBHDbase dbase;

        public PayPalAccountDetail()
        {
        }
        public PayPalAccountDetail(int Id,int PayeeId,string ClientId,string ClientSecretId,int PayPalSurCharge,DateTime insertedDate,DateTime updateddate,IBHDbase dbase):this()
        {
            this._PayPalAccountId = Id;
            this._PayeeId = PayeeId;
            this._ClientID = ClientId;
            this._ClientSecret = ClientSecretId;
            this._InsertedOn = insertedDate;
            this._UpdatedOn = updateddate;
            this.dbase = dbase;
            this._PayPalSurCharge = PayPalSurCharge;
        }

        ~PayPalAccountDetail()
        {
        }
        public int Insert()
        {
            return dbase.InsertPaypalAccountDetail(this);
        }

        public void Update()
        {
            dbase.UpdatePapPalAcountDetails(this);
        }

        public static void Delete(IBHDbase dbase,int Id)
        {
            dbase.DeletePayPalAccountDetails(Id);
        }

        public static List<PayPalAccountDetail> GetAllPayPalAccountDetail(IBHDbase dbase)
        {
            return dbase.GetAllPayPalAccountDetails();
        }

        public static PayPalAccountDetail GetPayPalAccountDetail(IBHDbase dbase, int CustomerId)
        {
            return dbase.GetPayPalAccountDetail(CustomerId);
        }
    }
   
}
