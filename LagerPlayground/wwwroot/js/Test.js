﻿
var barcode = '';
var interval;
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
    if (!evt.code.includes("Shift")) {
        barcode += evt.key;
    }
    interval = setInterval(() => barcode = '', 20);
});

function handleBarcode(scanned_barcode) {
    if (scanned_barcode.length >= 1) {

        /*        console.log(scanned_barcode);*/

        $(document).ready(function () {
            $.ajax({
                type: "GET",
                url: "/Scanner/GetJson",
                data: { productID: scanned_barcode },
                dataType: "json",
                success: function (msg) {
                    if (msg != false) {
                        console.log(msg);
                        postHtml(msg);
                    }
                },
                error: function (req, status, error) {
                    Console.log(error);
                }
            });
        });
    }
}

function postHtml(data) {
    var table = document.getElementById("Table");

    $(table).append('<tr><td>' + data.name + '</td><td>' + data.productID + '</td><td>' + data.brandName + '</td><td>' + data.category + '</td><tr/>');

}