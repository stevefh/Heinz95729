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
            Products = null;
            _type = "cart";
        }
        public Guid Guid { get; set; }

        public ICollection<string> Products { get; set; }

        public string _type { get; set; }

        
        public bool Add(string uid)
        {
            if (Products == null)
            {
                Products = new List<string>();
                Products.Add(uid);
                return true;
            }
            if (!Products.Contains(uid))
            {
                Products.Add(uid);
                return true;
            }
            return false;
        }

        public bool Remove(string uid)
        {
            if (Products.Contains(uid))
            {
                Products.Remove(uid);
                return true;
            }
            return false;
        }

    }
}
