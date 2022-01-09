const addBtn = $(".addOrSubtract-add-btn");
const subtractBtn = $(".addOrSubtract-subtract-btn");
const inputField = $(".addOrSubtract-input");
let inputValue = 0;

addBtn.click(function () {
    InputFunction("+")
});

subtractBtn.click(function () {
    InputFunction("-");
});

function InputFunction(addOrSubtract) {

    if (inputField.val() < 0) {
        inputField.val(0);
    }

    inputValue = inputField.val();

    if (addOrSubtract == "+") {
        inputField.val(inputValue++ + 1);
    }

    if (addOrSubtract == "-") {
        if (inputValue != 0) {
            inputField.val(inputValue-- - 1);
        }
    }
}