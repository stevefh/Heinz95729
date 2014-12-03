using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Order
{
    public interface IOrder : IHelpCategorizeNoSqlData
    {
        Guid Guid { get; set; }
        ICollection<ulong> Orders { get; set; }

        void AddOrder(SingleOrder singleOrder);
    }
}
