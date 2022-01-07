var barcode = '';
var interval;

var errorTextOutputTag = $(".create-overlay-modal-error-text");
var errorIconOutputTag = $(".create-overlay-modal-error-icon");
var submitBtn = $(".create-overlay-modal-enter-btn");
var modalInput = $(".create-overlay-modal-input");
var modalInputValue = modalInput.val().trim();
var loader = $(".loader");
var result = false;

document.addEventListener("keydown", function (evt) {
    if ($(".create-overlay-container").is(":visible")) {
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
        if (!evt.code.includes("Shift")) {
            barcode += evt.key;
        }
        interval = setInterval(() => barcode = '', 20);
    }

    if (modalInput.is(":focus") && evt.code.includes("Enter") && modalInput.val().trim().length != 0) {
        ajaxCall(modalInput.val().trim())
        loadingTimeout();
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
        ajaxCall(modalInput.val().trim());
        loadingTimeout();
    }
});

// Click function that close the Modal.
$(".create-overlay-modal-close-btn").click(function () {
    $(".create-overlay-container").css("display", "none");
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
        data: { productID: barcode },
        dataType: "json",
        success: function (dataResult) {
            result = true;
            loader.css("display", "none");
            if (dataResult != false) {
                errorTextIconColor("#dc3545");
                errorTextOutputTag.text("This product already exist");
                errorIconOutputTag.text("error_outline");
            }
            else {
                errorTextIconColor("#28a745")
                errorTextOutputTag.text("This product does not exist");
                errorIconOutputTag.text("check_circle_outline");
            }
        },
        error: function (req, status, error) {
            Console.log(error);
            console.log("Error")
        }
    });
}