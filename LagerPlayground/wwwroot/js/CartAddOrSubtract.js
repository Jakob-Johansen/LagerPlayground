const cartSubmitBtn = $(".cart-buy-btn");
const cartRemoveBtn = $(".cart-remove-btn");
const cartInput = $(".cart-input");
let cartInputVal = "";

const addToCartBtn = $(".shop-addToCart-btn");
const cartProductCount = $(".cart-product-count");

cartInput.on('input', function () {
    cartInputVal = $(this).val().trim();
    if (cartInputVal < 1 && cartInputVal.match(/^[0-9]+$/) == false || cartInput == "0" || cartInputVal == "" || cartInputVal.startsWith("0") || cartInputVal.startsWith("-")) {
        cartInput.val("1");
    }
    AjaxUpdate($(this).attr("data-id"), $(this).val().trim());
});

addToCartBtn.click(function () {
    AjaxUpdateOnlyID($(this).attr("data-id"));
});

cartRemoveBtn.click(function () {
    AjaxRemove($(this).attr("data-id"), $(this));
});

cartSubmitBtn.click(function () {
    console.log("Submit")
});

function AjaxUpdateOnlyID(ID) {
    $.ajax({
        type: "GET",
        url: "/Shop/Buy",
        data: { id: ID },
        success: function () {
            let count = parseInt(cartProductCount.closest("span").text());
            cartProductCount.text(count + 1);
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

function AjaxUpdate(ID, Quantity) {
    $.ajax({
        type: "GET",
        url: "/Shop/Buy",
        data: { id: ID, quantity: Quantity},
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

function AjaxRemove(ID, test) {
    $.ajax({
        type: "GET",
        url: "/Shop/Remove",
        data: { id: ID},
        success: function () {
            test.closest("tr").remove();
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}