using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousing_BO;

namespace BingHousingMVC_DAL
{
    public class BHDbase : IBHDbase
    {

        #region Get

        public string GetCustomerEmail(string projectnumber)
        {
            return GetOperations.GetCustomerEmail(projectnumber);
        }

        public string GetPassword2(string UserName)
        {
            return GetOperations.GetPassword(UserName);

        }

        public UserACHBankAccount GetCustomerStripeProfile(int UserId)
        {
            return GetOperations.GetCustomerStripeProfile(UserId);

        }
        public int GetUserId(string UserName)
        {

            return GetOperations.GetUserId(UserName);

        }

        public Customer GetCustomer(int CustomerId)
        {
            return GetOperations.GetCustomer(CustomerId);

        }
        public int InsertACHDepositPaymentDetail(ChargeDetail model, List<int> invoiceIdlist)
        {

            return InsertOperations.InsertACHDepositPaymentDetail(model, invoiceIdlist);

        }
        public CustomerDetail GetCustomerDetails(int CustomerId, int GroupId=0)
        {
                return GetOperations.GetCustomerDetail(CustomerId,GroupId);
                    }

        public List<CustomerDetail> GetCustomerList(int UserId)
        {
                return GetOperations.GetCustomerDetailList(UserId);
           
        }

        public List<CustomerDetail> GetCustomerList()
        {
                return GetOperations.GetCustomerDetailList();
            
        }

        public string GetUserEmail(string UserName)
        {
                return GetOperations.GetUserEmail(UserName);
            
        }

        public string GetUserEmail(int UserId)
        {
                return GetOperations.GetUserEmail("", UserId);
           
        }

        public List<string> GetallUserName(bool isUseradmin)
        {
                return GetOperations.GetAllUserName(isUseradmin);
           
        }

        public List<string> GetallUserName()
        {
                return GetOperations.GetAllUserName();
           
        }
        public bool updateACHRegistration(UserACHBankAccount model)
        {
            return UpdateOperations.updateACHRegistration(model);
        }

        public UserACHBankAccount GetUserACHBankAccount(string CustomerId)
        {
            return GetOperations.GetUserACHBankAccount(CustomerId);
        }

        public ChargeDetail GetAllChargeDetails(string ChargeResourceId)
        {
            return GetOperations.GetAllChargeDetails(ChargeResourceId);
        }
        public string GetConfirmationToken(string UserName)
        {
                return GetOperations.GetConfirmationToken(UserName);
           
        }

        public UserDetail GetUserDetails(string UserName)
        {
                return GetOperations.GetUserDetail(UserName);
            
        }

        public List<AccountUserModel> GetAllActiveUser()
        {
                return GetOperations.GetAllActiveUsers();
            
        }

        public List<AccountUserModel> GetAllArchiveUser()
        {
                return GetOperations.GetAllArchivedUsers();
            
        }


        public List<InvoiceDetail> GetInvoiceDetails(string InvoiceNumber, InvoiceType type)
        {
                return GetOperations.GetInvoiceDetails(InvoiceNumber, type);
           
        }

        public List<InvoiceDetail> GetInvoiceDetails(int Id, IdType idtype, InvoiceType invoiceType = InvoiceType.All,string type="",string search="")
        {
                return GetOperations.GetInvoiceDetails("", invoiceType, Id, idtype,type,search);
            
        }

        public CustomerProfile GetCustomerProfile(int UserId)
        {
                return GetOperations.GetCustomerProfile(UserId);
           
        }


        public CheckDetail GetCheckDetail(int Id, bool? isuserid = null)
        {
                if (isuserid == null)
                {
                    return GetOperations.GetCheckDetail(Id, false);
                }
                else
                {
                    return GetOperations.GetCheckDetail(Id, true);
                }
            
        }

        public CheckDetail GetCheckDetailByPaymentId(int PaymentId)
        {

            return GetOperations.GetCheckDetailByPaymentId(PaymentId);

        }

