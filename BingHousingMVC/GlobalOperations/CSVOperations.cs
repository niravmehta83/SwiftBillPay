using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousingMVC.Models;
using System.IO;
using System.Configuration;

namespace BingHousingMVC.GlobalOperations
{
    public static class CSVOperations
    {
        public static bool CreateCSV(Tuple<CheckOutModel, CheckModel> model, string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var stream = new StreamWriter(fs))
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

                        stream.Write(csvRow);
                        stream.Flush();
                        stream.Close();
                        stream.Dispose();

                    }
                   
                    GC.Collect();
                }
            }
            catch (Exception)
            {

                return false;
            }



            return true;
        }

        public static void MakeCopy(string sourcepath, string destinationPath)
        {
            if (!File.Exists(destinationPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                File.Copy(sourcepath, destinationPath);
            }
            File.Copy(sourcepath, destinationPath,true);
        }

        public static void CreateFile(string sourcepath)
        {
            if (!File.Exists(sourcepath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(sourcepath));
                File.Create(sourcepath);
            }
        }
    }
}