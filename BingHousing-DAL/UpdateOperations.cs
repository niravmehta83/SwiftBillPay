using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousing_BO;


namespace BingHousingMVC_DAL
{
    internal class UpdateOperations
    {
        internal static void UpdateUserDetails(UserDetail model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                UserDetail newmodel = Dbase.UserDetails.Select(c => c).Where(c => c.UserId == model.UserId).SingleOrDefault();



                newmodel.UserId = model.UserId;
                newmodel.FirstName = model.FirstName;
                newmodel.LastName = model.LastName;
                newmodel.Address = model.Address;
                newmodel.Address2 = model.Address2;
                newmodel.City = model.City;
                newmodel.State = model.State;
                newmodel.Country = model.Country;
                newmodel.ZipCode = model.ZipCode;
                newmodel.PhoneNumber = model.PhoneNumber;
                newmodel.Mobile = model.Mobile;

                Dbase.SaveChanges();

            }
        }

        internal static bool updateACHRegistration(UserACHBankAccount model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                UserACHBankAccount detail = Dbase.ACHBankAccounts.SingleOrDefault(a => a.CustomerId == model.CustomerId);
                if (detail != null)
                {
                    Dbase.Entry(detail).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }
            }
            return true;
        }

        internal static void UpdateCustomerProfileDetails(CustomerProfile model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                CustomerProfile newmodel = Dbase.CustomerProfiles.Select(c => c).Where(c => c.CustomerProfileId == model.CustomerProfileId).SingleOrDefault();
                if (newmodel != null)
                {
                    Dbase.Entry(newmodel).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }

            }
        }
        internal static void UpdateACHDepositAcountDetails(ACHAccountDepositDetail model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                ACHAccountDepositDetail detail = Dbase.ACHAccountDepositDetails.SingleOrDefault(a => a.ACHDepositAccountId == model.ACHDepositAccountId);
                if (detail != null)
                {
                    Dbase.Entry(detail).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }
            }
        }

        internal static void UpdateCustomerDetails(Customer model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Customer newmodel = Dbase.Customers.Select(c => c).Where(c => c.CustomerId == model.CustomerId).SingleOrDefault();
                if (newmodel != null)
                {
                    Dbase.Entry(newmodel).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }
            }
        }


        internal static void UpdatePayee(Payee model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Payee newmodel = Dbase.Payees.Select(c => c).Where(c => c.PayeeId == model.PayeeId).SingleOrDefault();
                if (newmodel != null)
                {
                    Dbase.Entry(newmodel).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }
            }
        }

        internal static void changePassword2(string UserName, string Password, string Password2)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                var newmodel = Dbase.UserSecurities
                    .Join(Dbase.UserProfiles, a => a.UserId, b => b.UserId, (a, b) => new { obj = a, UserName = b.UserName })
                    .Where(c => c.UserName == UserName).SingleOrDefault();

                newmodel.obj.Password2 = Password2;
                newmodel.obj.Password = Password;

                Dbase.SaveChanges();

            }
        }


        internal static void UpdatePapPalAcountDetails(PayPalAccountDetail model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                PayPalAccountDetail detail = Dbase.PayPalAccountDetails.SingleOrDefault(a => a.PayPalAccountId == model.PayPalAccountId);
                if (detail != null)
                {
                    Dbase.Entry(detail).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }

            }
        }

        internal static void UpdatePayeeDetails(Payee model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Payee detail = Dbase.Payees.SingleOrDefault(a => a.PayeeId == model.PayeeId);
                if (detail != null)
                {
                    Dbase.Entry(detail).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }

            }
        }

        internal static void UpdatePlan(Plan model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Plan detail = Dbase.Plans.SingleOrDefault(a => a.PlanId == model.PlanId);
                if (detail != null)
                {
                    Dbase.Entry(detail).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }
            }
        }

        internal static void UpdateSubcription(Subscription model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Subscription detail = Dbase.Subscriptions.SingleOrDefault(a => a.SubscriptionId == model.SubscriptionId);
                if (detail != null)
                {
                    Dbase.Entry(detail).CurrentValues.SetValues(model);
                    Dbase.SaveChanges();
                }
            }
        }


        internal static void UpdateGroup(Group group)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
               var detail= Dbase.Groups.SingleOrDefault(a => a.GroupId == group.GroupId);
               if (detail != null)
               {
                   Dbase.Entry(detail).CurrentValues.SetValues(group);
                   Dbase.SaveChanges();
               }

            }
        }

        internal static void UpdateBillDesc(string[] CustomerIds, string BillDesc, int GroupId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                foreach(var customersId in CustomerIds)
                {
                    int Id = Convert.ToInt32(customersId);
                    var detail = Dbase.GroupMembers.SingleOrDefault(a => a.CustomerId == Id && a.GroupId== GroupId);
                    if (detail != null)
                    {
                        detail.Billdescription = BillDesc;                        
                        Dbase.SaveChanges();
                    }
                }
            }
        }
        internal static void UpdateGroupMember(GroupMember groupMember)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                GroupMember grpMem = Dbase.GroupMembers.Where(gm => gm.GroupMemberId == groupMember.GroupMemberId).FirstOrDefault();
                if (grpMem != null)
                {
                    Dbase.Entry(grpMem).CurrentValues.SetValues(groupMember);
                    Dbase.SaveChanges();
                }

            }
        }

        internal static void UpdateBillDescription(List<CustomerDetail> details, int groupId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                foreach (CustomerDetail det in details)
                {
                    GroupMember gm = Dbase.GroupMembers.SingleOrDefault(a => a.CustomerId == det.CustomerId && a.GroupId==groupId);
                    if (gm != null)
                    {
                        gm.Billdescription = det.BillDescription;
                        gm.BillAmount = det.AmountDue;
                        if (det.NextBillDate.Date == DateTime.MinValue || det.NextBillDate == null)
                        {
                            gm.Nextbilldate = DateTime.Now.AddYears(1);
                        }
                        else
                        {
                            gm.Nextbilldate = det.NextBillDate;
                        }
                        Dbase.SaveChanges();
                    }
                    
                }
                //Dbase.SaveChanges();
            }
        }

        internal static bool UpdateOnlineCheck(OnlineCheck check)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                bool success = false;
                var checkOnline = Dbase.OnlineChecks.Where(x => x.PaymentId == check.PaymentId).FirstOrDefault();
                if (checkOnline != null)
                {
                    checkOnline.RoutingNumber = check.RoutingNumber;
                    checkOnline.AccountNumber = check.AccountNumber;
                    checkOnline.CsvFileData = check.CsvFileData;
                    Dbase.SaveChanges();
                }
                var paymentDetails = Dbase.PaymentDetails.Where(x => x.PaymentId == check.PaymentId).FirstOrDefault();
                var checkDetail = Dbase.CheckDetails.Where(x => x.CheckId == paymentDetails.CheckId).FirstOrDefault();
                if(checkDetail != null)
                {
                    checkDetail.RountingNumber = check.RoutingNumber;
                    checkDetail.AccountNumber = check.AccountNumber;
                    Dbase.SaveChanges();
                }
                success = true;
                return success;
            }
        }
    }
}