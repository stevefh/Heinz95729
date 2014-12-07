/*global define, JSON*/

define('controllers/homeController', {
    init: function (routes, viewEngine, Products, Product, Cart, Orders) {
        "use strict";


        var onSearch, onCart, onPayment, onStripeSubmit;
        var onOrders;



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

        routes.post(/^\/api\/cart\/add\/?/i, function (context) {
            return $.ajax({
                url: '/api/cart/add' + context.params.q,
                method: 'POST'
            });
        });

        routes.get(/^\/#\/orders/, function (context) {
            onOrders(context);
        });

        routes.post('/#/create-order-test', function (context) {
            var data = context.params.orderData;
            $.ajax({
                url: '/api/order/add/?q=' + data,
                method: 'GET'
            }).done(function (data) {
                console.log("create order done!");
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
                    // viewEngine.headerVw.cartCount(results.cart().length);
                } else {
                    viewEngine.setView({
                        template: 't-login'
                    });
                }
            });
        };

        routes.get(/^\/#\/payment\/?/i, function (context) {
            onPayment(context);
        });

        onPayment = function (context) {
            return  $.ajax({
                url: '/api/cart',
                method: 'GET'
            }).done(function (data) {
                if (data.charAt(0) != '<') {
                    JSON.parse(data);
                    var results = new Cart(JSON.parse(data));
                    //cart = results;
                    if (results.message() == "") {
                        viewEngine.setView({
                            template: 't-payment-info',
                            data: results
                        });
                    } else {
                        viewEngine.setView({
                            template: 't-cart-grid',
                            data: results
                        });
                    }         
                    viewEngine.headerVw.cartCount(results.cart().length);
                } else {
                    viewEngine.setView({
                        template: 't-login'
                    });
                }
            });
        }

        onStripeSubmit = function (event) {
            var $form = $(this);

            // Disable the submit button to prevent repeated clicks
            $form.find('button').prop('disabled', true);

            Stripe.card.createToken($form, stripeResponseHandler);

            // Prevent the form from submitting with the default action
            return false;
        };

        function stripeResponseHandler(status, response) {
            var $form = $('#payment-form');

            if (response.error) {
                // Show the errors on the form
                $form.find('.payment-errors').text(response.error.message);
                $form.find('button').prop('disabled', false);
            } else {
                // response contains id and card, which contains additional card details
                var token = response.id;
                // Insert the token into the form so it gets submitted to the server
                $form.append($('<input type="hidden" name="stripeToken" />').val(token));
                // and submit
                $form.get(0).submit();
            }
        };

        return {
            onSearch: onSearch,
            onCart: onCart,
            onPayment: onPayment,
            onStripeSubmit: onStripeSubmit,
            onOrders: onOrders
        };
    }
});
