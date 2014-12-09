/*global define, JSON*/
define('controllers/paymentController', {
    init: function ($, routes, viewEngine, Orders) {
        "use strict";

        var onConfirm, onTokenCreate;

        Stripe.setPublishableKey('pk_test_XTCnCpSPjzYL6y7xtp6jEcBg');

        //------------------------------------------------

        // handle routes to the payment page
        routes.post(/^\/#\/confirm/, function (context) {
            onConfirm(context);
        });

        // callback method of stripe handling the stripe repsonse
        var stripeResponseHandler = function (status, response) {
            console.log(status);
            if (response.error) {
                // alert the error message
                alert(response.error.message);
            } else {
                // token contains id, last4, and card type
                var token = response.id;
                console.log("token" + token);
                onTokenCreate(token);
            }
        };

        // send request to back end after token is created
        onTokenCreate = function (token) {
            console.log("token:"+token);
            return $.ajax({
                url: '/api/Payment/?t=' + token + '&a=' + $('#order-total').val(),
                method: 'POST'
            }).done(function (data) {
                var resp = JSON.parse(data);
                if (resp.statusCode == 200) {
                    alert("Payment Successful");
                    var orderData = $("#orderData").val();
                    $.ajax({
                        url: '/api/order/add/?q=' + orderData,
                        method: 'GET'
                    }).done(function (data) {
                        console.log("create order done!");
                        if (data.charAt(0) != '<') {
                            var results = new Orders(JSON.parse(data));
                            viewEngine.setView({
                                template: 't-order-grid',
                                data: results
                            });
                            viewEngine.headerVw.cartCount(1);
                            viewEngine.headerVw.subtractFromCart();
                        } else {
                            viewEngine.setView({
                                template: 't-login'
                            });
                        }
                    });
                }
                else {
                    alert("Payment Declined");
                    routes.navigate("/cart");
                }
            });
        };

        // handles the request to pay by card and generates token
        onConfirm = function (context) {
            console.log($('#exp-month').val());
            Stripe.card.createToken({
                number: $('#number').val(),
                cvc: $('#cvc').val(),
                exp_month: $('#exp-month').val(),
                exp_year: $('#exp-year').val()
            }, stripeResponseHandler);

            // Prevent the form from submitting with the default action
            //return false;
        };

        //----------------------------------------------------------------------------------------

        return {
            onConfirm: onConfirm,
            onTokenCreate: onTokenCreate
        };
    }
});
