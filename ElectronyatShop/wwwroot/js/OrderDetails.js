function ConfirmOrderCancellation() {
    const cancelBtn = document.getElementById("cancel-btn");
    if (cancelBtn != null) {
        cancelBtn.addEventListener('click', event => {
            const result = window.confirm("Once you cancel the order you cannot undo this action, Are you sure about cancelling this order?")
            if (result) {
                window.location.href = cancelBtn.getAttribute("href");
            }
            else {
                event.preventDefault();
            }
        });
    }
}

function SetStatusBackgroundColor() {
    const orderStatus = document.getElementById("order-status");

    if (orderStatus.innerText.trim() == "CANCELLED") {
        orderStatus.style.setProperty("background-color", "red");
    }
    else if (orderStatus.innerText.trim() == "DELIVERED") {
        orderStatus.style.setProperty("background-color", "green");
    }
    else if (orderStatus.innerText.trim() == "ORDERED" || orderStatus.innerText.trim() == "ONTHEWAY") {
        orderStatus.style.setProperty("background-color", "#17a2b8");
    }
}

SetStatusBackgroundColor();
ConfirmOrderCancellation();