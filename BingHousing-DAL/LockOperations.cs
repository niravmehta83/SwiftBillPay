using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BingHousing_BO;
using System.Web.Configuration;



namespace BingHousingMVC_DAL
{
    internal static class LockOperations
    {
         

        internal static void LockUser(int UserId = 0, string UserName = "")
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                try
                {
                    UserSecurity obj = null;//latebinding

                    if (UserId != 0)
                    {

                        obj = Dbase.UserSecurities.Select(a => a).Where(a => a.UserProfile.UserId == UserId).SingleOrDefault();


                    }
                    else
                    {

                        obj = Dbase.UserSecurities.Select(a => a).Where(a => a.UserProfile.UserName == UserName).SingleOrDefault();

                    }

                    obj.Islocked = true;

                    Dbase.SaveChanges();

                }

                catch (Exception)
                {

                    throw;
                }

            }
        }

        internal static void UnLockUser(int UserId = 0, string UserName = "")
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                try
                {
                    UserSecurity obj = null;//latebinding

                    if (UserId != 0)
                    {

                        obj = Dbase.UserSecurities.Select(a => a).Where(a => a.UserProfile.UserId == UserId).SingleOrDefault();


                    }
                    else
                    {

                        obj = Dbase.UserSecurities.Select(a => a).Where(a => a.UserProfile.UserName == UserName).SingleOrDefault();

                    }

                    obj.Islocked = false;

                    Dbase.SaveChanges();

                }

                catch (Exception)
                {

                    throw;
                }

            }
        }

        internal static bool IsUserLocked(int UserId = 0, string UserName = "")
        {
            using (BHDbaseEntities Dbase = new BHDbaseEntities())
            {
                try
                {
                    UserSecurity obj = null;//latebinding

                    if (UserId != 0)
                    {

                        obj = Dbase.UserSecurities.Select(a => a).Where(a => a.UserProfile.UserId == UserId).SingleOrDefault();


                    }
                    else
                    {

                        obj = Dbase.UserSecurities.Select(a => a).Where(a => a.UserProfile.UserName == UserName).SingleOrDefault();

                    }

                    return obj.Islocked;

                }

                catch (Exception)
                {

                    throw;
                }

            }
        }


    }
}