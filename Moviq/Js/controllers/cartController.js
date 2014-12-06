/*global define, JSON*/
define('controllers/cartController', {
    init: function ($, routes, viewEngine, Cart) {
        "use strict";
        var cart;
        var onRemoveItem;

        routes.get(/^\/#\/removeItem\/?/i, function (context) {
            onRemoveItem(context);
        });

        onRemoveItem = function (context) {
            //console.log("q:" + context.params.q);
            //cart.removeTest(context.params.q);
            //return true;
            return $.ajax({
                url: '/api/cart/remove/?q=' + context.params.q,
                method: 'GET'
            }).done(function (data) {
                if (data.charAt(0) != '<') {
                    JSON.parse(data);
                    var results = new Cart(JSON.parse(data));
                    cart = results;
                    viewEngine.setView({
                        template: 't-cart-grid',
                        data: results
                    });
                    viewEngine.headerVw.subtractFromCart();
                } else {
                    viewEngine.setView({
                        template: 't-login'
                    });
                }
            });
        }
        return {
            onRemoveItem: onRemoveItem,
            cart: cart
        };
    }

    
});
