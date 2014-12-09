/*jslint plusplus: true*/
/*global define*/
/*Modified from code structure provided by Andy Wright*/
define('models/orders', {
    init: function (ko, Order) {
        "use strict";

        if (!ko) {
            throw new Error('Argument Exception: ko is required to init the orders module');
        }

        if (!Order) {
            throw new Error('Argument Exception: Order is required to init the orders module');
        }

        var Orders = function (orders) {
            var $this = this;
            $this.orders = ko.observableArray();

            $this.addOrder = function (order) {
                if (!order) {
                    throw new Error('Argument Exception: the argument, order, must be defined to add a order');
                }

                if (!(order instanceof Order)) {
                    order = new Order(order);
                }

                $this.orders.push(order);
            };

            $this.addOrders = function (orders) {
                if (!orders) {
                    throw new Error('Argument Exception: the argument, orders, must be defined to add orders');
                }

                var i = 0;

                for (i; i < orders.length; i++) {
                    $this.addOrder(orders[i]);
                }
            };

            if (orders) {
                $this.addOrders(orders);
            }
        };

        return Orders;
    }
});
