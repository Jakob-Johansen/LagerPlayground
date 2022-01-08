var barcode = '';
var interval;

const createOverlayContainer = $(".create-overlay-container");
const errorTextOutputTag = $(".create-overlay-modal-error-text");
const errorIconOutputTag = $(".create-overlay-modal-error-icon");
const submitBtn = $(".create-overlay-modal-enter-btn");
const modalInput = $(".create-overlay-modal-input");
const createBarcodeInput = $(".productIDInputClass");
const loader = $(".loader");
const getID = $("#ID").attr("value");
let modalInputValue = modalInput.val().trim();
let result = false;

document.addEventListener("keydown", function (evt) {
    if (createOverlayContainer.is(":visible")) {
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
        //!evt.code.includes("Shift")
        if (!evt.code.includes("Shift") && !evt.code.includes("CapsLock")) {
            barcode += evt.key;
        }
        interval = setInterval(() => barcode = '', 20);
    }

    if (modalInput.is(":focus") && evt.code.includes("Enter") && modalInput.val().trim().length != 0) {
        loadingTimeout();
        ajaxCall(modalInput.val().trim())
    }
});

// When typing in the input field.
modalInput.on('input', function () {
    if (!loader.is(":visible")) {
        errorTextIconClearText();
        submitBtn.css("display", "block");
    }
});

submitBtn.click(function () {
    if (modalInput.val().trim().length != 0) {
        loadingTimeout();
        ajaxCall(modalInput.val().trim());
    }
});

// Click function that close the Modal.
$(".create-overlay-modal-close-btn").click(function () {
    createOverlayContainer.css("display", "none");
});

// Click function that open the Modal
createBarcodeInput.click(function () {
    createOverlayContainer.css("display", "block");
    if (modalInput.val().length == 0) {
        modalInput.val(createBarcodeInput.attr("value"));
    }
    createBarcodeInput.blur();
});

function errorTextIconColor(color) {
    errorTextOutputTag.css("color", color);
    errorIconOutputTag.css("color", color);
}

function errorTextIconClearText() {
    errorTextOutputTag.text("");
    errorIconOutputTag.text("");
}

function loadingTimeout() {
    submitBtn.css("display", "none");
    createBarcodeInput.val("");
    setTimeout(function () {
        if (result == false) {
            loader.css("display", "block");
        }
    }, 20);
}

// Barcode scanner function
function handleBarcode(scanned_barcode) {
    scanned_barcode = scanned_barcode.trim();
    if (scanned_barcode.length >= 1) {

        errorTextIconClearText();
        loadingTimeout();

        //console.log(scanned_barcode);
        modalInput.val(scanned_barcode);

        ajaxCall(scanned_barcode);
    }
}

// Ajax call function
function ajaxCall(barcode) {
    $.ajax({
        type: "GET",
        url: "/Scanner/BarcodeExist",
        data: { id: getID, productID: barcode },
        dataType: "json",
        success: function (dataResult) {
            result = true;
            loader.css("display", "none");
            errorTextOutputTag.text(dataResult.msg);
            if (dataResult.boolean == false) {
                errorTextIconColor("#dc3545");
                errorIconOutputTag.text("error_outline");
            }
            else {
                errorTextIconColor("#28a745")
                errorIconOutputTag.text("check_circle_outline");
                createBarcodeInput.val(barcode);

                setTimeout(function () {
                    createOverlayContainer.css("display", "none");
                }, 820);
            }
        },
        error: function (req, status, error) {
            Console.log(error);
            console.log("Error")
        }
    });
}