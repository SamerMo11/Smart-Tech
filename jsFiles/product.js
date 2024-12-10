const imgsListDiv = document.querySelector(".all--images");
const imgSlider = document.querySelector(".img--slider .imgs");
const zoomDiv = document.querySelector(".zoomDiv");
let product;
console.log(zoomDiv);
const productInfoDiv = document.querySelector(".product--info");
const quickSpecifications = document.querySelector(".quick--specifications");
const xZoom = 2;
let allowZoom = true;
let imgsArray = [];
let activeImg = 0;
let activeImgElement;
let imgsArrayLength;
const productSpecificSection = document.querySelector(".product--specific");
const addToWishListBtn = document.querySelector(
  ".product--info #addToWishlist"
);

window.addEventListener("load", () => {
  allowZoom = window.innerWidth >= 768;
});
const currQuantity = document.querySelector(".quantity input");
function handleQuantityInput(e) {
  if (e.target.classList.contains("dec")) {
    if (currQuantity.value > 1) currQuantity.value = +currQuantity.value - 1;
  } else if (e.target.classList.contains("inc")) {
    currQuantity.value = +currQuantity.value + 1;
  }
}
const qa = document.querySelector(".quantity");
qa.addEventListener("click", (e) => {
  handleQuantityInput(e);
});
function ProductImgs(product) {
  if (product && product.images) {
    return product.images.flatMap((obj) => obj.main);
  } else {
    console.warn("Product images not available.");
    return [];
  }
}

function displayProductImgs() {
  imgsArrayLength = imgsArray.length;
  imgsListDiv.innerHTML = "";
  imgSlider.innerHTML = "";
  for (let i = 0; i < imgsArray.length; i++) {
    const img1 = document.createElement("img");
    img1.src = "../products/" + imgsArray[i];
    img1.alt = "Product Image";
    img1.classList.add("img" + i);
    imgSlider.appendChild(img1);
    img1.getBoundingClientRect().width;
    const img2 = document.createElement("img");
    img2.src = "../products/" + imgsArray[i];
    img2.alt = "Product Image";
    imgsListDiv.appendChild(img2);
    activeImgElement = img2;
    img2.addEventListener("load", () => {
      setZoomImgSize(activeImg);
    });
    img2.addEventListener("click", () => {
      handleImgClick(i);
    });
  }
}

function display() {
  displayProductImgs();
  handleImgClick(0);
  handleImgSliderBtns();
}

function handleImgClick(indx) {
  imgSlider.style.transform = `translateX(${100 * -indx}%)`;
  activeImg = indx;
  console.log(activeImg);
  const img = document.querySelector(".img" + indx);

  setZoomImgSize(activeImg);

  zoomDiv.style.backgroundImage = `url(../products/${imgsArray[activeImg]})`;
}

function handleImgSliderBtns() {
  const controllers = Array.from(
    document.querySelectorAll(".img--slider .controllers button")
  );
  controllers.forEach((btn) => {
    btn.addEventListener("mouseenter", () => {
      productInfoDiv.classList.remove("active");
    });
    btn.addEventListener("mouseleave", () => {
      productInfoDiv.classList.add("active");
    });
    btn.addEventListener("click", (e) => {
      if (e.currentTarget.id === "nextImg" && activeImg < imgsArrayLength - 1) {
        activeImg += 1;
      } else if (e.currentTarget.id === "prevImg" && activeImg > 0) {
        activeImg -= 1;
      }
      handleImgClick(activeImg);
    });
  });
}
async function getProduct() {
  const req = await fetch("../jsFiles/product.json");
  const data = await req.json();
  product = await data;
  return data;
}

