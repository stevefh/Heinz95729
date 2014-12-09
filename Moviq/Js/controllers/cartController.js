/*global define, JSON*/
/*module designed to handle request generated on cart page or related to cart action*/
define('controllers/cartController', {
    init: function ($, routes, viewEngine, Cart) {
        "use strict";

        var onAddItem, onRemoveItem, onPayment;

        // handles the routes to add items
        routes.post(/^\/#\/addItem\/?/i, function (context) {
            onAddItem(context);
        });

        // process the request to add item
        onAddItem = function (context) {
            var uid = context.params.uid;

            // call the back end api to add items to cart
            return $.ajax({
                url: '/api/cart/add/?q=' + uid,
                method: 'POST'
            }).done(function (data) {
                // check if authentication is requested
                if (data == true) {
                    routes.navigate("/cart");
                } else {
                    viewEngine.setView({
                        template: 't-login'
                    });
                }
            });
        };

        // handles the routes to remove item from cart
        routes.get(/^\/#\/removeItem\/?/i, function (context) {
            onRemoveItem(context);
        });

        // process the request to remove item
        onRemoveItem = function (context) {
            return $.ajax({
                url: '/api/cart/remove/?q=' + context.params.q,
                method: 'POST'
            }).done(function (data) {

                // check if the authentication is needed
                if (data== true) {
                    routes.navigate("/cart");
                } else {
                    viewEngine.setView({
                        template: 't-login'
                    });
                }
            });
        };

        // handles the routes to display payment page
        routes.get(/^\/#\/payment\/?/i, function (context) {
            onPayment(context);
        });

        // process the payment request by checking inventory
        onPayment = function (context) {

            // check if no items in the cart
            var total = $("#cartTotal").val();
            if (total == 0) {
                alert("Please add items to pay!");
                routes.navigate("/cart");
                return true;
            };

            // check if all inventories are available
            return $.ajax({
                url: '/api/cart',
                method: 'GET'
            }).done(function (data) {

                // check the authentication
                if (data.charAt(0) != '<') {
                    JSON.parse(data);
                    var results = new Cart(JSON.parse(data));

                    // check if there's any unavailable items
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
        };

        return {
            onAddItem: onAddItem,
            onRemoveItem: onRemoveItem,
            onPayment: onPayment
        };
    }


});
