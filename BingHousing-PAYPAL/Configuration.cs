using System.Collections.Generic;
using PayPal;
using System.Configuration;
using System;

namespace BingHousing_PAYPAL
{ 
    public static class Configuration
    {
        // Create the configuration map that contains mode and other optional configuration details.
        private static Dictionary<string, string> GetConfig()
        {
            Dictionary<string, string> configMap = new Dictionary<string, string>();



            ///Configuration mapping
            ///********************
            /// createdby Raja 
            /// On:11/06/2014
            /// 
            /// reason:for dynamic data( test credential or live credential) taken from web config file
            /// 
            /// Endpoints are varied depending on whether sandbox OR live is chosen for mode
            /// 
            ///*************************

            
            bool test = Convert.ToBoolean(ConfigurationManager.AppSettings["PayPalMode"]);

            if (test)
            {
                //for test credentials
                configMap.Add("mode", "sandbox");//Raja on 11-06-2014
                configMap.Add("endpoint", "https://api.sandbox.paypal.com"); 
            }
            else
            {
                ///For live credentials

                configMap.Add("mode", "live");//Raja on 11-06-2014
            }

            

            // These values are defaulted in SDK. If you want to override default values, uncomment it and add your value
            // configMap.Add("connectionTimeout", "360000");
            // configMap.Add("requestRetries", "1");
            return configMap;
        }

        // Create accessToken
        private static string GetAccessToken(string ClientId, string ClientSecret)
        {
            // ###AccessToken
            // Retrieve the access token from
            // OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and
            // reused within the expiry window                
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            //string accessToken = new OAuthTokenCredential("AQlFU96TQumRzw8CmsnaykeDM46HbZi0Fq9H5_xBIjU76wci_tNVXgURYyheUrbGJ6V7n8cJ49tWiv6M", "EPu_5mjJyEZ8fBsPlBP7DB1KMUbusWYtqCrG32A3iDEnI2kjIXyp7cmQIo2SP3YFt2_DIxfYbP4OFrxq", GetConfig()).GetAccessToken();
            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext(string ClientId,string ClientSecret)
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            APIContext apiContext = new APIContext(GetAccessToken(ClientId,ClientSecret));
            apiContext.Config = GetConfig();

            // Use this variant if you want to pass in a request id  
            // that is meaningful in your application, ideally 
            // a order id.
            // String requestId = Long.toString(System.nanoTime();
            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

            return apiContext;
        }

    }
}
 