const productData = localStorage.getItem("product");
let productId = 0;
getProduct().then((data) => {
  product = data;
  console.log(data);
  imgsArray = product.images;
  display();
  productPath(product.category);
  productDetails(product);
  displayReviews(product.review);
  displayRelatedProducts(product.realtedProducts);
  displaySpecifications(product.specifications);
  const activeImgContainer = document.querySelector("#img--slider");
  const mouseDiv = document.querySelector(".hover");
  const overlay = document.querySelector(".overlay");
  productId = data.sendedId;
  handleAddToCartClicked();
  addToWishList();

  activeImgContainer.addEventListener("mouseenter", (e) => {
    if (!allowZoom) return;
    productInfoDiv.classList.add("active");
  });
  activeImgContainer.addEventListener("mouseleave", (e) => {
    if (!allowZoom) return;
    productInfoDiv.classList.remove("active");
  });
  window.addEventListener("resize", () => {
    setZoomImgSize(activeImg);
    allowZoom = window.innerWidth >= 768;
  });
  activeImgContainer.addEventListener("mousemove", function mouseMoveHandle(e) {
    if (!allowZoom) return;
    console.log(activeImg);
    const clientBounding = activeImgContainer.getBoundingClientRect();
    const imgBounding = imgSlider
      .querySelector(".img" + activeImg)
      .getBoundingClientRect();

    const divX = clientBounding.x + +mouseDiv.getBoundingClientRect().width / 2;
    const divY = clientBounding.y + mouseDiv.getBoundingClientRect().height / 2;
    const mouseX = e.clientX;
    const mouseY = e.clientY;
    const totalX = Math.min(
      Math.max(mouseX - divX, 0),
      clientBounding.width - mouseDiv.getBoundingClientRect().width
    );
    const totalY = Math.min(
      Math.max(mouseY - divY, 0),
      clientBounding.height - mouseDiv.getBoundingClientRect().height
    );
    mouseDiv.style.transform = `translate(${totalX}px , ${totalY}px)`;
    zoomDiv.style.backgroundPosition = `${
      (-totalX - (clientBounding.x - imgBounding.x)) * xZoom
    }px ${(-totalY - (clientBounding.y - imgBounding.y)) * xZoom}px`;
  });
});

function setZoomImgSize(i) {
  const divD = imgSlider.querySelector(".img" + i).getBoundingClientRect();
  zoomDiv.style.backgroundSize = `${divD.width * xZoom}px ${
    divD.height * xZoom
  }px`;
}
function productPath(category) {
  const productPath = document.querySelector(".product--head");
  productPath.innerHTML = `<a href="index.html">Home</a>
    <a  href="index.html?${category.id}" class="product--name">${category.name}</a>
            <p class="product--name">Regular Fit Windbreaker</p>`;
}
function productDetails(product) {
  const { name, description, sendedId, price, products, details } = product;
  document.querySelector(".product--title").innerHTML = name;
  document.querySelector(".product--text").innerHTML = description;
  document.querySelector(".product--price").innerHTML = "$" + price;
  const stars = Array.from(
    document.querySelectorAll(".product--details .stars > *")
  );
  displayRating(details.totalRating, details.userCountRating, stars);
  addToWishListBtn.setAttribute("isFav", details.isFavourite);
  displayColors(product.colors);
  relatedProductsSpecifations(product.combinedAttributes, product.details.id);
}
function displayRating(totalRating, ratingCounter, starsArr) {
  const parentElement = starsArr[0].parentElement.parentElement;
  console.log(parentElement);
  for (let i = 0; i < 5; i++) {
    if (i < totalRating) {
      starsArr[i].style.fill = "#FFAD33";
    } else {
      starsArr[i].style.fill = "#656f7e";
    }
  }
  if (ratingCounter) {
    if (parentElement.querySelector(".rating--number"))
      parentElement.querySelector(
        ".rating--number"
      ).innerHTML = `(${ratingCounter} Reviews)`;
  }
}
function displayColors(products) {
  const colorsDiv = productSpecificationDiv("colors");
  let html = "";
  for (let i = 0; i < products.length; i++) {
    html += `<li style="
    background-color: ${products[i].color};
"><a href="?id=${products[i].id}"></a></li>`;
  }
  colorsDiv.innerHTML += html;
  quickSpecifications.appendChild(colorsDiv);
}
function productSpecificationDiv(title) {
  const div = document.createElement("div");
  div.classList.add(title);
  div.style.display = "flex";
  div.innerHTML = `<p class="specifinc--name">${title}:</p>`;
  return div;
}
function relatedProductsSpecifations(obj, activeId) {
  for (const [key, value] of Object.entries(obj)) {
    const div = productSpecificationDiv(key);
    quickSpecifications.appendChild(div);
    function handleClick() {
      console.log("clicked");
    }
    for (let i = 0; i < value.length; i++) {
      const li = document.createElement("li");
      li.innerHTML = `<button href="?id=${value[i].id}">
                    ${value[i].value}
                  </button>`;
      li.addEventListener("click", handleClick);

      div.appendChild(li);
    }
  }
}
const specificationContainer = document.querySelector(
  ".specification--container"
);
function displaySpecifications(arr) {
  const maxLength = 4;
  let html = "<div class='short--specific'>";
  for (let i = 0; i < arr.length; i++) {
    if (i == maxLength) {
      specificationContainer.innerHTML +=
        html +
        " <button class='specific--btns'  id='seeMoreBtn'>See More</button></div>";
      html = "<div class='seeMore'><div>";
    }
    html += `
    <div>
    <p class="specific--title">${arr[i].key}</p>
                <p class="specific--value">${arr[i].value}</p>
         </div>     `;
  }
  specificationContainer.innerHTML +=
    html +
    "<button class='specific--btns' id='seeLessBtn'>See Less</button></div></div>";
  const seeMoreBtn = specificationContainer.querySelector("#seeMoreBtn");
  const seeLessBtn = specificationContainer.querySelector("#seeLessBtn");
  seeMoreBtn.addEventListener("click", handleSpecificationBtnClick);
  seeLessBtn.addEventListener("click", handleSpecificationBtnClick);
}

