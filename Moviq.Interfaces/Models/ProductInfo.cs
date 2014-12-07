using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Cart
{
    public class ProductInfo : IProductInfo
    {
        public string Uid { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
