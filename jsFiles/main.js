// // -------------------------------------------------------------
// // -----------------LOADER--------------------------------------
// // -------------------------------------------------------------
// let loader = document.getElementsByClassName('loader');
// // let click = document.getElementsByClassName('click');

// for (let a = 0; a < click.length; a++) {
//     click[a].onclick = function () {
//         document.documentElement.style.setProperty('--height', "100%");
//         btn[a].style.cssText = `
//         opacity:1;
//         transition:2s;
//         `
//     }
// }
// -------------------------------------------------------------
// -----------------REVEAL-TOP----------------------------------
// -------------------------------------------------------------
window.addEventListener("scroll", reveal);

function reveal() {
  var reveals = document.querySelectorAll(".reveal");

  for (var i = 0; i < reveals.length; i++) {
    let windowheight = window.innerHeight;
    let revealtop = reveals[i].getBoundingClientRect().top;
    let revealpoint = 100;

    if (revealtop < windowheight - revealpoint) {
      reveals[i].classList.add("active");
    } else {
      reveals[i].classList.remove("active");
    }
  }
}
// -------------------------------------------------------------
// -----------------REVEAL-BOTTOM-------------------------------
// -------------------------------------------------------------

window.addEventListener("scroll", bottom);

function bottom() {
  var bottoms = document.querySelectorAll(".bottom");

  for (var b = 0; b < bottoms.length; b++) {
    let windowheight = window.innerHeight;
    let revealbottom = bottoms[b].getBoundingClientRect().top;
    let bottompoint = 100;

    if (revealbottom < windowheight - bottompoint) {
      bottoms[b].classList.add("active");
    } else {
      bottoms[b].classList.remove("active");
    }
  }
}

// -------------------------------------------------------------
// -----------------REVEAL-LEFT---------------------------------
// -------------------------------------------------------------

window.addEventListener("scroll", left);

function left() {
  var lefts = document.querySelectorAll(".left");

  for (var l = 0; l < lefts.length; l++) {
    let windowheight = window.innerHeight;
    let revealleft = lefts[l].getBoundingClientRect().top;
    let leftpoint = 100;

    if (revealleft < windowheight - leftpoint) {
      lefts[l].classList.add("active");
    } else {
      lefts[l].classList.remove("active");
    }
  }
}

// -------------------------------------------------------------
// -----------------REVEAL-RIGHT--------------------------------
// -------------------------------------------------------------

window.addEventListener("scroll", right);

function right() {
  var rights = document.querySelectorAll(".right");

  for (var r = 0; r < rights.length; r++) {
    let windowheight = window.innerHeight;
    let revealright = rights[r].getBoundingClientRect().top;
    let rightpoint = 100;

    if (revealright < windowheight - rightpoint) {
      rights[r].classList.add("active");
    } else {
      rights[r].classList.remove("active");
    }
  }
}

// -------------------------------------------------------------
// -----------------ROTATE--------------------------------------
// -------------------------------------------------------------

window.addEventListener("scroll", rotate);

function rotate() {
  var rotates = document.querySelectorAll(".rotate");

  for (var t = 0; t < rotates.length; t++) {
    let windowheight = window.innerHeight;
    let revealtop = rotates[t].getBoundingClientRect().top;
    let revealpoint = 100;

    if (revealtop < windowheight - revealpoint) {
      rotates[t].classList.add("active");
    } else {
      rotates[t].classList.remove("active");
    }
  }
}

// ---------------------------------------------------
// ---------------------------------------------------
// ---------------------------------------------------
let searchIcon = document.getElementById("searchIcon");
const searchProductContainer = document.querySelector(".prods");
searchProductContainer.innerHTML = "";
searchIcon.addEventListener("click", function () {
  console.log("ddd");
  getRecentSearch().then(() => {
    qsearch.classList.add("show");
    qsearch.classList.remove("hide");
    searchIcon.classList.add("color");
  });
});

function displaySearchProducts(products) {
  searchProductContainer.innerHTML = "";
  products.forEach((product) => {
    console.log(product);
    const a = document.createElement("a");
    a.href = `../htmlFiles/product.html?variantId=${product.variantId}&mainId=${product.mainId}`;
    a.classList.add("prod");
    a.innerHTML = `
            <img src="./products/product1/4.png" alt="productImg" loading="lazy">
            <div class="info">
                <p class="title">smart watch 1</p>
                <p>Lorem Ipsum has been the industry's standard.</p>
                <p class="price">price: <span>1000 l.e</span> </p>
            </div>
     `;
    searchProductContainer.appendChild(a);
  });
}
const searchInput = document.querySelector("#textInput");
let searchText = "";
async function search() {
  //    method: "POST",
  // body: JSON.stringify({text :searchText }),
  const req = await fetch("../jsFiles/getsearchProduct.json");
  const data = await req.json();
  displaySearchProducts(data);
  return data;
}
let searchTimeOut;
searchInput.addEventListener("input", (e) => {
  searchText = e.target.value;
  if (searchTimeOut) clearTimeout(searchTimeOut);
  if (searchText !== "") searchTimeOut = setTimeout(search, 1000);
});
async function getRecentSearch() {
  const req = await fetch("/jsFiles/LastSearch.json");
  const data = await req.json();
  displayRecentSearch(data);
}

function displayRecentSearch(arr) {
  const recentSearches = document.querySelector(".recents");
  recentSearches.innerHTML = "";
  arr.forEach((search) => {
    console.log(search);
    const div = document.createElement("div");
    div.classList.add("rec");
    div.innerHTML = `  <p>${search.searchText}</p>
          <i class="fa-solid fa-x" id="remove"></i>`;
    const removeBtn = div.querySelector("#remove");
    removeBtn.addEventListener("click", () => {
      removeSearch(search.searchId).then(getRecentSearch());
    });
    recentSearches.appendChild(div);
  });
}
async function removeSearch(id) {
  console.log(id);
  //    method: "POST",
  // body: JSON.stringify({id : id}),
  const req = await fetch("../jsFiles/LastSearch.json");

  return await req.json();
}
// ----------------------------------------------------
// ----------------------------------------------------
// ----------------------------------------------------
const tab = document.querySelector(".tab");
const links = document.querySelector(".links");
const navicons = document.querySelectorAll(".navicons");
tab.addEventListener("click", () => {
  tab.classList.toggle("active");
  // التحكم في الـ links
  if (links.classList.contains("show")) {
    links.classList.remove("show");
    links.classList.add("hide");
  } else {
    links.classList.remove("hide");
    links.classList.add("show");
  }
  // التحكم في جميع عناصر navicons
  navicons.forEach((icon) => {
    if (icon.classList.contains("show")) {
      icon.classList.remove("show");
      icon.classList.add("hide");
    } else {
      icon.classList.remove("hide");
      icon.classList.add("show");
    }
  });
});
