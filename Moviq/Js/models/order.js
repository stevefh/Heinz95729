/*jslint nomen: true*/
/*global define*/
define('models/order', {
    init: function (ko,Products) {
        "use strict";

        if (!ko) {
            throw new Error('Argument Exception: ko is required to init the order module');
        }

        var Order = function (order) {
            var $this = this;

            $this.setOrderData = function (order, orderData) {
                if (!order) {
                    throw new Error('cannot extend the properties of undefined');
                }

                orderData = orderData || {};

                var type = orderData._type || 'order';

                order.orderDate = ko.observable(orderData.orderDate);
                order.totalPrice = ko.observable(orderData.totalPrice);

                var products = orderData.products;
                if (!products) {
                    throw new Error('Argument Exception: the argument, products, must be defined to add an order');
                }

                if (!( products instanceof Products)) {
                    products = new Products(products);
                }

                order.products = ko.observable(products.products());
            };

            if (order) {
                $this.setOrderData($this, order);
            }
        };

        return Order;
    }
});