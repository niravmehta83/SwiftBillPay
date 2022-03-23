using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingHousing_BO
{
    public partial class RemainderEmail
    {
        protected readonly IBHDbase dbase;

        public RemainderEmail()
        {

        }

        public RemainderEmail(int RemainderEmailId,int InvoiceId,DateTime SentDate,IBHDbase dbase):this()
        {
            this.RemainderEmailId = RemainderEmailId;
            this.InvoiceId = InvoiceId;
            this.SentDate = SentDate;
            this.dbase = dbase;
        }

        ~RemainderEmail()
        {

        }
        public int Insert()
        {
            return dbase.InsertRemainderEmail(this);
        }

        //public void Update(int Id)
        //{
        //    this.RemainderEmailId = Id;
        //    dbase.UpdateRemainderEmail(this);
        //}

        //public static void Delete(IBHDbase dbase, int Id)
        //{
        //    dbase.DeleteRemainderEmail(Id);
        //}

        //public static List<RemainderEmail> GetAllRemainderEmails(IBHDbase dbase, int UserId)
        //{
        //    return dbase.GetAllRemainderEmails(UserId);
        //}


        //public static RemainderEmail GetRemainderEmail(IBHDbase dbase, int PayeeId)
        //{
        //    return dbase.GetRemainderEmail(PayeeId);
        //}
    }
}
