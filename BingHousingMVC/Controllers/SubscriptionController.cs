using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BingHousing_BO;
using BingHousingMVC_DAL;
using BingHousingMVC.Models;
using BingHousingMVC.GlobalOperations;
namespace BingHousingMVC.Controllers
{
    public class SubscriptionController : Controller
    {
        //
        // GET: /Subscription/

        public ActionResult ShowSubscriptions()
        {
            ViewBag.Planlist = GetPlanList();
            return View();
        }

        public ActionResult AddSubscriptionPlan()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubscriptionPlan(PlanModel model)
        {
            if (ModelState.IsValid)
            {


                IBHDbase dbase = new BHDbase();

                Plan plan = new Plan();
                plan.PlanId = 0;
                plan.PlanName = model.PlanName;
                plan.PlanDescription = model.PlanDescription;
                plan.PlanAmount = model.PlanAmount;
                plan.IsAdminPlan = model.IsAdminPlan;

                dbase.InsertPlan(plan);
                
                return RedirectToAction("ShowSubscriptions", "Subscription");


            }
            return View(model);
        }

        public ActionResult EditPlan(int PlanId)
        {
            if (PlanId != 0)
            {
                IBHDbase dbase = new BHDbase();

                PlanModel Model = dbase.GetPlan(PlanId).ToPlanModel();

                return View(Model);
            }

            return RedirectToAction("ShowSubscriptions");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPlan(PlanModel model)
        {
            if (ModelState.IsValid)
            {


                IBHDbase dbase = new BHDbase();

                Plan plan = new Plan();
                plan.PlanId = model.PlanId;
                plan.PlanName = model.PlanName;
                plan.PlanDescription = model.PlanDescription;
                plan.PlanAmount = model.PlanAmount;
                plan.IsAdminPlan = model.IsAdminPlan;

                dbase.UpdatePlan(plan);

                return RedirectToAction("ShowSubscriptions", "Subscription");


            }
            return View(model);
        }


        public ActionResult DeletePlan(int PlanId)
        {
            if (PlanId != 0)
            {
                IBHDbase dbase = new BHDbase();

                dbase.DeletePlan(PlanId);

                            
            }

            return RedirectToAction("ShowSubscriptions");
        }

        public List<Plan> GetPlanList()
        {
            IBHDbase dbase = new BHDbase();

            return dbase.GetAllPlans();

        }
    }
}
