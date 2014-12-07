using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Cart
{
    public class ShoppingCart : ICart, IHelpCategorizeNoSqlData 
    {
        public ShoppingCart(Guid Guid)
        {
            this.Guid = Guid;
            Products = new List<ProductInfo>();
            _type = "cart";
        }
        public Guid Guid { get; set; }

        public ICollection<ProductInfo> Products { get; set; }

        public string _type { get; set; }

        
        public bool Add(ProductInfo productInfo)
        {
            foreach (var product in Products)
            {
                if (product.Uid.Equals(productInfo.Uid))
                    return false;
            }
            Products.Add(productInfo);
            return true;
        }

        public bool Remove(string uid)
        {
            foreach (var productInfo in Products) {
                if (productInfo.Uid.Equals(uid))
                {
                    Products.Remove(productInfo);
                    return true;
                }     
            }
            return false;
        }

    }
}
