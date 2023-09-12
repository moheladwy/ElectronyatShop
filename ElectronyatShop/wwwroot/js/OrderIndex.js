function SetOrderStatusBackgroundColor() {
    const orderStatus = document.getElementsByClassName("order-status");

    for (var i = 0; i < orderStatus.length; i++) {
        if (orderStatus[i].innerText.trim() == "CANCELLED") {
            orderStatus[i].style.setProperty("background-color", "red");
        }
        else if (orderStatus[i].innerText.trim() == "DELIVERED") {
            orderStatus[i].style.setProperty("background-color", "green");
        }
        else if (orderStatus[i].innerText.trim() == "ORDERED" || orderStatus[i].innerText.trim() == "ONTHEWAY") {
            orderStatus[i].style.setProperty("background-color", "#17a2b8");
        }
    }
}

SetOrderStatusBackgroundColor();