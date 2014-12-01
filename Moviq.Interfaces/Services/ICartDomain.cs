using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Interfaces.Services
{
    public interface ICartDomain
    {
        ICartRepository Repo { get; set; }
    }
}
