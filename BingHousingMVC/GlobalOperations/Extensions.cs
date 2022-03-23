using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BingHousingMVC.Models;
using BingHousing_BO;

namespace BingHousingMVC.GlobalOperations
{
    public static partial class Extensions
    {
        public static List<SelectListItem> ToSelectListItem(this List<string> usernamelist)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (string user in usernamelist)
            {
                list.Add(new SelectListItem { Text = user, Value = user });

            }
            return list;
        }

        public static CheckOutRegisterModel ToCheckOutRegisterModel(this CustomerProfile model)
        {
            CheckOutRegisterModel newmodel = new CheckOutRegisterModel();
            if (model != null)
            {
                newmodel.UserId = model.UserId;
                newmodel.Address = model.Address;
                newmodel.Address2 = model.Address2;
                newmodel.City = model.City;
                newmodel.Comments = model.Comments;
                newmodel.Company = model.Company;
                newmodel.Country = model.Country;
                newmodel.FirstName = model.FirstName;
                newmodel.JOML = model.JOML;
                newmodel.LastName = model.LastName;
                newmodel.PaymentMode = model.PaymentMode;
                newmodel.PhoneNumber = model.PhoneNumber;
                newmodel.State = model.State;
                newmodel.ZipCode = model.ZipCode;
                newmodel.Email = model.Email;
            }
            return newmodel;
        }

        public static CheckOutRegisterModel ToCheckOutRegisterModel(this CustomerDetail model)
        {
            CheckOutRegisterModel newmodel = new CheckOutRegisterModel();
            if (model != null)
            {
                newmodel.UserId = model.UserId;
                newmodel.Address = model.CustomerAddress;
                newmodel.Address2 = model.CustomerAddress2;
                newmodel.City = model.CustomerCity;               
                newmodel.Country = model.CustomerCountry;
                newmodel.FirstName = model.CustomerFirstName;
                newmodel.PaymentMode = "PaperCheck";
                newmodel.LastName = model.CustomerLastName;
                newmodel.State = model.CustomerState;
                newmodel.ZipCode = model.CustomerZipCode;
                newmodel.Email = model.CustomerEmail;
            }
            return newmodel;
        }

        public static CustomerProfile ToCustomerProfile(this CheckOutRegisterModel model)
        {
            CustomerProfile newmodel = new CustomerProfile();
            if (model != null)
            {
                newmodel.UserId = model.UserId;
                newmodel.Address = model.Address;
                newmodel.Address2 = model.Address2;
                newmodel.City = model.City;
                newmodel.Comments = model.Comments;
                newmodel.Company = model.Company;
                newmodel.Country = model.Country;
                newmodel.FirstName = model.FirstName;
                newmodel.JOML = model.JOML;
                newmodel.LastName = model.LastName;
                newmodel.PaymentMode = model.PaymentMode;
                newmodel.PhoneNumber = model.PhoneNumber;
                newmodel.State = model.State;
                newmodel.ZipCode = model.ZipCode;
                newmodel.Email = model.Email;
            }

            return newmodel;
        }

        public static CheckDetail ToCheckDetail(this CheckModel model)
        {
            CheckDetail nmodel = new CheckDetail();
            if (model != null)
            {
                nmodel.AccountNumber = model.AccountNumber;
                nmodel.AddressOnCheck = model.AddressOnCheck;
                nmodel.Amount = model.AmountOnCheck;
                nmodel.BankAddress = model.BankAddress;
                nmodel.BankCity = model.BankCity;
                nmodel.BankName = model.BankName;
                nmodel.BankState = model.BankState;
                nmodel.BankZip = model.BankZip;
                nmodel.CheckId = model.CheckId;
                nmodel.CheckMemo = model.CheckMemo;
                nmodel.CheckNumber = model.CheckNumber;
                nmodel.CityOnCheck = model.CityOnCheck;
                nmodel.Comments = model.Comment;
                nmodel.DateOnCheck = model.DateOnCheck;
                nmodel.NameOnCheck = model.NameOnCheck;
                nmodel.Payee = model.Payee;
                nmodel.RountingNumber = model.RoutingNumber;
                nmodel.StateOnCheck = model.StateOnCheck;
                nmodel.UserId = model.UserId;
                nmodel.ZipOnCheck = model.ZipOnCheck;
                nmodel.InsertedOn = model.InsertedOn;
            }
            return nmodel;

        }

