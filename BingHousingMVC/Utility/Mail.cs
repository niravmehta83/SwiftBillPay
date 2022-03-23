using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using System.Configuration;
using BingHousingMVC.Models;
using System.IO;
using BingHousing_BO;
using BingHousingMVC_DAL;

namespace BingHousingMVC.Utility
{
    public enum SupportMailType
    {
        Verification,
        ResetPassword,
        ResetPasswordCustomer
    }
    public interface IMembershipService
    {

        // Additions to Interface for EmailConfirmation...
        bool SendEmail(MailModel mailmodel);

        bool SendVerification(string confirmationtoken, string UserName, SupportMailType Subject);


    }
    public class AccountMembershipService : IMembershipService
    {

        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {

        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;

        }


        public bool SendVerification(string confirmationtoken, string UserName, SupportMailType Subject)
        {
            bool success = false;
            IBHDbase dbase = new BHDbase();
            MailModel mailmodel = new MailModel();
            string verifyUrl = "";
            mailmodel.From = ConfigurationManager.AppSettings["EmailFrom"];
            mailmodel.To = dbase.GetUserEmail(UserName);
            mailmodel.Type = MailType.EmailVerification;

            if (!string.IsNullOrEmpty(mailmodel.To))
            {
                switch (Subject)
                {
                    case SupportMailType.Verification:

                        mailmodel.Sub = " Verfication Email ";

                        verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Verify?ID=" + confirmationtoken;

                        mailmodel.Message = "Click the following link or Copy and paste the link in your browser address bar to validate your Account " + verifyUrl;

                        break;

                    case SupportMailType.ResetPassword:

                        mailmodel.Sub = " Reset Password ";

                        verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/ResetPassword?ID=" + confirmationtoken + '@' + UserName;

                        mailmodel.Message = "Click the following link or Copy and paste the link in your browser address bar to Reset your Password " + verifyUrl;

                        break;
                    case SupportMailType.ResetPasswordCustomer:

                        mailmodel.Sub = "Password ";

                        mailmodel.Message = "Your Password : " + confirmationtoken;

                        break;

                }


                success = SendEmail(mailmodel);
            }
            

            return success;
        }


        public bool SendEmail(MailModel mailmodel)
        {

            SmtpClient mailClient = new SmtpClient();
            //string userName = null, password = null, 
            string Mailbody = null;
            //string Host;
            //int port;
            var templatePath = "";
            string[] arry = null;
            string eMailProvider = ConfigurationManager.AppSettings["eMailProvider"];
            StreamReader filereader = null;
            System.Net.Mail.Attachment attachment = null ;
            if (!string.IsNullOrEmpty(mailmodel.Attachment))
            {
                attachment = new System.Net.Mail.Attachment(mailmodel.Attachment);
            }
            switch (mailmodel.Type)
            {
                case MailType.EmailPaymentUser:
                case MailType.EmailPamentCustomer:
                    PaymentMailModel pmodel = (PaymentMailModel)mailmodel;
                   
                    if (pmodel.Type == MailType.EmailPaymentUser)
                    {                        
                        templatePath = ConfigurationManager.AppSettings["EmailPaymentUserTemplate"];
                    }
                    else
                    {
                        templatePath = ConfigurationManager.AppSettings["EmailPaymentCustomerTemplate"];
                    }
                    templatePath = HttpContext.Current.Server.MapPath(templatePath);
                    filereader = new StreamReader(templatePath);
                    Mailbody = filereader.ReadToEnd();
                    filereader.Close();
                    Mailbody = string.Format(Mailbody,pmodel.Name,pmodel.PaymentDate,pmodel.Sub,pmodel.To,pmodel.OrderId,pmodel.Amount,pmodel.ShippingCost,pmodel.ShippingMethod,pmodel.Tax,pmodel.PaymentType,pmodel.Phone,pmodel.Address1,pmodel.Address2,pmodel.Country,pmodel.ProjectNumber,pmodel.BillDescription,pmodel.BillingDate,pmodel.BillingId,pmodel.From);

                    break;

                case MailType.EmailInvoice:

                    InvoiceMailModel model = (InvoiceMailModel)mailmodel;
                    if (!string.IsNullOrEmpty(model.Attachment)) { attachment = new System.Net.Mail.Attachment(model.Attachment); }
                    
                    arry = mailmodel.From.Split(',').ToArray();
                    string verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Cart/CartLogin?Payee=" + arry[1];
                    templatePath = ConfigurationManager.AppSettings["EmailTemplate"];
                    templatePath = HttpContext.Current.Server.MapPath(templatePath);
                    var reminderHeading = "";
                    if (model.IsReminder)
                    {
                        reminderHeading = "Reminder Rent Bill";
                    }
                    filereader = new StreamReader(templatePath);
                    Mailbody = filereader.ReadToEnd();
                    filereader.Close();
                    Mailbody = string.Format(Mailbody, arry[0], model.Name, model.Sub, model.Amount, model.ProjectNumber, model.To, model.Message, model.Note, verifyUrl,model.BillId,model.Payee, reminderHeading);

                    break;

                case MailType.EmailVerification:

                    templatePath = ConfigurationManager.AppSettings["EmailVerificationTemplate"];
                    templatePath = HttpContext.Current.Server.MapPath(templatePath);

                    filereader = new StreamReader(templatePath);
                    Mailbody = filereader.ReadToEnd();
                    filereader.Close();
                    Mailbody = string.Format(Mailbody, mailmodel.From, mailmodel.To, mailmodel.Sub, mailmodel.Message);

                    break;

            }




            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.IsBodyHtml = true;
            msg.To.Add(new MailAddress(mailmodel.To));
            if (!string.IsNullOrEmpty(mailmodel.Payee))
            {
                msg.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "support@" + mailmodel.Payee); //(new MailAddress(arry != null ? arry[0] : mailmodel.From));//changed by Raja
            }
            else
            {
                msg.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"]); //(new MailAddress(arry != null ? arry[0] : mailmodel.From));//changed by Raja
            }
            msg.Subject = mailmodel.Sub.Replace("\n","");
            msg.Body = Mailbody;
            //if ((mailmodel.Type == MailType.EmailPaymentUser || mailmodel.Type == MailType.EmailInvoice) && attachment != null)
            if (attachment != null)
            {               
                    msg.Attachments.Add(attachment);               
            }


            try
            {
               
                mailClient.Timeout = 100000;
                mailClient.Send(msg);
                mailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}