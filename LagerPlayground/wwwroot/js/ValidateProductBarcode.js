


var barcode = '';
var interval;
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
        console.log(scanned_barcode);
        $(".inputTest").val(scanned_barcode);
    }
}

$(".create-overlay-modal-close-btn").click(function () {
    $(".create-overlay-container").css("display", "none");
});