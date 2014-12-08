/*global define, JSON*/
define('controllers/cartController', {
    init: function ($, routes, viewEngine, Cart) {
        "use strict";

        var onAddItem, onRemoveItem, onPayment;

        routes.post(/^\/api\/cart\/add\/?/i, function (context) {
            return $.ajax({
                url: '/api/cart/add' + context.params.q,
                method: 'POST'
            });
        });

        routes.post(/^\/#\/addItem\/?/i, function (context) {
            onAddItem(context);
        });

        onAddItem = function (context) {
            var uid = context.params.uid;
            console.log("add uid"+uid);
            return $.ajax({
                url: '/api/cart/add/?q=' + uid,
                method: 'POST'
            }).done(function (data) {
                console.log("data:" + data);
                if (data == true) {
                    console.log("true");
                    routes.navigate("/cart");
                } else {
                    console.log("not true");
                    viewEngine.setView({
                        template: 't-login'
                    });
                }
            });
        };

        routes.get(/^\/#\/removeItem\/?/i, function (context) {
            onRemoveItem(context);
        });

        onRemoveItem = function (context) {
            return $.ajax({
                url: '/api/cart/remove/?q=' + context.params.q,
                method: 'POST'
            }).done(function (data) {
                if (data== true) {
                    routes.navigate("/cart");
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
            var total = $("#cartTotal").val();
            if (total == 0) {
                alert("Please add items to pay!");
                routes.navigate("/cart");
                return true;
            };
            return $.ajax({
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
        };

        return {
            onAddItem: onAddItem,
            onRemoveItem: onRemoveItem,
            onPayment: onPayment
        };
    }


});
