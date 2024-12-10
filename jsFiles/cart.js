// ------------------------------
function handleQuantityInput(card, productId, currentQuantity, type) {
  const increaseBtn = card.querySelector(".increace");
  const decreaseBtn = card.querySelector(".decreace");
  const quantitySpan = card.querySelector(".num");
  const priceSpan = card.querySelector(".price"); // السعر الفردي
  const totalSpan = card.querySelector(".total"); // الإجمالي الفرعي

  let quantity = parseInt(quantitySpan.textContent);
  let unitPrice = parseInt(priceSpan.textContent.replace(/,/g, ""));

  increaseBtn.addEventListener("click", () => {
    if (quantity + 1 <= currentQuantity) {
      quantity++;
      updatePrice();
      updateQuantity(productId, quantity, type);
    }
  });

  decreaseBtn.addEventListener("click", () => {
    if (quantity > 1) {
      quantity--;
      updatePrice();
      updateQuantity(productId, quantity, type);
    }
  });

  function updatePrice() {
    quantitySpan.textContent = quantity;
    totalSpan.textContent = (quantity * unitPrice).toLocaleString();
  }
}
function handeleRemoveBtn(card, type) {
  const closeBtn = card.querySelector(".remove");

  closeBtn.addEventListener("click", () => {
    const productId = card.getAttribute("productId");
    removeItem({
      type: type,
      id: productId,
    }).then(() => {
      card.classList.add("hide");
      setTimeout(() => {
        card.remove();
      }, 1000);
    });
  });
}
async function removeItem(obj) {
  console.log(obj);
  const req = await fetch("https://jsonplaceholder.typicode.com/posts", {
    method: "POST",
    body: JSON.stringify(obj),
  });
  return await req.json();
}
async function getData() {
  const req = await fetch("/jsFiles/GetCartData.json");
  return await req.json();
}
getData().then((data) => {
  console.log(data);
  displayProducts(data.cartProducts);
  displayOffersProducts(data.cartOffers);
});

function displayProducts(cartProducts) {
  console.log(cartProducts);
  const productContainer = document.querySelector(".main .content");
  console.log(productContainer);

  cartProducts.forEach((product) => {
    const {
      cartDetailId,
      variantId,
      imagePath,
      price,
      quantity,
      quantityInCart,
      discount,
    } = product;
    const div = document.createElement("div");
    div.classList.add("product", "card");
    div.setAttribute("productId", variantId);
    div.innerHTML = `
            <i class="fa-solid fa-xmark remove"></i>
            <div class="img">
            <img src="../products/product1/2.png" alt="productimg" loading="lazy">
            </div>
            <p><span class="price">${price}</span> le</p>
            <div class="quantity">
                <div class="qty">
                    <span class="num">${quantityInCart}</span>
                    <div class="arrows">
                        <i class="fa-solid fa-angle-up increace"></i>
                        <i class="fa-solid fa-angle-down decreace"></i>
                    </div>
                </div>
            </div>
            <p><span class="total">${price * quantityInCart}</span> le</p>
        `;
    productContainer.insertAdjacentElement("beforeend", div);
    handleQuantityInput(div, variantId, quantity, "product");
    handeleRemoveBtn(div, "product");
    console.log(productContainer);
  });
}
function displayOffersProducts(cartProducts) {
  const productContainer = document.querySelector(".main .content");
  console.log(productContainer);

  cartProducts.forEach((product) => {
    const {
      cartDetailId,
      offerId,
      img1,
      img2,
      title1,
      title2,
      quantityInCart,
      price,
    } = product;
    const div = document.createElement("div");
    div.classList.add("coll", "card");
    div.setAttribute("productId", offerId);
    div.innerHTML = `
            <div class="coll card">
            <i class="fa-solid fa-xmark remove"></i>
            <div class="imgs">
            <img src="../products/product1/2.png" alt="productimg" loading="lazy">
            <span>+</span>
            <img src="../products/product4/1.png" alt="productimg" loading="lazy">
            </div>
            <p><span class="price">${price}</span> le</p>
            <div class="quantity">
                <div class="qty">
                    <span class="num">1</span>
                    <div class="arrows">
                        <i class="fa-solid fa-angle-up increace"></i>
                        <i class="fa-solid fa-angle-down decreace"></i>
                    </div>
                </div>
            </div>
            <p><span class="total">${price}</span> le</p>
        </div>
        `;
    productContainer.insertAdjacentElement("beforeend", div);
    handleQuantityInput(div, offerId, quantityInCart, "offer");

    handeleRemoveBtn(div, "offer");
    console.log(productContainer);
  });
}
async function updateQuantity(productId, quantity, type) {
  console.log(productId, quantity, type);
  const req = await fetch("https://jsonplaceholder.typicode.com/posts", {
    method: "POST",
    body: JSON.stringify({
      id: productId,
      quantity: quantity,
      type: type,
    }),
  });
  return await req.json();
}
