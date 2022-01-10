// Barcode scanner variables
let barcode = '';
let lastScannedBarcode = '';
var currentBarcode = '';
var productExist = false;
let interval;

// Add Or Subtract control variables
const addBtn = $(".addOrSubtract-add-btn");
const subtractBtn = $(".addOrSubtract-subtract-btn");
const quantityInputField = $(".addOrSubtract-input");
let inputValue = 0;

const barcodeInputField = $(".addOrSubtract-search");
const updateProductBtn = $(".addOrSubtract-save-btn");

const functionControls = $(".invisibleWhileLoading");
const loader = $(".loader");
const errorMsg = $(".errorMsg");

let ajaxResult = false;

addBtn.click(function () {
    InputFunction("+")
});

subtractBtn.click(function () {
    InputFunction("-");
});

function InputFunction(addOrSubtract) {

    if (quantityInputField.val() < 0) {
        quantityInputField.val(0);
    }

    inputValue = quantityInputField.val();

    if (addOrSubtract == "+") {
        quantityInputField.val(inputValue++ + 1);
    }

    if (addOrSubtract == "-") {
        if (inputValue != 0) {
            quantityInputField.val(inputValue-- - 1);
        }
    }
}

// Sends a post request to the backend
updateProductBtn.click(function () {
    if (barcodeInputField.val().trim().length == 0) {
        errorMsg.text("You need to scan a product");
        return;
    }

    if (productExist == false) {
        errorMsg.text("The product dosent exist");
        return;
    }

    if (quantityInputField.val() == '0') {
        errorMsg.text("You need to add more than 0 products");
        return;
    }

    console.log("Save");
    ajaxPost();
});

document.addEventListener("keydown", function (evt) {
    if (interval) {
        clearInterval(interval);
    }
    // if input is enter
    if (evt.code.includes("Enter")) {
        if (barcode) {
            handleBarcode(barcode);
            barcode = '';
            return;
        }
    }
    if (!evt.code.includes("Shift") && !evt.code.includes("CapsLock")) {
        barcode += evt.key;
    }
    interval = setInterval(() => barcode = '', 20);
});

function handleBarcode(scanned_barcode) {
    scanned_barcode = scanned_barcode.trim();
    if (scanned_barcode.length >= 1) {
        errorMsg.text("");
        if (scanned_barcode != lastScannedBarcode) {

            loadingTimeout();
            currentBarcode = scanned_barcode;
            lastScannedBarcode = scanned_barcode;
            barcodeInputField.val(scanned_barcode);
            ajaxGet(scanned_barcode);
        }
        else {
            if (productExist) {
                InputFunction("+")
            }
            console.log("Scan");
        }
    }
}

function loadingTimeout() {
    setTimeout(function () {
        if (ajaxResult == false) {
            functionControls.css("display", "none");
            loader.css("display", "block");
        }
    }, 20);
}

// Gets the scanned product
function ajaxGet(scanned_barcode) {
    $.ajax({
        type: "GET",
        url: "/Scanner/GetProduct",
        data: { productID: scanned_barcode },
        dataType: "json",
        success: function (productData) {
            ajaxResult = true;
            loader.css("display", "none");
            functionControls.css("display", "block");

            if (productData != false) {
                productExist = true;
                barcodeInputField.val(scanned_barcode);
                quantityInputField.val("1");
            }
            else {
                productExist = false;
                barcodeInputField.val("");
                quantityInputField.val("0");
            }
        },
        error: function (req, status, error) {
            Console.log(error);
        }
    });
}

// Post the scanned product and updates the producs quantity
function ajaxPost() {
    $.ajax({
        type: "POST",
        url: "/Scanner/AddMoreStock",
        headers: { "RequestVerificationToken": "@GetAntiXsrfRequestToken()" },
        data: { quantity: quantityInputField.val(), productID: currentBarcode },
        success: function (result) {
            if (result.boolean == true) {
                console.log("Den er true");
            }
            else {
                console.log("Den er false");
            }
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
}