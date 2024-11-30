import {
  displayToast,
  handleKababMenus,
  handleItemClicked,
  tableViewContainer,
  displayFormInputs,
} from "./main.js";
const form = document.querySelector("form");
async function getFormInputs(cat) {
  try {
    const req = await fetch(`/js/${cat}.json`);
    if (!req.ok) {
      throw new Error(`Can't Load Category Inputs`);
    }
    return req.json();
  } catch (e) {
    displayToast("error", new Error(`Can't Load Category Inputs`));
    throw e;
  }
}

const formInputs = {
  Name: "text",
  Description: "textarea",
  "Mega Pixels": "text",
  "Lens Type": "text",
  "Sensor Type": "text",
  "ISO Range": "text",
  "Shutter Speed": "text",
  "Video Resolution": "text",
  "AutoFocus Points": "number",
  "Battery Life": "text",
  Weight: "text",
  Storage: "text",
  Connectivity: "textarea",
};

function itemsClickHandler() {
  const products = document.querySelectorAll(".table > ul:not(.table--head)");
  products.forEach((item) => {
    item.addEventListener("click", (e) => {
      handleItemClicked(e.currentTarget);
      console.log(e.currentTargext);
    });
  });
}

async function getProducts(filter) {
  try {
    const req = await fetch("/js/products.json", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(filter || {}),
    });
    console.log(JSON.stringify(filter) ?? "{}");

    const data = await req.json();
    console.log(data);
    if (!req.ok) {
      throw new Error("Cant't Get Products");
    }
    return data;
  } catch (e) {
    displayToast("error", e.message);
    throw e;
  }
}
getProducts().then((products) => {
  displayProducts(products);
  itemsClickHandler();
});

function displayProducts(products) {
  let html = "";
  const table = document.querySelector(".table");
  table.innerHTML = ` <ul class="table--head">
              <li>Id</li>
              <li>Name</li>
              <li>Category</li>
              <li>Status</li>
              <li>Payment</li>
              <li class="quantity--num">Quantity</li>
              <li class="sells--num">Sells</li>
              <li class="price">Price</li>
              <li>Action</li>
            </ul>`;
  for (let i = 0; i < products.length; i++) {
    const item = products[i];
    html += `  <ul>
              <li>${item.Id}</li>
              <li>${item.Name}</li>
              <li>${item.Category}</li>
      
              <li  data-value = '${item.Status}'><span>${item.Status}</span></li>
              <li data-value = ${item.PaymentOption}><span>${item.PaymentOption}</span></li>
                      <li class="quantity--num">333</li>
              <li class="sells--num">666</li>
              <li class="price">${item.Price}</li>
              <li><button class="edit--btn btn" category =${item.Category} popovertarget="editPopover"><i class="fa-solid fa-pen-to-square"></i></button>
             <button class="remove--btn btn">  <i class="fa-solid fa-trash"></i></button></li>
            </ul>
    `;
  }
  table.innerHTML += html;
  const editBtns = document.querySelectorAll("[popovertarget=editPopover]");
  console.log(editBtns);
  editBtns.forEach((btn) => btn.addEventListener("click", handleEditClick));
}
function handleEditClick(e) {
  e.preventDefault();
  e.stopPropagation();
  console.log(e.currentTarget);
  getFormInputs(e.currentTarget.getAttribute("category"))
    .then((data) => {
      console.log(data);
      displayFormInputs(data, form);
      const editPopover = document.querySelector("#editPopover");
      editPopover.showPopover();
    })
    .catch((er) => console.log(er));
}
const filters = document.querySelectorAll(
  ".table--controllers .dropdown--container li"
);
filters.forEach((ele) =>
  ele.addEventListener(
    "click",
    () => {
      const obj = {
        [ele.parentElement.getAttribute("data-value")]: ele.innerHTML,
      };
      console.log(obj);
      console.log();
      getProducts(obj).then((products) => {
        displayProducts(products);
        itemsClickHandler();
      });
    } // add your code here to send the request and display the filterd Products
  )
);
async function getProductsStats() {
  try {
    const req = await fetch("./js/productStats.json");
    if (!req.ok) throw new Error("Cant't load Products Stats");
    return req.json();
  } catch (e) {
    throw e;
  }
}
getProductsStats().then((e) => displayStats(e));
function displayStats(obj) {
  const { total, instuck, out } = obj;
  const values = document.querySelectorAll(".table--statistics .value");
  values[0].innerHTML = total;
  values[1].innerHTML = instuck;
  values[2].innerHTML = out;
}
