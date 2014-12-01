using Moviq.Domain.Cart;
using Moviq.Domain.Order;
using Moviq.Domain.Products;
using Moviq.Helpers;
using Moviq.Interfaces;
using Moviq.Interfaces.Models;
using Moviq.Interfaces.Services;
using Nancy;
using Nancy.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moviq.Api
{
    public class OrderModule : NancyModule
    {
        public OrderModule(IOrderDomain orders, IProductDomain products, IModuleHelpers helper){

            this.Get["/api/order"] = args =>
            {
                this.RequiresAuthentication();

                ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
                string guid = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();

                var order = orders.Repo.Get(guid);
                return helper.ToJson(order);
            };

            // sample: api/order/add?q=[{"Uid":"before_go","Title":"One Last Thing I Go","Price":8.77},{"Uid":"leave_you","Title":"This Is Where I Leave You: A Novel","Price":7.99}]
            this.Get["/api/order/add"] = args =>
            {
                this.RequiresAuthentication();

                ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
                string guid = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
                
                var ProductInfos = this.Request.Query.q;

                var result = JsonConvert.DeserializeObject<List<ProductInfo>>(ProductInfos);
                List<Product> orderProducts = new List<Product>();
                decimal totalPrice = 0;

                foreach (var productInfo in result)
                {
                    var currentProduct = products.Repo.Get(productInfo.Uid);
                    orderProducts.Add(currentProduct);
                    totalPrice = totalPrice + currentProduct.Price;
                }

                orders.Repo.AddOrder(guid, new SingleOrder 
                {
                    Products = orderProducts,
                    OrderDate = DateTime.Now,
                    TotalPrice = totalPrice
                });

                var order = orders.Repo.Get(guid); 
                return helper.ToJson(order);
            };
        }
    }
}