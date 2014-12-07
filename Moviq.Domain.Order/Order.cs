using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Order
{
    public class Order : IOrder
    {
        public Order(Guid Guid)
        {
            this.Guid = Guid;
            this.Orders = new List<ulong>();
            this._type = "order";
        }

        public Guid Guid { get; set; }

        public ICollection<ulong> Orders { get; set; }

        public string _type { get; set; }

        public void AddOrder(SingleOrder singleOrder)
        {
            Orders.Add(singleOrder.OrderID);   
        }
    }
}
