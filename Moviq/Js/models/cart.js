/*jslint plusplus: true*/
/*global define*/
define('models/cart', {
    init: function (ko, CartItem) {
        "use strict";

        if (!ko) {
            throw new Error('Argument Exception: ko is required to init the cart module');
        }

        if (!CartItem) {
            throw new Error('Argument Exception: CartItem is required to init the products module');
        }

        var Cart = function (cart) {
            var $this = this;
            var total = 0;
            $this.cart = ko.observableArray();
            $this.total = undefined;
            

            $this.addCartItem = function (cartItem) {
                if (!cartItem) {
                    throw new Error('Argument Exception: the argument, cartItem, must be defined to add a product');
                }

                if (!(cartItem instanceof CartItem)) {
                    cartItem = new CartItem(cartItem);
                }

                $this.cart.push(cartItem);
                total += cartItem.price();
            };

            $this.addCart = function (cart) {
                if (!cart) {
                    throw new Error('Argument Exception: the argument, cart, must be defined to add cart');
                }

                var i = 0;

                for (i; i < cart.length; i++) {
                    $this.addCartItem(cart[i]);
                }
            };

            if (cart) {
                $this.addCart(cart);
                $this.total = ko.computed(function () {
                    return total.toFixed(2);
                });;
            }
        };

        return Cart;
    }
});
