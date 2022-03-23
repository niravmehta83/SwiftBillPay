using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BingHousingMVC.Filters;
using BingHousing_BO;
using BingHousingMVC_DAL;
using BingHousingMVC.Models;
using WebMatrix.WebData;
using BingHousingMVC.GlobalOperations;
using AutoMapper;

namespace BingHousingMVC.Controllers
{
    [DenyAccessToUser(Roles = "Customer")]
    public class GroupController : Controller
    {
        private IBHDbase dbase = new BHDbase();

        public ActionResult Create()
        {            
            GroupModel model = new GroupModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GroupModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (model.GroupName != null && !string.IsNullOrWhiteSpace(model.GroupName))
                {
                    int UserId = dbase.GetUserId(System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name);
                    int existingId = dbase.GetGroupId(model.GroupName, UserId);
                    if (existingId == 0)
                    {
                        Group dbGroup = new Group
                        {
                            GroupName = model.GroupName,
                            GroupDescription = model.GroupDescription,
                            IsActive = true,
                            CreatedOn = DateTime.Now,
                            LastModified = DateTime.Now,
                            UserId = UserId
                        };

                        dbase.InsertGroup(dbGroup);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Group with the same name already exists");
                        return View(model);
                    }
                }

                return RedirectToAction("ManageGroup", "Group");
            }
            else
            {
                return View(model);
            }
        }


        public ActionResult ManageGroup()
        {
            
            string username = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            List<GroupModel> groups = GroupModel.GetGroupsByUserName(username);

            ViewBag.Groups = groups;

            return View();
        }

