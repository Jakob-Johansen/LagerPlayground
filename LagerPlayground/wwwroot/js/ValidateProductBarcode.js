


var barcode = '';
var interval;

var errorTextOutputTag = $(".create-overlay-modal-error-text");
var errorIconOutputTag = $(".create-overlay-modal-error-icon");
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
});

function handleBarcode(scanned_barcode) {
    if (scanned_barcode.length >= 1) {

        errorTextOutputTag.text("");
        errorIconOutputTag.text("");

        setTimeout(function () {
            if (result == false) {
                loader.css("display", "block");
            }
        }, 20)

        console.log(scanned_barcode);
        $(".inputTest").val(scanned_barcode);

        $.ajax({
            type: "GET",
            url: "/Scanner/BarcodeExist",
            data: { productID: scanned_barcode },
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
        })
    }
}

$(".create-overlay-modal-close-btn").click(function () {
    $(".create-overlay-container").css("display", "none");
});

function errorTextIconColor(color) {
    errorTextOutputTag.css("color", color);
    errorIconOutputTag.css("color", color);
}