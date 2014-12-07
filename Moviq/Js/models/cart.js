/*jslint plusplus: true*/
/*global define*/
define('models/cart', {
    init: function (ko, viewEngine, CartItem) {
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

            //$this.setCartItem = function (cartItem) {
            //    if (!cartItem) {
            //        throw new Error('Argument Exception: the argument, cartItem, must be defined to set a cartItem');
            //    }

            //    if (!(cartItem instanceof CartItem)) {
            //        cartItem = new CartItem(cartItem);
            //    }

            //    cartItem.setCart(this);
            //};

            //$this.setCart = function (cart) {
            //    if (!cart) {
            //        throw new Error('Argument Exception: the argument, cart, must be defined to set cart');
            //    }
            //    var i = 0;

            //    for (i; i < cart.length; i++) {
            //        $this.setCartItem(cart[i],cart);
            //    }
            //}

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

            //$this.removeCartItem = function (cartItem) {
            //    if (!cartItem) {
            //        throw new Error('Argument Exception: the argument, cartItem, must be defined to remove a');
            //    }
            //    if (!(cartItem instanceof CartItem)) {
            //        cartItem = new CartItem(cartItem);
            //    }

            //    $this.cart.remove(cartItem);
            //}
            $this.removeTest = function (uid) {
                console.log("removeTest!");
                // var uid = child.uid();
                console.log("removeTest:" + uid);

                $.ajax({
                    url: '/api/cart/remove/?q=' + uid,
                    method: 'GET'
                }).done(function (data) {
                    if (data.charAt(0) != '<') {
                        cart = $this.cart();

                        var i = 0;

                        for (i; i < cart.length; i++) {
                            console.log(cart[i].uid);
                            if (cart[i].uid() == uid) {
                                $this.cart.remove(cart[i]);
                            }
                        }
                        console.log("cart length" + cart.length);
                        viewEngine.headerVw.subtractFromCart();
                    } else {
                        alert("delete failed!");
                    }
                });

            }
        };

        return Cart;
    }
});
