using Moviq.Domain.Cart;
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
            this.Post["/api/cart/add"] = args =>
            {
                this.RequiresAuthentication();

                var uid = this.Request.Query.q;
                ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
                string guid = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
                carts.Repo.AddToCart(guid, uid);

                return true;
            }; 

            this.Get["/api/cart"] = args =>
            {
                this.RequiresAuthentication();

                ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
                string guid = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
                var cart = carts.Repo.Get(guid);
                List<ProductInfo> result = new List<ProductInfo>();
                List<string> unavailableTitles = new List<string>();
                foreach (var productInfo in cart.Products)
                {
                    var product = products.Repo.Get(productInfo.Uid);
                    if (product == null)
                    {
                        unavailableTitles.Add(productInfo.Title);
                        carts.Repo.RemoveFromCart(guid, productInfo.Uid);
                    }
                    else
                    {
                        result.Add(new ProductInfo
                        {
                            Uid = product.Uid,
                            Title = product.Title,
                            Price = product.Price
                        });
                    }
                }

                var cartInfo = new CartInfo{
                    products = result,
                    unavailableTitles = unavailableTitles
                };

                return helper.ToJson(cartInfo);
            };

            this.Post["/api/cart/remove"] = args =>
            {
                this.RequiresAuthentication();

                var uid = this.Request.Query.q;
                ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
                string guid = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
                carts.Repo.RemoveFromCart(guid, uid);
                var cart = carts.Repo.Get(guid);
                List<ProductInfo> result = new List<ProductInfo>();
                List<string> unavailableTitles = new List<string>();
                foreach (var productInfo in cart.Products)
                {
                    var product = products.Repo.Get(productInfo.Uid);
                    if (product == null)
                    {
                        unavailableTitles.Add(productInfo.Uid);
                    }
                    else
                    {
                        result.Add(new ProductInfo
                        {
                            Uid = product.Uid,
                            Title = product.Title,
                            Price = product.Price
                        });
                    }
                }

                var cartInfo = new CartInfo
                {
                    products = result,
                    unavailableTitles = unavailableTitles
                };

                return true;
            };

            
        }

        private class CartInfo
        {
            public List<ProductInfo> products { get; set; }
            public List<string> unavailableTitles { get; set; }
        }

    }
}