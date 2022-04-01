using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BingHousingMVC.Models;
using BingHousingMVC.Filters;
using WebMatrix.WebData;
using System.Web.Security;
using BingHousingMVC.GlobalOperations;
using System.Web.Routing;
using System.Configuration;
using System.IO;
using BingHousingMVC.Utility;
using BingHousing_BO;
using BingHousingMVC_DAL;
using System.Web.Configuration;
using PayPal.Api.Payments;
using PayPal;
using BingHousing_PAYPAL;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BingHousingMVC.Controllers
{
    //[InitializeSimpleMembership]
    public class CartController : Controller
    {
        public IMembershipService MembershipService { get; set; }
        private IBHDbase dbase = new BHDbase();
        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
            { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        # region Actions


        //get method for cart Login
        public ActionResult CartLogin(string Payee)
        {
            if (Payee == null)
            {
                Session["Payee"] = "Smart Rent Pay";
            }
            else
            {
                
                CustomerDetail detail = dbase.GetCustomerDetails(Convert.ToInt32(Payee));
                Session["Payee"] = detail.Payee;
            }
            return View();
        }


        //Post method for cart Login
        [HttpPost]
        public ActionResult CartLogin(CartLoginModel model)
        {
            if (ValidateCartlogin(model))
            {

                return RedirectToAction("Cart", new { projectNumber = model.ProjectNumber });
            }
            else { ModelState.AddModelError("", "Invalid Credentials or Cannot Find the Project"); }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {

            WebSecurity.Logout();

            return RedirectToAction("CartLogin", "Cart");
        }

        //get method for cart Login
        public ActionResult Cart(string projectNumber = "")
        {
            List<InvoiceDetail> list = null;
            if (projectNumber != "")
            {
                
                list = dbase.GetInvoiceDetails(projectNumber, InvoiceType.Notpaid);
                list = LateChargesHelper.FindLateCharges(list);
                Session["cartitems"] = list;
                string Title = Session["Payee"] != null ? Session["Payee"] as string : "Smart Rent Pay";
                if (Title == "Smart Rent Pay" & list.Count > 0)
                {
                    CustomerDetail detail = dbase.GetCustomerDetails(list[0].CustomerId);
                    Session["Payee"] = detail.Payee;
                }
            }
            else
            {
                list = (List<InvoiceDetail>)Session["cartitems"];


            }
            ViewBag.Results = list;
            ViewBag.Count = list.Count;
            ViewBag.checkoutAmount = list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges);
            return View();
        }

        //get method for cart Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CartRecalculate(string invoicelist)
        {
            if (invoicelist != "")
            {
                string[] arry = invoicelist.Split(',').ToArray();
                List<InvoiceDetail> list = null;

                list = (List<InvoiceDetail>)Session["cartitems"];

                foreach (string s in arry)
                {
                    string[] cus = s.Split('-').ToArray();

                    list.Where(a => a.InvoiceId == Convert.ToInt32(cus[0])).Select(a => a).FirstOrDefault().Quantity = Convert.ToInt32(cus[1]);
                    decimal amount = Convert.ToInt32(list.Where(a => a.InvoiceId == Convert.ToInt32(cus[0])).Select(a => a).FirstOrDefault().AmountDue);
                    list.Where(a => a.InvoiceId == Convert.ToInt32(cus[0])).Select(a => a).FirstOrDefault().TotalAmountDue = Convert.ToInt32(cus[1]) * amount;

                }

                ViewBag.Results = list;
                ViewBag.Count = list.Count;
                ViewBag.checkoutAmount = list.Sum(a => a.TotalAmountDue);
                Session["cartitems"] = list;
            }
            return RedirectToAction("Cart"); ;
        }

        //get method for cart Login
        public ActionResult RemoveCartItems(int invoiceId)
        {
           
            

            try
            {
                dbase.DeleteInvoice(invoiceId);
                List<InvoiceDetail> list = (List<InvoiceDetail>)Session["cartitems"];
                Session["cartitems"] = list.Select(a => a).Where(a => a.InvoiceId != invoiceId).ToList<InvoiceDetail>();
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Cart");
        }

        //get method for checkout register
        public ActionResult CheckOutRegister()
        {
            
            PayPalAccountDetail pad = null;
            if (Session["cartitems"] != null)
            {
                Lazy<List<InvoiceDetail>> list = new Lazy<List<InvoiceDetail>>(() => (List<InvoiceDetail>)Session["cartitems"]);
                int payeeId = dbase.GetPayeeId(list.Value[0].CustomerId);
                pad = dbase.GetPayPalAccountDetail(payeeId);
            }
            if (pad == null)
            {
                ViewBag.DD = new List<SelectListItem>
                                       {
                                           new SelectListItem(){Text="Check Online", Value="CheckOnline"}
                                       };
            }
            else
            {
                ViewBag.DD = new List<SelectListItem>
                                       {
                                           new SelectListItem(){Text="PayPal", Value="PayPal"},
                                           new SelectListItem(){Text="Check Online", Value="CheckOnline"}
                                       };
            }
            return View();
        }

        //Post method for checkout register
        [HttpPost]
        public ActionResult CheckOutRegister(CheckOutRegisterModel model)
        {
            

            if (ModelState.IsValid)
            {
                try
                {

                    WebSecurity.CreateUserAndAccount(model.Email, model.Password, null, false);
                    dbase.InsertPassword2(new RegisterData(model.Email, model.Password, "", model.Email));
                    Roles.AddUserToRole(model.Email, "Customer");
                    model.UserId = dbase.GetUserId(model.Email);
                    dbase.InsertCustomerProfile(model.ToCustomerProfile());
                    Session["PaymentMode"] = model.PaymentMode;
                    return RedirectToAction("CheckOut", "Cart");

                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", e.Message);
                }

            }
            PayPalAccountDetail pad = null;
            if (Session["cartitems"] != null)
            {
                Lazy<List<InvoiceDetail>> list = new Lazy<List<InvoiceDetail>>(() => (List<InvoiceDetail>)Session["cartitems"]);
                int payeeId = dbase.GetPayeeId(list.Value[0].CustomerId);
                pad = dbase.GetPayPalAccountDetail(payeeId);
            }
            if (pad == null)
            {
                ViewBag.DD = new List<SelectListItem>
                                       {
                                           new SelectListItem(){Text="Check Online", Value="CheckOnline"}
                                       };
            }
            else
            {
                ViewBag.DD = new List<SelectListItem>
                                       {
                                           new SelectListItem(){Text="PayPal", Value="PayPal"},
                                           new SelectListItem(){Text="Check Online", Value="CheckOnline"}
                                       };
            }
            return View(model);
        }

        public ActionResult CheckoutLoggOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("CartLogin", "Cart");
        }
        //get method for checkout login
        public ActionResult CheckOutLogin(string returnUrl)
        {


            if (returnUrl != "/Cart/CheckoutLoggOff")
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            else
            {
                ViewBag.ReturnUrl = "/Cart/CheckOut";
            }
            return View();
        }

        //Post method for checkout login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOutLogin(CheckOutLoginModel model, string returnUrl)
        {

            if (WebSecurity.Login(model.Email, model.Password))
            {
                HttpCookie authcookie = System.Web.Security.FormsAuthentication.GetAuthCookie(model.Email, false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            return View();
        }

        [CustomerAuthorize]
        public ActionResult CheckOut()
        {
            
            int UserId = dbase.GetUserId(User.Identity.Name);
            CheckOutModel model = dbase.GetCustomerProfile(UserId).ToCheckOutRegisterModel();
            PayPalAccountDetail pad = null;
            if (Session["cartitems"] != null)
            {
                Lazy<List<InvoiceDetail>> list = new Lazy<List<InvoiceDetail>>(() => (List<InvoiceDetail>)Session["cartitems"]);
                int payeeId = dbase.GetPayeeId(list.Value[0].CustomerId);
                pad = dbase.GetPayPalAccountDetail(payeeId);
            }
            if (pad == null)
            {
                ViewBag.DD = new List<SelectListItem>
                                       {
                                           new SelectListItem(){Text="Check Online", Value="CheckOnline"}
                                       };
            }
            else
            {
                ViewBag.DD = new List<SelectListItem>
                                       {
                                           new SelectListItem(){Text="PayPal", Value="PayPal"},
                                           new SelectListItem(){Text="Check Online", Value="CheckOnline"},
                                           new SelectListItem(){Text="ACH Deposit", Value="ACHDeposit"}
                                       };
            }
            return View(model);
        }

        [CustomerAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(CheckOutModel model)
        {
            Session["PaymentMode"] = model.PaymentMode;
            return RedirectToPaymentPage(model.PaymentMode);

        }

        //[CustomerAuthorize]
        //public ActionResult ConfirmOrder()
        //{

        //    List<InvoiceDetails> list = null;

        //    list = (List<InvoiceDetails>)Session["cartitems"];
        //    ViewBag.Results = list;
        //    ViewBag.Count = list.Count;
        //    ViewBag.checkoutAmount = list.Sum(a => a.TotalAmountDue);
        //    return View();
        //}

        //[CustomerAuthorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ConfirmOrder(string name)
        //{
        //    List<InvoiceDetails> list = null;

        //    list = (List<InvoiceDetails>)Session["cartitems"];
        //    decimal Amount = list.Sum(a => a.TotalAmountDue);
        //    string PaymentMode = Session["PaymentMode"].ToString();
        //    return RedirectToPaymentPage(PaymentMode);

        //}

        [CustomerAuthorize]
        public ActionResult CheckOnline()
        {
            List<InvoiceDetail> list = null;
            CheckModel model = new CheckModel();

            if (Session["cartitems"] != null)
            {
                
                int UserId = dbase.GetUserId(User.Identity.Name);
                int cUserid = WebSecurity.CurrentUserId;
                list = (List<InvoiceDetail>)Session["cartitems"];

               CheckDetail cdetail = dbase.GetCheckDetail(UserId, true);
               // CheckDetail cdetail = dbase.GetCheckDetail(1296, null);
                model.AmountOnCheck = list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges);
                model.Payee = list[0].Payee;
                model.UserId = cUserid;
                model.DateOnCheck = DateTime.Now.Date;
                model.InvoiceNumber = list.FirstOrDefault().InvoiceNumber;
                if (cdetail == null)
                {
                    CheckOutModel cmodel = dbase.GetCustomerProfile(UserId).ToCheckOutRegisterModel();
                    model.NameOnCheck = cmodel.FirstName + " " + cmodel.LastName;
                    model.AddressOnCheck = cmodel.Address;
                    model.CityOnCheck = cmodel.City;
                    model.StateOnCheck = cmodel.State;
                    model.ZipOnCheck = cmodel.ZipCode;

                }
                else
                {
                    model.NameOnCheck = cdetail.NameOnCheck;
                    model.AddressOnCheck = cdetail.AddressOnCheck;
                    model.CityOnCheck = cdetail.CityOnCheck;
                    model.StateOnCheck = cdetail.StateOnCheck;
                    model.ZipOnCheck = cdetail.ZipOnCheck;
                    model.BankName = cdetail.BankName;
                    model.BankAddress = cdetail.BankAddress;
                    model.BankCity = cdetail.BankCity;
                    model.BankState = cdetail.BankState;
                    model.BankZip = cdetail.BankZip;
                    model.AccountNumber = cdetail.AccountNumber;
                    model.RoutingNumber = cdetail.RountingNumber;
                }

            }

            return View(model);
        }

        [CustomerAuthorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CheckOnline(CheckModel model)
        {
            if (ModelState.IsValid)
            {
                List<InvoiceDetail> list = null;
                if (Session["cartitems"] != null)
                {
                    list = (List<InvoiceDetail>)Session["cartitems"];

                    IBHDbase dbase = new BHDbase();

                    List<int> invoicelist = list.Select(a => a.InvoiceId).ToList();
                    model.InsertedOn = DateTime.Now;
                    model.RoutingNumber = model.RoutingNumber.TrimStart('0');//to remove zeros in the front
                    int PaymentId = dbase.InsertCheckPaymentDetail(model.ToCheckDetail(), invoicelist,2);
                    var monthYearFolder = DateTime.Now.Month + "." + DateTime.Now.Year;
                    string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + monthYearFolder + "/" + PaymentId + ".csv";
                    //string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + monthYearFolder+"/" + list[0].UserId + "/" + list[0].CustomerId + "/" + PaymentId + ".csv";

                    if (!System.IO.File.Exists(Server.MapPath(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(Server.MapPath(path)));
                    }

                    Tuple<CheckOutModel, CheckModel> tmodel = new Tuple<CheckOutModel, CheckModel>(dbase.GetCustomerProfile(model.UserId).ToCheckOutRegisterModel(), model);
                    OnlineCheck check = new OnlineCheck();
                    check.CustomerName = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
                    check.PaymentId = PaymentId;
                    check.PhoneNumber = tmodel.Item1.PhoneNumber;
                    check.Email = tmodel.Item1.Email;

                    check.CheckNumber = tmodel.Item2.CheckNumber;
                    check.AmountOnCheck = Convert.ToDecimal(tmodel.Item2.AmountOnCheck);
                    check.BankName = tmodel.Item2.BankName;
                    check.BankAddress = tmodel.Item2.BankAddress.Replace(',', ' ');
                    check.BankCity = tmodel.Item2.BankCity + " " + tmodel.Item2.BankState + " " + tmodel.Item2.BankZip;
                    check.Comment = tmodel.Item2.Comment;
                    check.PayeeName = tmodel.Item2.Payee;
                    check.DateOnCheck = model.DateOnCheck;
                    check.Address = tmodel.Item2.AddressOnCheck.Replace(',', ' ');
                    check.Address2 = tmodel.Item2.CityOnCheck + " " + tmodel.Item2.StateOnCheck + " " + tmodel.Item2.ZipOnCheck;
                    check.RoutingNumber = tmodel.Item2.RoutingNumber;
                    check.AccountNumber = tmodel.Item2.AccountNumber;
                    check.Comment = tmodel.Item2.Comment;
                    check.CheckMemo = tmodel.Item2.CheckMemo;
                    var csvData = BingHousingMVC.GlobalOperations.Extensions.getCheckCsvFileData(tmodel);
                    check.CsvFileData = csvData;
                    var res = dbase.InsertCheckOnline(check);

                    if (res > 0 && CSVOperations.CreateCSV(tmodel, Server.MapPath(path)))
                    {
                        //string bckpath = Server.MapPath(path.Replace(ConfigurationManager.AppSettings["CheckPath"], ConfigurationManager.AppSettings["CheckBckUpPath"]));

                        //CSVOperations.MakeCopy(Server.MapPath(path), bckpath);
                        PaymentMailModel pmodel = new PaymentMailModel();
                        pmodel.BillDescription = list[0].BillDescription;
                        pmodel.BillingDate = list[0].EmailSentDate;
                        pmodel.ShippingCost = Convert.ToDecimal(0);
                        pmodel.Tax = Convert.ToDecimal(0); ;
                        pmodel.ShippingMethod = "Normal Post";
                        pmodel.PaymentType = "Check On-Line";
                        pmodel.Address1 = tmodel.Item1.Address;
                        pmodel.Address2 = tmodel.Item1.City + " " + tmodel.Item1.State + " " + tmodel.Item1.ZipCode;
                        pmodel.Country = tmodel.Item1.Country;
                        pmodel.Phone = tmodel.Item1.PhoneNumber;
                        pmodel.PaymentDate = DateTime.Now.Date;
                        pmodel.OrderId = PaymentId;
                        pmodel.BillingId = String.Join(",", invoicelist.Select(a => a.ToString()).ToArray());
                        pmodel.Amount = (list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges)).ToString();

                        pmodel.From = list[0].PayeeEmail;
                        pmodel.To = tmodel.Item1.Email;
                        pmodel.Payee = tmodel.Item2.Payee;
                        pmodel.Sub = "Thanks for the Payment of Order Number " + PaymentId.ToString();
                        pmodel.Name = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
                        pmodel.Type = MailType.EmailPamentCustomer;
                        pmodel.ProjectNumber = list[0].InvoiceNumber;

                        IMembershipService Mail = new AccountMembershipService();

                        Mail.SendEmail(pmodel);

                        pmodel.From = tmodel.Item1.Email;
                        pmodel.To = list[0].PayeeEmail;
                        pmodel.Sub = "Check import text file for Order# " + PaymentId.ToString();
                        pmodel.Type = MailType.EmailPaymentUser;

                        pmodel.Attachment = Server.MapPath(path);

                        Mail.SendEmail(pmodel);

                        pmodel.To = dbase.GetUserEmail(list[0].UserId);

                        Mail.SendEmail(pmodel);
                        pmodel.Address2 = tmodel.Item1.City + "$" + tmodel.Item1.State + "$" + tmodel.Item1.ZipCode;
                        return RedirectToAction("PaymentSuccess", pmodel);
                    }
                }
            }
            return View(model);
        }

        [CustomerAuthorize]
        public ActionResult ACHDeposit()
        {
            List<InvoiceDetail> list = null;
            ACHModel model = new ACHModel();

            if (Session["cartitems"] != null)
            {

                int UserId = dbase.GetUserId(User.Identity.Name);
                int cUserid = WebSecurity.CurrentUserId;
                list = (List<InvoiceDetail>)Session["cartitems"];

                CheckDetail cdetail = dbase.GetCheckDetail(UserId, true);
                // CheckDetail cdetail = dbase.GetCheckDetail(1296, null);
                model.AmountOnCheck = list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges);
                model.Payee = list[0].Payee;
                model.UserId = cUserid;
                model.DateOnCheck = DateTime.Now.Date;
                model.InvoiceNumber = list.FirstOrDefault().InvoiceNumber;
                if (cdetail == null)
                {
                    CheckOutModel cmodel = dbase.GetCustomerProfile(UserId).ToCheckOutRegisterModel();
                    model.NameOnCheck = cmodel.FirstName + " " + cmodel.LastName;
                    model.AddressOnCheck = cmodel.Address;
                    model.CityOnCheck = cmodel.City;
                    model.StateOnCheck = cmodel.State;
                    model.ZipOnCheck = cmodel.ZipCode;

                }
                else
                {
                    model.NameOnCheck = cdetail.NameOnCheck;
                    model.AddressOnCheck = cdetail.AddressOnCheck;
                    model.CityOnCheck = cdetail.CityOnCheck;
                    model.StateOnCheck = cdetail.StateOnCheck;
                    model.ZipOnCheck = cdetail.ZipOnCheck;
                    model.BankName = cdetail.BankName;
                    model.BankAddress = cdetail.BankAddress;
                    model.BankCity = cdetail.BankCity;
                    model.BankState = cdetail.BankState;
                    model.BankZip = cdetail.BankZip;
                    model.AccountNumber = cdetail.AccountNumber;
                    model.RoutingNumber = cdetail.RountingNumber;
                }

            }

            return View(model);
        }

        //[CustomerAuthorize]
        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public ActionResult ACHDeposit(ACHModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        List<InvoiceDetail> list = null;
        //        if (Session["cartitems"] != null)
        //        {
        //            list = (List<InvoiceDetail>)Session["cartitems"];

        //            IBHDbase dbase = new BHDbase();

        //            List<int> invoicelist = list.Select(a => a.InvoiceId).ToList();
        //            model.InsertedOn = DateTime.Now;
        //            model.RoutingNumber = model.RoutingNumber.TrimStart('0');//to remove zeros in the front
        //            int PaymentId = dbase.InsertCheckPaymentDetail(model.ToCheckDetail(), invoicelist, 2);
        //            var monthYearFolder = DateTime.Now.Month + "." + DateTime.Now.Year;
        //            string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + monthYearFolder + "/" + PaymentId + ".csv";
        //            //string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + monthYearFolder+"/" + list[0].UserId + "/" + list[0].CustomerId + "/" + PaymentId + ".csv";

        //            if (!System.IO.File.Exists(Server.MapPath(path)))
        //            {
        //                Directory.CreateDirectory(Path.GetDirectoryName(Server.MapPath(path)));
        //            }

        //            Tuple<CheckOutModel, CheckModel> tmodel = new Tuple<CheckOutModel, CheckModel>(dbase.GetCustomerProfile(model.UserId).ToCheckOutRegisterModel(), model);
        //            OnlineCheck check = new OnlineCheck();
        //            check.CustomerName = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
        //            check.PaymentId = PaymentId;
        //            check.PhoneNumber = tmodel.Item1.PhoneNumber;
        //            check.Email = tmodel.Item1.Email;

        //            check.CheckNumber = tmodel.Item2.CheckNumber;
        //            check.AmountOnCheck = Convert.ToDecimal(tmodel.Item2.AmountOnCheck);
        //            check.BankName = tmodel.Item2.BankName;
        //            check.BankAddress = tmodel.Item2.BankAddress.Replace(',', ' ');
        //            check.BankCity = tmodel.Item2.BankCity + " " + tmodel.Item2.BankState + " " + tmodel.Item2.BankZip;
        //            check.Comment = tmodel.Item2.Comment;
        //            check.PayeeName = tmodel.Item2.Payee;
        //            check.DateOnCheck = model.DateOnCheck;
        //            check.Address = tmodel.Item2.AddressOnCheck.Replace(',', ' ');
        //            check.Address2 = tmodel.Item2.CityOnCheck + " " + tmodel.Item2.StateOnCheck + " " + tmodel.Item2.ZipOnCheck;
        //            check.RoutingNumber = tmodel.Item2.RoutingNumber;
        //            check.AccountNumber = tmodel.Item2.AccountNumber;
        //            check.Comment = tmodel.Item2.Comment;
        //            check.CheckMemo = tmodel.Item2.CheckMemo;
        //            var csvData = BingHousingMVC.GlobalOperations.Extensions.getCheckCsvFileData(tmodel);
        //            check.CsvFileData = csvData;
        //            var res = dbase.InsertCheckOnline(check);

        //            if (res > 0 && CSVOperations.CreateCSV(tmodel, Server.MapPath(path)))
        //            {
        //                //string bckpath = Server.MapPath(path.Replace(ConfigurationManager.AppSettings["CheckPath"], ConfigurationManager.AppSettings["CheckBckUpPath"]));

        //                //CSVOperations.MakeCopy(Server.MapPath(path), bckpath);
        //                PaymentMailModel pmodel = new PaymentMailModel();
        //                pmodel.BillDescription = list[0].BillDescription;
        //                pmodel.BillingDate = list[0].EmailSentDate;
        //                pmodel.ShippingCost = Convert.ToDecimal(0);
        //                pmodel.Tax = Convert.ToDecimal(0); ;
        //                pmodel.ShippingMethod = "Normal Post";
        //                pmodel.PaymentType = "Check On-Line";
        //                pmodel.Address1 = tmodel.Item1.Address;
        //                pmodel.Address2 = tmodel.Item1.City + " " + tmodel.Item1.State + " " + tmodel.Item1.ZipCode;
        //                pmodel.Country = tmodel.Item1.Country;
        //                pmodel.Phone = tmodel.Item1.PhoneNumber;
        //                pmodel.PaymentDate = DateTime.Now.Date;
        //                pmodel.OrderId = PaymentId;
        //                pmodel.BillingId = String.Join(",", invoicelist.Select(a => a.ToString()).ToArray());
        //                pmodel.Amount = (list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges)).ToString();

        //                pmodel.From = list[0].PayeeEmail;
        //                pmodel.To = tmodel.Item1.Email;
        //                pmodel.Payee = tmodel.Item2.Payee;
        //                pmodel.Sub = "Thanks for the Payment of Order Number " + PaymentId.ToString();
        //                pmodel.Name = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
        //                pmodel.Type = MailType.EmailPamentCustomer;
        //                pmodel.ProjectNumber = list[0].InvoiceNumber;

        //                IMembershipService Mail = new AccountMembershipService();

        //                Mail.SendEmail(pmodel);

        //                pmodel.From = tmodel.Item1.Email;
        //                pmodel.To = list[0].PayeeEmail;
        //                pmodel.Sub = "Check import text file for Order# " + PaymentId.ToString();
        //                pmodel.Type = MailType.EmailPaymentUser;

        //                pmodel.Attachment = Server.MapPath(path);

        //                Mail.SendEmail(pmodel);

        //                pmodel.To = dbase.GetUserEmail(list[0].UserId);

        //                Mail.SendEmail(pmodel);
        //                pmodel.Address2 = tmodel.Item1.City + "$" + tmodel.Item1.State + "$" + tmodel.Item1.ZipCode;
        //                return RedirectToAction("PaymentSuccess", pmodel);
        //            }
        //        }
        //    }
        //    return View(model);
        //}
        public JsonResult UpdateCheckOnline (int PaymentId)
        {
            string[] paymentIds = new string[] { PaymentId.ToString() };
            //var PaymentId = PaymentIds.Split(',');
            var pModel = dbase.GetCustomerOnlineCheckDetail(paymentIds).FirstOrDefault();
            return Json(pModel,JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateRoutngNumber(int PaymentId, string RoutingNumber, string AccountNumber)
        {

            var cdetail = dbase.GetCheckDetailByPaymentId(PaymentId);
            CheckModel model = new CheckModel();

            model.AmountOnCheck = cdetail.Amount;
            model.Payee = cdetail.Payee;
            model.UserId = cdetail.UserId;
            model.DateOnCheck = DateTime.Now.Date;
            //model.InvoiceNumber = checkDetail.;        
            
            model.NameOnCheck = cdetail.NameOnCheck;
            model.AddressOnCheck = cdetail.AddressOnCheck;
            model.CityOnCheck = cdetail.CityOnCheck;
            model.StateOnCheck = cdetail.StateOnCheck;
            model.ZipOnCheck = cdetail.ZipOnCheck;
            model.BankName = cdetail.BankName;
            model.BankAddress = cdetail.BankAddress;
            model.BankCity = cdetail.BankCity;
            model.BankState = cdetail.BankState;
            model.BankZip = cdetail.BankZip;
            model.AccountNumber = AccountNumber;
            model.RoutingNumber = RoutingNumber;
            model.CheckNumber = cdetail.CheckNumber;
            var monthYearFolder = DateTime.Now.Month + "." + DateTime.Now.Year;
            string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + monthYearFolder + "/" + PaymentId + ".csv";
          
            if (!System.IO.File.Exists(Server.MapPath(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Server.MapPath(path)));
            }

            Tuple<CheckOutModel, CheckModel> tmodel = new Tuple<CheckOutModel, CheckModel>(dbase.GetCustomerProfile(cdetail.UserId).ToCheckOutRegisterModel(), model);
            OnlineCheck check = new OnlineCheck();
            check.CustomerName = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
            check.PaymentId = PaymentId;            
            check.RoutingNumber = RoutingNumber;
            check.AccountNumber = AccountNumber;
            var csvData = BingHousingMVC.GlobalOperations.Extensions.getCheckCsvFileData(tmodel);
            check.CsvFileData = csvData;
            var res = dbase.UpdateOnlineCheck(check);

            if (res && CSVOperations.CreateCSV(tmodel, Server.MapPath(path)))
            { 
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        [DenyAccessToUser(Roles = "Customer")]
        public ActionResult PaperCheck(string InvoiceNumber="")
        {
            CheckModel model = new CheckModel();
            model.InvoiceNumber = InvoiceNumber;
            int UserId = dbase.GetUserId(User.Identity.Name);
            int cUserid = WebSecurity.CurrentUserId;
            model.UserId = cUserid;
            model.DateOnCheck = DateTime.Now.Date;
            List<InvoiceDetail> list = null;
            if (!String.IsNullOrEmpty(InvoiceNumber))
            {
                list = dbase.GetInvoiceDetails(InvoiceNumber, InvoiceType.Notpaid);
                list =LateChargesHelper.FindLateCharges(list);
                Session["cartitems"] = list;
                string Title = Session["Payee"] != null ? Session["Payee"] as string : "Smart Rent Pay";
                if (Title == "Smart Rent Pay" & list.Count > 0)
                {
                    CustomerDetail detail = dbase.GetCustomerDetails(list[0].CustomerId);
                    Session["Payee"] = detail.Payee;
                }  

                CheckDetail cdetail = dbase.GetCheckDetail(UserId, true);
                // CheckDetail cdetail = dbase.GetCheckDetail(1296, null);
                model.AmountOnCheck = list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges);
                model.Payee = list[0].Payee;               
                
                if (cdetail == null)
                {
                    CheckOutModel cmodel = dbase.GetCustomerDetails(list[0].CustomerId).ToCheckOutRegisterModel();
                    model.NameOnCheck = cmodel.FirstName + " " + cmodel.LastName;
                    model.AddressOnCheck = cmodel.Address;
                    model.CityOnCheck = cmodel.City;
                    model.StateOnCheck = cmodel.State;
                    model.ZipOnCheck = cmodel.ZipCode;

                }
                else
                {
                    model.NameOnCheck = cdetail.NameOnCheck;
                    model.AddressOnCheck = cdetail.AddressOnCheck;
                    model.CityOnCheck = cdetail.CityOnCheck;
                    model.StateOnCheck = cdetail.StateOnCheck;
                    model.ZipOnCheck = cdetail.ZipOnCheck;
                    model.BankName = cdetail.BankName;
                    model.BankAddress = cdetail.BankAddress;
                    model.BankCity = cdetail.BankCity;
                    model.BankState = cdetail.BankState;
                    model.BankZip = cdetail.BankZip;
                    model.AccountNumber = cdetail.AccountNumber;
                    model.RoutingNumber = cdetail.RountingNumber;
                }                
            }
            return View(model);
        }

        [DenyAccessToUser(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PaperCheck(CheckModel model)
        {
            if (ModelState.IsValid)
            {
                List<InvoiceDetail> list = null;
                if (Session["cartitems"] == null)
                {
                    list = dbase.GetInvoiceDetails(model.InvoiceNumber, InvoiceType.Notpaid);
                    list = LateChargesHelper.FindLateCharges(list);
                    Session["cartitems"] = list;
                    string Title = Session["Payee"] != null ? Session["Payee"] as string : "Smart Rent Pay";
                    if (Title == "Smart Rent Pay" & list.Count > 0)
                    {
                        CustomerDetail detail = dbase.GetCustomerDetails(list[0].CustomerId);
                        Session["Payee"] = detail.Payee;
                    }
                }
                if (Session["cartitems"] != null)
                {
                    list = (List<InvoiceDetail>)Session["cartitems"];

                    IBHDbase dbase = new BHDbase();

                    List<int> invoicelist = list.Select(a => a.InvoiceId).ToList();
                    model.InsertedOn = DateTime.Now;
                    model.RoutingNumber = model.RoutingNumber.TrimStart('0');//to remove zeros in the front
                    int PaymentId = dbase.InsertCheckPaymentDetail(model.ToCheckDetail(), invoicelist,3);                    

                    Tuple<CheckOutModel, CheckModel> tmodel = new Tuple<CheckOutModel, CheckModel>(dbase.GetCustomerDetails(list[0].CustomerId).ToCheckOutRegisterModel(), model);
                    OnlineCheck check = new OnlineCheck();
                    check.CustomerName = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
                    check.PaymentId = PaymentId;
                    check.PhoneNumber = tmodel.Item1.PhoneNumber;
                    check.Email = tmodel.Item1.Email;

                    check.CheckNumber = model.CheckNumber;
                    check.AmountOnCheck = Convert.ToDecimal(model.AmountOnCheck);
                    check.BankName = model.BankName;
                    check.BankAddress = model.BankAddress.Replace(',', ' ');
                    check.BankCity = model.BankCity + " " + model.BankState + " " + model.BankZip;
                    check.Comment = model.Comment;
                    check.PayeeName = model.Payee;
                    check.DateOnCheck = model.DateOnCheck;
                    check.Address = model.AddressOnCheck.Replace(',', ' ');
                    check.Address2 = model.CityOnCheck + " " + model.StateOnCheck + " " + model.ZipOnCheck;
                    check.RoutingNumber = model.RoutingNumber;
                    check.AccountNumber = model.AccountNumber;
                    check.Comment = model.Comment;
                    check.CheckMemo = model.CheckMemo;
                    var csvData = BingHousingMVC.GlobalOperations.Extensions.getCheckCsvFileData(tmodel);
                    check.CsvFileData = csvData;
                    var res = dbase.InsertCheckOnline(check);

                    if (res > 0)
                    {
                       
                        PaymentMailModel pmodel = new PaymentMailModel();
                        pmodel.BillDescription = list[0].BillDescription;
                        pmodel.BillingDate = list[0].EmailSentDate;
                        pmodel.ShippingCost = Convert.ToDecimal(0);
                        pmodel.Tax = Convert.ToDecimal(0); ;
                        pmodel.ShippingMethod = "Normal Post";
                        pmodel.PaymentType = "Paper Check";
                        pmodel.Address1 = tmodel.Item1.Address;
                        pmodel.Address2 = tmodel.Item1.City + " " + tmodel.Item1.State + " " + tmodel.Item1.ZipCode;
                        pmodel.Country = tmodel.Item1.Country;
                        pmodel.Phone = tmodel.Item1.PhoneNumber;
                        pmodel.PaymentDate = DateTime.Now.Date;
                        pmodel.OrderId = PaymentId;
                        pmodel.BillingId = String.Join(",", invoicelist.Select(a => a.ToString()).ToArray());
                        pmodel.Amount = (list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges)).ToString();
                        pmodel.Payee = tmodel.Item2.Payee;
                        pmodel.From = list[0].PayeeEmail;
                        pmodel.To = tmodel.Item1.Email;
                        pmodel.Sub = "Thanks for the Payment of Order Number " + PaymentId.ToString();
                        pmodel.Name = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
                        pmodel.Type = MailType.EmailPamentCustomer;
                        pmodel.ProjectNumber = list[0].InvoiceNumber;

                        IMembershipService Mail = new AccountMembershipService();

                        Mail.SendEmail(pmodel);
                        pmodel.Address2 = tmodel.Item1.City + "$" + tmodel.Item1.State + "$" + tmodel.Item1.ZipCode;
                        return RedirectToAction("PaperCheckPaymentSuccess", pmodel);
                    }
                }
            }
            return View(model);
        }

        [DenyAccessToUser(Roles = "Customer")]
        public ActionResult PaperCheckPaymentSuccess(PaymentMailModel tmodel)
        {

            if (Session["cartitems"] != null)
            {
                var list = (List<InvoiceDetail>)Session["cartitems"];
                ViewBag.Model = tmodel;
                ViewBag.Results = list;

                ViewBag.checkoutAmount = list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges);

            }
            return View("PaymentSuccess");

        }

        [CustomerAuthorize]
        public ActionResult PayPal()
        {
            List<InvoiceDetail> list = null;
            if (Session["cartitems"] != null)
            {
                list = (List<InvoiceDetail>)Session["cartitems"];
                ViewBag.Results = list;
                ViewBag.Count = list.Count;
                

                Payment pymnt = null;

                
                int payeeId = dbase.GetPayeeId(list[0].CustomerId);
                PayPalAccountDetail pad = dbase.GetPayPalAccountDetail(payeeId);
                decimal totamount = Math.Round(list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges), 2);
                decimal paypalsurcharge = Math.Round(totamount * pad.PayPalSurCharge / 100,2);
                decimal totalamount = Math.Round((totamount + paypalsurcharge), 2);
                ViewBag.checkoutAmount = totalamount;
                // dbase.GetPayPalAccountDetail(list[0].UserId);
                // ### Api Context
                // Pass in a `APIContext` object to authenticate 
                // the call and to send a unique request id 
                // (that ensures idempotency). The SDK generates
                // a request id if you do not pass one explicitly. 
                // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext..
                APIContext apiContext = BingHousing_PAYPAL.Configuration.GetAPIContext(pad.ClientID, pad.ClientSecret);

                if (Request.Params["PayerID"] != null)
                {
                    pymnt = new Payment();
                    if (Request.Params["guid"] != null)
                    {
                        pymnt.id = (string)Session[Request.Params["guid"]];

                    }
                    try
                    {
                        PaymentExecution pymntExecution = new PaymentExecution();
                        pymntExecution.payer_id = Request.Params["PayerID"];

                        Payment executedPayment = pymnt.Execute(apiContext, pymntExecution);
                        dynamic data = JObject.Parse(executedPayment.ConvertToJson());
                        // CurrContext.Items.Add("ResponseJson", JObject.Parse(executedPayment.ConvertToJson()).ToString(Formatting.Indented));

                        string pamentresourceid = data.id;

                        string transactionId = data.transactions[0].related_resources[0].sale.id;




                        List<int> invoicelist = list.Select(a => a.InvoiceId).ToList();

                        PayPalDetail model = new PayPalDetail();
                        model.InsertedOn = DateTime.UtcNow;
                        model.TransactionID = transactionId;
                        model.UserId = WebSecurity.CurrentUserId;
                        model.PaymentResourceId = pamentresourceid;
                        model.Amount = (list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges));

                        int PaymentId = dbase.InsertPayPalPaymentDetail(model, invoicelist);

                        Tuple<CheckOutModel, PayPalDetail> tmodel = new Tuple<CheckOutModel, PayPalDetail>(dbase.GetCustomerProfile(model.UserId).ToCheckOutRegisterModel(), model);


                        PaymentMailModel pmodel = new PaymentMailModel();
                        pmodel.BillDescription = list[0].BillDescription;
                        pmodel.BillingDate = list[0].EmailSentDate;
                        pmodel.ShippingCost = Convert.ToDecimal(0);
                        pmodel.Tax = Convert.ToDecimal(0); ;
                        pmodel.ShippingMethod = "Normal Post";
                        pmodel.PaymentType = "PayPal";
                        pmodel.Address1 = tmodel.Item1.Address;
                        pmodel.Address2 = tmodel.Item1.City + " " + tmodel.Item1.State + " " + tmodel.Item1.ZipCode;
                        pmodel.Country = tmodel.Item1.Country;
                        pmodel.Phone = tmodel.Item1.PhoneNumber;
                        pmodel.PaymentDate = DateTime.Now.Date;
                        pmodel.OrderId = PaymentId;
                        pmodel.BillingId = String.Join(",", invoicelist.Select(a => a.ToString()).ToArray());
                        pmodel.Amount = (list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges)).ToString();
                        pmodel.Payee = list[0].Payee;
                        pmodel.From = list[0].PayeeEmail;
                        pmodel.To = tmodel.Item1.Email;
                        pmodel.Sub = "Thanks for the Payment of Order Number " + PaymentId.ToString();
                        pmodel.Name = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
                        pmodel.Type = MailType.EmailPamentCustomer;
                        pmodel.ProjectNumber = list[0].InvoiceNumber;

                        IMembershipService Mail = new AccountMembershipService();

                        Mail.SendEmail(pmodel);

                        pmodel.From = tmodel.Item1.Email;
                        pmodel.To = list[0].PayeeEmail;
                        pmodel.Sub = "PayPal Payment Successfull for Order# " + PaymentId.ToString();
                        pmodel.Type = MailType.EmailPaymentUser;
                        pmodel.Attachment = "";
                        Mail.SendEmail(pmodel);

                        pmodel.To = dbase.GetUserEmail(list[0].UserId);

                        Mail.SendEmail(pmodel);
                        pmodel.Address2 = tmodel.Item1.City + "$" + tmodel.Item1.State + "$" + tmodel.Item1.ZipCode;
                        return RedirectToAction("PaymentSuccess", pmodel);
                    }


                    catch (PayPal.Exception.PayPalException ex)
                    {
                        throw ex;
                    }
                }
                else
                {

                    // ## ExecutePayment
                    // ###Items
                    // Items within a transaction.
                    List<Item> itms = new List<Item>();
                    foreach (var i in list)
                    {
                        Item item = new Item();
                        item.name = i.BillDescription;
                        item.currency = "USD";
                        item.price = Convert.ToString(i.AmountDue + i.LateCharges);
                        item.quantity = Convert.ToString(i.Quantity);
                        item.sku = Convert.ToString(i.InvoiceId);                       
                        itms.Add(item);
                    }

                    ItemList itemList = new ItemList();
                    itemList.items = itms;

                    // ###Payer
                    // A resource representing a Payer that funds a payment
                    // Payment Method
                    // as `paypal`
                    Payer payr = new Payer();
                    payr.payment_method = "paypal";
                    Random rndm = new Random();
                    var guid = Convert.ToString(rndm.Next(100000));

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "Cart/PayPal?";

                    // # Redirect URLS
                    RedirectUrls redirUrls = new RedirectUrls();
                    redirUrls.return_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/PayPal?" + "guid=" + guid;
                    redirUrls.cancel_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/PayPalCancel?" + "guid=" + guid;

                    // ###Details
                    // Let's you specify details of a payment amount.
                    Details details = new Details();
                    details.tax = "0";
                    details.shipping =  Convert.ToString(paypalsurcharge);
                    details.subtotal = Convert.ToString(totamount);

                    // ###Amount
                    // Let's you specify a payment amount.
                    Amount amnt = new Amount();
                    amnt.currency = "USD";
                    // Total must be equal to sum of shipping, tax and subtotal.
                    amnt.total = Convert.ToString(totalamount);
                    amnt.details = details;

                    // ###Transaction
                    // A transaction defines the contract of a
                    // payment - what is the payment for and who
                    // is fulfilling it. 
                    List<Transaction> transactionList = new List<Transaction>();
                    Transaction tran = new Transaction();
                    tran.description = "Transaction description.";
                    tran.amount = amnt;
                    tran.item_list = itemList;
                    // The Payment creation API requires a list of
                    // Transaction; add the created `Transaction`
                    // to a List
                    transactionList.Add(tran);

                    // ###Payment
                    // A Payment Resource; create one using
                    // the above types and intent as `sale` or `authorize`
                    pymnt = new Payment();
                    pymnt.intent = "sale";
                    pymnt.payer = payr;
                    pymnt.transactions = transactionList;
                    pymnt.redirect_urls = redirUrls;

                    try
                    {
                        // Create a payment using a valid APIContext
                        Payment createdPayment = pymnt.Create(apiContext);

                        var links = createdPayment.links.GetEnumerator();
                        string link = "";
                        while (links.MoveNext())
                        {
                            Links lnk = links.Current;
                            if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                            {
                                link = ViewBag.Url = lnk.href;
                            }
                        }
                        Session.Add(guid, createdPayment.id);

                        return Redirect(link);
                    }
                    catch (PayPal.Exception.PayPalException ex)
                    {
                        throw ex;
                    }

                }

            }
            return View();
        }


        [CustomerAuthorize]
        [HttpPost]
        public ActionResult PayPal(string guid)
        {
            //HttpContext CurrContext = HttpContext.;
            Payment pymnt = null;
            List<InvoiceDetail> list = null;
            if (Session["cartitems"] != null)
            {
                list = (List<InvoiceDetail>)Session["cartitems"];

                

                PayPalAccountDetail pad = dbase.GetPayPalAccountDetail(list[0].UserId);
                // ### Api Context
                // Pass in a `APIContext` object to authenticate 
                // the call and to send a unique request id 
                // (that ensures idempotency). The SDK generates
                // a request id if you do not pass one explicitly. 
                // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext..
                APIContext apiContext = BingHousing_PAYPAL.Configuration.GetAPIContext(pad.ClientID, pad.ClientSecret);

                // ## ExecutePayment
                if (Request.Params["PayerID"] != null)
                {
                    pymnt = new Payment();
                    if (Request.Params["guid"] != null)
                    {
                        pymnt.id = (string)Session[Request.Params["guid"]];

                    }
                    try
                    {
                        PaymentExecution pymntExecution = new PaymentExecution();
                        pymntExecution.payer_id = Request.Params["PayerID"];

                        Payment executedPayment = pymnt.Execute(apiContext, pymntExecution);
                        dynamic data = JObject.Parse(executedPayment.ConvertToJson());
                        // CurrContext.Items.Add("ResponseJson", JObject.Parse(executedPayment.ConvertToJson()).ToString(Formatting.Indented));

                        string pamentresourceid = data.id;

                        string transactionId = data.transactions[0].related_resources[0].sale.id;




                        List<int> invoicelist = list.Select(a => a.InvoiceId).ToList();

                        PayPalDetail model = new PayPalDetail();
                        model.InsertedOn = DateTime.UtcNow;
                        model.TransactionID = transactionId;
                        model.UserId = WebSecurity.CurrentUserId;
                        model.PaymentResourceId = pamentresourceid;
                        model.Amount = (list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges));


                        int PaymentId = dbase.InsertPayPalPaymentDetail(model, invoicelist);

                        Tuple<CheckOutModel, PayPalDetail> tmodel = new Tuple<CheckOutModel, PayPalDetail>(dbase.GetCustomerProfile(model.UserId).ToCheckOutRegisterModel(), model);


                        PaymentMailModel pmodel = new PaymentMailModel();
                        pmodel.BillDescription = list[0].BillDescription;
                        pmodel.BillingDate = list[0].EmailSentDate;
                        pmodel.ShippingCost = Convert.ToDecimal(0);
                        pmodel.Tax = Convert.ToDecimal(0); ;
                        pmodel.ShippingMethod = "Normal Post";
                        pmodel.PaymentType = "PayPal";
                        pmodel.Address1 = tmodel.Item1.Address;
                        pmodel.Address2 = tmodel.Item1.City + " " + tmodel.Item1.State + " " + tmodel.Item1.ZipCode;
                        pmodel.Country = tmodel.Item1.Country;
                        pmodel.Phone = tmodel.Item1.PhoneNumber;
                        pmodel.PaymentDate = DateTime.Now.Date;
                        pmodel.OrderId = PaymentId;
                        pmodel.BillingId = String.Join(",", invoicelist.Select(a => a.ToString()).ToArray());
                        pmodel.Amount = list.Sum(a => a.TotalAmountDue).ToString();
                        pmodel.Payee = list[0].Payee;
                        pmodel.From = list[0].PayeeEmail;
                        pmodel.To = tmodel.Item1.Email;
                        pmodel.Sub = "Thanks for the Payment of Order Number " + PaymentId.ToString();
                        pmodel.Name = tmodel.Item1.FirstName + " " + tmodel.Item1.LastName;
                        pmodel.Type = MailType.EmailPamentCustomer;
                        pmodel.ProjectNumber = list[0].InvoiceNumber;

                        IMembershipService Mail = new AccountMembershipService();

                        Mail.SendEmail(pmodel);

                        pmodel.From = tmodel.Item1.Email;
                        pmodel.To = list[0].PayeeEmail;
                        pmodel.Sub = "Check import text file for Order# " + PaymentId.ToString();
                        pmodel.Type = MailType.EmailPaymentUser;
                        pmodel.Attachment = "";
                        Mail.SendEmail(pmodel);

                        pmodel.To = dbase.GetUserEmail(list[0].UserId);

                        Mail.SendEmail(pmodel);
                        pmodel.Address2 = tmodel.Item1.City + "$" + tmodel.Item1.State + "$" + tmodel.Item1.ZipCode;
                        return RedirectToAction("PaymentSuccess", pmodel);
                    }


                    catch (PayPal.Exception.PayPalException ex)
                    {
                        throw ex;
                    }
                }
            }
            return View();
        }


        [CustomerAuthorize]

        public ActionResult PayPalCancel()
        {
            ViewBag.Message = "your transaction is cancelled";
            return View();
        }
        //get method for PaymentSuccess
        [CustomerAuthorize]
        public ActionResult PaymentSuccess(PaymentMailModel tmodel)
        {

            if (Session["cartitems"] != null)
            {
                var list = (List<InvoiceDetail>)Session["cartitems"];
                ViewBag.Model = tmodel;
                ViewBag.Results = list;

                ViewBag.checkoutAmount = list.Sum(a => a.TotalAmountDue) + list.Sum(a => a.LateCharges);

            }
            return View();

        }


        //get send verification again
        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            ViewBag.flag = false;
            return View();
        }


        //post send verification again
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ForgetPassword(LoginModel model)
        {

            if (model.UserName != null)
            {
                
                if (WebSecurity.UserExists(model.UserName)) //checking whether user name is available on the table or not
                {
                    string[] roles = Roles.GetRolesForUser(model.UserName);

                    bool flag = roles.Length == 0 ? true : roles[0] == "Admin" ? true : false;

                    if (flag)
                    {
                        // user has no role
                        ModelState.AddModelError("", "Invalid User email.");

                        return View();
                    }
                    else
                    {
                        // user has at least one role
                        string Password = dbase.GetCustomerPassword(model.UserName);
                        bool sucess = MembershipService.SendVerification(Password, model.UserName, SupportMailType.ResetPasswordCustomer);

                        if (sucess)
                        {

                            ViewBag.msg = "An email with your current password was sent to your email.";
                            ViewBag.flag = true;
                            return View();
                        }
                        else
                        {
                            ModelState.AddModelError("", "Sorry Try Again Later");

                            return View();
                        }
                    }



                }

                else
                {
                    ModelState.AddModelError("", "User email does not exist.");


                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Please Enter User Email");
                ViewBag.flag = false;
                return View();
            }

        }


        //[AllowAnonymous]
        //public ActionResult repass()
        //{
        //    string password1 = "Raja2015";
        //    string password2 = "7252433842";
        //    string id = "Raja";
        //    
        //    string confiramtiontoken = WebSecurity.GeneratePasswordResetToken(id, 1440);
        //    if (WebSecurity.ResetPassword(confiramtiontoken, password1))
        //    {
        //       //dbase.ChangePassword2(id,password1, password2);
        //    }

        //    return RedirectToAction("Index", "Home");
        //}
        #endregion


        #region Methods

        private bool ValidateCartlogin(CartLoginModel model)
        {
            bool result = false;
            
            string Email = dbase.GetCustomerEmail(model.ProjectNumber.Trim());
            if (Email != "")
            {
                if (Email.ToLower().Equals(model.CustomerEmail.Trim()))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;

        }

        private ActionResult RedirectToPaymentPage(string PaymentMode)
        {
            switch (PaymentMode)
            {
                case "PayPal":
                    return RedirectToAction("PayPal", "Cart");

                case "CheckOnline":

                    return RedirectToAction("CheckOnline", "Cart");

                case "ACHDeposit":

                    return RedirectToAction("ACHDeposit", "Cart");

            }
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("CheckOut", "Cart");
            }
        }

        //private Decimal CalculateLateCharges(int startday, decimal startdayAmount, int thereafterday, decimal thereafterdayAmount, DateTime Edate)
        //{
        //    int sd = startday;
        //    int ta = thereafterday;
        //    decimal sdAmount = startdayAmount;
        //    decimal taAmount = thereafterdayAmount;
        //    decimal totalAmount = 0;
        //    DateTime date = new DateTime(Edate.Year,Edate.AddMonths(1).Month,1);
        //    //DateTime date = new DateTime(Edate.Year, Edate.Month, 20); //old changed on 22/10/2016
        //    int diff = Convert.ToInt32((DateTime.Now.Date - date.Date).TotalDays);
        //    if (diff < (sd - 1))
        //    {
        //        totalAmount = 0;
        //    }
        //    else if (diff >= (sd - 1) && diff < ta)
        //    {
        //        totalAmount = sdAmount;
        //    }
        //    else
        //    {
        //        totalAmount = sdAmount + ((diff - ta + 1) * taAmount);
        //    }
        //    return totalAmount;
        //}

        //private List<InvoiceDetail> FindLateCharges(List<InvoiceDetail> list)
        //{
        //    if (list.Count > 0)
        //    {
        //        foreach (InvoiceDetail item in list)
        //        {
        //            if (item.IsLateCharges)
        //            {

        //                item.LateCharges = LateChargesHelper.CalculateLateCharges(item.LateChargesStartday, item.LateChargesStartdayAmount, item.LateChargesThereAfterday, item.LateChargesThereAfterdayAmount, item.EmailSentDate);
        //            }

        //        }
        //    }
        //    return list;
        //}

        #endregion


    }
}