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
    public interface ISingleOrder
    {
        ulong OrderID { get; set; }
        ICollection<Product> Products { get; set; }
        DateTime OrderDate { get; set; }
        decimal TotalPrice { get; set; }
    }
}
