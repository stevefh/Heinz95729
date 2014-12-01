﻿using Couchbase;
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

        public CartRepository(IRepository<IProduct> productRepository, IFactory<ICart> cartFactory, ICouchbaseClient db, ILocale locale, IRestClient restClient, string searchUrl)
        {
            this.productRepository = productRepository;
            this.productFactory = cartFactory;
            this.db = db;
            this.locale = locale;
            this.dataType = ((IHelpCategorizeNoSqlData)cartFactory.GetInstance())._type;
            this.keyPattern = String.Concat(this.dataType, "::{0}");
            this.restClient = restClient;
            this.searchUrl = searchUrl;
        }

        IRepository<IProduct> productRepository;
        IFactory<ICart> productFactory;
        ICouchbaseClient db;
        ILocale locale;
        IRestClient restClient;
        string searchUrl;
        string query = "{ \"query\": { \"query_string\": { \"query_string\": { \"query\": \"{0}\" } } } }";


        public ICart Get(string guid)
        {
            if (!KeyExists(String.Format(keyPattern, guid)))
                return db.GetJson<ShoppingCart>(String.Format(keyPattern, guid));
            else
                return Set(new ShoppingCart(Guid.Parse(guid)));
        }

        public ICart Set(ICart cart)
        {
            if (db.StoreJson(StoreMode.Set, String.Format(keyPattern, cart.Guid), cart))
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
            /*
            // alternatively we could use the elasticsearch.NET option
            // http://www.elasticsearch.org/guide/en/elasticsearch/client/net-api/current/_elasticsearch_net.html

            var response = await restClient.ExecutePostTaskAsync(BuildSearchPostRequest(searchFor));
            var result = JsonConvert.DeserializeObject<NoSqlSearchResult>(response.Content);
            
            if (result.hits == null || result.hits.hits == null || result.hits.hits.Count < 1)
            {
                return null;
            }

            List<string> keys = new List<string> { };

            foreach (var item in result.hits.hits)
            {
                keys.Add(item._id);
            }
            
            if (keys.Count > 1)
                throw new Exception(locale.CartFetchFailure);

            return Get(keys);
             */
             
            // TODO: We are breaking Liskov Subsitution by not implementing this method!

            // http://localhost:8092/moviq/_design/dev_books/_view/books?stale=false&connection_timeout=60000&limit=20&skip=0
            throw new Exception(locale.LiskovSubstitutionInfraction);
        }

        private RestRequest BuildSearchPostRequest(string searchFor)
        {
            var request = new RestRequest(searchUrl, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new
            {
                query = new
                {
                    query_string = new
                    {
                        query_string = new
                        {
                            query = searchFor
                        }
                    }
                }
            });

            return request;

            //var request = new RestRequest(searchUrl, Method.GET);
            //request.AddParameter("q", searchFor);

            //return request;
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