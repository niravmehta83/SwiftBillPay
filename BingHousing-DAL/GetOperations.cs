using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousing_BO;

namespace BingHousingMVC_DAL
{

    internal static class GetOperations
    {

        internal static UserACHBankAccount GetCustomerStripeProfile(int UserId)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {



                var obj = Dbase.CustomerStripeProfile
                        .Select(a => a)
                        .Where(a => a.UserId == UserId)
                          .SingleOrDefault();

                return (UserACHBankAccount)obj;




            }
        }

        internal static UserDetail GetUserDetail(string UserName)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {



                var obj = Dbase.UserDetails
                          .Join(Dbase.UserProfiles, a => a.UserId, b => b.UserId, (a, b) => new { User = a, Usename = b.UserName })
                          .Where(b => b.Usename == UserName)
                          .SingleOrDefault();

                return (UserDetail)obj.User;




            }
        }
        internal static ACHAccountDepositDetail GetACHDepositAccountDetail(int Id)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                ACHAccountDepositDetail model = Dbase.ACHAccountDepositDetails.SingleOrDefault(a => a.PayeeId == Id);

                return model;


            }
        }

        internal static CustomerProfile GetCustomerProfile(int UserId)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {



                var obj = Dbase.CustomerProfiles
                        .Select(a => a)
                        .Where(a => a.UserId == UserId)
                          .SingleOrDefault();

                return (CustomerProfile)obj;




            }
        }

        internal static string GetPassword(string UserName)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                var obj = Dbase.UserSecurities
                          .Join(Dbase.UserProfiles, first => first.UserId, second => second.UserId, (first, second) => new { Password = first.Password2, Username = second.UserName })
                          .Where(c => c.Username == UserName).SingleOrDefault();

                return obj.Password;


            }
        }

        internal static string GetCustomerEmail(string ProjectNumber)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                var obj = Dbase.Customers
                          .Join(Dbase.Payees, first => first.PayeeId, second => second.PayeeId, (first, second) => new { CustomerId = first.CustomerId, UserId = second.UserId, Email = first.CustomerEmail })
                          .Join(Dbase.Invoices, first => first.CustomerId, second => second.CustomerId, (first, second) => new { UserId = first.UserId, Email = first.Email, ProNumber = second.InvoiceNumber, Date = second.EmailSentDate, IsPaid = second.IsPaid })
                          .Join(Dbase.UserSecurities, first => first.UserId, second => second.UserId, (first, second) => new { Email = first.Email, ProNumber = first.ProNumber, Date = first.Date, Islocked = second.Islocked, IsPaid = first.IsPaid })
                          .Where(c => c.ProNumber == ProjectNumber && !c.Islocked && !c.IsPaid).OrderByDescending(c => c.Date).Select(a => a.Email).FirstOrDefault();
                string Email = obj ?? "";
                return Email;


            }
        }

        internal static int GetUserId(string UserName)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                var obj = Dbase.UserProfiles
                          .Select(a => a)
                          .Where(c => c.UserName == UserName).SingleOrDefault();

                return obj.UserId;


            }
        }

        internal static List<CustomerDetail> GetCustomerDetailList(int UserId = 0)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {


                if (UserId != 0)
                {
                    List<CustomerDetail> obj = (Dbase.GetCustomerDetails(UserId, false).ToList());
                    //.Join(Dbase.UserSecurities, first => first.UserId, second => second.UserId, (first, second) => new { Islocked = second.Islocked, first }).Where(c => c.first.UserId == UserId))
                    //                       .Select(a => a.first).ToList<CustomerDetail>();

                    return obj;
                }

                else
                {
                    //List<CustomerDetail> obj = (Dbase.CustomerDetails
                    //    .Join(Dbase.UserSecurities, first => first.UserId, second => second.UserId, (first, second) => new { Islocked = second.Islocked, first }).Where(c => !c.Islocked))
                    //                           .Select(a => a.first).ToList<CustomerDetail>();
                    List<CustomerDetail> obj = (Dbase.GetCustomerDetails(UserId, false).ToList());
                    return obj;
                }



            }
        }

        internal static CustomerDetail GetCustomerDetail(int CustomerId,int GroupId)
        {
            CustomerDetail obj = new CustomerDetail();
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                if (GroupId > 0)
                {
                    obj = Dbase.GetCustomerDetails(CustomerId, true).SingleOrDefault();
                    var gm = Dbase.GroupMembers.SingleOrDefault(x=>x.GroupId==GroupId && x.CustomerId==CustomerId);
                    if (gm != null)
                    {
                        obj.BillDescription = !string.IsNullOrEmpty(gm.Billdescription) ? gm.Billdescription : obj.BillDescription;
                        obj.NextBillDate = gm.Nextbilldate != null  ? Convert.ToDateTime(gm.Nextbilldate) : obj.NextBillDate;
                        obj.AmountDue = gm.BillAmount>0 && gm.BillAmount != null  ? gm.BillAmount : obj.AmountDue;
                    }
                }
                else
                {
                    obj = Dbase.GetCustomerDetails(CustomerId, true).SingleOrDefault();                    
                }
                return obj;
            }
        }

        internal static Customer GetCustomer(int CustomerId)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                Customer obj = Dbase.Customers.SingleOrDefault(a => a.CustomerId == CustomerId);
                return obj;

            }
        }

        internal static string GetUserEmail(string UserName = "", int UserId = 0)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                if (UserName != "")
                {
                    var obj = Dbase.UserSecurities
                                .Join(Dbase.UserProfiles, a => a.UserId, b => b.UserId, (a, b) => new { Email = a.Email, UserName = b.UserName })
                                .SingleOrDefault(r => r.UserName == UserName);
                    return obj == null ? "" : obj.Email;

                }
                else
                {
                    var obj = Dbase.UserSecurities.SingleOrDefault(r => r.UserId == UserId);

                    return obj == null ? "" : obj.Email;
                }



            }
        }

        internal static List<string> GetAllUserName(bool IsAdmin = false)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {


                if (!IsAdmin)
                {
                    List<string> obj = Dbase.UserProfiles.Where(x=>x.IsInActive == false)
                                       .Select(a => a.UserName.ToLower())
                                       .ToList<string>();

                    return obj;
                }
                else
                {
                    var ob = Dbase.UserProfiles.Where(x => x.IsInActive == false).Where(a => a.webpages_Roles.FirstOrDefault().RoleId != 2 && a.IsInActive == false).ToList();
                    List<string> obj = Dbase.UserProfiles.Where(x => x.IsInActive == false).Where(a => a.webpages_Roles.FirstOrDefault().RoleId != 2 && a.IsInActive ==false )
                                       .Select(a => a.UserName)
                                       .ToList<string>();

                    return obj;
                }



            }
        }

        internal static string GetConfirmationToken(string UserName)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                var obj = Dbase.webpages_Membership
                             .Join(Dbase.UserProfiles, a => a.UserId, b => b.UserId, (a, b) => new { ConfirmationToken = a.ConfirmationToken, UserName = b.UserName })
                             .Where(r => r.UserName == UserName).SingleOrDefault();

                return obj != null ? obj.ConfirmationToken.ToString() : "";

            }
        }


        internal static List<InvoiceDetail> GetInvoiceDetails(string InvoiceNumber = "", InvoiceType type = InvoiceType.All, int Id = 0, IdType idtype = IdType.UserId,string TYpe="",string search="")
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<InvoiceDetail> list = null;


                //List<InvoiceDetail> list = Dbase.GetInvoiceDetails(InvoiceNumber,false,0).ToList<InvoiceDetails>();

                if (InvoiceNumber != "")
                {
                    switch (type)
                    {
                        case InvoiceType.Notpaid:
                            list = Dbase.GetInvoiceDetails(InvoiceNumber, 0, 0, 0, 0, TYpe,search).ToList();
                            break;
                        case InvoiceType.Paid:
                            list = Dbase.GetInvoiceDetails(InvoiceNumber, 1, 0, 0, 0, TYpe, search).ToList();
                            break;
                        case InvoiceType.All:
                            list = Dbase.GetInvoiceDetails(InvoiceNumber, 2, 0, 0, 0, TYpe, search).ToList();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (idtype)
                    {
                        case IdType.UserId:
                            switch (type)
                            {
                                case InvoiceType.Notpaid:
                                    list = Dbase.GetInvoiceDetails("", 0, 0, 0, Id, TYpe, search).ToList();
                                    break;
                                case InvoiceType.Paid:
                                    list = Dbase.GetInvoiceDetails("", 1, 0, 0, Id, TYpe, search).ToList();
                                    break;
                                case InvoiceType.All:
                                    list = Dbase.GetInvoiceDetails("", 2, 0, 0, Id, TYpe, search).ToList();
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case IdType.PaymentId:
                            list = Dbase.GetInvoiceDetails("", 0, Id, 0, 0, TYpe, search).ToList();
                            break;
                        case IdType.InvoiceId:
                            list = Dbase.GetInvoiceDetails("", 0, 0, Id, 0, TYpe, search).ToList();
                            break;

                        default:
                            break;
                    }

                }
                
                return list;


            }
        }


        internal static List<AccountUserModel> GetAllActiveUsers()
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<AccountUserModel> list = Dbase.UserSecurities.Where(a => !a.Islocked && a.UserProfile.IsInActive==false)
                                              .Select(r => new AccountUserModel { UserId = r.UserId, Name = r.UserProfile.UserName ,RoleName=r.UserProfile.webpages_Roles.FirstOrDefault().RoleName})
                                              .ToList<AccountUserModel>();


                return list;

            }
        }

        internal static List<AccountUserModel> GetAllArchivedUsers()
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<AccountUserModel> list = Dbase.UserSecurities.Where(a => a.Islocked&& a.UserProfile.IsInActive==false)
                                                  .Select(r => new AccountUserModel { UserId = r.UserId, Name = r.UserProfile.UserName,RoleName=r.UserProfile.webpages_Roles.FirstOrDefault().RoleName })
                                                  .ToList<AccountUserModel>();


                return list;



            }
        }

        internal static CheckDetail GetCheckDetail(int Id, bool isuserid)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                CheckDetail list = null;//late binding 

                if (!isuserid)
                {
                    list = Dbase.CheckDetails.Where(a => a.CheckId == Id)
                          .Select(a => a)
                          .SingleOrDefault();
                }
                else
                {
                    list = Dbase.CheckDetails.Where(a => a.UserId == Id)
                           .Select(a => a).OrderByDescending(a => a.CheckId)
                           .FirstOrDefault();
                }


                return list;

            }
        }

        
        internal static CheckDetail GetCheckDetailByPaymentId(int PaymentId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                var paymentDetails = Dbase.PaymentDetails.Where(x => x.PaymentId == PaymentId).FirstOrDefault();
                var checkDetail = Dbase.CheckDetails.Where(x => x.CheckId == paymentDetails.CheckId).FirstOrDefault();
                return checkDetail;
            }
        }

        internal static List<CheckDetail> GetAllCheckDetail()
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<CheckDetail> list = Dbase.CheckDetails.Select(a => a)
                                              .ToList<CheckDetail>();


                return list;



            }
        }

        internal static List<Payments> GetAllPayments(int UserId, string Type = "")
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<Payments> list = Dbase.GetAllPayments(UserId, Type).Select(a => a).ToList();

                return list;


            }
        }

        internal static PayPalAccountDetail GetPayPalAccountDetail(int Id)
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                PayPalAccountDetail model = Dbase.PayPalAccountDetails.SingleOrDefault(a => a.PayeeId == Id);

                return model;


            }
        }

        internal static List<PayPalAccountDetail> GetAllPayPalAccountDetail()
        {


            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<PayPalAccountDetail> model = Dbase.PayPalAccountDetails.Select(a => a).ToList();

                return model;

            }
        }


        internal static List<IntervalType> GetAllIntervalTypes()
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Lazy<List<IntervalType>> model = new Lazy<List<IntervalType>>(() => Dbase.IntervalTypes.Select(a => a).ToList());

                return model.Value;


            }
        }

        internal static Payee GetPayee(int PayeeId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Lazy<Payee> model = new Lazy<Payee>(() => Dbase.Payees.Where(a => a.PayeeId == PayeeId).Select(a => a).SingleOrDefault());

                return model.Value;


            }
        }

        internal static Payee GetPayeeByCustomerId(int CustomerId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Lazy<Payee> model = new Lazy<Payee>(() => Dbase.Customers.SingleOrDefault(a => a.CustomerId == CustomerId).Payee);

                return model.Value;


            }
        }

        internal static List<Payee> GetAllPayee(int UserId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                Lazy<List<Payee>> model = new Lazy<List<Payee>>(() => Dbase.Payees.Where(a => a.UserId == UserId).ToList());

                return model.Value;


            }
        }

        internal static int GetPayeeId(int Id)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                int payeeId = Dbase.Customers.SingleOrDefault(a => a.CustomerId == Id).PayeeId;
                return payeeId;



            }
        }

        internal static List<EmailedReport> GetEmailReports(int UserId, DateTime? from = null, DateTime? to = null)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<EmailedReport> lst = Dbase.GetAllEmailedReport(UserId, from, to).ToList();
                return lst;

            }
        }


        internal static List<EmailedReport> GetRemainderEmailReport(int UserId, DateTime? from = null, DateTime? to = null)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<EmailedReport> lst = Dbase.GetAllRemainderEmailReport(UserId, from, to).ToList();

                return lst;

            }
        }

        internal static List<EmailedReport> GetEmailSummaryReport(int UserId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                //List<EmailSummaryReport> list = null;

                List<EmailedReport> lst = Dbase.GetAllEmailedReport(UserId, null, null).OrderByDescending(x=>x.EmailSentDate).ToList();

                //list = lst.Select
                //    (a => new { b = a, date = a.EmailSentDate.Date })
                //    .GroupBy(a => a.date)
                //    .Select(a => new EmailSummaryReport { Date = a.FirstOrDefault().b.EmailSentDate, TotalAmount = a.Sum(b => b.b.TotalAmountDue),
                //        list = a.Select(b => b.b).ToList<EmailedReport>() }).ToList();
                return lst;

            }
        }

        internal static List<EmailedReport> GetRemainderEmailSummaryReport(int UserId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                //List<EmailSummaryReport> list = null;
                //int month = DateTime.Now.Month;
                List<EmailedReport> lst = Dbase.GetAllRemainderEmailReport(UserId, null, null).OrderByDescending(x=>x.EmailSentDate).ToList();

                //list = lst.Select(a => new { b = a, date = a.EmailSentDate.Date }).GroupBy(a => a.date).Select(a => new EmailSummaryReport { Date = a.FirstOrDefault().b.EmailSentDate, TotalAmount = a.Sum(b => b.b.TotalAmountDue), list = a.Select(b => b.b).ToList<EmailedReport>() }).ToList();
                return lst;

            }
        }

        internal static List<PaymentSummaryReport> GetPaymentsSummaryReport(int UserId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                List<PaymentSummaryReport> list = null;
                //int month = DateTime.Now.Month;
                List<Payments> lst = Dbase.GetAllPayments(UserId).Where(a => a.UserId == UserId).ToList();

                list = lst.Select(a => new { b = a, date = a.PaidDate.Value.Date }).GroupBy(a => a.date).Select(a => new PaymentSummaryReport { Date = a.FirstOrDefault().b.PaidDate.Value, TotalAmount = a.Sum(b => b.b.Amount), list = a.Select(c => c.b).ToList<Payments>() }).ToList();
                return list;


            }
        }

        internal static string GetCustomerPassword(string UserName)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                var obj = Dbase.UserSecurities
                         .Join(Dbase.UserProfiles, first => first.UserId, second => second.UserId, (first, second) => new { Password = first.Password, Username = second.UserName })
                         .Where(c => c.Username == UserName).SingleOrDefault();

                return obj.Password;

            }
        }

        internal static Plan GetPlan(int PlanId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                var obj = Dbase.Plans
                         .Where(c => c.PlanId == PlanId).SingleOrDefault();

                return obj;



            }
        }

        internal static List<Plan> GetAllPlans()
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<Plan> list = null;
                list = Dbase.Plans.Select(a => a).ToList();
                return list;

            }
        }

        internal static Subscription GetSubscription(int UserId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {

                var obj = Dbase.Subscriptions
                         .Where(c => c.UserId == UserId).SingleOrDefault();

                return obj;



            }
        }

        internal static List<Subscription> GetAllSubscriptions()
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<Subscription> list = null;
                list = Dbase.Subscriptions.Select(a => a).ToList();
                return list;

            }
        }

          internal static List<Group> GetAllGroups()
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<Group> list = null;
                list = Dbase.Groups.Select(g => g).ToList();
                
                return list;
            }
        }

        internal static List<Group> GetAllGroupsOfUser(int userId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<Group> list = null;
                list = Dbase.Groups.Where(g => g.UserId == userId && g.IsActive == true).OrderByDescending(x=>x.GroupName).ToList();

                return list;
            }
        }

        internal static List<GroupMember> GetAllGroupMembersByGroupId(int groupId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<GroupMember> list = null;
                list = Dbase.GroupMembers.Where(g => g.GroupId == groupId && g.IsActive == true).ToList();

                return list;
            }
        }

        internal static List<GroupMember> GetAllGroupMembersByUserId(int userId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                List<Group> list = null;
                list = Dbase.Groups.Where(g => g.UserId == userId).ToList();

                List<GroupMember> gmList = null;

                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        List<GroupMember> gms = Dbase.GroupMembers.Where(gm => gm.GroupId == item.GroupId && gm.IsActive == true).ToList();

                        if (gms != null && gms.Count > 0)
                        {
                            if (gmList == null)
                            {
                                gmList = new List<GroupMember>();
                            }

                            gmList.AddRange(gms);
                        }
                    } 
                }

                return gmList;
            }
        }

        internal static int GetGroupId(string groupName, int userId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                int groupId = 0;
                groupId = Dbase.Groups.Where(g => g.GroupName.ToLower() == groupName.Trim().ToLower() 
                                                    && g.UserId == userId
                                                    && g.IsActive == true)
                                        .Select(i => i.UserId).FirstOrDefault() ;

                return groupId;
            }
        }


        internal static Group GetGroupByGroupId(int groupId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                return Dbase.Groups.Where(g => g.GroupId == groupId && g.IsActive == true).FirstOrDefault();
            }
        }

        internal static string GetUserProfileName(int userId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                return Dbase.UserProfiles.Where(p => p.UserId == userId).Select(n => n.UserName).FirstOrDefault();
            }
        }

        internal static Customer GetCustomerByName(string firstpluslastname)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                return Dbase.Customers.Where(c =>
                                (c.CustomerFirstName + c.CustomerLastName) == firstpluslastname
                            ).FirstOrDefault();
            }
        }
        internal static List<CustomerDetail> GetCustomerDetailsByGroupIds(List<int> groupids, int userid)
        {
            List<CustomerDetail> customerDetails = new List<CustomerDetail>();
            using (BHDbaseEntities bHDbaseEntity = new BHDbaseEntities())
            {
                //List<CustomerDetail> customerDetails1 = new List<CustomerDetail>();
                foreach (int groupid in groupids)
                {
                    var customers = (from c in bHDbaseEntity.Customers
                                     join gm in bHDbaseEntity.GroupMembers on c.CustomerId equals gm.CustomerId
                                     where gm.GroupId == groupid
                                     select c).ToList();
                    

                    foreach (Customer list in customers)
                    {
                        var groupMem = bHDbaseEntity.GroupMembers.Where(x => x.GroupId == groupid && x.CustomerId ==list.CustomerId).FirstOrDefault();
                        var payee = (from c in bHDbaseEntity.Payees                                     
                                     where c.PayeeId == list.PayeeId
                                     select c).FirstOrDefault();
                        customerDetails.Add(new CustomerDetail()
                        {
                            CustomerId = list.CustomerId,
                            CustomerFirstName = list.CustomerFirstName,
                            CustomerLastName = list.CustomerLastName,
                            CustomerEmail = list.CustomerEmail,
                            PayeeId = list.PayeeId,
                            Payee = payee.Payee1,
                            PayeeEmail = payee.PayeeEmail,
                            AmountDue = groupMem.BillAmount>0? groupMem.BillAmount:list.AmountDue,
                            BillDescription = (!string.IsNullOrEmpty(groupMem.Billdescription) ? groupMem.Billdescription : list.BillDescription),
                            NextBillDate = (groupMem.Nextbilldate.HasValue ? Convert.ToDateTime(groupMem.Nextbilldate.Value.Date) : Convert.ToDateTime(list.NextBillDate.Date)),
                            CustomerAddress = list.CustomerAddress,
                            CustomerAddress2 = list.CustomerAddress2,
                            CustomerCity = list.CustomerCity,
                            CustomerCountry = list.CustomerCountry,
                            CustomerState = list.CustomerState,
                            CustomerZipCode = list.CustomerZipCode,
                            Intervals = list.Intervals,
                            IntervalTypeId = list.IntervalTypeId,
                            LateCharges = list.LateCharges,
                            LateChargesStartday = list.LateChargesStartday,
                            LateChargesStartdayAmount = list.LateChargesStartdayAmount,
                            LateChargesThereAfterday = list.LateChargesThereAfterday,
                            LateChargesThereAfterdayAmount = list.LateChargesThereAfterdayAmount,
                            UserId = userid,
                            CCEmail = list.CCEmail,
                            BCCEmail = list.BCCEmail,
                            CustomerPhoneNo = list.CustomerPhoneNo,
                            EmergencyPhoneNo = list.EmergencyPhoneNo
                        });
                    }

                }
            }
            return customerDetails;

        }


        internal static SendBillsModel GetCustomerDetailsForSendBillsByGroupIds(List<int> groupids, int userid)
        {
            var SendBillsModel = new SendBillsModel();
            var id = groupids.FirstOrDefault();
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                SendBillsModel.CustomerDetail = GetCustomerDetailsByGroupIds(groupids, userid);
                List<Group> grpList = GetAllGroupsOfUser(userid).ToList();

                int index = grpList.FindIndex(a => a.GroupId == id);

                SendBillsModel.Group = grpList.Where(p => p.GroupId == id).FirstOrDefault();
                if (index >= 1)
                {
                    SendBillsModel.PrevGroupId = grpList[index - 1].GroupId;
                }
                else
                {
                    SendBillsModel.PrevGroupId = 0;
                }
                if (index < grpList.Count - 1)
                {
                    index = index + 1;
                    SendBillsModel.NextGroupId = grpList[index].GroupId;
                }
                else
                {
                    SendBillsModel.NextGroupId = 0;
                }
            }
            return SendBillsModel;
        }
        internal static List<OnlineCheck> GetCustomerOnlineCheckDetail(string[] PaymentId)
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                int[] myInts = PaymentId.Select(int.Parse).ToArray();

                return Dbase.OnlineChecks.Where(x=>myInts.Any(p=>p ==x.PaymentId)).ToList();


            }
        }

        

        
    }

}