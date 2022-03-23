using BingHousing_BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BingHousingMVC.Utility
{
    public static class LateChargesHelper
    {
        public static Decimal CalculateLateCharges(int startday, decimal startdayAmount, int thereafterday, decimal thereafterdayAmount, DateTime Edate)
        {
            int sd = startday;
            int ta = thereafterday;
            decimal sdAmount = startdayAmount;
            decimal taAmount = thereafterdayAmount;
            decimal totalAmount = 0;
            DateTime date = new DateTime(Edate.Year, Edate.AddMonths(1).Month, 1);
            //DateTime date = new DateTime(Edate.Year, Edate.Month, 20); //old changed on 22/10/2016
            int diff = Convert.ToInt32((DateTime.Now.Date - date.Date).TotalDays);
            if (diff < (sd - 1))
            {
                totalAmount = 0;
            }
            else if (diff >= (sd - 1) && diff < ta)
            {
                totalAmount = sdAmount;
            }
            else
            {
                totalAmount = sdAmount + ((diff - ta + 1) * taAmount);
            }
            return totalAmount;
        }

        public static List<InvoiceDetail> FindLateCharges(List<InvoiceDetail> list)
        {
            if (list.Count > 0)
            {
                foreach (InvoiceDetail item in list)
                {
                    if (item.IsLateCharges)
                    {

                        item.LateCharges = LateChargesHelper.CalculateLateCharges(item.LateChargesStartday, item.LateChargesStartdayAmount, item.LateChargesThereAfterday, item.LateChargesThereAfterdayAmount, item.EmailSentDate);
                    }

                }
            }
            return list;
        }
    }
}