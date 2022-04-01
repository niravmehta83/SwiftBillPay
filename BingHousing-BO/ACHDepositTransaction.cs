using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{
    public partial class ACHAccountDepositDetail
    {
        protected readonly IBHDbase dbase;


        public int Insert()
        {
            return dbase.InsertACHAccountDepositDetail(this);
        }

        public void Update()
        {
            dbase.UpdateACHDepositAcountDetails(this);
        }

        public static void Delete(IBHDbase dbase, int Id)
        {
            dbase.DeleteACHDepositAccountDetails(Id);
        }

        public static List<ACHAccountDepositDetail> GetAllACHDepositAccountDetail(IBHDbase dbase)
        {
            return dbase.GetAllACHDepositAccountDetails();
        }
        public static ACHAccountDepositDetail GetACHDepositAccountDetail(IBHDbase dbase, int CustomerId)
        {
            return dbase.GetACHDepositAccountDetail(CustomerId);
        }
    }

}
