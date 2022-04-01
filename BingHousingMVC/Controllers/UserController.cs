using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.OleDb;
using BingHousingMVC_DAL;
using BingHousingMVC.Models;
using BingHousingMVC.Utility;
using BingHousingMVC.GlobalOperations;
using BingHousingMVC.Filters;
using WebMatrix.WebData;
using System.Configuration;
using System.Web.Security;
using BingHousing_BO;
using Hirs.Coverter;
using System.Text;
using AutoMapper;

namespace BingHousingMVC.Controllers
{

    [DenyAccessToUser(Roles = "Customer")]
    //[Authorize]
    public class UserController : Controller
    {
        //
        // GET: ImportBills
        private IBHDbase dbase = new BHDbase();

        public ActionResult ImportBills()
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

            int UserId = WebSecurity.GetUserId(name);
            
            ViewBag.payeelist = dbase.GetAllPayee(UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

            return View();
        }


        //
        // POST: ImportBills
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportBills(HttpPostedFileBase file, string PayeeId)
        {
            if (file != null && file.FileName != "")
            {
                if (FileIsValidate(file.FileName))
                {
                    try
                    {
                        string filepath = Server.MapPath("~") + "Files\\" + file.FileName;
                        file.SaveAs(filepath);

                        if (System.IO.File.Exists(filepath))
                        {
                            ImportFile(filepath, Convert.ToInt32(PayeeId));

                            return RedirectToAction("Bills", "User");
                        }
                        else
                        {
                            ModelState.AddModelError("", "File Not uploaded please try again later");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "File Extension Not Allowed use .xls files");

                }
            }
            else
            {
                // No file
                ModelState.AddModelError("", "No file choosen");

            }
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);

            ViewBag.payeelist = dbase.GetAllPayee(UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

            return View();
        }


        //Get: ImportPayees
        public ActionResult ImportPayees()
        {

            return View();
        }


        //
        // POST: ImportPayees
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportPayees(HttpPostedFileBase file)
        {
            if (file != null && file.FileName != "")
            {
                if (FileIsValidate(file.FileName))
                {
                    try
                    {
                        string filepath = Server.MapPath("~") + "Files\\" + file.FileName;
                        file.SaveAs(filepath);

                        if (System.IO.File.Exists(filepath))
                        {
                            ImportFile(filepath);

                            return RedirectToAction("Payees", "User");
                        }
                        else
                        {
                            ModelState.AddModelError("", "File Not please try again later");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }


                }
                else
                {
                    ModelState.AddModelError("", "File Extension Not Allowed use .xls files");

                }
            }
            else
            {
                // No file
                ModelState.AddModelError("", "No file choosen");

            }
            return View();
        }


        public ActionResult ACHDepositSetup()
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);

            List<SelectListItem> payeelist = dbase.GetAllPayee(UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();
            ViewBag.payeelist = payeelist;

            if (payeelist.Count > 0)
            {
                ACHAccountDepositDetail dtl = ACHAccountDepositDetail.GetACHDepositAccountDetail(dbase, Convert.ToInt32(payeelist[0].Value));
                ACHDepositAccountModel model = new ACHDepositAccountModel();

                if (dtl != null)
                {
                    model.PayeeId = dtl.PayeeId;
                    model.StripeProductionKey = dtl.StripeProductionKey;
                    model.ACHDepositAccountId = dtl.ACHDepositAccountId;
                    model.StripeTestKey = dtl.StripeTestKey;
                }
                return View(model);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ACHDepositSetup(ACHDepositAccountModel model)
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);

            List<SelectListItem> payeelist = dbase.GetAllPayee(UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();
            ViewBag.payeelist = payeelist;


            if (ModelState.IsValid)
            {
                ACHAccountDepositDetail dtl = new ACHAccountDepositDetail();
                dtl.ACHDepositAccountId = model.ACHDepositAccountId;
                dtl.PayeeId = model.PayeeId;
                dtl.StripeTestKey = model.StripeTestKey;
                dtl.StripeProductionKey = model.StripeProductionKey;
                dtl.InsertedOn = DateTime.Now;
                dtl.UpdatedOn = DateTime.Now;


                if (model.ACHDepositAccountId == 0)
                {
                    dbase.InsertACHAccountDepositDetail(dtl);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    dbase.UpdateACHDepositAcountDetails(dtl);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        //
        // GET: ImportBills

        public ActionResult Bills()
        {
            
            int UserId = 0;
            string name = "";
            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

                UserId = dbase.GetUserId(name);

                ViewBag.Results = dbase.GetCustomerList(UserId).OrderByDescending(i => i.NextBillDate).ToList();

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult Customers()
        {
            
            int UserId = 0;
            string name = "";
            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

                UserId = dbase.GetUserId(name);

                ViewBag.Results = dbase.GetCustomerList(UserId).OrderByDescending(i => i.NextBillDate).ToList();

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult Payees()
        {
            
            int UserId = 0;
            string name = "";
            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

                UserId = WebSecurity.GetUserId(name);

                ViewBag.Results = dbase.GetAllPayee(UserId);// dbase.GetCustomerList(UserId);

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult CopyCustomerDetails(CustomerModel model)
        {
            
            if (model.Type == "Company")
            {
                model.CustomerLastName = "";
            }
            dbase.InsertCustomer(Mapper.Map<Customer>(model));
            return Json(Url.Action("Bills", "User"));
        }


        [HttpPost]
        public ActionResult CopyPayeeDetails(PayeeModel model)
        {
            try
            {
                
                Payee payee = new Payee();
                  payee.UserId=  model.UserId;
                payee.Payee1= model.Payee;
                  payee.PayeeAddress= model.PayeeAddress;
                  payee.PayeeAddress2= model.PayeeAddress2;
                  payee.PayeeCity=model.PayeeCity;
                  payee.PayeeComments= model.PayeeComments;
                  payee.PayeeContactPerson= model.PayeeContactPerson;
                  payee.PayeeCountry= model.PayeeCountry;
                  payee.PayeeEmail= model.PayeeEmail;
                  payee.PayeeState= model.PayeeState;
                  payee.PayeeZipCode= model.PayeeZipCode;
                  payee.PayeeWebsite= model.PayeeWebsite;
                  dbase.InsertPayee(payee);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Json(Url.Action("Payees", "User"));
        }
        //
        // GET: ImportBills


        //
        // GET:  AddCustomerDetails


        public ActionResult AddCustomerDetails()
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);
            
            ViewBag.payeelist = dbase.GetAllPayee( UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

            ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();

            return View();
        }


        //
        // POST: AddCustomerDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomerDetails(CustomerModel model)
        {

            

            if (model.Type == "Company")
            {
                ModelState.Remove("CustomerLastName");
                model.CustomerLastName = "";
            }
            if (!model.LateCharges)
            {
                ModelState.Remove("LateChargesStartday");
                ModelState.Remove("LateChargesThereAfterdayAmount");
                ModelState.Remove("LateChargesThereAfterday");
                ModelState.Remove("LateChargesStartdayAmount");
            }
            if (ModelState.IsValid)
            {

                try
                {

                    dbase.InsertCustomer(Mapper.Map<Customer>(model));

                    return RedirectToAction("Bills", "User");

                }
                catch (Exception e)
                {

                    ModelState.AddModelError("", e.Message);
                }



            }
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);
            ViewBag.payeelist = dbase.GetAllPayee( UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

            ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();
            return View();
        }

        public ActionResult EditCustomerDetails(int CustomerId)
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);
            
            ViewBag.payeelist = dbase.GetAllPayee( UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

            CustomerModel model = new CustomerModel();
            try
            {
                Customer detail = dbase.GetCustomer(CustomerId);

                model = Mapper.Map<CustomerModel>(detail);
                if (detail.CustomerLastName == "" || detail.CustomerLastName == null)
                {
                    model.Type = "Company";
                }
                else
                {
                    model.Type = "Customer";
                }
                ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }


            return View(model);
        }


        //
        // POST: ImportBills
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomerDetails(CustomerModel model)
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);
            
            ViewBag.payeelist = dbase.GetAllPayee( UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

            if (model.Type == "Company")
            {
                ModelState.Remove("CustomerLastName");
                model.CustomerLastName = "";
            }
            if (!model.LateCharges)
            {
                ModelState.Remove("LateChargesStartday");
                ModelState.Remove("LateChargesThereAfterdayAmount");
                ModelState.Remove("LateChargesThereAfterday");
                ModelState.Remove("LateChargesStartdayAmount");
            }
            if (ModelState.IsValid)
            {

                try
                {
                    dbase.UpdateCustomer(Mapper.Map<Customer>(model));

                    return RedirectToAction("Bills");

                }
                catch (Exception e)
                {

                    ModelState.AddModelError("", e.Message);
                }
            }
            ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();
            return View(model);
        }


        //
        // GET: AddPayeeDetails

        public ActionResult AddPayeeDetails()
        {


            return View();
        }


        //
        // POST: AddPayeeDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPayeeDetails(PayeeModel model)
        {
            
            if (ModelState.IsValid)
            {

                try
                {
                    string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

                    int UserId = WebSecurity.GetUserId(name);
                    model.UserId = UserId;
                    Payee payee = new Payee();
                    payee.UserId = model.UserId;
                    payee.Payee1 = model.Payee;
                    payee.PayeeAddress = model.PayeeAddress;
                    payee.PayeeAddress2 = model.PayeeAddress2;
                    payee.PayeeCity = model.PayeeCity;
                    payee.PayeeComments = model.PayeeComments;
                    payee.PayeeContactPerson = model.PayeeContactPerson;
                    payee.PayeeCountry = model.PayeeCountry;
                    payee.PayeeEmail = model.PayeeEmail;
                    payee.PayeeState = model.PayeeState;
                    payee.PayeeZipCode = model.PayeeZipCode;
                    payee.PayeeWebsite = model.PayeeWebsite;
                    dbase.InsertPayee(payee);
                    return RedirectToAction("Payees");

                }
                catch (Exception e)
                {

                    ModelState.AddModelError("", e.Message);
                }
            }
            ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();
            return View();
        }

        //
        // GET: EditPayeeDetails

        public ActionResult EditPayeeDetails(int PayeeId)
        {
            
            PayeeModel model = new PayeeModel();
            try
            {
                model = Mapper.Map<PayeeModel>(dbase.GetPayee(PayeeId));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }


        //
        // POST: EditPayeeDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPayeeDetails(PayeeModel model)
        {
            
            if (ModelState.IsValid)
            {

                try
                {
                    dbase.UpdatePayee(Mapper.Map<Payee>(model));
                    return RedirectToAction("Payees");

                }
                catch (Exception e)
                {

                    ModelState.AddModelError("", e.Message);
                }
            }
            ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();
            return View();
        }

        //
        // GET: DeleteCustomerDetails

        public ActionResult DeleteCustomerDetails(int CustomerId)
        {
            

            try
            {
                dbase.DeleteCustomer(CustomerId);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Bills");
        }

        // GET: DeleteCustomerDetails

        public ActionResult DeleteInvoice(int InvoiceId)
        {
            

            try
            {
                dbase.DeleteInvoice(InvoiceId);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Invoice", new { isSendmail = false });
        }

        public ActionResult DeletePayeeDetails(int PayeeId)
        {
            

            try
            {


                dbase.DeletePayee( PayeeId);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Payees");
        }

        [HttpPost]
        public ActionResult EMailCustomer1()
        {
            if (Request.Files.Count > 0)
            {

                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    HttpPostedFileBase file = files[i];
                }

                string CustomerSelected = Request.Params["CustomerSelected"];
                string EmailNote = Request.Params["EmailNote"];

            }


            return Json(true);
        }

        [HttpPost]
        public ActionResult EMailCustomern()//(string CustomerSelected, string EmailNote)
        {
            HttpPostedFileBase file = null;
            string filepath = null;
            if (Request.Files.Count > 0)
            {

                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;

                file = files[0];
                string datestr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt").Replace(":", "").Replace(".", "").Replace("-", "").Replace(" ", "").Replace("AM", "").Replace("PM", "");
                filepath = String.Concat(Server.MapPath("~"), "Files\\", datestr, file.FileName);

                file.SaveAs(filepath);
            }

            string CustomerSelected = Request.Params["CustomerSelected"];
            string EmailNote = Request.Params["EmailNote"];
            int groupId = Convert.ToInt32(Request.Params["GroupId"]);
            IMembershipService Mail = new AccountMembershipService();
            string[] array = CustomerSelected.Split(',').ToArray();

            foreach (string s in array)
            {
                CustomerDetail detail = dbase.GetCustomerDetails(Convert.ToInt32(s), groupId);
                if (detail.AmountDue > 0)
                {
                    InvoiceMailModel model = new InvoiceMailModel();
                    model.From = detail.PayeeEmail + "," + s;
                    model.Payee = detail.Payee;
                    model.To = detail.CustomerEmail;
                    model.Name = detail.CustomerFirstName + " " + detail.CustomerLastName;
                    model.Amount = detail.AmountDue.ToString();
                    char[] name = detail.CustomerFirstName != null ? detail.CustomerFirstName.Trim().ToCharArray() : null;
                    //model.ProjectNumber = name[0].ToString().ToUpper() + detail.UserId.ToString() + s + DateTime.Now.Month.ToString()+DateTime.Now.ToString("yy");
                    model.ProjectNumber = name[0].ToString().ToUpper() + detail.UserId.ToString() + s;
                    model.Sub = detail.BillDescription;
                    model.Note = EmailNote;
                    model.Message = "Please Pay the Bill As Soon As Possible";
                    model.IsReminder = false;
                    model.Type = MailType.EmailInvoice;
                    model.Attachment = filepath;
                    try
                    {

                        try
                        {
                            model.BillId = dbase.InsertInvoice(new Invoice { CustomerId = detail.CustomerId, UserId = detail.UserId, InvoiceNumber = model.ProjectNumber, Quantity = 1, TotalAmountDue = (decimal)detail.AmountDue, EmailSentDate = DateTime.Now,BillDesc=detail.BillDescription });
                            detail.Intervals = detail.Intervals - 1;
                            detail.NextBillDate = GetNextBillDate(detail.NextBillDate, detail.IntervalTypeId);
                            if (!Mail.SendEmail(model))
                            {
                                Mail.SendEmail(model);
                            }
                            if (groupId <= 0)
                            {
                                this.dbase.UpdateCustomer(new Customer()
                                {
                                    CustomerId = detail.CustomerId,
                                    PayeeId = detail.PayeeId,
                                    AmountDue = detail.AmountDue,
                                    CustomerAddress = detail.CustomerAddress,
                                    CustomerAddress2 = detail.CustomerAddress2,
                                    CustomerCity = detail.CustomerCity,
                                    CustomerCountry = detail.CustomerCountry,
                                    BillDescription = detail.BillDescription,
                                    CustomerEmail = detail.CustomerEmail,
                                    CustomerFirstName = detail.CustomerFirstName,
                                    CustomerLastName = detail.CustomerLastName,
                                    CustomerState = detail.CustomerState,
                                    CustomerZipCode = detail.CustomerState,
                                    Intervals = detail.Intervals,
                                    IntervalTypeId = detail.IntervalTypeId,
                                    NextBillDate = detail.NextBillDate,
                                    LateCharges = detail.LateCharges,
                                    BCCEmail = detail.BCCEmail,
                                    CCEmail = detail.CCEmail,
                                    CustomerPhoneNo = detail.CustomerPhoneNo,
                                    EmergencyPhoneNo = detail.EmergencyPhoneNo,
                                    LateChargesStartday = detail.LateChargesStartday,
                                    LateChargesStartdayAmount = detail.LateChargesStartdayAmount,
                                    LateChargesThereAfterday = detail.LateChargesThereAfterday,
                                    LateChargesThereAfterdayAmount = detail.LateChargesThereAfterdayAmount
                                });
                            }
                            else
                            {
                                List<CustomerDetail> cust = new List<CustomerDetail>();
                                cust.Add(detail);
                                this.dbase.UpdateBillDescription(cust, groupId);
                            }                           

                        }
                        catch (Exception e)
                        {

                            throw e;
                        }


                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }
            }

            return Json(true);
        }

        [HttpPost]
        public ActionResult EMailCustomer()
        {
            HttpPostedFileBase file = null;
            string filepath = null;
            if (Request.Files.Count > 0)
            {

                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;

                file = files[0];
                string datestr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt").Replace(":", "").Replace(".", "").Replace("-", "").Replace(" ", "").Replace("AM", "").Replace("PM", "");
                filepath = String.Concat(Server.MapPath("~"), "Files\\", datestr, file.FileName);

                file.SaveAs(filepath);


            }

            string CustomerSelected = Request.Params["CustomerSelected"];
            string EmailNote = Request.Params["EmailNote"];

            IMembershipService Mail = new AccountMembershipService();
            string[] array = CustomerSelected.Split(',').ToArray();
            

            foreach (string s in array)
            {
                CustomerDetail detail = dbase.GetCustomerDetails(Convert.ToInt32(s));
                InvoiceMailModel model = new InvoiceMailModel();
                model.From = detail.PayeeEmail + "," + s;
                model.Payee = detail.Payee;
                model.To = detail.CustomerEmail;
                model.Name = detail.CustomerFirstName + " " + detail.CustomerLastName;
                model.Amount = detail.AmountDue.ToString();
                char[] name = detail.CustomerFirstName.Trim().ToCharArray();
                //model.ProjectNumber = name[0].ToString().ToUpper() + detail.UserId.ToString() + s + DateTime.Now.Month.ToString()+DateTime.Now.ToString("yy");
                model.ProjectNumber = name[0].ToString().ToUpper() + detail.UserId.ToString() + s;
                model.Sub = detail.BillDescription;
                model.Note = EmailNote;
                model.Message = "Please Pay the Bill As Soon As Possible";
                model.Type = MailType.EmailInvoice;
                model.IsReminder = false;
                model.Attachment = filepath;
                try
                {

                    try
                    {
                        model.BillId = dbase.InsertInvoice(new Invoice { CustomerId = detail.CustomerId, UserId = detail.UserId, InvoiceNumber = model.ProjectNumber, Quantity = 1, TotalAmountDue = (decimal)detail.AmountDue, EmailSentDate = DateTime.Now, BillDesc = detail.BillDescription });
                        detail.Intervals = detail.Intervals - 1;
                        detail.NextBillDate = GetNextBillDate(detail.NextBillDate, detail.IntervalTypeId);
                        
                        if (!Mail.SendEmail(model))
                        {
                            Mail.SendEmail(model);
                        }
                        dbase.UpdateCustomer(new Customer
                        {
                            CustomerId = detail.CustomerId,
                            PayeeId = detail.PayeeId,
                            AmountDue = detail.AmountDue,
                            CustomerAddress = detail.CustomerAddress,
                            CustomerAddress2 = detail.CustomerAddress2,
                            CustomerCity = detail.CustomerCity,
                            CustomerCountry = detail.CustomerCountry,
                            BillDescription = detail.BillDescription,
                            CustomerEmail = detail.CustomerEmail,
                            CustomerFirstName = detail.CustomerFirstName,
                            CustomerLastName = detail.CustomerLastName,
                            CustomerState = detail.CustomerState,
                            CustomerZipCode = detail.CustomerState,
                            Intervals = detail.Intervals,
                            IntervalTypeId = detail.IntervalTypeId,
                            NextBillDate = detail.NextBillDate,
                            LateCharges = detail.LateCharges,
                            LateChargesStartday = detail.LateChargesStartday,
                            LateChargesStartdayAmount = detail.LateChargesStartdayAmount,
                            LateChargesThereAfterday = detail.LateChargesThereAfterday,
                            LateChargesThereAfterdayAmount = detail.LateChargesThereAfterdayAmount
                        });
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            return Json(true);
        }


        [HttpPost]
        public ActionResult RemainderEmail()
        {

            HttpPostedFileBase file = null;
            string filepath = null;
            if (Request.Files.Count > 0)
            {

                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;

                file = files[0];
                string datestr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt").Replace(":", "").Replace(".", "").Replace("-", "").Replace(" ", "").Replace("AM", "").Replace("PM", "");
                filepath = String.Concat(Server.MapPath("~"), "Files\\", datestr, file.FileName);
                file.SaveAs(filepath);


            }

            string InvoiceSelected = Request.Params["InvoiceSelected"];
            string EmailNote = Request.Params["EmailNote"];
            IMembershipService Mail = new AccountMembershipService();
            string[] array = InvoiceSelected.Split(',').ToArray();
            

            foreach (string s in array)
            {
                InvoiceDetail detail = dbase.GetInvoiceDetails(Convert.ToInt32(s), IdType.InvoiceId).FirstOrDefault();
                InvoiceMailModel model = new InvoiceMailModel();
                model.From = detail.PayeeEmail + "," + detail.CustomerId;
                model.To = detail.CustomerEmail;
                model.Payee = detail.Payee;
                model.Name = detail.CustomerFirstName + " " + detail.CustomerLastName;
                model.Amount = detail.AmountDue.ToString();
                char[] name = detail.CustomerFirstName.ToCharArray();
                model.ProjectNumber = name[0].ToString().ToUpper() + detail.UserId.ToString() + detail.CustomerId;
                model.Sub = "Reminder Rent Bill for " + detail.BillDescription;
                model.Note = EmailNote;
                model.Message = "Please Pay the Bill As Soon As Possible";
                model.Type = MailType.EmailInvoice;
                model.BillId = detail.InvoiceId;
                model.IsReminder = true;
                model.Attachment = filepath;
                try
                {
                    if (Mail.SendEmail(model))
                    {
                        RemainderEmail remodel = new RemainderEmail(0, model.BillId, DateTime.Now, dbase);
                        remodel.Insert();
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            return Json(true);
        }




        public ActionResult Invoice(bool isSendmail = true)
        {

            
            int UserId = 0;
            string name = "";
            ViewBag.Title = isSendmail ? "Send Reminder Bills" : "Invoices";

            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

                UserId = dbase.GetUserId(name);

                //ViewBag.Results = dbase.GetInvoiceDetails(UserId, IdType.UserId, InvoiceType.Notpaid);

                var list= dbase.GetInvoiceDetails(UserId, IdType.UserId, InvoiceType.Notpaid);
                ViewBag.Results = LateChargesHelper.FindLateCharges(list);

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invoice(string InvoiceNumber, string Type, bool isSendmail, string UserName = "")
        {



            
            int UserId = 0;
            string name = "";
            //ViewBag.Users = CreateUserNameList(dbase.GetallUserName().ToList<string>());
            ViewBag.Title = isSendmail ? "Send Reminder Bills" : "Invoices";
            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

                UserId = dbase.GetUserId(name);

                if (Type == "Number")
                {
                    ViewBag.Results = this.dbase.GetInvoiceDetails(InvoiceNumber, InvoiceType.All);
                }
                else
                {
                    ViewBag.Results = this.dbase.GetInvoiceDetails(UserId, IdType.UserId, InvoiceType.All, Type, InvoiceNumber).ToList<InvoiceDetail>();
                }

                //if (Type == "Id")
                //{
                //    int id;
                //    if (int.TryParse(InvoiceNumber, out id))
                //    {
                //        ViewBag.Results = dbase.GetInvoiceDetails(UserId, IdType.UserId, InvoiceType.All).Select(a => a).Where(a => a.InvoiceId == id).ToList();
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("", "Please Enter Proper InvoiceId");
                //        ViewBag.Results = dbase.GetInvoiceDetails(UserId, IdType.UserId, InvoiceType.Notpaid);
                //    }


                //}
                //else
                //{

                //    ViewBag.Results = dbase.GetInvoiceDetails(InvoiceNumber, InvoiceType.All);
                //}


            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }


        public ActionResult ViewUsers()
        {
            
            ViewBag.Results = dbase.GetAllActiveUser().Select(a => a).Where(a => a.Name != User.Identity.Name).ToList<AccountUserModel>();
            return View();
        }

        public ActionResult ViewArchivedUsers()
        {
            
            ViewBag.Results = dbase.GetAllArchiveUser();

            return View();
        }

        public ActionResult LockUser(int UserId)
        {
            
            dbase.LockUser(UserId);
            return RedirectToAction("ViewUsers");
        }

        public ActionResult UnLockUser(int UserId)
        {
            
            dbase.UnLockUser(UserId);
            return RedirectToAction("ViewArchivedUsers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserSelection(string SelectedUserName)
        {

            System.Web.HttpContext.Current.Session["SelectedUserName"] = SelectedUserName;

            string returnUrl = Request.UrlReferrer.ToString();

            if (returnUrl.Contains("Group/SendBills"))
            {
                return RedirectToAction("ManageMembers", "Group", new { isSendmail = true });
            }
            else { return Redirect(returnUrl); }



        }
        public ActionResult Payments()
        {
            int userId = 0;
            try
            {
                string item = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? base.User.Identity.Name;
                userId = this.dbase.GetUserId(item);
                List<Payments> allPayments = this.dbase.GetAllPayments(userId, "payment");
                base.ViewBag.Results = this.GetPayments((
                    from a in allPayments
                    orderby a.PaidDate descending
                    select a).ToList<Payments>());
            }
            catch (Exception exception)
            {
                base.ModelState.AddModelError("", exception.Message);
            }
            return base.View();
        }

        private List<PaymentsDto> GetPayments(List<Payments> payment)
        {
            List<PaymentsDto> paymentsDtos = new List<PaymentsDto>();
            string str = base.Server.MapPath("/PrintedChecks/printedChecksPaymentIds.txt");
            string str1 = "";
            if (System.IO.File.Exists(str))
            {
                str1 = System.IO.File.ReadAllText(str);
            }
            List<int> list = (
                from x in str1.Split(new char[] { ',' })
                where !string.IsNullOrEmpty(x)
                select int.Parse(x)).ToList<int>();
            foreach (Payments payment1 in payment)
            {
                PaymentsDto paymentsDto = new PaymentsDto();
                if (list.Count<int>() <= 0)
                {
                    paymentsDto.Css = "";
                    paymentsDto.IsPrinted = false;
                }
                else if (list.Contains(payment1.PaymentId))
                {
                    paymentsDto.Css = "color-blue";
                    paymentsDto.IsPrinted = true;
                }
                else
                {
                    paymentsDto.Css = "color-red";
                    paymentsDto.IsPrinted = false;
                }
                paymentsDto.Amount = payment1.Amount;
                paymentsDto.FirstName = payment1.FirstName;
                paymentsDto.InvoiceId = payment1.InvoiceId;
                paymentsDto.LastName = payment1.LastName;
                paymentsDto.PaidDate = payment1.PaidDate;
                paymentsDto.Payee = payment1.Payee;
                paymentsDto.PayeeId = payment1.PayeeId;
                paymentsDto.PaymentId = payment1.PaymentId;
                paymentsDto.PaymentMode = payment1.PaymentMode;
                paymentsDto.UserId = payment1.UserId;
                paymentsDto.BillDescription = payment1.BillDescription;
                paymentsDtos.Add(paymentsDto);
            }
            return paymentsDtos;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payments(string Search)
        {
            
            int UserId = 0;
            string name = "";

            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                UserId = dbase.GetUserId(name);
                List<Payments> list = this.dbase.GetAllPayments(UserId, "payment");
                ViewBag.Results = list.Where(a => a.UserId == UserId).Select(a => a).OrderByDescending(a => a.PaidDate).ToList();



            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public FileResult CsvDownload(int PaymentId)
        {
            if (PaymentId > 0)
            {
                string[] paymentIds = new string[] { PaymentId.ToString() };
                //var PaymentId = PaymentIds.Split(',');
                var pModel = dbase.GetCustomerOnlineCheckDetail(paymentIds).FirstOrDefault();
                if (pModel != null)
                {
                    var fileName = PaymentId.ToString() + ".csv";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;fpayilename=" + fileName);
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(pModel.CsvFileData);
                    Response.Flush();
                    Response.End();
                    return null;
                }
                else
                {
                    List<InvoiceDetail> list = dbase.GetInvoiceDetails(PaymentId, IdType.PaymentId);
                    var monthYearFolder = DateTime.Now.Month + "." + DateTime.Now.Year;
                    string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + monthYearFolder + "/" + PaymentId + ".csv";
                    //string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + list[0].UserId + "/" + list[0].CustomerId + "/" + PaymentId + ".csv";
                    return File(Server.MapPath(path), "text/csv", Path.GetFileName(path));
                }
            }
            return null;

        }

        public void SavePrintedCheckIdInMemory(string PaymentId)
        {
            string data = System.IO.File.ReadAllText(Server.MapPath("/PrintedChecks/printedChecksPaymentIds.txt"));
            string[] list = data.Split(',');
            string memString = PaymentId + ",";
            // convert string to stream
            byte[] buffer = Encoding.ASCII.GetBytes(memString);
            MemoryStream ms = new MemoryStream(buffer);
            //write to file
            FileStream file = new FileStream(Server.MapPath("/PrintedChecks/printedChecksPaymentIds.txt"), FileMode.Append, FileAccess.Write);
            ms.WriteTo(file);
            file.Close();
            ms.Close();

        }

        public ActionResult PrintCheck(int PaymentId)
        {
            SavePrintedCheckIdInMemory(PaymentId.ToString());
            List<OnlineCheck> pModel = new List<OnlineCheck>();
            if (PaymentId > 0)
            {
                string[] paymentIds = new string[] { PaymentId.ToString() };
                //var PaymentId = PaymentIds.Split(',');
                pModel = dbase.GetCustomerOnlineCheckDetail(paymentIds);
                if (pModel.Count > 0)
                {
                    pModel = printCheckModelFromDB(pModel);

                }
                else
                {
                    var printModel = GetPCheckModel(Convert.ToInt32(PaymentId));
                    if (printModel != null)
                    {
                        pModel.Add(printModel);
                    }
                }
            }
            return View(pModel);
        }

        public ActionResult PrintSelectedCheck(string PaymentIds)
        {
            List<OnlineCheck> pModel = new List<OnlineCheck>();
            if (!string.IsNullOrEmpty(PaymentIds))
            {
                var PaymentId = PaymentIds.Split(',');
                pModel = dbase.GetCustomerOnlineCheckDetail(PaymentId);
                if (pModel.Count > 0)
                {
                    pModel = printCheckModelFromDB(pModel);
                }
                else
                {
                    for (int i = 0; i < PaymentId.Length; i++)
                    {
                        SavePrintedCheckIdInMemory(PaymentId[i].ToString());
                        var printModel = GetPCheckModel(Convert.ToInt32(PaymentId[i]));
                        if (printModel != null)
                        {
                            pModel.Add(printModel);
                        }
                    }
                }
            }

            return View("PrintCheck", pModel);
        }

        public List<OnlineCheck> printCheckModelFromDB(List<OnlineCheck> pModel)
        {
            foreach (var model in pModel)
            {
                if (model.CheckNumber.Length < 6)
                {
                    var diff = 6 - model.CheckNumber.Length;
                    var zeros = addChar('0', diff);
                    model.CheckNumber = zeros + model.CheckNumber;
                }

                if (model.RoutingNumber.Length < 9)
                {
                    var diff = 9 - model.RoutingNumber.Length;
                    var zeros = addChar('0', diff);
                    model.RoutingNumber = zeros + model.RoutingNumber;
                }

                MultiCurrency cur = new MultiCurrency(Criteria.Foreign);

                model.Comment = cur.ConvertToWord(Convert.ToDecimal(model.AmountOnCheck).ToString().Split('.')[0], System.Drawing.Color.Black) + "and " + Convert.ToDecimal(model.AmountOnCheck).ToString("0.00").Split('.')[1] + "/100";
                if (model.Comment.Length <= 90)
                {
                    var diff = 90 - model.Comment.Length;
                    var ast = addChar('*', diff);
                    model.Comment = model.Comment + ast;
                }
            }
            return pModel;
        }

        public OnlineCheck GetPCheckModel(int PaymentId)
        {
            OnlineCheck printModel = new OnlineCheck();

            List<InvoiceDetail> list = dbase.GetInvoiceDetails(PaymentId, IdType.PaymentId);
            var monthYearFolder = DateTime.Now.Month + "." + DateTime.Now.Year;
            string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + monthYearFolder + "/" + PaymentId + ".csv";
            //string path = ConfigurationManager.AppSettings["CheckPath"] + "/" + list[0].UserId + "/" + list[0].CustomerId + "/" + PaymentId + ".csv";
            if (System.IO.File.Exists(Server.MapPath(path)))
            {
                var lines = System.IO.File.ReadAllLines(Server.MapPath(path)).Select(a => a.Split(',')).ToArray();
                CheckOutModel model = new CheckOutModel();
                if (lines.Count() > 0)
                {
                    printModel.CustomerName = lines[0][9].ToString();

                    printModel.Address = lines[0][10].ToString();
                    printModel.Address2 = lines[0][11].ToString();
                    printModel.PhoneNumber = lines[0][7].ToString();
                    printModel.Email = lines[0][8].ToString();

                    //CheckModel cmodel = new CheckModel();
                    printModel.PayeeName = lines[0][15].ToString();
                    printModel.AmountOnCheck = Convert.ToDecimal(lines[0][14].ToString());

                    printModel.CheckNumber = lines[0][13].ToString().Trim();
                    if (printModel.CheckNumber.Length < 6)
                    {
                        var diff = 6 - printModel.CheckNumber.Length;
                        var zeros = addChar('0', diff);
                        printModel.CheckNumber = zeros + printModel.CheckNumber;
                    }
                    printModel.BankName = lines[0][16].ToString();
                    printModel.BankAddress = lines[0][17].ToString();
                    printModel.BankCity = lines[0][18].ToString();
                    printModel.DateOnCheck = Convert.ToDateTime(lines[0][12].ToString());
                    printModel.RoutingNumber = lines[0][19].ToString().Trim();
                    if (printModel.RoutingNumber.Length < 9)
                    {
                        var diff = 9 - printModel.RoutingNumber.Length;
                        var zeros = addChar('0', diff);
                        printModel.RoutingNumber = zeros + printModel.RoutingNumber;
                    }
                    printModel.AccountNumber = lines[0][20].ToString().Trim();
                    MultiCurrency cur = new MultiCurrency(Criteria.Foreign);

                    printModel.Comment = cur.ConvertToWord(lines[0][14].ToString().Split('.')[0], System.Drawing.Color.Black) + "and " + lines[0][14].ToString().Split('.')[1] + "/100";
                    if (printModel.Comment.Length <= 90)
                    {
                        var diff = 90 - printModel.Comment.Length;
                        var ast = addChar('*', diff);
                        printModel.Comment = printModel.Comment + ast;
                    }
                    if (lines[0].Length >= 21)
                    {
                        printModel.CheckMemo = lines[0][21].ToString();
                    }
                    //printModel.CheckOutModel = model;
                    //printModel.CheckModel = cmodel;
                }
            }
            return printModel;
        }

        static string addChar(Char character,int n)
        {
            return new String(character, n);
        }

       
      


        public ActionResult PayPalSetup()
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);
            
            List<SelectListItem> payeelist = dbase.GetAllPayee( UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();
            ViewBag.payeelist = payeelist;
            if (payeelist.Count > 0)
            {
                PayPalAccountDetail dtl = dbase.GetPayPalAccountDetail(Convert.ToInt32(payeelist[0].Value));
                PayPalAccountModel model = new PayPalAccountModel();

                if (dtl != null)
                {
                    model.PayeeId = dtl.PayeeId;
                    model.ClientId = dtl.ClientID;
                    model.PayPalAccountId = dtl.PayPalAccountId;
                    model.ClientSecretId = dtl.ClientSecret;
                    model.PayPalSurCharge = dtl.PayPalSurCharge;
                }
                return View(model);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayPalSetup(PayPalAccountModel model)
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);
            
            ViewBag.payeelist = dbase.GetAllPayee( UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

            if (ModelState.IsValid)
            {
                PayPalAccountDetail dtl = new PayPalAccountDetail();
                dtl.PayPalAccountId = model.PayPalAccountId;
                dtl.PayeeId = model.PayeeId;
                dtl.ClientID = model.ClientId;
                dtl.ClientSecret = model.ClientSecretId;
                dtl.PayPalSurCharge = model.PayPalSurCharge;
                dtl.InsertedOn = DateTime.Now;
                dtl.UpdatedOn = DateTime.Now;


                if (model.PayPalAccountId == 0)
                {
                    dbase.InsertPaypalAccountDetail(dtl);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    dbase.UpdatePapPalAcountDetails(dtl);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetPaypalDetails(int PayeeId)
        {
            
            PayPalAccountDetail dtl = dbase.GetPayPalAccountDetail(PayeeId);

            if (dtl != null)
            {
                PayPalAccountModel model = new PayPalAccountModel();
                model.PayPalAccountId = dtl.PayPalAccountId;
                model.ClientId = dtl.ClientID;
                model.ClientSecretId = dtl.ClientSecret;
                return Json(model);
            }
            else
            {
                return Json("");
            }
        }


        public ActionResult EmailedReport()
        {
            
            int UserId = 0;
            string name = "";
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            ReportDurationModel model = new ReportDurationModel();
            model.Month = 0;
            model.Year = 0;
            model.Payee = 0;
            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                UserId = dbase.GetUserId(name);
                List<EmailedReport> list = dbase.GetEmailSummaryReport(UserId);//.OrderBy(a => a.Date).ToList();
                //ViewBag.Results = list.Select(a => a).OrderByDescending(a => a.Date).ToList();
                Session["EmailedReport"] = list;
                ViewBag.Results = list.OrderByDescending(a => a.EmailSentDate).ToList<EmailedReport>();
                

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailedReport(ReportDurationModel model)
        {            
            int UserId = 0;
            string name = "";
            List<EmailedReport> list = null;
            try
            {
                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                UserId = dbase.GetUserId(name);
                if (Session["EmailedReport"] == null)
                {
                    name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                    UserId = dbase.GetUserId(name);
                    list = dbase.GetEmailSummaryReport(UserId).ToList();//.OrderByDescending(a => a.Date).ToList();
                    Session["EmailedReport"] = list;
                }
                else
                {
                    list = (List<EmailedReport>)Session["EmailedReport"];
                }
                if (string.IsNullOrEmpty(model.PayeeIds))
                {
                    ViewBag.Results = list.Where(a => a.EmailSentDate.Month == model.Month && a.EmailSentDate.Year == model.Year).OrderByDescending(a => a.EmailSentDate).ToList<EmailedReport>(); ;//.Where(a => a.Date.Month == model.Month && a.Date.Year == model.Year).OrderByDescending(a => a.Date).ToList<EmailSummaryReport>();
                }
                else
                {
                    var payeeIds = model.PayeeIds.Split(',').Select(int.Parse).ToList();
                    ViewBag.Results = list.Where(a => a.EmailSentDate.Month == model.Month && a.EmailSentDate.Year == model.Year && payeeIds.Contains(a.PayeeId)).OrderByDescending(a => a.EmailSentDate).ToList<EmailedReport>(); ;//.Where(a => a.Date.Month == model.Month && a.Date.Year == model.Year).OrderByDescending(a => a.Date).ToList<EmailSummaryReport>();
                }
                ViewBag.monthlist = CreateMonthList();
                ViewBag.yearlist = list.GroupBy(a => a.EmailSentDate.Year).Select(b => new SelectListItem { Text = b.Key.ToString(), Value = b.Key.ToString() }).Distinct().ToList();//GroupBy(a => a.).Select(a=>new SelectListItem{ Text= a.}).ToList();
                ViewBag.PayeeList = CreatePayeeList(list, model);


            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }




            return PartialView("_EmailedReports",model);
        }


        public ActionResult RemainderEmailedReport()
        {
            
            int UserId = 0;
            string name = "";
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            ReportDurationModel model = new ReportDurationModel();
            model.Month = 0;
            model.Year = 0;
            model.Payee = 0;

            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                UserId = dbase.GetUserId(name);
                List<EmailedReport> list = dbase.GetRemainderEmailSummaryReport(UserId).ToList();
                //ViewBag.Results = list.Select(a => a).OrderByDescending(a => a.Date).ToList();
                var payeeList = CreatePayeeList(list, model);
                Session["RemainderEmailReport"] = list;
                ViewBag.Results = list.OrderByDescending(a => a.EmailSentDate).ToList<EmailedReport>(); ;//.Where(a => a.Date.Month == month && a.Date.Year == year).OrderByDescending(a => a.Date).ToList<EmailSummaryReport>();
                ViewBag.monthlist = CreateMonthList();
                ViewBag.yearlist = list.GroupBy(a => a.EmailSentDate.Year).Select(b => new SelectListItem { Text = b.Key.ToString(), Value = b.Key.ToString() }).ToList();//GroupBy(a => a.).Select(a=>new SelectListItem{ Text= a.}).ToList();
                ViewBag.PayeeList = payeeList;               
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemainderEmailedReport(ReportDurationModel model)
        {
            
            int UserId = 0;
            string name = "";
            List<EmailedReport> list = null;
            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                UserId = dbase.GetUserId(name);
                if (Session["RemainderEmailReport"] == null)
                {
                    name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                    UserId = dbase.GetUserId(name);
                    list = dbase.GetRemainderEmailSummaryReport(UserId);//.OrderByDescending(a => a.Date).ToList();
                    Session["RemainderEmailReport"] = list;

                }
                else
                {
                    list = (List<EmailedReport>)Session["RemainderEmailReport"];
                }
                if (string.IsNullOrEmpty(model.PayeeIds))
                {
                    ViewBag.Results = list.Where(a => a.EmailSentDate.Month == model.Month && a.EmailSentDate.Year == model.Year).OrderByDescending(a => a.EmailSentDate).ToList<EmailedReport>(); ;//.Where(a => a.Date.Month == model.Month && a.Date.Year == model.Year).OrderByDescending(a => a.Date).ToList<EmailSummaryReport>();
                }
                else
                {
                    var payeeIds = model.PayeeIds.Split(',').Select(int.Parse).ToList();
                    ViewBag.Results = list.Where(a => a.EmailSentDate.Month == model.Month && a.EmailSentDate.Year == model.Year && payeeIds.Contains(a.PayeeId)).OrderByDescending(a => a.EmailSentDate).ToList<EmailedReport>(); ;//.Where(a => a.Date.Month == model.Month && a.Date.Year == model.Year).OrderByDescending(a => a.Date).ToList<EmailSummaryReport>();
                }
                ViewBag.monthlist = CreateMonthList();
                ViewBag.yearlist = list.GroupBy(a => a.EmailSentDate.Year).Select(b => new SelectListItem { Text = b.Key.ToString(), Value = b.Key.ToString() }).ToList();//GroupBy(a => a.).Select(a=>new SelectListItem{ Text= a.}).ToList();
                ViewBag.PayeeList = CreatePayeeList(list, model);

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }




            return PartialView("_ReminderEmailedReports", model);
        }


        public ActionResult PaymentsReport()
        {
            
            int UserId = 0;
            string name = "";

            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                UserId = dbase.GetUserId(name);
                List<Payments> list = dbase.GetAllPayments(UserId);
                ViewBag.Results = list.Where(a => a.UserId == UserId).Select(a => a).OrderByDescending(a => a.PaidDate).ToList();



            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult UnPaidBillsReport()
        {


            int UserId = 0;
            string name = "";           

            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;

                UserId = dbase.GetUserId(name);

                var list = dbase.GetInvoiceDetails(UserId, IdType.UserId, InvoiceType.Notpaid);
                ViewBag.Results = LateChargesHelper.FindLateCharges(list);

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult PaymentReport()
        {
            
            int UserId = 0;
            string name = "";
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            ReportDurationModel model = new ReportDurationModel();
            try
            {

                name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                UserId = dbase.GetUserId(name);
                List<PaymentSummaryReport> list = dbase.GetPaymentsSummaryReport(UserId);
                Session["PaymentsReport"] = list;
                ViewBag.Results = list.Where(a => a.Date.Month == month && a.Date.Year == year).OrderByDescending(a => a.Date).ToList<PaymentSummaryReport>();
                ViewBag.monthlist = CreateMonthList();
                ViewBag.yearlist = list.GroupBy(a => a.Date.Year).Select(b => new SelectListItem { Text = b.Key.ToString(), Value = b.Key.ToString() }).ToList();//GroupBy(a => a.).Select(a=>new SelectListItem{ Text= a.}).ToList();

                model.Month = month;
                model.Year = year;

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentReport(ReportDurationModel model)
        {
            
            int UserId = 0;
            string name = "";
            List<PaymentSummaryReport> list = null;


            try
            {
                if (Session["PaymentsReport"] == null)
                {
                    name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                    UserId = dbase.GetUserId(name);
                    list = dbase.GetPaymentsSummaryReport(UserId).OrderByDescending(a => a.Date).ToList();
                    Session["PaymentsReport"] = list;

                }
                else
                {
                    list = (List<PaymentSummaryReport>)Session["PaymentsReport"];
                }
                ViewBag.Results = list.Where(a => a.Date.Month == model.Month && a.Date.Year == model.Year).OrderByDescending(a => a.Date).ToList<PaymentSummaryReport>();
                ViewBag.monthlist = CreateMonthList();
                ViewBag.yearlist = list.GroupBy(a => a.Date.Year).Select(b => new SelectListItem { Text = b.Key.ToString(), Value = b.Key.ToString() }).ToList();//GroupBy(a => a.).Select(a=>new SelectListItem{ Text= a.}).ToList();


            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
            }




            return View(model);
        }

        #region Helpers

        private bool FileIsValidate(string FileName)
        {
            bool result = true;

            if (Path.GetExtension(FileName).ToLower() != ".xls")
            {
                result = false;
            }

            return result;

        }

        private void ImportFile(string FilePath, int PayeeId = 0)
        {

            string oledbcon = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;FMT=Delimited'";


            using (OleDbConnection olefileconnection = new OleDbConnection(oledbcon))
            {

                Customer cust = null;



                

                string Payee1 = "";
                string PayeeAddress = "";
                string PayeeAddress2 = "";
                string PayeeCity = "";
                string PayeeComments = "";
                string PayeeContactPerson = "";
                string PayeeCountry = "";
                string PayeeEmail = "";
                string PayeeState = "";
                string PayeeZipCode = "";
                string PayeeWebsite = "";


                try
                {
                    string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                    int UserId = WebSecurity.GetUserId(name);

                    olefileconnection.Open();

                    string Query = @" Select * from [customer_payee_form$]";

                    OleDbCommand command = new OleDbCommand(Query, olefileconnection);
                    //However, if you create the items manually in Access they seem to query fine
                    using (OleDbDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {

                            if (PayeeId == 0)
                            {

                                if (rdr["payee"].ToString() != "")
                                {
                                    Payee1 = rdr["payee"].ToString();
                                    PayeeAddress = rdr["payeeaddress"].ToString();
                                    PayeeAddress2 = rdr["payeeaddress2"].ToString();
                                    PayeeCity = rdr["payeecity"].ToString();
                                    PayeeComments = rdr["payeecomments"].ToString();
                                    PayeeContactPerson = rdr["payeecontactperson"].ToString();
                                    PayeeCountry = rdr["payeecountry"].ToString();
                                    PayeeEmail = rdr["payeeemail"].ToString();
                                    PayeeState = rdr["payeestate"].ToString();
                                    PayeeZipCode = rdr["payeezipcode"].ToString();
                                    PayeeWebsite = rdr["website"].ToString();

                                    //Payee payee = new Payee(UserId, Payee1, PayeeAddress, PayeeAddress2, PayeeCity, PayeeComments, PayeeContactPerson, PayeeCountry, PayeeEmail, PayeeState, PayeeZipCode, PayeeWebsite, dbase);
                                    Payee payee = new Payee();
                                    payee.UserId = UserId;
                                    payee.Payee1 = Payee1;
                                    payee.PayeeAddress = PayeeAddress;
                                    payee.PayeeAddress2 = PayeeAddress2;
                                    payee.PayeeCity = PayeeCity;
                                    payee.PayeeComments = PayeeComments;
                                    payee.PayeeContactPerson = PayeeContactPerson;
                                    payee.PayeeCountry = PayeeCountry;
                                    payee.PayeeEmail = PayeeEmail;
                                    payee.PayeeState = PayeeState;
                                    payee.PayeeZipCode = PayeeZipCode;
                                    payee.PayeeWebsite = PayeeWebsite;
                                    dbase.InsertPayee(payee);
                                }
                            }
                            else
                            {
                                if (rdr["firstname"].ToString() != "")
                                {
                                    cust = new Customer();
                                    cust.PayeeId = PayeeId;
                                    cust.CustomerFirstName = rdr["firstname"].ToString();
                                    cust.CustomerLastName = rdr["lastname"].ToString();
                                    cust.CustomerAddress = rdr["address"].ToString();
                                    cust.CustomerAddress2 = rdr["address2"].ToString();
                                    cust.CustomerCity = rdr["city"].ToString();
                                    cust.CustomerState = rdr["state"].ToString();
                                    cust.CustomerZipCode = rdr["zipcode"].ToString();
                                    cust.CustomerCountry = rdr["country"].ToString();
                                    cust.CustomerEmail = rdr["email"].ToString();
                                    cust.AmountDue = Convert.ToDecimal(rdr["amountdue"].ToString());
                                    cust.BillDescription = rdr["billdescription"].ToString();
                                    cust.NextBillDate = DateTime.Now.Date;
                                    cust.Intervals = 12;
                                    cust.IntervalTypeId = 1;
                                    if (rdr["latecharges"].ToString().ToLower() == "yes")
                                    {
                                        cust.LateCharges = true;
                                    }
                                    else
                                    {
                                        cust.LateCharges = false;
                                    }
                                    cust.LateChargesStartday = Convert.ToInt32(rdr["latechargesstartday"].ToString());
                                    cust.LateChargesStartdayAmount = Convert.ToDecimal(rdr["latechargesstartdayamount"].ToString());
                                    cust.LateChargesThereAfterday = Convert.ToInt32(rdr["latechargesthereafterday"].ToString());
                                    cust.LateChargesThereAfterdayAmount = Convert.ToDecimal(rdr["latechargesthereafterdayamount"].ToString());

                                    dbase.InsertCustomer(cust);//inserting the Customer.
                                }
                            }




                        }
                        //Response.Write(cust.firstname);//
                    }

                }




                catch (Exception)
                {
                    throw;
                }


            }


        }


        public List<SelectListItem> CreateEmailNote()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "" });
            list.Add(new SelectListItem { Text = "Happy Holiday", Value = "Happy Holiday" });
            list.Add(new SelectListItem { Text = "Reminder Bill", Value = "Reminder Bill" });
            list.Add(new SelectListItem { Text = "No Later Charge for this Month", Value = "No Later Charge for this Month" });
            return list;
        }

        public DateTime GetNextBillDate(DateTime dat, int type)
        {
            switch (type)
            {
                case 1:
                    dat = dat.AddMonths(1);
                    break;

                case 2:
                    dat = dat.AddDays(14);
                    break;

                case 3:
                    dat = dat.AddDays(7);
                    break;

            }

            return dat;
        }

        public List<SelectListItem> CreateMonthList()
        {
            List<SelectListItem> list = new List<SelectListItem>(){
                       new SelectListItem(){ Text="January", Value="1"},
                       new SelectListItem(){ Text="February", Value="2"},
                       new SelectListItem(){ Text="March", Value="3"},
                       new SelectListItem(){ Text="April", Value="4"},
                       new SelectListItem(){ Text="May", Value="5"},
                       new SelectListItem(){ Text="June", Value="6"},
                       new SelectListItem(){ Text="July", Value="7"},
                       new SelectListItem(){ Text="August", Value="8"},
                       new SelectListItem(){ Text="September", Value="9"},
                       new SelectListItem(){ Text="October", Value="10"},
                       new SelectListItem(){ Text="November", Value="11"},
                       new SelectListItem(){ Text="December", Value="12"},
                       
                   };
            return list;
        }

        public List<SelectListItem> CreatePayeeList(List<EmailedReport> list, ReportDurationModel model)
        {
            List<SelectListItem> selectList = new List<SelectListItem>() { new SelectListItem() { Text = "Select Payee", Value = "0" } };
            var reportData = list.Where(x => x.EmailSentDate.Month == model.Month && x.EmailSentDate.Year == model.Year).ToList();
            foreach (var custList in reportData)
            {
                if (custList.TotalAmountDue > 0)
                {
                    var name = custList.Payee;
                    var item = new SelectListItem() { Text = name, Value = custList.PayeeId.ToString() };
                    var count = selectList.Where(x => x.Value == custList.PayeeId.ToString()).Count();
                    if (count == 0)
                    {
                        selectList.Add(item);
                    }
                }
            }
            
           
            return selectList.Distinct().ToList();
        }

        #endregion
    }
}