        public List<CheckDetail> GetAllCheckDetail()
        {
                return GetOperations.GetAllCheckDetail();
            
        }


        public List<Payments> GetAllPayments(int Userid, string Type = "")
        {

            return GetOperations.GetAllPayments(Userid, Type);

        }


        public PayPalAccountDetail GetPayPalAccountDetail(int CustomerId)
        {
            
                return GetOperations.GetPayPalAccountDetail(CustomerId);
           
        }

        public List<PayPalAccountDetail> GetAllPayPalAccountDetails()
        {
            
                return GetOperations.GetAllPayPalAccountDetail();
           
        }

        public List<IntervalType> GetAllIntervalTypes()
        {
             
                return GetOperations.GetAllIntervalTypes();
             
        }

        public Payee GetPayee(int PayeeId)
        {
             
                return GetOperations.GetPayee(PayeeId);
             
        }

        public Payee GetPayeeByCustomerId(int CustomerId)
        {
             
                return GetOperations.GetPayeeByCustomerId(CustomerId);
             
        }

        public List<Payee> GetAllPayee(int UserId)
        {
             
                return GetOperations.GetAllPayee(UserId);
             
        }

        public int GetPayeeId(int customerId)
        {
             
                return GetOperations.GetPayeeId(customerId);
             
        }

        public List<EmailedReport> GetEmailReport(int UserId)
        {
             
                return GetOperations.GetEmailReports(UserId);
            
        }

        public List<EmailedReport> GetEmailReport(int UserId, DateTime from, DateTime to)
        {
             
                return GetOperations.GetEmailReports(UserId, from, to);
             

        }

        public List<EmailedReport> GetRemainderEmailReport(int UserId)
        {
            
                return GetOperations.GetRemainderEmailReport(UserId, null, null);
             
        }

        public List<EmailedReport> GetRemainderEmailReport(int UserId, DateTime from, DateTime to)
        {
             
                return GetOperations.GetRemainderEmailReport(UserId, from, to);
             
        }


        public List<PaymentSummaryReport> GetPaymentsSummaryReport(int UserId)
        {
             
                return GetOperations.GetPaymentsSummaryReport(UserId);
            
        }

        public List<EmailedReport> GetRemainderEmailSummaryReport(int UserId)
        {
            
                return GetOperations.GetRemainderEmailSummaryReport(UserId);
            
        }

        public List<EmailedReport> GetEmailSummaryReport(int UserId)
        {
            
                return GetOperations.GetEmailSummaryReport(UserId);
            
        }


        public string GetCustomerPassword(string Username)
        {
            
                return GetOperations.GetCustomerPassword(Username);
            
        }


        public Plan GetPlan(int PlanId)
        {
             
                return GetOperations.GetPlan(PlanId);
             
        }

        public List<Plan> GetAllPlans()
        {
            
                return GetOperations.GetAllPlans();
            
        }

        public Subscription GetSubscription(int UserId)
        {
            
                return GetOperations.GetSubscription(UserId);
            
        }

        public List<Subscription> GetAllSubscriptions()
        {
             
                return GetOperations.GetAllSubscriptions();
            
        }



        public List<Group> GetAllGroups()
        {
            return GetOperations.GetAllGroups();
        }

        public List<Group> GetAllGroupsOfUser(int userId)
        {
            return GetOperations.GetAllGroupsOfUser(userId);
        }

        public List<GroupMember> GetAllGroupMembersByGroupId(int groupId)
        {
            return GetOperations.GetAllGroupMembersByGroupId(groupId);
        }

        public List<GroupMember> GetAllGroupMembersByUserId(int userId)
        {
            return GetOperations.GetAllGroupMembersByUserId(userId);
        }

        public int GetGroupId(string groupName, int userId)
        {
            return GetOperations.GetGroupId(groupName, userId);
        }

