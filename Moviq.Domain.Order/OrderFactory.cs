using Moviq.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Order
{
    public class OrderFactory : IFactory<IOrder>
    {
        public IOrder GetInstance()
        {
            return new Order(Guid.Empty) as IOrder;
        }
    }
}
