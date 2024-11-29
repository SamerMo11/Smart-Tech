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

async function getProducts() {
  const req = await fetch("./js/products.json");
  const data = await req.json();
  return data;
}
getProducts().then((products) => {
  displayProducts(products);
  itemsClickHandler();
});

function displayProducts(products) {
  let html = "";
  const table = document.querySelector(".table");
  for (let i = 0; i < products.length; i++) {
    const item = products[i];
    html += `  <ul>
              <li>${item.Id}</li>
              <li>${item.Name}</li>
              <li>${item.Category}</li>
      
              <li>${item.Status}</li>
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
