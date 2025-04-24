function initPayPal(totalAmount, dotNetRef) {
    paypal.Buttons({
        createOrder: function (data, actions) {
            return actions.order.create({
                purchase_units: [{
                    amount: {
                        value: totalAmount
                    }
                }]
            });
        },
        onApprove: function (data, actions) {
            return actions.order.capture().then(function (details) {
                dotNetRef.invokeMethodAsync('OnPaymentSuccess', details.id);
            });
        }
    }).render('#paypal-button-container');
}
