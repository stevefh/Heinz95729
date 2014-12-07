using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using RestSharp;
using System.Text;
using Moviq.Helpers;

namespace Moviq.Api
{
    public class stripeModule :NancyModule
    {
        public stripeModule(IModuleHelpers helper)
        {
            this.Post["/api/Payment"] = args =>
            {
                var token = this.Request.Query.t;
                var amount = this.Request.Query.a;
                decimal a = (decimal)amount;
                a = a * 100;
                int total = (int)a;
                RestClient myClient = new RestClient("https://api.stripe.com/v1/charges");
                
                RestRequest myRequest = new RestRequest(Method.POST);
                myRequest.RequestFormat = DataFormat.Json;
                string mykey = "sk_test_ZwxIpFE6CwwqyyZ364K5FBlr";
                            
                myRequest.AddHeader("Authorization", "Bearer " + mykey);

                myRequest.AddParameter("card", token);
                myRequest.AddParameter("amount", total);
                myRequest.AddParameter("currency", "usd");
                myRequest.AddParameter("description", "hello");
                var response = myClient.Execute(myRequest);

                return helper.ToJson(response);
            };

        }
    }
}