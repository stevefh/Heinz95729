/*global define, JSON*/

define('controllers/homeController', {
    init: function (routes, viewEngine, Products, Product, Cart, Orders) {
        "use strict";

        var onSearch, onCart, onOrders;

        // GET /#/search/?q=searchterm
        // search for products
        routes.get(/^\/#\/search\/?/i, function (context) {
            onSearch(context);
        });

        routes.get('/', function (context) {
            viewEngine.setView({
                template: 't-empty',
                message: 'hello word!'
            });
        });

        routes.get(/^\/#\/cart/, function (context) {
            onCart(context);
        });

        routes.get(/^\/#\/orders/, function (context) {
            onOrders(context);
        });

        onOrders = function (context) {
            return $.ajax({
                url: '/api/order',
                method: 'GET'
            }).done(function (data) {
                if (data.charAt(0) != '<') {
                    var results = new Orders(JSON.parse(data));
                    viewEngine.setView({
                        template: 't-order-grid',
                        data: results
                    });
                } else {
                    viewEngine.setView({
                        template: 't-login'
                    });
                }
            });
        };

        onSearch = function (context) {
            return $.ajax({
                url: '/api/search/?q=' + context.params.q,
                method: 'GET'
            }).done(function (data) {
                var results = new Products(JSON.parse(data));

                if (results.products().length > 0) {
                    viewEngine.setView({
                        template: 't-product-grid',
                        data: results
                    });
                } else {
                    viewEngine.setView({
                        template: 't-no-results',
                        data: { searchterm: context.params.q }
                    });
                }
            });
        };


        onCart = function (context) {
            return $.ajax({
                url: '/api/cart',
                method: 'GET'
            }).done(function (data) {
                if (data.charAt(0) != '<') {
                    var results = new Cart(JSON.parse(data));
                    //cart = results;
                    viewEngine.setView({
                        template: 't-cart-grid',
                        data: results
                    });
                    viewEngine.headerVw.cartCount(results.cart().length);
                } else {
                    viewEngine.setView({
                        template: 't-login'
                    });
                }
            });
        };

        return {
            onSearch: onSearch,
            onCart: onCart,
            onOrders: onOrders
        };
    }
});
