using Moviq.Domain.Cart;
using Moviq.Interfaces.Factories;
using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Cart
{
    public class CartFactory : IFactory<ICart>
    {
        public ICart GetInstance()
        {
            return new ShoppingCart(Guid.Empty) as ICart;
        }
    }
}
