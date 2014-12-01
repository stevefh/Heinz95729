using Couchbase;
using Couchbase.Extensions;
using Moviq.Interfaces.Factories;
using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enyim.Caching.Memcached;

namespace Moviq.Domain.Order
{
    public class OrderRepository : IOrderRepository, IRepository<IOrder>
    {
        protected string keyPattern;
        protected string dataType;

        public OrderRepository(IRepository<IProduct> productRepository, IFactory<IOrder> orderFactory, ICouchbaseClient db, ILocale locale, IRestClient restClient, string searchUrl)
        {
            this.productRepository = productRepository;
            this.productFactory = orderFactory;
            this.db = db;
            this.locale = locale;
            this.dataType = ((IHelpCategorizeNoSqlData)orderFactory.GetInstance())._type;
            this.keyPattern = String.Concat(this.dataType, "::{0}");
            this.restClient = restClient;
            this.searchUrl = searchUrl;
        }

        IRepository<IProduct> productRepository;
        IFactory<IOrder> productFactory;
        ICouchbaseClient db;
        ILocale locale;
        IRestClient restClient;
        string searchUrl;
        string query = "{ \"query\": { \"query_string\": { \"query_string\": { \"query\": \"{0}\" } } } }";

        public IOrder Get(string guid)
        {
            if (!KeyExists(String.Format(keyPattern, guid)))
                return db.GetJson<Order>(String.Format(keyPattern, guid));
            else
                return Set(new Order(Guid.Parse(guid)));
        }

        public bool KeyExists(string Guid)
        {
            return db.KeyExists(String.Format(keyPattern, Guid));
        }

        public IOrder Set(IOrder order)
        {
            if (db.StoreJson(StoreMode.Set, String.Format(keyPattern, order.Guid), order))
            {
                return Get(order.Guid.ToString());
            }

            throw new Exception(locale.OrderSetFailure);
        }

        public IEnumerable<IOrder> List(int take, int skip)
        {
            // TODO: We are breaking Liskov Subsitution by not implementing this method!
            throw new Exception(locale.LiskovSubstitutionInfraction);
        }

        public Task<IEnumerable<IOrder>> Find(string searchBy)
        {
            // TODO: We are breaking Liskov Subsitution by not implementing this method!
            throw new Exception(locale.LiskovSubstitutionInfraction);
        }

        public bool Delete(string guid)
        {
            return db.Remove(String.Format(keyPattern, guid));
        }

        public void Dispose()
        {
            // don't dispose the db - it's a singleton
        }

        public void AddOrder(string guid, SingleOrder singleOrder)
        {
            var orders = Get(guid);
            SetSingleOrderID(singleOrder);
            orders.AddOrder(singleOrder);

            Set(orders);
        }

        private void SetSingleOrderID(ISingleOrder singleOrder)
        {
            var countKey = String.Format(keyPattern, "countOrder");
            var id = db.Increment(countKey, 1, 1);
            singleOrder.OrderID = id;
        }

    }
}
