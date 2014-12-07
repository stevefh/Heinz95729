using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Interfaces.Models
{
    public interface IProductInfo
    {
        string Uid { get; set; }
        string Title { get; set; }
        decimal Price { get; set; }
    }
}