function handleSpecificationBtnClick() {
  specificationContainer.setAttribute(
    "aria-extend",
    specificationContainer.getAttribute("aria-extend") == "false"
      ? "true"
      : "false"
  );
  productSpecificSection.scrollIntoView({ behavior: "smooth" });
}

const relatedProductsContainer = document.querySelector(
  ".products--container > div"
);
let allowScroll = false;

function displayRelatedProducts(products) {
  console.log(products);
  let html = "";

  for (let i = 0; i < products.length; i++) {
    const {
      name,
      image,
      totalRating,
      userCountRating,
      isFavourite,
      price,
      id,
    } = products[i];
    html += `<div class="related-${id}">
    <div class="product--contollers">
    <button class="wishlist--btn"> <div></div> </button>
    <button class="Quick--view"><div></div></button>
    
    </div>
                <div class="product--img">
                <button class="addToCart">Add To Card</button>
                  <img src="../products/${image}" alt="realted" srcset="">
                </div>
                <p class="product--name">${name}</p>
                <p class="product--price">$${price}</p>
                <div class="product--rating">
                  <div class="stars">
                    <svg width="16" height="15" viewBox="0 0 16 15" fill="#656f7e" xmlns="http://www.w3.org/2000/svg">
                      <path d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"></path>
                    </svg>
                    <svg width="16" height="15" viewBox="0 0 16 15" fill="#656f7e" xmlns="http://www.w3.org/2000/svg">
                      <path d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"></path>
                    </svg>
                    <svg width="16" height="15" viewBox="0 0 16 15" fill="#656f7e" xmlns="http://www.w3.org/2000/svg">
                      <path d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"></path>
                    </svg>
                    <svg width="16" height="15" viewBox="0 0 16 15" fill="#656f7e" xmlns="http://www.w3.org/2000/svg">
                      <path d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"></path>
                    </svg>
                    <svg width="16" height="15" viewBox="0 0 16 15" fill="#656f7e" xmlns="http://www.w3.org/2000/svg">
                      <path d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"></path>
                    </svg>
                  </div>
                  <span class="rating--number">(${userCountRating})</span>
                </div>
              </div>`;
  }

  relatedProductsContainer.innerHTML += html;
  const starsDivs = relatedProductsContainer.querySelectorAll(".stars");
  starsDivs.forEach((stars, i) => {
    const starsArray = starsDivs[i].querySelectorAll("svg");
    displayRating(
      products[i].totalRating,
      products[i].userCountRating,
      starsArray
    );
  });
  // handle related products buttons
  const btns = Array.from(
    document.querySelectorAll(".products--container .arrowBtn")
  );
  let x = 0;
  const div = relatedProductsContainer.querySelector("*>div");

  checkIsScroll(products.length);
  window.addEventListener("resize", () => {
    checkIsScroll(products.length);
  });
  btns.forEach((btn) => {
    console.log(btn);
    if (!allowScroll) {
      btn.style.display = "none";
    }
    btn.addEventListener("click", (e) => {
      console.log(e);
      console.log("Sss");
      const width = div.getBoundingClientRect().width;
      console.log(width);
      if (e.currentTarget.id == "nextProduct") {
        if (x == products.length - 1) return;
        x++;
        console.log(x);
        relatedProductsContainer.style.transform = `translateX(${
          (-245 - 15) * x
        }px)`;
      } else {
        if (x == 0) return;

        x--;

        relatedProductsContainer.style.transform = `translateX(${
          (-245 - 15) * x
        }px)`;
      }
    });
  });
}
function checkIsScroll(i) {
  console.log(i);
  const productWidth = 260;
  const gap = 15;
  const neededWidth = (productWidth + gap) * i;
  console.log(neededWidth);
  const parentContainerWidth =
    relatedProductsContainer.parentElement.offsetWidth;
  allowScroll = neededWidth > parentContainerWidth;

  const btns = document.querySelectorAll(".products--container .arrowBtn");
  btns.forEach((btn) => {
    btn.style.display = allowScroll ? "block" : "none";
  });
}
function displayReviews(reviews) {
  const reviewsDiv = document.querySelector(".reviews");
  reviewsDiv.innerHTML = "";
  let html = "";
  reviews.forEach((review) => {
    const { date, image, message, rateValue, userName } = review;
    html += `
     <div>
                <img src="../products/user.png" alt="" srcset="" />
                <div class="user--info">
                  <p class="user--name">${userName}</p>
                  <p class="review--text">
                  ${message}
                  </p>
                </div>
                <p class="review--date">Reviewed on ${date}</p>
                <div class="stars">
                  <svg
                    width="16"
                    height="15"
                    viewBox="0 0 16 15"
                    fill="#656f7e"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"
                    />
                  </svg>
                  <svg
                    width="16"
                    height="15"
                    viewBox="0 0 16 15"
                    fill="#656f7e"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"
                    />
                  </svg>
                  <svg
                    width="16"
                    height="15"
                    viewBox="0 0 16 15"
                    fill="#656f7e"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"
                    />
                  </svg>
                  <svg
                    width="16"
                    height="15"
                    viewBox="0 0 16 15"
                    fill="#656f7e"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"
                    />
                  </svg>
                  <svg
                    width="16"
                    height="15"
                    viewBox="0 0 16 15"
                    fill="#656f7e"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M14.673 7.17173C15.7437 6.36184 15.1709 4.65517 13.8284 4.65517H11.3992C10.7853 4.65517 10.243 4.25521 10.0617 3.66868L9.33754 1.32637C8.9309 0.0110567 7.0691 0.0110564 6.66246 1.32637L5.93832 3.66868C5.75699 4.25521 5.21469 4.65517 4.60078 4.65517H2.12961C0.791419 4.65517 0.215919 6.35274 1.27822 7.16654L3.39469 8.78792C3.85885 9.1435 4.05314 9.75008 3.88196 10.3092L3.11296 12.8207C2.71416 14.1232 4.22167 15.1704 5.30301 14.342L7.14861 12.9281C7.65097 12.5432 8.34903 12.5432 8.85139 12.9281L10.6807 14.3295C11.7636 15.159 13.2725 14.1079 12.8696 12.8046L12.09 10.2827C11.9159 9.71975 12.113 9.10809 12.5829 8.75263L14.673 7.17173Z"
                    />
                  </svg>
                </div>
              </div>`;

    // displayRating(totalRating, ratingCounter, starsArr);
  });
  reviewsDiv.insertAdjacentHTML("beforeend", html);
  const starsDivs = reviewsDiv.querySelectorAll(".stars");
  console.log(starsDivs);
  starsDivs.forEach((div, i) => {
    console.log(div);
    const stars = div.querySelectorAll("svg");
    console.log(stars);
    displayRating(reviews[i].rateValue, undefined, stars);
  });
}
const reviewStars = document.querySelectorAll(".user--review .stars svg");
console.log(reviewStars);
reviewStars.forEach((stars, i) => {
  console.log(stars);
  stars.addEventListener("mousemove", (e) => {
    displayRating(i + 1, undefined, reviewStars);
  });
});

