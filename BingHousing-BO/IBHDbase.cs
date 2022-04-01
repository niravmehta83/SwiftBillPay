using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BingHousing_BO
{
    public interface IBHDbase
    {

        #region Lock operations

        void LockUser(int UserId);

        void LockUser(string UserName);

        void UnLockUser(int UserId);
        
        void UnLockUser(string UserName);

        bool IsUserLocked(int UserId);

        bool IsUserLocked(string UserName);


        #endregion

        #region Insert

        void InsertPassword2(RegisterData model);

        void InsertUserDetails(UserDetail model);

        void InsertCustomer(Customer model);

        int InsertInvoice(Invoice model);

        void InsertCustomerProfile(CustomerProfile model);

        int InsertCheckPaymentDetail(CheckDetail model,List<int>invoiceIdlist, int PaymentModeId);

        int InsertPayPalPaymentDetail(PayPalDetail model, List<int> invoiceIdlist);

        int InsertPaypalAccountDetail(PayPalAccountDetail model);

        int InsertPayee(Payee model);

        int InsertRemainderEmail(RemainderEmail model);

        void InsertSubscription(Subscription model);

        void InsertPlan(Plan model);

        void InsertGroup(Group group);

        void InsertGroupMember(GroupMember groupMember);

        int InsertCheckOnline(OnlineCheck model);

        int InsertACHAccountDepositDetail(ACHAccountDepositDetail model);


        #endregion


        #region Update

        void UpdateCustomer(Customer model);

        void UpdateUserDetails(UserDetail model);

        void ChangePassword2(string UserName, string Password, string Password2);

        void UpdatePapPalAcountDetails(PayPalAccountDetail model);

        void UpdatePayee(Payee model);

        void UpdatePlan(Plan model);
        void UpdateACHDepositAcountDetails(ACHAccountDepositDetail model);

        void UpdateSubcription(Subscription model);

        void UpdateGroup(Group group);
        void UpdateBillDesc(string[] CustomerIds, string BillDesc,int Groupid);

        void UpdateGroupMember(GroupMember groupMember);

        void UpdateBillDescription(List<CustomerDetail> details, int GroupId);

        bool UpdateOnlineCheck(OnlineCheck OnlineCheeck);

        #endregion


        #region Delete

        void DeleteCustomer(int CustomerId);

        void DeletePayee(int Id);

        void DeletePayPalAccountDetails(int Id);

        void DeletePlan(int Id);

        void DeleteSubcription(int Id);

        void DeleteGroup(int groupId);
        void DeleteACHDepositAccountDetails(int Id);


        void DeleteInvoice(int invoiceId);
        void DeleteGroupMember(int groupMemberId, bool IsCustomerId);

        #endregion


        #region Get

        string GetPassword2(string UserName);

        string GetUserEmail(string UserName);

        string GetUserEmail(int UserId);

        string GetConfirmationToken(string UserName);

        string GetCustomerEmail(string projectnumber);

        List<string> GetallUserName();

        List<ACHAccountDepositDetail> GetAllACHDepositAccountDetails();


        List<string> GetallUserName(bool isAdmin);

        List<AccountUserModel> GetAllActiveUser();

        List<AccountUserModel> GetAllArchiveUser();

        int GetUserId(string UserName);

        UserDetail GetUserDetails(string UserName);

        CustomerDetail GetCustomerDetails(int CustomerId,int GroupId=0);

        Customer GetCustomer(int CustomerId);

        List<CustomerDetail> GetCustomerList(int UserId);

        ACHAccountDepositDetail GetACHDepositAccountDetail(int UserId);


        List<CustomerDetail> GetCustomerList();

        List<InvoiceDetail> GetInvoiceDetails(string InvoiceNumber,InvoiceType type);

        List<InvoiceDetail> GetInvoiceDetails(int Id, IdType idtype, InvoiceType invoiceType = InvoiceType.All, string type = "", string search = "");

        CustomerProfile GetCustomerProfile(int UserId);

        CheckDetail GetCheckDetail(int Id,bool? isuserid=null);

        List<CheckDetail> GetAllCheckDetail();

        List<Payments> GetAllPayments(int UserId, string Type = "");

        List<PayPalAccountDetail> GetAllPayPalAccountDetails();

        PayPalAccountDetail GetPayPalAccountDetail(int UserId);

        List<IntervalType> GetAllIntervalTypes();

        Payee GetPayee(int PayeeId);

        Payee GetPayeeByCustomerId(int CustomerId);

        List<Payee> GetAllPayee(int UserId);

        int GetPayeeId(int customerId);

        List<EmailedReport> GetEmailReport(int UserId);

        List<EmailedReport> GetEmailReport(int UserId, DateTime from, DateTime to);

        List<EmailedReport> GetRemainderEmailReport(int UserId);

        List<EmailedReport> GetRemainderEmailReport(int UserId, DateTime from, DateTime to);

        List<PaymentSummaryReport> GetPaymentsSummaryReport(int UserId);

        List<EmailedReport> GetRemainderEmailSummaryReport(int UserId);

        List<EmailedReport> GetEmailSummaryReport(int UserId);

        string GetCustomerPassword(string UserName);

        Plan GetPlan(int PlanId);

        List<Plan> GetAllPlans();

        Subscription GetSubscription(int UserId);

        List<Subscription> GetAllSubscriptions();

        Group GetGroupByGroupId(int groupId);

        List<Group> GetAllGroups();

        List<Group> GetAllGroupsOfUser(int userId);

        List<GroupMember> GetAllGroupMembersByGroupId(int groupId);

        List<GroupMember> GetAllGroupMembersByUserId(int userId);

        int GetGroupId(string groupName, int userId);

        string GetUserProfileName(int userId);

        Customer GetCustomerByName(string firstpluslastname);

        List<CustomerDetail> GetCustomerDetailsByGroupIds(List<int> groupids, int userid);

        SendBillsModel GetCustomerDetailsForSendBillsByGroupIds(List<int> groupids, int userid);        

        List<OnlineCheck> GetCustomerOnlineCheckDetail(string[] PaymentId);

        CheckDetail GetCheckDetailByPaymentId(int PaymentId);
        #endregion


    }
}