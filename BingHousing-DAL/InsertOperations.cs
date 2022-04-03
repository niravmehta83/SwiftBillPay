using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using BingHousing_BO;

namespace BingHousingMVC_DAL
{
    internal class InsertOperations
    {
        internal static void InsertPassword(RegisterData model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                UserSecurity obj = new UserSecurity();
                obj.Password = model.Password;
                obj.Password2 = model.Password2;
                obj.UserId = GetOperations.GetUserId(model.UserName);
                obj.Email = model.Email;

                Dbase.UserSecurities.Add(obj);

                Dbase.SaveChanges();

            }
        }
        internal static int InsertACHDepositPaymentDetail(ChargeDetail model, List<int> invoiceIdlist)
        {

            int paymentId = 0;
            var scope = new TransactionScope(
            // a new transaction will always be created
            TransactionScopeOption.RequiresNew,
            // we will allow volatile data to be read during transaction
            new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
        );
            using (scope)
            {
                using (BHDbaseEntities Dbase = new BHDbaseEntities())
                {

                    model.InsertedOn = DateTime.Now;
                    model.ChargeResourceId = model.ChargeId.ToString();
                    model.TransactionId = model.TransactionId.ToString();
                    Dbase.ACHDetails.Add(model);

                    Dbase.SaveChanges();

                    PaymentDetail pmodel = new PaymentDetail();

                    pmodel.PaymentModeId = 4;

                    pmodel.PayPalId = model.ChargeId;

                    pmodel.Insertedby = model.UserId;

                    pmodel.InsertedOn = DateTime.Now;

                    Dbase.PaymentDetails.Add(pmodel);

                    Dbase.SaveChanges();

                    paymentId = pmodel.PaymentId;

                    foreach (int i in invoiceIdlist)
                    {
                        var Inv = Dbase.Invoices.SingleOrDefault(a => a.InvoiceId == i);
                        Inv.IsPaid = true;
                        Inv.PaymentId = paymentId;
                    }

                    Dbase.SaveChanges();


                }

                // everything good; complete
                scope.Complete();

                return paymentId;
            }

        }

        internal static string InsertACHRegistration(UserACHBankAccount model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Dbase.ACHBankAccounts.Add(model);

                Dbase.SaveChanges();

                return model.CustomerId;


            }
        }

        internal static void InsertUserDetails(UserDetail model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {


                Dbase.UserDetails.Add(model);

                Dbase.SaveChanges();


            }
        }
        internal static int InsertACHDepositAccountDetail(ACHAccountDepositDetail model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Dbase.ACHAccountDepositDetails.Add(model);

                Dbase.SaveChanges();

                return model.ACHDepositAccountId;
            }
        }

        internal static void InsertCustomerDetails(Customer obj)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Dbase.Customers.Add(obj);

                Dbase.SaveChanges();

            }
        }

        internal static int InsertInvoice(Invoice obj)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.Invoices.Add(obj);

                Dbase.SaveChanges();
                return obj.InvoiceId;

            }
        }

        internal static void InsertCustomerProfile(CustomerProfile model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.CustomerProfiles.Add(model);

                Dbase.SaveChanges();

            }
        }

        internal static int InsertCheckPaymentDetail(CheckDetail model, List<int> invoiceIdlist, int PaymentModeId)
        {

            int paymentId = 0;
            var scope = new TransactionScope(
            // a new transaction will always be created
            TransactionScopeOption.RequiresNew,
            // we will allow volatile data to be read during transaction
            new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
        );
            using (scope)
            {
                using (BHDbaseEntities Dbase = new BHDbaseEntities())
                {


                    Dbase.CheckDetails.Add(model);

                    Dbase.SaveChanges();

                    PaymentDetail pmodel = new PaymentDetail();

                    pmodel.PaymentModeId = PaymentModeId;

                    pmodel.CheckId = model.CheckId;

                    pmodel.Insertedby = model.UserId;

                    pmodel.InsertedOn = model.InsertedOn;

                    Dbase.PaymentDetails.Add(pmodel);

                    Dbase.SaveChanges();

                    paymentId = pmodel.PaymentId;

                    foreach (int i in invoiceIdlist)
                    {
                        var Inv = Dbase.Invoices.SingleOrDefault(a => a.InvoiceId == i);
                        Inv.IsPaid = true;
                        Inv.PaymentId = paymentId;
                    }

                    Dbase.SaveChanges();


                }

                // everything good; complete
                scope.Complete();

                return paymentId;
            }

        }


        internal static int InsertPaypalPaymentDetail(PayPalDetail model, List<int> invoiceIdlist)
        {

            int paymentId = 0;
            var scope = new TransactionScope(
            // a new transaction will always be created
            TransactionScopeOption.RequiresNew,
            // we will allow volatile data to be read during transaction
            new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
        );
            using (scope)
            {
                using (BHDbaseEntities Dbase = new BHDbaseEntities())
                {


                    Dbase.PayPalDetails.Add(model);

                    Dbase.SaveChanges();

                    PaymentDetail pmodel = new PaymentDetail();

                    pmodel.PaymentModeId = 1;

                    pmodel.PayPalId = model.PayPalId;

                    pmodel.Insertedby = model.UserId;

                    pmodel.InsertedOn = model.InsertedOn;

                    Dbase.PaymentDetails.Add(pmodel);

                    Dbase.SaveChanges();

                    paymentId = pmodel.PaymentId;

                    foreach (int i in invoiceIdlist)
                    {
                        var Inv = Dbase.Invoices.SingleOrDefault(a => a.InvoiceId == i);
                        Inv.IsPaid = true;
                        Inv.PaymentId = paymentId;
                    }

                    Dbase.SaveChanges();


                }

                // everything good; complete
                scope.Complete();

                return paymentId;
            }

        }


        internal static int InsertPaypalAccountDetail(PayPalAccountDetail model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Dbase.PayPalAccountDetails.Add(model);

                Dbase.SaveChanges();

                return model.PayPalAccountId;


            }
        }

        internal static int InsertPayee(Payee model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.Payees.Add(model);

                Dbase.SaveChanges();

                return model.PayeeId;


            }
        }

        internal static int InsertRemainderEmail(RemainderEmail model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.RemainderEmails.Add(model);

                Dbase.SaveChanges();

                return model.RemainderEmailId;
            }
        }

        internal static void InsertPlan(Plan model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.Plans.Add(model);

                Dbase.SaveChanges();

            }
        }

        internal static void InsertSubscription(Subscription model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.Subscriptions.Add(model);

                Dbase.SaveChanges();

            }
        }


        internal static void InsertGroup(Group group)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.Groups.Add(group);

                Dbase.SaveChanges();

            }
        }

        internal static void InsertGroupMember(GroupMember groupMember)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.GroupMembers.Add(groupMember);

                Dbase.SaveChanges();

            }
        }
        internal static int InsertCheckOnline(OnlineCheck model)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Dbase.OnlineChecks.Add(model);
                Dbase.SaveChanges();
                return model.OnlineCheckId;
            }
        }
    }
}