using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousing_BO;
using BingHousingMVC_DAL;
using System.ComponentModel.DataAnnotations;

namespace BingHousingMVC.Models
{
    public class GroupMemberModel
    {
        [Display(Name = "Member Id")]
        public int GroupMemberId { get; set; }

        [Required]
        [Display(Name = "Group Id")]
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Added On")]
        public DateTime AddedOn { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Billdescription { get; set; }
        public string Payee { get; set; }
        public string PayeeEmail { get; set; }
        public Nullable<System.DateTime> Nextbilldate { get; set; }

        #region Helper Methods
        public static List<GroupMemberModel> GetAllGroupMemberOf(int userId)
        {
            List<GroupMemberModel> grpMembers = null;
            IBHDbase dbase = new BHDbase();

            List<Payee> payees = dbase.GetAllPayee(userId);
            if (payees != null && payees.Count > 0)
            {
                grpMembers = new List<GroupMemberModel>();
                foreach (var payee in payees)
                {
                    List<CustomerDetail> customers = dbase.GetCustomerList(userId);
                    if (customers != null && customers.Count > 0)
                    {
                        foreach (var c in customers)
                        {
                            grpMembers.Add(new GroupMemberModel
                            {
                                CustomerId = c.CustomerId
                              ,
                                CustomerName = (c.CustomerFirstName ?? "") + " " + (c.CustomerLastName ?? "")
                              ,
                                IsActive = true
                            });
                        }
                    }
                }
            }
            return grpMembers;
        }
        #endregion
    }
}