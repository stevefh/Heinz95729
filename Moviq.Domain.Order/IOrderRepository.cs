using Moviq.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Order
{
    public interface IOrderRepository : IRepository<IOrder>
    {
        void AddOrder(string guid, SingleOrder singleOrder);
    }
}