        public Group GetGroupByGroupId(int groupId)
        {
            return GetOperations.GetGroupByGroupId(groupId);
        }

        public string GetUserProfileName(int userId)
        {
            return GetOperations.GetUserProfileName(userId);
        }

        public Customer GetCustomerByName(string firstpluslastname)
        {
            return GetOperations.GetCustomerByName(firstpluslastname);
        }

        public List<CustomerDetail> GetCustomerDetailsByGroupIds(List<int> groupids, int userid)
        {
            return GetOperations.GetCustomerDetailsByGroupIds(groupids, userid);
        }

        public SendBillsModel GetCustomerDetailsForSendBillsByGroupIds(List<int> groupids, int userid)
        {
            return GetOperations.GetCustomerDetailsForSendBillsByGroupIds(groupids, userid);
        }

        public List<OnlineCheck> GetCustomerOnlineCheckDetail(string[] PaymentId)
        {
            
                return GetOperations.GetCustomerOnlineCheckDetail(PaymentId);
            
        }

        
        #endregion

        #region Insert

        public void InsertPassword2(RegisterData model)
        {
            
                InsertOperations.InsertPassword(model);
             

        }

        public string InsertACHRegistration(UserACHBankAccount model)
        {

            return InsertOperations.InsertACHRegistration(model);

        }
        public void InsertUserDetails(UserDetail model)
        {
               InsertOperations.InsertUserDetails(model);
             
        }


        public void InsertCustomer(Customer model)
        {
            
                InsertOperations.InsertCustomerDetails(model);
             
        }


        public int InsertInvoice(Invoice model)
        {
             
                return InsertOperations.InsertInvoice(model);
            
             
        }


        public void InsertCustomerProfile(CustomerProfile model)
        {
             
                InsertOperations.InsertCustomerProfile(model);
             
        }


        public int InsertCheckPaymentDetail(CheckDetail model, List<int> invoiceIdlist, int PaymentModeId)
        {
            
                return InsertOperations.InsertCheckPaymentDetail(model, invoiceIdlist, PaymentModeId);
             
        }


        public int InsertPayPalPaymentDetail(PayPalDetail model, List<int> invoiceIdlist)
        {
            
                return InsertOperations.InsertPaypalPaymentDetail(model, invoiceIdlist);
           
        }


        public int InsertPaypalAccountDetail(PayPalAccountDetail model)
        {
            
                return InsertOperations.InsertPaypalAccountDetail(model);
            
        }

        public int InsertPayee(Payee model)
        {
            
                return InsertOperations.InsertPayee(model);
            
        }


        public int InsertRemainderEmail(RemainderEmail model)
        {

            return InsertOperations.InsertRemainderEmail(model);

        }

        public void InsertSubscription(Subscription model)
        {
            InsertOperations.InsertSubscription(model);
        }

        public void InsertPlan(Plan model)
        {
            InsertOperations.InsertPlan(model);
        }


        public void InsertGroup(Group group)
        {
            InsertOperations.InsertGroup(group);
        }

        public void InsertGroupMember(GroupMember groupMember)
        {
            InsertOperations.InsertGroupMember(groupMember);
        }

        public int InsertCheckOnline(OnlineCheck model)
        {
            return InsertOperations.InsertCheckOnline(model);
        }

        #endregion

        #region Update

        public void UpdateUserDetails(UserDetail model)
        {
             
                UpdateOperations.UpdateUserDetails(model);
           
        }
        public void UpdateCustomerProfile(CustomerProfile model)
        {

            UpdateOperations.UpdateCustomerProfileDetails(model);

        }
        public void UpdateCustomer(Customer model)
        {
            
                UpdateOperations.UpdateCustomerDetails(model);
            
        }

        public void ChangePassword2(string UserName, string Password, string Password2)
        {
            
                UpdateOperations.changePassword2(UserName, Password, Password2);
             
        }

