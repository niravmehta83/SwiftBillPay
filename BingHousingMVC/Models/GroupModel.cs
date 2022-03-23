using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousing_BO;
using BingHousingMVC_DAL;
using System.ComponentModel.DataAnnotations;

namespace BingHousingMVC.Models
{
    public class GroupModel
    {
        [Display(Name = "Group Id")]
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        [StringLength(100)]
        public string GroupName { get; set; }

        [Display(Name = "Group Description")]
        [StringLength(250)]
        public string GroupDescription { get; set; }

        [Display(Name = "User Id")]
        public int UserId { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Added On")]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; set; }

        public bool IsActive { get; set; }
        public int NextGroupId { get; set; }
        public int PrevGroupId { get; set; }

        public string SelectedCustomers { get; set; }

        public List<GroupMemberModel> GroupMembers { get; set; }

        #region Helper Methods

        public static List<GroupModel> GetGroupsByUserName(string username)
        {
            List<GroupModel> groups = null;
            IBHDbase dbase = new BHDbase();

            int userId = dbase.GetUserId(username);
            if (userId != 0)
            {
                List<Group> grps = dbase.GetAllGroupsOfUser(userId).ToList();
                groups = new List<GroupModel>();
                foreach (var grp in grps)
                {
                    GroupModel gModel = new GroupModel
                    {
                        GroupId = grp.GroupId,
                        GroupName = grp.GroupName,
                        GroupDescription = grp.GroupDescription,
                        CreatedOn = grp.CreatedOn,
                        LastModified = grp.LastModified,
                        IsActive = grp.IsActive,
                        UserId = userId,
                        UserName = username,
                        GroupMembers = new List<GroupMemberModel>()
                    };
                    List<GroupMember> grpMembers = dbase.GetAllGroupMembersByGroupId(grp.GroupId);
                    foreach (var gm in grpMembers)
                    {
                        CustomerDetail customer = dbase.GetCustomerDetails(gm.CustomerId);
                        if (customer != null)
                        {
                            gModel.GroupMembers.Add(new GroupMemberModel
                            {
                                GroupMemberId = gm.GroupMemberId,
                                CustomerId = gm.CustomerId,
                                CustomerName = (customer.CustomerFirstName ?? "") + " " + (customer.CustomerLastName ?? ""),
                                AddedOn = gm.AddedOn,
                                LastModified = gm.LastModified,
                                IsActive = gm.IsActive,
                                UserId = userId,
                                UserName = username,
                                Billdescription = !string.IsNullOrEmpty(gm.Billdescription) ? gm.Billdescription : customer.BillDescription,
                                Nextbilldate = gm.Nextbilldate != null ? gm.Nextbilldate : customer.NextBillDate,
                                Payee=customer.Payee,
                                PayeeEmail = customer.PayeeEmail,
                            });
                        }
                    }

                    groups.Add(gModel);
                }
            }
            return groups;
        }

        public static GroupModel GetGroupByGroupId(int groupId)
        {
            GroupModel group = null;
            IBHDbase dbase = new BHDbase();

            if (groupId != 0)
            {
                Group grp = dbase.GetGroupByGroupId(groupId);
                string userName = dbase.GetUserProfileName(grp.UserId);

                group = new GroupModel
                {
                    GroupId = grp.GroupId,
                    GroupName = grp.GroupName,
                    GroupDescription = grp.GroupDescription,
                    CreatedOn = grp.CreatedOn,
                    LastModified = grp.LastModified,
                    IsActive = grp.IsActive,
                    UserId = grp.UserId,
                    UserName = userName,
                    GroupMembers = new List<GroupMemberModel>()
                };

                List<GroupMember> grpMembers = dbase.GetAllGroupMembersByGroupId(grp.GroupId);
                foreach (var gm in grpMembers)
                {
                    CustomerDetail customer = dbase.GetCustomerDetails(gm.CustomerId);
                    group.GroupMembers.Add(new GroupMemberModel
                    {
                        GroupMemberId = gm.GroupMemberId,
                        CustomerId = gm.CustomerId,
                        GroupId = gm.GroupId,
                        CustomerName = (customer.CustomerFirstName ?? "") + " " + (customer.CustomerLastName ?? ""),
                        AddedOn = gm.AddedOn,
                        LastModified = gm.LastModified,
                        IsActive = gm.IsActive,
                        UserId = grp.UserId,
                        UserName = userName,
                        Payee=customer.Payee,
                        PayeeEmail = customer.PayeeEmail
                    });
                }

            }
            return group;
        }

        #endregion
    }

    
}