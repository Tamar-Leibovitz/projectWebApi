

const getCart = () => {
    updateHeaderValues();
    const products = JSON.parse(sessionStorage.getItem("cart")) || [];
    const template = document.getElementById('temp-row').content;

    products.forEach(product => {
        const clone = document.importNode(template, true);
        clone.querySelector('.imageColumn a').href = `Images/${product.imageUrl}`;
        clone.querySelector('.imageColumn img').src = `Images/${product.imageUrl}`;
        clone.querySelector('.itemName').textContent = product.productName;
        clone.querySelector('.price').dataset.price = product.price;
        clone.querySelector('.price').textContent = `${product.price * product.qty}$`;
        clone.querySelector('.quantity-num').dataset.productId = product.productId;
        clone.querySelector('.quantity-num').textContent = product.qty;
        clone.querySelector('#remove').addEventListener('click', () => removeProduct(product));
        clone.querySelector('#plus').addEventListener('click', () => updateQuantity(product, 1));
        clone.querySelector('#minus').addEventListener('click', () => updateQuantity(product, -1));
        document.getElementById('tbody').appendChild(clone);
    });

    updateTotalAmount();
};
const updateQuantity = (product,change) => {
    let cart = JSON.parse(sessionStorage.getItem('cart')) || [];

    cart.forEach(p => {
        if (p.productId == product.productId) {
            p.qty += change;
            if (p.qty == 0) {
                removeProduct(p)
            }
            const quantityDiv = document.querySelector(`.quantity-num[data-product-id="${product.productId}"]`);
            quantityDiv.textContent = p.qty;
            const priceElement = quantityDiv.closest('.item-row').querySelector('.price');
            priceElement.textContent = (p.qty * parseFloat(priceElement.dataset.price)) + '$';
        }
    });

    sessionStorage.setItem('cart', JSON.stringify(cart));
    updateTotalAmount(); 
}


const removeProduct = (product) => {
    let cart = JSON.parse(sessionStorage.getItem('cart')) || [];
    cart = cart.filter(p => p.productId !== product.productId);
    sessionStorage.setItem("cart", JSON.stringify(cart));
    document.getElementById('tbody').replaceChildren();
    getCart();
    updateTotalAmount();
};

const updateTotalAmount = () => {
    const products = JSON.parse(sessionStorage.getItem("cart")) || [];
    let totalAmount = 0;
    products.forEach(product => {
            totalAmount += product.qty * product.price;
        });
    document.getElementById('totalAmount').textContent = `${totalAmount}$`;
    document.getElementById('itemCount').textContent = products.length;
};

const updateHeaderValues = () => {
    const cart = JSON.parse(sessionStorage.getItem('cart')) || [];
        let totalPrice = 0
        cart?.forEach(p => {
            totalPrice += p.price * p.qty
        })
    document.getElementById("itemCount").textContent = cart.length;
    document.getElementById("totalAmount").textContent = `${totalPrice}$`;
};

const placeOrder = async () => {
    const cart = JSON.parse(sessionStorage.getItem('cart'));
    if (!cart || cart.length === 0) {
        alert("הסל ריק, אתה מועבר להוספת מוצרים");
        window.location = 'Products.html';
        return;
    }

    const user = JSON.parse(sessionStorage.getItem('user'));
    if (!user) {
        alert("יש להתחבר/להירשם לפני ביצוע הזמנה");
        return;
    }

    const orderItems = cart.map(p => ({
        "ProductId": p.productId,
        "Quantity": p.qty
    }));
        const order = {
            UserId: user.userId,
            Date: new Date(),
            Sum: document.getElementById("totalAmount").value,
            OrderItems: orderItems
        }

    const response = await fetch("api/Order", {
        method: "POST",
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(order)
    });

    const data = await response.json();
    if (response.ok) {
        sessionStorage.removeItem("cart");
        alert("הזמנתך בוצעה בהצלחה!!");
        window.location = 'Products.html';
    } else {
        alert("התרחשה בעיה במהלך סגירת הזמנה...");
    }
};

getCart();
