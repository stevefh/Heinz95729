using Moviq.Helpers;
using Moviq.Interfaces;
using Moviq.Interfaces.Models;
using Moviq.Interfaces.Services;
using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moviq.Api
{
    public class CartModule : NancyModule
    {
        public CartModule(ICartDomain carts, IProductDomain products, IModuleHelpers helper)
        {
            this.Get["/api/cart"] = args =>
            {
                this.RequiresAuthentication();

                ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
                string guid = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
                var cart = carts.Repo.Get(guid);
                List<ProductInfo> result = new List<ProductInfo>();
                foreach (var productUid in cart.Products)
                {
                    var product = products.Repo.Get(productUid);
                    result.Add(new ProductInfo {
                        Uid = product.Uid,
                        Title = product.Title,
                        Price = product.Price
                    });
                }

                return helper.ToJson(result);
            };

            this.Get["/api/cart/add"] = args =>
            {
                this.RequiresAuthentication();

                var uid = this.Request.Query.q;
                ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
                string guid = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
                carts.Repo.AddToCart(guid, uid);

                var cart = carts.Repo.Get(guid);
                return helper.ToJson(cart);
            };

            this.Get["/api/cart/remove"] = args =>
            {
                this.RequiresAuthentication();

                var uid = this.Request.Query.q;
                ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
                string guid = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
                carts.Repo.RemoveFromCart(guid, uid);

                var cart = carts.Repo.Get(guid);
                return helper.ToJson(cart);
            };
        }

        private class ProductInfo
        {
            public string Uid { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
        }
    }
}