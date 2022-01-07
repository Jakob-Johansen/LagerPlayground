﻿


var barcode = '';
var interval;

var errorOutputTag = $(".create-overlay-modal-error-text");

document.addEventListener("keydown", function (evt) {
    if ($(".create-overlay-container").is(":visible")) {
        errorOutputTag.text("");

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
        console.log(scanned_barcode);
        $(".inputTest").val(scanned_barcode);

        $.ajax({
            type: "GET",
            url: "/Scanner/BarcodeExist",
            data: { productID: scanned_barcode },
            dataType: "json",
            success: function (dataResult) {
                if (dataResult != false) {
                    console.log("This product already exist");
                    errorOutputTag.text("This product already exist");
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