async function handleFavBtn(obj) {
  console.log(obj);
  const req = await fetch("https://jsonplaceholder.typicode.com/posts", {
    method: "POST",
    body: JSON.stringify(obj),
  });
  return await req.json();
}
async function handleAddToCart(obj) {
  console.log(obj);
  const req = await fetch("https://jsonplaceholder.typicode.com/", {
    method: "POST",
    body: JSON.stringify(obj),
  });
  return await req.json();
}
function handleAddToCartClicked() {
  const addToCartBtn = document.querySelector("#addToCart");
  const quantity = document.querySelector(".quantity input").value;
  addToCartBtn.addEventListener("click", () => {
    handleAddToCart({ productId: productId, quantity: quantity });
  });
}
function addToWishList() {
  addToWishListBtn.addEventListener("click", (obj) => {
    const currStat = addToWishListBtn.getAttribute("isFav");
    const req = async (obj) => {
      const req = await fetch("https://jsonplaceholder.typicode.com/", {
        method: "POST",
        body: JSON.stringify({}),
      });
      console.log(obj);
      console.log(req);

      return await req.json();
    };
    req({
      productId: productId,
      isFav: currStat == "false" ? "true" : "false",
    }).then(() => {
      addToWishListBtn.setAttribute(
        "isFav",
        currStat == "false" ? "true" : "false"
      );
    });
  });
}
