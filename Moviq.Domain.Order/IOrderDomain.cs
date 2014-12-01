using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Order
{
    public interface IOrderDomain 
    {
        IOrderRepository Repo { get; set; }
    }
}
