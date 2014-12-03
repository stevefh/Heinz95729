namespace Moviq.Domain.Cart.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using System.Collections.Generic;
    using Moviq.Interfaces.Models;
    using Moviq.Interfaces.Repositories;
    using Newtonsoft.Json;
    using Couchbase;
    using Moviq.Interfaces.Factories;
    using Moviq.Locale;
    using RestSharp;
    using Moq;
    using System.Threading.Tasks;
    using Enyim.Caching.Memcached;
    using Enyim.Caching.Memcached.Results;
    using Moviq.Domain.Products;
    using FluentAssertions;

    [TestClass]
    public class OrderRepositoryFixture
    {
        public OrderRepositoryFixture()
        {
            // Fixture setup
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());
            mockCart = fixture.Freeze<ShoppingCart>();
            string mockCartString = JsonConvert.SerializeObject(mockCart);

            ICouchbaseClient db = MakeMockCbClient(mockCartString);
            IFactory<ICart> cartFactory = new CartFactory();
            ILocale locale = fixture.Freeze<DefaultLocale>();

            IRestClient restClient = MakeMockRestClient();
            IFactory<IProduct> productFactory = new ProductFactory();
            productRepo = new ProductNoSqlRepository(productFactory, db, locale, restClient, "http://localhost:9200/unittests/_search");
            //ICouchbaseClient db, IFactory<IUser> userFactory, ILocale locale
            cartRepo = new CartRepository(productRepo, cartFactory, db, locale);
        }

        IRepository<ICart> cartRepo;
        ICart mockCart;
        IRepository<IProduct> productRepo;

        private IRestClient MakeMockRestClient()
        {
            return Mock.Of<IRestClient>(cli =>
                cli.ExecutePostTaskAsync(It.IsAny<IRestRequest>()) == Task.FromResult<IRestResponse>(
                    new RestResponse
                    {
                        Content = "{\"took\":5,\"timed_out\":false,\"_shards\":{\"total\":5,\"successful\":5,\"failed\":0},\"hits\":{\"total\":4,\"max_score\":0.9581257,\"hits\":[{\"_index\":\"unittests\",\"_type\":\"couchbaseDocument\",\"_id\":\"product::hitchhikers-guide-galaxy\",\"_score\":0.9581257, \"_source\" : {\"meta\":{\"rev\":\"19-00000669babcd3260000000000000112\",\"flags\":274,\"expiration\":0,\"id\":\"product::hitchhikers-guide-galaxy\"}}},{\"_index\":\"unittests\",\"_type\":\"couchbaseDocument\",\"_id\":\"product::restaurant-at-end-universe\",\"_score\":0.15583158, \"_source\" : {\"meta\":{\"rev\":\"19-00000669bcac710c0000000000000112\",\"flags\":274,\"expiration\":0,\"id\":\"product::restaurant-at-end-universe\"}}},{\"_index\":\"unittests\",\"_type\":\"couchbaseDocument\",\"_id\":\"product::universe-everything\",\"_score\":0.13290596, \"_source\" : {\"meta\":{\"rev\":\"19-00000669bca4b6700000000000000112\",\"flags\":274,\"expiration\":0,\"id\":\"product::universe-everything\"}}},{\"_index\":\"unittests\",\"_type\":\"couchbaseDocument\",\"_id\":\"product::dirk-gentlys-detective-agency\",\"_score\":0.092983946, \"_source\" : {\"meta\":{\"rev\":\"19-00000669bcb2c5e00000000000000112\",\"flags\":274,\"expiration\":0,\"id\":\"product::dirk-gentlys-detective-agency\"}}}]}}"
                    }
                )
            );
        }

        private ICouchbaseClient MakeMockCbClient(string mockCartString) //, IDictionary<string, object> mockFindResultSet) 
        {
            var service = new Mock<ICouchbaseClient>();

            service.Setup(cli =>
                cli.Get<string>(It.IsAny<string>())
            ).Returns(mockCartString);

            service.Setup(cli =>
                cli.ExecuteStore(StoreMode.Set, It.IsAny<string>(), It.IsAny<object>())
            ).Returns(new StoreOperationResult
            {
                Success = true
            });

            //service.Setup(cli => 
            //    cli.Get(It.IsAny<IEnumerable<string>>())
            //).Returns(mockFindResultSet);

            return service.Object;
        }

        [TestMethod]
        [TestCategory("CartRepository, when Set is executed with valid data, it")]
        public void should_return_the_cart_that_was_created()
        {
            // given
            var expected = mockCart;

            // when
            var actual = cartRepo.Set(expected);

            // then
            actual.Guid.ShouldBeEquivalentTo(expected.Guid);
            actual.Products.ShouldBeEquivalentTo(expected.Products);
        }

        [TestMethod]
        [TestCategory("ProductNoSqlRepository, when Get is executed with a valid Id, it")]
        public void should_return_a_product_with_the_given_id()
        {
            // given
            var expected = cartRepo.Set(mockCart);

            // when
            var actual = cartRepo.Get(expected.Guid.ToString());

            // then
            actual.Guid.ShouldBeEquivalentTo(expected.Guid);
            actual.Products.ShouldBeEquivalentTo(expected.Products);
        }
    }
}
