using Stripe;
using System;
using System.Collections.Generic;

namespace BingHousing_Stripe
{
    public static class StripeACHDeposit
    {

        public static readonly string StripeKey = "sk_test_51Kdc4BSDoOtgQoti4bS3IyuThMVr8fhevSPKBRzPPo08DGEwe82aEJC3L3P9oFUarC6ivebwEhXLvC5MgQWAWQgY00ifxGqQxV";

        /// <summary>
        /// Create Bank Token
        /// </summary>
        /// <param name="banktoken"></param>
        /// <returns></returns>
        public static bool CreateBankToken(out string banktoken, string accountholdername, string accountholdertype, string routingnumber, string accountnumber)
        {
            bool success = false;
            try
            {
                StripeConfiguration.ApiKey = StripeKey;
                var banktokenOptions = new TokenCreateOptions
                {
                    BankAccount = new TokenBankAccountOptions
                    {
                        Country = "US",
                        Currency = "usd",
                        AccountHolderName = accountholdername,
                        AccountHolderType = accountholdertype,
                        RoutingNumber = routingnumber,
                        AccountNumber = accountnumber,
                    },
                };
                var banktokenservice = new TokenService();
                var banktokenresponse = banktokenservice.Create(banktokenOptions);
                banktoken = banktokenresponse.Id;
                success = true;
            }
            catch (Exception ex)
            {
                banktoken = "";
                ex.Message.ToString();
            }
            return success;
        }


        /// <summary>
        /// Get the customer details based on bank Account
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="customerdefaultsourceid"></param>
        /// <param name="banktokenid"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool GetCustomerDetails(out string customerid, out string customerdefaultsourceid, string banktokenid, string description)
        {
            bool success = false;
            try
            {
                StripeConfiguration.ApiKey = StripeKey;
                var options = new CustomerCreateOptions
                {
                    Description = description,
                    Source = banktokenid,
                };
                var service = new CustomerService();
                var customer = service.Create(options);
                customerid = customer.Id;
                customerdefaultsourceid = customer.DefaultSourceId;
                success = true;
            }
            catch (Exception ex)
            {
                customerid = "";
                customerdefaultsourceid = "";
                ex.Message.ToString();
            }
            return success;
        }


        /// <summary>
        /// Verify Bank Account
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="customersourceid"></param>
        /// <returns></returns>
        public static bool VerifyBankAccount(string customerid, string customersourceid)
        {
            bool success = false;
            try
            {
                StripeConfiguration.ApiKey = StripeKey;
                var verifyoptions = new BankAccountVerifyOptions
                {
                    Amounts = new List<long> { 32, 45 },
                };
                var bankverifyresponse = new BankAccountService();
                var verifyresponse = bankverifyresponse.Get(customerid,
                        customersourceid,
                        null);
                if (verifyresponse.Status == "new" || verifyresponse.Status == "validated")
                {
                    verifyresponse = bankverifyresponse.Verify(
                         customerid,
                         customersourceid,
                         verifyoptions
                       );
                }
                if (verifyresponse.Status == "verified")
                    success = true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="banktoken"></param>
        /// <returns></returns>
        public static bool ChargeCustomer(out Charge charge, string customerid, long amount)
        {
            bool success = false;
            charge = null;
            try
            {
                StripeConfiguration.ApiKey = StripeKey;
                var chargeoptions = new ChargeCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Customer = customerid,
                };
                var chargeservice = new ChargeService();
                var response = chargeservice.Create(chargeoptions);
                charge = response;
                success = true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return success;
        }


        /// <summary>
        /// Check Payment Status
        /// </summary>
        /// <param name="charge"></param>
        /// <param name="chargeid"></param>
        /// <returns></returns>
        public static bool checkPayMentStatus(out Charge charge, string chargeid)
        {
            bool status = false;
            charge = null;
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51Kdc4BSDoOtgQoti4bS3IyuThMVr8fhevSPKBRzPPo08DGEwe82aEJC3L3P9oFUarC6ivebwEhXLvC5MgQWAWQgY00ifxGqQxV";

                var service = new ChargeService();
                charge = service.Get("ch_3KfzXJ2eZvKYlo2C0nJ3JwNv");
                status = true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return status;
        }
    }

}
