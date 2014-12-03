/*global define, JSON*/

define('controllers/StripController', { init: function (routes, viewEngine, Products, Product) {
    "use strict";

    var onSubmit;

    routes.get('#payment-form', function (context) {
        onSubmit(context);
    });


    onSubmit = function (context) {
            var $form = $(this);

            // Disable the submit button to prevent repeated clicks
            $form.find('button').prop('disabled', true);

            Stripe.card.createToken($form, stripeResponseHandler);

            // Prevent the form from submitting with the default action
            return false;
    }

    return {
        onSubmit: onSubmit
    };

}
});
