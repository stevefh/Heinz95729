using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using Moviq.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Cart
{
    public class CartDomain : ICartDomain
    {
        public CartDomain(ICartRepository repo)
        {
            this.Repo = repo;
        }

        public ICartRepository Repo { get; set; }
    }
}
