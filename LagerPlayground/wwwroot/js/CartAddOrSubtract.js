const cartSubmit = $(".cart-buy-btn");
const cartInput = $(".cart-input");
let cartInputVal = "";

cartInput.on('input', function () {
    cartInputVal = cartInput.val().trim();
    if (cartInputVal < 1 && cartInputVal.match(/^[0-9]+$/) == false || cartInputVal == "" || cartInputVal.startsWith("0") || cartInputVal.startsWith("-")) {
        cartInput.val("1");
    }
});