using Couchbase;
using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using Moviq.Domain.Products;
using Moviq.Interfaces.Factories;
using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using Moviq.Interfaces.Services;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Cart
{
    public class CartRepository : ICartRepository, IRepository<ICart>
    {
        protected string keyPattern;
        protected string dataType;

        public CartRepository(IRepository<IProduct> productRepository, IFactory<ICart> cartFactory, ICouchbaseClient db, ILocale locale)
        {
            this.productRepository = productRepository;
            this.productFactory = cartFactory;
            this.db = db;
            this.locale = locale;
            this.dataType = ((IHelpCategorizeNoSqlData)cartFactory.GetInstance())._type;
            this.keyPattern = String.Concat(this.dataType, "::{0}");
        }

        IRepository<IProduct> productRepository;
        IFactory<ICart> productFactory;
        ICouchbaseClient db;
        ILocale locale;


        public ICart Get(string guid)
        {
            var result = db.GetJson<ShoppingCart>(String.Format(keyPattern, guid));
            if (result != null)
                return result;
            else
                return Set(new ShoppingCart(Guid.Parse(guid)));
        }

        public ICart Set(ICart cart)
        {
            if (db.StoreJson(StoreMode.Set, String.Format(keyPattern, cart.Guid.ToString()), cart))
            {
                return Get(cart.Guid.ToString());
            }

            throw new Exception(locale.CartSetFailure);
        }

        public IEnumerable<ICart> List(int take, int skip)
        {
            // TODO: We are breaking Liskov Subsitution by not implementing this method!
            throw new Exception(locale.LiskovSubstitutionInfraction);
        }

        public Task<IEnumerable<ICart>> Find(string searchFor)
        {
            // TODO: We are breaking Liskov Subsitution by not implementing this method!
            throw new Exception(locale.LiskovSubstitutionInfraction);
        }

        public bool Delete(string Guid)
        {
            return db.Remove(String.Format(keyPattern, Guid));
        }

        public bool KeyExists(string Guid)
        {
            return db.KeyExists(String.Format(keyPattern, Guid));
        }

        public void Dispose()
        {
            // don't dispose the db - it's a singleton
        }

        public bool AddToCart(string guid, string uid)
        {
            if (productRepository.Get(uid) == null)
                return false;
            var cart = Get(guid);
            if (cart.Add(uid))
            {
                Set(cart);
                return true;
            }                
            return false;
        }

        public bool RemoveFromCart(string guid, string uid)
        {
            var cart = Get(guid);
            if (cart.Remove(uid))
            {
                Set(cart);
                return true;
            }
            return false;
        }
    }
}
