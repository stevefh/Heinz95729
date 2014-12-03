using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Interfaces.Models
{
    public interface ICart
    {
        Guid Guid { get; set; }
        ICollection<string> Products { get; set; }

        bool Add(string uid);
        bool Remove(string uid);
    }
}