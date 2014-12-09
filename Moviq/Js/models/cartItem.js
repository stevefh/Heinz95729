/*jslint nomen: true*/
/*global define*/
/*Modified from code structure provided by Andy Wright*/
define('models/cartItem', {
    init: function (ko) {
        "use strict";

        if (!ko) {
            throw new Error('Argument Exception: ko is required to init the cartItem module');
        }

        var CartItem = function (cartItem) {
            var $this = this;
            $this.cart = undefined;
            var c = undefined;
            

            $this.setCartItemData = function (cartItem, cartItemData) {
                if (!cartItem) {
                    throw new Error('cannot extend the properties of undefined');
                }

                cartItemData = cartItemData || {};

                cartItem.uid = ko.observable(cartItemData.uid);
                cartItem.title = ko.observable(cartItemData.title || undefined);
                cartItem.price = ko.observable(cartItemData.price || undefined);
                cartItem.removeLink = ko.computed(function () {
                    return "remove/q=" + cartItem.uid();
                });

            };

            if (cartItem) {
                $this.setCartItemData($this, cartItem);
            };

            //$this.setCart = function (cart) {
            //    if (!cart) {
            //        throw new Error('cannot find cart to set the cart for cartItem');
            //    }
                
            //    $this.cart = cart;
            //    console.log("setCart:" + $this.cart.test);
            //    c = cart;
            //}
            //$this.remove = function () {
            //    console.log("remove:" + $this.cart.test);
            //    cartItem.cart.removeCartItem(cartItem);
            //}
        };

        return CartItem;
    }
    //remove:function () {
    //    $this.cart.removeCartItem($this);
    //},
    //setCart:function (cart) {
    //    if (!cart) {
    //        throw new Error('cannot find cart to set the cart for cartItem');
    //    }
    //    cartItem.cart = cart;
    //}
});
