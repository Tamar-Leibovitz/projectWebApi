












const categoryIds = [];

const getProductsByFiltering = async () => {
    const minPrice = document.getElementById('minPrice')?.value;
    const maxPrice = document.getElementById('maxPrice')?.value;
    const desc = document.getElementById('nameSearch')?.value;

    let stringUrl = `api/products?desc=${encodeURIComponent(desc || '')}&minPrice=${minPrice}&maxPrice=${maxPrice}`;
    categoryIds.forEach(id => {
        stringUrl += `&categoryIds=${id}`;
    });

    const response = await fetch(stringUrl);
    const products = await response.json();

    if (response.ok) {
        drawProduct(products);
    }
};



const getAllProducts = async () => {
    const cart = JSON.parse(sessionStorage.getItem('cart')) || [];
    document.getElementById("ItemsCountText").textContent = cart.length;

    const response = await fetch(`api/Products`);
    const products = await response.json();

    if (response.ok) {
        drawProduct(products);
        setPrices(products);
    }
};

const filterProducts = () => {
    document.getElementById('ProductList').innerHTML = '';
    getProductsByFiltering();
};


const getAllCategories = async () => {
    const response = await fetch('api/Categories');
    const categories = await response.json();

    if (response.ok) {
        drawCategory(categories);
    }
};

const drawProduct = (products) => {
    const template = document.getElementById('temp-card').content;
    products.forEach(product => {
        const clone = template.cloneNode(true);
        clone.querySelector('img').src = `Images/${product.imageUrl}`;
        clone.querySelector('h1').textContent = product.productName;
        clone.querySelector('.price').textContent = product.price;
        clone.querySelector('.description').textContent = product.description;
        clone.querySelector('button').addEventListener('click', () => addToBasket(product));
        document.getElementById('ProductList').appendChild(clone);
    });
};

const drawCategory = (categories) => {
    const template = document.getElementById('temp-category').content;
    categories.forEach(category => {
        const clone = template.cloneNode(true);
        clone.querySelector('.opt').id = category.categoryId;
        clone.querySelector('.opt').value = category.categoryName;
        clone.querySelector('label').htmlFor = category.categoryName;
        clone.querySelector('.OptionName').textContent = category.categoryName;

        clone.querySelector('input').addEventListener('change', (e) => {
            const categoryId = parseInt(e.target.id, 10);
            if (e.target.checked) {
                if (!categoryIds.includes(categoryId)) categoryIds.push(categoryId);
            }
            else {
                const index = categoryIds.indexOf(categoryId);
                if (index > -1) categoryIds.splice(index, 1);
            }
            filterProducts();
        });

        document.getElementById('categoryList').appendChild(clone);
    });
};

const addToBasket = (product) => {
    const cart = JSON.parse(sessionStorage.getItem('cart')) || [];
    const existingProduct = cart.find(p => p.productId === product.productId);

    if (existingProduct) {
        existingProduct.qty += 1;
        existingProduct.price += product.price;
    } else {
        cart.push({ ...product, qty: 1 });
    }

    sessionStorage.setItem("cart", JSON.stringify(cart));
    document.getElementById("ItemsCountText").textContent = cart.length;
};

const setPrices = (products) => {
    const prices = products.map(p => p.price);
    const minPrice = Math.min(...prices);
    const maxPrice = Math.max(...prices);
    document.getElementById('minPrice').value = minPrice;
    document.getElementById('maxPrice').value = maxPrice;
};



const clearCart = () => {
    sessionStorage.removeItem("cart");
};

getAllCategories();
getAllProducts();
