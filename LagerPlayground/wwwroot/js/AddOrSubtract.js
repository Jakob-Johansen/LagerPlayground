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
    if (productExist == true) {
        console.log("Save");
    }
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
    if (scanned_barcode.length >= 1) {
        if (scanned_barcode != lastScannedBarcode) {

/*            console.log(lastScannedBarcode);*/
            currentBarcode = scanned_barcode;
            lastScannedBarcode = scanned_barcode;

            ajaxGet(scanned_barcode);
        }
        else {
            if (productExist) {
                InputFunction("+")
            }
            console.log("Test");
        }
    }
}

// Gets the scanned product
function ajaxGet(scanned_barcode) {
    $.ajax({
        type: "GET",
        url: "/Scanner/GetProduct",
        data: { productID: scanned_barcode },
        dataType: "json",
        success: function (productData) {
            if (productData != false) {
                productExist = true;
                quantityInputField.val("1");
                barcodeInputField.val(productData.productID);
            }
            else {
                productExist = false;
                console.log(false);
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
    $ajax({
        type: "POST",
        url: "/Scanner/AddMoreStock",
        data: { quantity: quantityInputField.val(), productID: currentBarcode },
        success: function (result) {
            console.log(result);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    });
}