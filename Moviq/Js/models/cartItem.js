/*jslint nomen: true*/
/*global define*/
define('models/cartItem', {
    init: function (ko) {
        "use strict";

        if (!ko) {
            throw new Error('Argument Exception: ko is required to init the cartItem module');
        }

        var CartItem = function (cartItem) {
            var $this = this;

            $this.setCartItemData = function (cartItem, cartItemData) {
                if (!cartItem) {
                    throw new Error('cannot extend the properties of undefined');
                }

                cartItemData = cartItemData || {};

                cartItem.uid = ko.observable(cartItemData.uid);
                cartItem.title = ko.observable(cartItemData.title || undefined);
                cartItem.price = ko.observable(cartItemData.price || undefined);

            };

            if (cartItem) {
                $this.setCartItemData($this, cartItem);
            }
        };

        return CartItem;
    }
});
