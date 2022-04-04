using BingHousing_BO;
using BingHousingMVC.GlobalOperations;
using BingHousingMVC.Models;
using BingHousingMVC.Utility;
using BingHousingMVC_DAL;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace BingHousingMVC.Controllers
{
    [Route("api/[controller]")]
    public class StripeWebHookController : Controller
    {
        private IBHDbase dbase = new BHDbase();

        [System.Web.Mvc.HttpPost]
        public async Task<int> Index()
        {

            var json = await new StreamReader(HttpContext.Request.InputStream).ReadToEndAsync();

            const string endpointSecret = "whsec_9ff6a118fca2d9a6585eb2a84b4dadee0e40ee6ba431f420fce5097ccec2e6cf";
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);

                if (stripeEvent.Type == Events.ChargeSucceeded)
                {
                    var charge = stripeEvent.Data.Object as Charge;

                    try
                    {
                        ChargeDetail chargeDetail = dbase.GetAllChargeDetails(charge.Id);
                        StripeChargeModel model = new StripeChargeModel();
                        model.UserId = chargeDetail.UserId;
                        if (chargeDetail != null)
                        {
                            CustomerProfile customerProfile = dbase.GetCustomerProfile(chargeDetail.UserId);
                            List<InvoiceDetail> item = dbase.GetInvoiceDetails(charge.InvoiceId, InvoiceType.All);
                            Tuple<CheckOutModel, StripeChargeModel> tuple = new Tuple<CheckOutModel, StripeChargeModel>(this.dbase.GetCustomerProfile(model.UserId).ToCheckOutRegisterModel(), model);
                            List<int> list = (
                                              from a in item
                                              select a.InvoiceId).ToList<int>();
                            int num = dbase.InsertACHDepositPaymentDetail(chargeDetail, list);
                            object month = DateTime.Now.Month;
                            DateTime now = DateTime.Now;
                            OnlineCheck onlineCheck = new OnlineCheck()
                            {
                                CustomerName = string.Concat(tuple.Item1.FirstName, " ", tuple.Item1.LastName),
                                PaymentId = new int?(num),
                                PhoneNumber = tuple.Item1.PhoneNumber,
                                Email = tuple.Item1.Email,
                                AmountOnCheck = new decimal?(Convert.ToDecimal(tuple.Item2.Amount)),
                                Comment = tuple.Item2.Comment,
                                PayeeName = tuple.Item2.Payee,
                            };
                            onlineCheck.Comment = tuple.Item2.Comment;
                            PaymentMailModel paymentMailModel = new PaymentMailModel()
                            {
                                BillDescription = item[0].BillDescription,
                                BillingDate = item[0].EmailSentDate,
                                ShippingCost = Convert.ToDecimal(0),
                                Tax = Convert.ToDecimal(0),
                                ShippingMethod = "Normal Post",
                                PaymentType = "ACH Deposit",
                                Address1 = tuple.Item1.Address,
                                Address2 = string.Concat(new string[] { tuple.Item1.City, " ", tuple.Item1.State, " ", tuple.Item1.ZipCode }),
                                Country = tuple.Item1.Country,
                                Phone = tuple.Item1.PhoneNumber,
                                PaymentDate = DateTime.Now.Date,
                                OrderId = num,
                                BillingId = string.Join(",", (
                                    from a in list
                                    select a.ToString()).ToArray<string>())
                            };
                            decimal num1 = item.Sum<InvoiceDetail>((InvoiceDetail a) => a.TotalAmountDue) + item.Sum<InvoiceDetail>((InvoiceDetail a) => a.LateCharges);
                            paymentMailModel.Amount = num1.ToString();
                            paymentMailModel.From = item[0].PayeeEmail;
                            paymentMailModel.To = tuple.Item1.Email;
                            paymentMailModel.Sub = string.Concat("Thanks for the Payment of Order Number ", num.ToString());
                            paymentMailModel.Name = string.Concat(tuple.Item1.FirstName, " ", tuple.Item1.LastName);
                            paymentMailModel.Type = MailType.EmailPamentCustomer;
                            paymentMailModel.ProjectNumber = item[0].InvoiceNumber;
                            paymentMailModel.Payee = item[0].Payee;
                            AccountMembershipService accountMembershipService = new AccountMembershipService();
                            ((IMembershipService)accountMembershipService).SendEmail(paymentMailModel);
                            paymentMailModel.From = tuple.Item1.Email;
                            paymentMailModel.To = item[0].PayeeEmail;
                            paymentMailModel.Sub = string.Concat("Check import text file for Order# ", num.ToString());
                            paymentMailModel.Type = MailType.EmailPaymentUser;
                            ((IMembershipService)accountMembershipService).SendEmail(paymentMailModel);
                            paymentMailModel.To = dbase.GetUserEmail(item[0].UserId);
                            ((IMembershipService)accountMembershipService).SendEmail(paymentMailModel);
                            paymentMailModel.Address2 = string.Concat(new string[] { tuple.Item1.City, "$", tuple.Item1.State, "$", tuple.Item1.ZipCode });
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                    Console.WriteLine("A successful payment for {0} was made.", charge.Amount);
                }
                else if (stripeEvent.Type == Events.ChargeFailed)
                {
                    var charge = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("A Failed payment for {0} was made.", charge.Amount);
                }
                else if (stripeEvent.Type == Events.AccountUpdated)
                {
                    try
                    {
                        var bankAccount = stripeEvent.Data.Object as BankAccount;
                        UserACHBankAccount userACHBankAccount = dbase.GetUserACHBankAccount(bankAccount.CustomerId);
                        userACHBankAccount.AccountStatus = bankAccount.Status;
                        dbase.updateACHRegistration(userACHBankAccount);
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                    }
                }
                return 0;
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return 0;
            }
            catch (Exception e)
            {
                return 1;
            }
        }
    }
}
