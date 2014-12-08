/*jslint plusplus: true*/
/*global define*/
define('models/cart', {
    init: function (ko, routes, viewEngine, CartItem) {
        "use strict";

        if (!ko) {
            throw new Error('Argument Exception: ko is required to init the cart module');
        }

        if (!CartItem) {
            throw new Error('Argument Exception: CartItem is required to init the products module');
        }

        var Cart = function (cartData) {
            var cart = cartData.products;
            var unavailables = cartData.unavailableTitles;
            var $this = this;
            var total = 0;
            $this.cart = ko.observableArray();
            $this.total = undefined;
            $this.message = ko.computed(function () {
                return "";
            });
            $this.cartData = ko.computed(function () {
                return JSON.stringify(cart);
            });


            $this.addCartItem = function (cartItem) {
                if (!cartItem) {
                    throw new Error('Argument Exception: the argument, cartItem, must be defined to add a cartItem');
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
                    if (total)
                        return total.toFixed(2);
                    else
                        return 0;
                });
                $this.test = total;
            }

            if (unavailables) {
                var unLen = unavailables.length;
                console.log("unavailable length:" + length);
                if (unLen != 0) {
                    $this.message = ko.computed(function () {
                        var m = "Sorry! "+unLen+" products no longer available are removed from your cart: "
                        for (var i = 0; i < unLen; i++) {
                            if (i != 0)
                                m += ", "
                            m += unavailables[i];
                        }
                        m += "."
                        return m;
                    });
                }

            }

            $this.removeTest = function (uid) {
                routes.navigate("/removeItem/?q="+uid);
            }
        };

        return Cart;
    }
});
