using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BingHousingMVC.Models
{
    public class PlanModel
    {
        private decimal _planamount;

         [Required]
        public decimal PlanAmount
        {
            get { return _planamount; }
            set { _planamount = value; }
        }

        private string _planname;

         [Required]
        public string PlanName
        {
            get { return _planname; }
            set { _planname = value; }
        }
        private string _plandescription;

         [Required]
        public string PlanDescription
        {
            get { return _plandescription; }
            set { _plandescription = value; }
        }
        private bool _isadminplan;

         [Required]
        public bool IsAdminPlan
        {
            get { return _isadminplan; }
            set { _isadminplan = value; }
        }
        private int _planid;

        public int PlanId
        {
            get { return _planid; }
            set { _planid = value; }
        }
    }
}