        [HttpPost]
        public JsonResult UpdateBillDesc()
        {
            bool success = false;
            string CustomertSelected = Request.Params["CustomertSelected"];
            if (!string.IsNullOrEmpty(CustomertSelected))
            {                
                string BillDesc = Request.Params["BillDesc"];
                string[] array = CustomertSelected.Split(',').ToArray();
                int GroupId = Convert.ToInt32(Request.Params["GroupId"]);
                dbase.UpdateBillDesc(array, BillDesc.Trim(),GroupId);
            }
            success = true;
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageMembers(bool isSendmail = false)
        {
            
            string username = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            List<GroupModel> groups = GroupModel.GetGroupsByUserName(username);

            ViewBag.Groups = groups;

            ViewBag.isSendmail = isSendmail;
            return View();
        }


        public ActionResult Edit(int groupId)
        {
            

            GroupModel grpModel = GroupModel.GetGroupByGroupId(groupId);

            return View(grpModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GroupModel grpModel)
        {
            

            if (ModelState.IsValid)
            {
                try
                {
                    dbase.UpdateGroup(grpModel.ToGroup());
                }
                catch (Exception e)
                {
                    throw e;
                }

                return RedirectToAction("ManageGroup");
            }

            return View(grpModel);


        }

        public ActionResult ViewMembers(int groupId)
        {
            

            string username = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int userId = dbase.GetUserId(username);

            GroupModel grpModel = GroupModel.GetGroupByGroupId(groupId);

            List<CustomerDetail> allMembers = dbase.GetCustomerList(userId);
            foreach (var item in grpModel.GroupMembers)
            {
                var objCustomerDetails = allMembers.Where(c => c.CustomerId == item.CustomerId).FirstOrDefault();
                if (objCustomerDetails != null)
                {
                    allMembers.Remove(objCustomerDetails);
                }
            }
            ViewBag.AllCustomers = allMembers;
            return View(grpModel);
        }

        public ActionResult AddMembers(int groupId)
        {
            

            string username = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int userId = dbase.GetUserId(username);

            GroupModel grpModel = GroupModel.GetGroupByGroupId(groupId);

            List<CustomerDetail> allMembers = dbase.GetCustomerList(userId);
            foreach (var item in grpModel.GroupMembers)
            {
                var objCustomerDetails = allMembers.Where(c => c.CustomerId == item.CustomerId).FirstOrDefault();
                if (objCustomerDetails != null)
                {
                    allMembers.Remove(objCustomerDetails);
                }
            }
            ViewBag.AllCustomers = allMembers;
            return View(grpModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMembers(string hdnSelectedMember)
        {
            

            try
            {
                if (!string.IsNullOrEmpty(hdnSelectedMember))
                {
                    string[] param = hdnSelectedMember.Split(',');

                    int groupId = 0;
                    Int32.TryParse(param[0].Trim(), out groupId);

                    if (groupId != 0)
                    {
                        string username = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                        int userId = dbase.GetUserId(username);
                        GroupModel grpModel = GroupModel.GetGroupByGroupId(groupId);
                        if (grpModel != null)
                        {
                            for (int i = 0; i < param.Length; i++)
                            {
                                if (param[i] != null && !string.IsNullOrEmpty(param[i]))
                                {
                                    int customerId = 0;
                                    Int32.TryParse(param[i].Replace("<", "").Replace(">", ""), out customerId);
                                    if (customerId != 0)
                                    {
                                        GroupMemberModel currenCustomer = grpModel.GroupMembers.Where(gm => gm.CustomerId == customerId).FirstOrDefault();
                                        if (currenCustomer != null)
                                        {
                                            dbase.DeleteGroupMember(currenCustomer.GroupMemberId,false);
                                        }

                                        grpModel.GroupMembers.Remove(currenCustomer);
                                    }
                                }

                                List<CustomerDetail> allMembers = dbase.GetCustomerList(userId);
                                foreach (var item in grpModel.GroupMembers)
                                {
                                    var objCustomerDetails = allMembers.Where(c => c.CustomerId == item.CustomerId).FirstOrDefault();
                                    if (objCustomerDetails != null)
                                    {
                                        allMembers.Remove(objCustomerDetails);
                                    }
                                }
                                ViewBag.AllCustomers = allMembers;


                               
                            }
                            return View(grpModel);
                        }
                        else
                        {
                            throw new Exception("Group model could not be found for id " + groupId);
                        }
                    }
                    else
                    {
                        throw new Exception("Parameter invalid!");
                    }
                }
                else
                {
                    throw new Exception("Parameter invalid!");
                }
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer(string hdnAddMember)
        {
            

            try
            {
                if (!string.IsNullOrEmpty(hdnAddMember))
                {
                    string[] param = hdnAddMember.Split(',');

                    int groupId = 0;
                    Int32.TryParse(param[0].Trim(), out groupId);

                    if (groupId != 0)
                    {
                        GroupModel grpModel = GroupModel.GetGroupByGroupId(groupId);

                        if (grpModel != null)
                        {
                            for (int i = 1; i < param.Length; i++)
                            {
                                if (param[i] != null && !string.IsNullOrEmpty(param[i]))
                                {
                                    int customerId = 0;
                                    Int32.TryParse(param[i].Replace("<", "").Replace(">", ""), out customerId);
                                    if (customerId != 0)
                                    {
                                        GroupMember grpMember = new GroupMember
                                        {
                                            CustomerId = customerId,
                                            GroupId = grpModel.GroupId,
                                            AddedOn = DateTime.Now,
                                            LastModified = DateTime.Now,
                                            IsActive = true
                                        };
                                        dbase.InsertGroupMember(grpMember);
                                    }
                                }
                            }
                            return RedirectToAction("AddMembers", "Group", new { groupId = groupId });
                        }
                        else
                        {
                            throw new Exception("Group model could not be found for id " + groupId);
                        }
                    }
                    else
                    {
                        throw new Exception("Parameter invalid!");
                    }
                }
                else
                {
                    throw new Exception("Parameter invalid!");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public ActionResult AddNewCustomer(string hdnGroupId)
        {
            string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            int UserId = WebSecurity.GetUserId(name);
            

            ViewBag.GroupId = hdnGroupId;
            ViewBag.payeelist = dbase.GetAllPayee(UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();
            ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewCustomer(CustomerModel model, string hdnGroupId)
        {

            
            int groupId = 0;
            Int32.TryParse(hdnGroupId, out groupId);

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
                }
                catch (Exception e)
                {

                    ModelState.AddModelError("", e.Message);

                    string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                    int UserId = WebSecurity.GetUserId(name);
                    ViewBag.GroupId = hdnGroupId;
                    ViewBag.payeelist = dbase.GetAllPayee( UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

                    ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();
                    return View(model);
                }

                try
                {

                    if (groupId > 0)
                    {
                        Customer customer = dbase.GetCustomerByName(model.CustomerFirstName + model.CustomerLastName);

                        if (customer != null)
                        {
                            GroupMember gm = new GroupMember
                            {
                                CustomerId = customer.CustomerId,
                                GroupId = groupId,
                                AddedOn = DateTime.Now,
                                LastModified = DateTime.Now,
                                IsActive = true
                            };

                            dbase.InsertGroupMember(gm);
                        }
                    }


                }
                catch (Exception)
                {
                    ModelState.AddModelError("AddInGroupFailed", "Customer created, but uanble to add in this group");
                }

                return RedirectToAction("AddMembers", "Group", new { groupId = groupId });
            }
            else
            {
                string name = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                int UserId = WebSecurity.GetUserId(name);
                ViewBag.GroupId = hdnGroupId;
                ViewBag.payeelist = dbase.GetAllPayee(UserId).Select(a => new SelectListItem { Text = a.Payee1, Value = a.PayeeId.ToString() }).ToList<SelectListItem>();

                ViewBag.IntervalType = dbase.GetAllIntervalTypes().Select(a => new SelectListItem { Value = a.IntervalTypeId.ToString(), Text = a.IntervalTypeDescription }).ToList<SelectListItem>();
                return View(model);
            }
        }


        public ActionResult SelectGroups()
        {
            
            string username = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
            List<GroupModel> groups = GroupModel.GetGroupsByUserName(username);

            ViewBag.Groups = groups;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectGroups(string hdnSelectedGroups)
        {
            

            try
            {
                if (!string.IsNullOrEmpty(hdnSelectedGroups))
                {
                    string[] param = hdnSelectedGroups.Split(',');

                    int userId = 0;
                    userId = dbase.GetUserId(param[0]);

                    if (userId != 0)
                    {
                        List<int> selectedGroupId = new List<int>();
                        for (int i = 1; i < param.Length; i++)
                        {
                            if (param[i] != null && !string.IsNullOrEmpty(param[i]))
                            {
                                int groupId = 0;
                                Int32.TryParse(param[i].Replace("<", "").Replace(">", ""), out groupId);

                                selectedGroupId.Add(groupId);
                            }
                        }

                        TempData["CustomerDetails"] = dbase.GetCustomerDetailsByGroupIds(selectedGroupId, userId);

                        return RedirectToAction("SendBills", "Group");
                    }
                    else
                    {
                        throw new Exception("Parameter invalid!");
                    }
                }
                else
                {
                    throw new Exception("Parameter invalid!");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult SendBills(string GroupId)
        {
            var model = getSendBillsModel(GroupId);
            return View(model);
        }

        public SendBillsModel getSendBillsModel(string GroupId)
        {

            try
            {
                int gid = Convert.ToInt32(GroupId);
                List<int> groupIds = new List<int>();

                groupIds.Add(gid);
                string username = System.Web.HttpContext.Current.Session["SelectedUserName"] as string ?? User.Identity.Name;
                int userId = dbase.GetUserId(username);
                var model = dbase.GetCustomerDetailsForSendBillsByGroupIds(groupIds, userId);
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult DeleteGroup(int GroupId)
        {
            
            dbase.DeleteGroup(GroupId);
            return RedirectToAction("ManageGroup");
        }

        [HttpPost]
        public ActionResult UpdateDescription(List<CustomerDetail> details , int GroupId)
        {
            try
            {
                dbase.UpdateBillDescription(details,GroupId);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
            
        }
    }
}
