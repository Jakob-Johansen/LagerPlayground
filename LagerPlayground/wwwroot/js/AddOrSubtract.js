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

const productInfoImageContainer = $(".addOrSubtract-productInfo-image-container");
const productInfoImage = $(".addOrSubtract-productInfo-image");
const productInfoName = $(".addOrSubtract-productInfo-name");

const errorMsg = $(".errorMsg");

// Save product modal variables
const savedProductModalContainer = $(".savedInDbModal-container");
const savedProductModalIcon = $(".savedInDbModal-logo");
const savedProductModalText = $(".savedInDbModal-text");

var red = "#dc3545";
var yellow = "#ffc107";
var green = "#28a745";

let ajaxResult = false;

$(document).ready(function () {
    DefaultPlaceholderInfo();
});

function DefaultPlaceholderInfo() {
    ImageStyling(true);
    productInfoImage.css("display", "block");
    productInfoImage.attr("src", "/Images/Logoer/4329934.png");
    productInfoName.text("Scan the product");
}

function ImageStyling(boolean) {
    if (boolean == true) {
        productInfoImageContainer.css("background-color", "rgb(25, 26, 28)");
        productInfoImage.css("width", "55%");
    }
    else {
        productInfoImageContainer.css("background-color", "white");
        productInfoImage.css("width", "80%");
    }
}

// Btn click events an functions

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

// Save btn
// Sends a post request to the backend
updateProductBtn.click(function () {
    SaveModalFunction("none", "", "", "", false);

    if (barcodeInputField.val().trim().length == 0) {
        DefaultPlaceholderInfo();
        return;
    }

    if (productExist == false) {
        productInfoImage.css("display", "none");
        ImageStyling(true);
        SaveModalFunction("block", "error_outline", red, "No product with the scanned barcode was found", false);
        return;
    }

    if (quantityInputField.val() == '0' || quantityInputField.val().startsWith("0")) {
        ImageStyling(false);
        SaveModalFunction("block", "error_outline", red, "You need to add more than 0 products", true);
        //errorMsg.text("You need to add more than 0 products");
        return;
    }
    ajaxPost();
});

// Scanner event and functions
document.addEventListener("keydown", function (evt) {
    if (interval) {
        clearInterval(interval);
    }
    // if input is enter
    if (evt.code.includes("Enter")) {
        if (barcode) {
            handleBarcode(barcode);
            barcode = '';

            addBtn.blur();
            subtractBtn.blur();

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
    SaveModalFunction("none", "", "", "", false);

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
            else {
                // Quick fix.
                if (ajaxResult == true) {
                    ImageStyling(true);
                    productInfoImage.css("display", "none");
                    SaveModalFunction("block", "error_outline", red, "No product with the scanned barcode was found", false);
                }
            }
        }
    }
}

// Timeout/delay functions
function loadingTimeout() {
    setTimeout(function () {
        if (ajaxResult == false) {
            functionControls.css("display", "none");
            productInfoImage.css("display", "none");
            productInfoName.text("Searching for product");
            loader.css("display", "block");
        }
    }, 20);
}

// Output data to html functions
function AppendData(productData) {
    if (productData.image != null) {
        productInfoImageContainer.css("display", "block");
        productInfoImage.attr("src", "/Images/Products/" + productData.image);
        productInfoName.text(productData.name);
    }
    else {
        productInfoImageContainer.css("display", "none");
        productInfoImage.attr("src", "");
        productInfoName.text("Scan the product");
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
            ajaxResult = true;
            loader.css("display", "none");
            SaveModalFunction("none", "", "", "", false);
            functionControls.css("display", "block");

            if (productData != false) {
                productExist = true;
                ImageStyling(false);

                productInfoImage.css("display", "block");
                barcodeInputField.val(scanned_barcode);
                quantityInputField.val("1");

                AppendData(productData);
            }
            else {
                productExist = false;
                ImageStyling(true);

                productInfoImage.css("display", "none");
                //barcodeInputField.val("");
                quantityInputField.val("0");

                productInfoImage.attr("src", "");
                productInfoName.text("Scan the product");

                SaveModalFunction("block", "error_outline", red, "No product with the scanned barcode was found", false);
            }
        },
        error: function (req, status, error) {
            console.log(status);
            SaveModalFunction("block", "warning_amber", yellow, "CONTACT SUPPORT", true);
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
            quantityInputField.val('0');
            if (result.boolean == true) {

                SaveModalFunction("block", "check_circle_outline", green, result.msg, true);
            }
            else {
                if (result.exception == true) {
                    SaveModalFunction("block", "warning_amber", yellow, result.msg, true);
                }
                else {
                    SaveModalFunction("block", "error_outline", red, result.msg, true);
                }
            }
        },
        error: function (req, status, error) {
            console.log(status);
            SaveModalFunction("block", "warning_amber", yellow, "CONTACT SUPPORT", true);
        }
    });
}

// if true:  Icon: check_circle_outline   Text: The products has been added!                                  Color: Green
// if false: Icon: error_outline          Text: No product with the scanned barcode was found                 Color: Red
// if error: Icon: warning_amber          Text: An database error has occurred, try again or contact support  Color: Yellow

function SaveModalFunction(displayStyle, icon, iconColor, message, backgroundcolor) {
    savedProductModalContainer.css("display", displayStyle);
    savedProductModalIcon.css("color", iconColor);
    savedProductModalIcon.text(icon);
    savedProductModalText.text(message);
    if (backgroundcolor == true) {
        savedProductModalContainer.css("background-color", "rgba(0,0,0, 0.91)");
    }
    else {
        savedProductModalContainer.css("background-color", "rgba(0,0,0, 0.0)");
    }
}