        public void UpdatePapPalAcountDetails(PayPalAccountDetail model)
        {
           
                UpdateOperations.UpdatePapPalAcountDetails(model);
             
        }

        public void UpdatePayee(Payee model)
        {
            
                UpdateOperations.UpdatePayeeDetails(model);
          
        }

        public void UpdatePlan(Plan model)
        {
            UpdateOperations.UpdatePlan(model);
        }

        public void UpdateSubcription(Subscription model)
        {
            UpdateOperations.UpdateSubcription(model);
        }


        public void UpdateGroup(Group group)
        {
            UpdateOperations.UpdateGroup(group);
        }

        public void UpdateBillDesc(string[] CustomerIds, string BillDesc, int Groupid)
        {
            UpdateOperations.UpdateBillDesc(CustomerIds, BillDesc, Groupid );
        }

        
        public void UpdateGroupMember(GroupMember groupMember)
        {
            UpdateOperations.UpdateGroupMember(groupMember);
        }

        public void UpdateBillDescription(List<CustomerDetail> details, int groupId)
        {
            UpdateOperations.UpdateBillDescription(details, groupId);
        }
        public bool UpdateOnlineCheck(OnlineCheck onlineCheck)
        {

            return UpdateOperations.UpdateOnlineCheck(onlineCheck);

        }
        #endregion

        #region Delete

        public void DeleteCustomer(int CustomerId)
        {
             
                DeleteOperations.DeleteCustomerDetails(CustomerId);
             
        }

        public void DeletePayee(int PayeeId)
        {
               DeleteOperations.DeletePayee(PayeeId);
             
        }

        public void DeletePayPalAccountDetails(int Id)
        {
             
                DeleteOperations.DeletePayPalAccountDetails(Id);
             
        }

        public void DeletePlan(int Id)
        {
            DeleteOperations.DeletePlan(Id);
        }

        public void DeleteSubcription(int Id)
        {
            DeleteOperations.DeleteSubcription(Id);
        }

        public void DeleteGroup(int groupId)
        {
            DeleteOperations.DeleteGroup(groupId);
        }

        public void DeleteGroupMember(int groupMemberId,bool IscustomerId)
        {
            DeleteOperations.DeleteGroupMember(groupMemberId, IscustomerId);
        }



        public void DeleteInvoice(int invoiceId)
        {
           DeleteOperations.DeleteInvoice(invoiceId);
        }
        #endregion

        #region Lock operations

        public void LockUser(int UserId)
        {
            
                LockOperations.LockUser(UserId);
            

        }

        public void LockUser(string UserName)
        {
             
                LockOperations.LockUser(0, UserName);
            
        }

        public void UnLockUser(int UserId)
        {
             
                LockOperations.UnLockUser(UserId);
             
        }

        public void UnLockUser(string UserName)
        {
            
                LockOperations.UnLockUser(0, UserName);
             
        }

        public bool IsUserLocked(int UserId)
        {
             
                return LockOperations.IsUserLocked(UserId);
             
        }

        public bool IsUserLocked(string UserName)
        {
             
                return LockOperations.IsUserLocked(0, UserName);
             
        }

        #endregion

        public int InsertACHAccountDepositDetail(ACHAccountDepositDetail model)
        {
            return InsertOperations.InsertACHDepositAccountDetail(model);

        }

        public void UpdateACHDepositAcountDetails(ACHAccountDepositDetail model)
        {
            UpdateOperations.UpdateACHDepositAcountDetails(model);
        }
        public List<ACHAccountDepositDetail> GetAllACHDepositAccountDetails()
        {
            throw new NotImplementedException();
        }

        public ACHAccountDepositDetail GetACHDepositAccountDetail(int CustomerId)
        {
            return GetOperations.GetACHDepositAccountDetail(CustomerId);

        }

       
        public void DeleteACHDepositAccountDetails(int Id)
        {
            DeleteOperations.DeleteACHDepositDetails(Id);

        }


    }
}