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
            if (Products == null)
            {
                Products = new List<ProductInfo>();
                Products.Add(productInfo);
                return true;
            }
            else if (!Products.Contains(productInfo))
            {
                Products.Add(productInfo);
                return true;
            }
            return false;
        }

        public bool Remove(ProductInfo productInfo)
        {
            if (Products.Contains(productInfo))
            {
                Products.Remove(productInfo);
                return true;
            }
            return false;
        }

    }
}
