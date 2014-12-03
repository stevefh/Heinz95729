using Moviq.Domain.Products;
using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Order
{
    public class SingleOrder : ISingleOrder
    {
        public SingleOrder() { }

        public SingleOrder(ICollection<Product> Products, DateTime OrderDate, decimal TotalPrice)
        {
            this.Products = Products;
            this.OrderDate = OrderDate;
            this.TotalPrice = TotalPrice;
        }

        public ulong OrderID { get; set; }

        public ICollection<Product> Products { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
                                                                         