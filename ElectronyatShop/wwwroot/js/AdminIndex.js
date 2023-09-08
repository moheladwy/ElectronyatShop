function DeleteProduct() {
    const buttons = document.querySelectorAll(".delete-prodcut-btn");

    buttons.forEach(button => {
        button.addEventListener('click', function (event) {
            const result = window.confirm("Are you sure about deleting this product?");
            if (result) {
                window.location.href = button.getAttribute("href");
            }
            else {
                event.preventDefault();
            }
        });
    });
}

DeleteProduct();