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
            this.Orders = new List<SingleOrder>();
            this._type = "order";
        }

        /*
        public Order(Guid Guid, int OrderID, IRepository<IProduct> Products, DateTime OrderDate, decimal TotalPrice)
        {
            this.Guid = Guid;
            this.OrderID = OrderID;
            this.Products = Products;
            this.OrderDate = OrderDate;
            this.TotalPrice = TotalPrice;
            this._type = "order";
        }
        */

        public Guid Guid { get; set; }

        public ICollection<SingleOrder> Orders { get; set; }

        public string _type { get; set; }
        /*
        public Guid Guid { get; set; }

        public int OrderID { get; set; }

        public IRepository<IProduct> Products { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }
        */


        public void AddOrder(SingleOrder singleOrder)
        {
            Orders.Add(singleOrder);   
        }
    }
}
