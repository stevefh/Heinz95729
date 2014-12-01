using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Order
{
    public class OrderDomain : IOrderDomain
    {
        public OrderDomain(IOrderRepository repo)
        {
            this.Repo = repo;
        }

        public IOrderRepository Repo { get; set; }
    }
}
