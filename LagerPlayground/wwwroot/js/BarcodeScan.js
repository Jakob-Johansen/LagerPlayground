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
    interval = setInterval(() => barcode = '', 18);
});

function handleBarcode(scanned_barcode) {
    if (scanned_barcode.length >= 1) {
        console.log(scanned_barcode);

        const para = document.createElement("p");
        const node = document.createTextNode(scanned_barcode);
        para.appendChild(node);

        var element = document.getElementById("scannedText");
        element.appendChild(para);
    }
}