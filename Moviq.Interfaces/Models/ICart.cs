using Moviq.Domain.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Interfaces.Models
{
    public interface ICart
    {
        Guid Guid { get; set; }
        ICollection<ProductInfo> Products { get; set; }

        bool Add(ProductInfo productInfo);
        bool Remove(ProductInfo productInfo);
    }
}