        public static CheckModel ToCheckModel(this CheckDetail model)
        {
            CheckModel nmodel = new CheckModel();
            nmodel.AccountNumber = model.AccountNumber;
            nmodel.AddressOnCheck = model.AddressOnCheck;
            nmodel.AmountOnCheck = model.Amount;
            nmodel.BankAddress = model.BankAddress;
            nmodel.BankCity = model.BankCity;
            nmodel.BankName = model.BankName;
            nmodel.BankState = model.BankState;
            nmodel.BankZip = model.BankZip;
            nmodel.CheckId = model.CheckId;
            nmodel.CheckMemo = model.CheckMemo;
            nmodel.CheckNumber = model.CheckNumber;
            nmodel.CityOnCheck = model.CityOnCheck;
            nmodel.Comment = model.Comments;
            nmodel.DateOnCheck = model.DateOnCheck;
            nmodel.NameOnCheck = model.NameOnCheck;
            nmodel.Payee = model.Payee;
            nmodel.RoutingNumber = model.RountingNumber;
            nmodel.StateOnCheck = model.StateOnCheck;
            nmodel.UserId = model.UserId;
            nmodel.ZipOnCheck = model.ZipOnCheck;

            return nmodel;

        }

        public static PlanModel ToPlanModel(this Plan model)
        {
            PlanModel Model = new PlanModel();
            Model.IsAdminPlan = model.IsAdminPlan;
            Model.PlanAmount = model.PlanAmount;
            Model.PlanDescription = model.PlanDescription;
            Model.PlanId = model.PlanId;
            Model.PlanName = model.PlanName;

            return Model;
        }

        public static Group ToGroup(this GroupModel grp)
        {
            Group Model = new Group();
            Model.GroupId = grp.GroupId;
            Model.GroupName = grp.GroupName;
            Model.GroupDescription = grp.GroupDescription;
            Model.CreatedOn = grp.CreatedOn;
            Model.LastModified =DateTime.UtcNow;
            Model.IsActive = grp.IsActive;
            Model.UserId = grp.UserId;
            return Model;
        }

        public static string getCheckCsvFileData(Tuple<CheckOutModel, CheckModel> model)
        {
            string csvRow = model.Item1.FirstName + ", "
                                      + model.Item1.LastName + ", "
                                      + model.Item1.Company + ", "
                                      + model.Item1.Address.Replace(',', ' ') + ", "
                                      + model.Item1.City + ", "
                                      + model.Item1.State + " ,"
                                      + model.Item1.ZipCode + " ,"
                                      + model.Item1.PhoneNumber + " ,"
                                      + model.Item1.Email + ", "
                                      + model.Item2.NameOnCheck + ", "
                                      + model.Item2.AddressOnCheck.Replace(',', ' ') + ", "
                                      + model.Item2.CityOnCheck + " " + model.Item2.StateOnCheck + " " + model.Item2.ZipOnCheck + ", "
                                      + model.Item2.DateOnCheck + ", "
                                      + Convert.ToInt64(model.Item2.CheckNumber) + " ,"
                                      + model.Item2.AmountOnCheck + ", "
                                      + model.Item2.Payee + " ,"
                                      + model.Item2.BankName + " ,"
                                      + model.Item2.BankAddress.Replace(',', ' ') + ", "
                                      + model.Item2.BankCity + " " + model.Item2.BankState + " " + model.Item2.BankZip + ", "
                                      + Convert.ToInt64(model.Item2.RoutingNumber) + " ,"
                                      + Convert.ToInt64(model.Item2.AccountNumber) + ", "
                                      + model.Item2.CheckMemo;
            return csvRow;
        }
    }
}