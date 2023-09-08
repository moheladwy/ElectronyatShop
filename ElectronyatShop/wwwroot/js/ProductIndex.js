function IncreamentCart() {
    const cart = document.getElementById("cart-form");

    const span = cart.getElementsByTagName("span").item(0);

    var numberOfProductsInCart = parseInt(span.innerHTML);
    numberOfProductsInCart++;
    span.innerHTML = numberOfProductsInCart.toString().trim();
}

const addcartBtn = document.querySelectorAll('.add-cart');

addcartBtn.forEach(button => {
    button.addEventListener('click', function (event) {
        IncreamentCart();
    });
});