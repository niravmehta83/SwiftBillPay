using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousing_BO;

namespace BingHousingMVC.Models
{
    public class PrintCheckModel
    {
        private CheckOutModel _CheckOutModel { get; set; }
        private CheckModel _CheckModel { get; set; }

        public CheckOutModel CheckOutModel
        {
            get
            {
                return _CheckOutModel;
            }

            set
            {
                _CheckOutModel = value;
            }
        }
        public CheckModel CheckModel
        {
            get
            {
                return _CheckModel;
            }

            set
            {
                _CheckModel = value;
            }
        }   
    }
    
}