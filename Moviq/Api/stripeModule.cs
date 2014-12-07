using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using RestSharp;
using System.Text;

namespace Moviq.Api
{
    public class stripeModule :NancyModule
    {
        public stripeModule()
        {
            this.Post["/api/Payment"] = args =>
            {
                var token = this.Request.Query.q;
                RestClient myClient = new RestClient("https://api.stripe.com/v1/charges");
                
                RestRequest myRequest = new RestRequest(Method.POST);
                //myRequest.RequestFormat = DataFormat.Json;
                string mykey = "sk_test_ZwxIpFE6CwwqyyZ364K5FBlr";
                            
                myRequest.AddHeader("Authorization", "Bearer " + mykey);
                //
                myRequest.AddBody(new { amount = "20", currency = "usd", card = token });
                //StringBuilder str = new StringBuilder();
                //str.AppendFormat("{amount={0}&", "20");
                //str.AppendFormat("currency={0}&", "usd");
                //str.AppendFormat("description={0}&", "card charges");
                //str.AppendFormat("card={0}&}", token);

                //myRequest.AddParameter("text/json", str.ToString(), ParameterType.RequestBody);
                var response = myClient.Execute(myRequest);

                
                return 1;
            };

        }
    }
}