using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousing_BO;

namespace BingHousingMVC_DAL
{
    internal class DeleteOperations
    {
        internal static void DeleteCustomerDetails(int CustomerId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Customer cd = Dbase.Customers.Select(c => c).Where(c => c.CustomerId == CustomerId).SingleOrDefault();



                if (cd != null)
                {
                    DeleteGroupMember(CustomerId, true);

                    Dbase.Customers.Remove(cd);

                    Dbase.SaveChanges();
                }

            }
        }
        internal static void DeleteACHDepositDetails(int Id)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                ACHAccountDepositDetail cd = Dbase.ACHAccountDepositDetails.SingleOrDefault(c => c.ACHDepositAccountId == Id);

                if (cd != null)
                {
                    Dbase.ACHAccountDepositDetails.Remove(cd);

                    Dbase.SaveChanges();
                }

            }
        }

        internal static void DeletePayee(int PayeeId)
        {
            List<Customer> cus = null;
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Payee cd = Dbase.Payees.Select(c => c).Where(c => c.PayeeId == PayeeId).SingleOrDefault();

                if (cd != null)
                {
                    cus = Dbase.Customers.Select(a => a).Where(c => c.PayeeId == PayeeId).ToList();

                    bool flag = (cus != null) ? ((cus.Count > 0) ? true : false) : false;
                    if (flag)
                    {

                        foreach (Customer c in cus)
                        {
                            Dbase.Customers.Remove(c);

                            Dbase.SaveChanges();
                        }
                    }

                    Dbase.Payees.Remove(cd);

                    Dbase.SaveChanges();
                }

            }
        }

        internal static void DeletePayPalAccountDetails(int Id)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                PayPalAccountDetail cd = Dbase.PayPalAccountDetails.SingleOrDefault(c => c.PayPalAccountId == Id);

                if (cd != null)
                {
                    Dbase.PayPalAccountDetails.Remove(cd);

                    Dbase.SaveChanges();
                }

            }
        }

        internal static void DeletePlan(int Id)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Plan cd = Dbase.Plans.SingleOrDefault(c => c.PlanId == Id);

                if (cd != null)
                {
                    Dbase.Plans.Remove(cd);

                    Dbase.SaveChanges();
                }

            }
        }

        internal static void DeleteSubcription(int Id)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Subscription cd = Dbase.Subscriptions.SingleOrDefault(c => c.SubscriptionId == Id);

                if (cd != null)
                {
                    Dbase.Subscriptions.Remove(cd);

                    Dbase.SaveChanges();
                }

            }
        }

        internal static void DeleteGroup(int groupId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Group cd = Dbase.Groups.SingleOrDefault(c => c.GroupId == groupId);

                if (cd != null)
                {
                    Dbase.GroupMembers.Select(a => a).Where(b => b.GroupId == cd.GroupId).ToList().ForEach(c => Dbase.GroupMembers.Remove(c));

                    Dbase.Groups.Remove(cd);

                    Dbase.SaveChanges();
                }

            }
        }

        internal static void DeleteGroupMember(int Id, bool IsustomerId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                if (!IsustomerId)
                {
                    GroupMember cd = Dbase.GroupMembers.SingleOrDefault(c => c.GroupMemberId == Id);
                    if (cd != null)
                    {
                        Dbase.GroupMembers.Remove(cd);

                       
                    }
                }
                else
                {
                    List<GroupMember> cds = Dbase.GroupMembers.Where(c => c.CustomerId == Id).ToList();

                    bool flag = cds == null ? false : cds.Count > 0 ? true : false;

                    if (flag)
                    {
                        foreach(GroupMember cd in cds)
                        {
                            if (cd != null)
                            {
                                Dbase.GroupMembers.Remove(cd);


                            }
                        }
                    }
                }
                Dbase.SaveChanges();


            }
        }



        //internal static void DeleteGroupMember(List<int> groupMemberIds)
        //{
        //    using (BHDbaseEntities Dbase = new BHDbaseEntities())
        //    {
        //        if (groupMemberIds.Count > 0)
        //        {
        //            groupMemberIds.ForEach(a=>Dbase.GroupMembers.r.Remove(b=>b.GroupId==a))(
        //        }
        //        GroupMember cd = Dbase.GroupMembers.SingleOrDefault(c => c.GroupMemberId == groupMemberId);

        //        if (cd != null)
        //        {
        //            Dbase.GroupMembers.Remove(cd);

        //            Dbase.SaveChanges();
        //        }

        //    }
        //}

        internal static void DeleteInvoice(int invoiceId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Invoice cd = Dbase.Invoices.SingleOrDefault(c => c.InvoiceId == invoiceId);

                if (cd != null)
                {
                    Dbase.PaymentDetails.Select(a => a).Where(b => b.PaymentId == cd.PaymentId).ToList().ForEach(c => Dbase.PaymentDetails.Remove(c));
                    Dbase.RemainderEmails.Select(a => a).Where(b => b.InvoiceId == cd.InvoiceId).ToList().ForEach(c => Dbase.RemainderEmails.Remove(c));

                    Dbase.Invoices.Remove(cd);

                    Dbase.SaveChanges();
                }

            }
        }
    }
}