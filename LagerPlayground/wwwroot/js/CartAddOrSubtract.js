﻿const cartSubmitBtn = $(".cart-buy-btn");
const cartRemoveBtn = $(".cart-remove-btn");
const cartInput = $(".cart-input");
let cartInputVal = "";

const addToCartBtn = $(".shop-addToCart-btn");

$(document).ready(function () {
    if (cartInput.val() == "0") {
        cartInput.val("1");
    }
});

cartInput.on('input', function () {
    cartInputVal = $(this).val().trim();
    if (cartInputVal < 1 && cartInputVal.match(/^[0-9]+$/) == false || cartInput == "0" || cartInputVal == "" || cartInputVal.startsWith("0") || cartInputVal.startsWith("-")) {
        cartInput.val("1");
    }
    AjaxUpdate($(this).attr("data-id"), cartInputVal);
});

addToCartBtn.click(function () {
    AjaxUpdateOnlyID($(this).attr("data-id"));
});

cartRemoveBtn.click(function () {
    AjaxRemove($(this).attr("data-id"));
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
            console.log("Success");
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
        success: function () {
            console.log("Success");
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}

function AjaxRemove(ID) {
    $.ajax({
        type: "GET",
        url: "/Shop/Remove",
        data: { id: ID},
        success: function () {
            console.log("Success");
        },
        error: function (req, status, error) {
            console.log(status);
        }
    });
}