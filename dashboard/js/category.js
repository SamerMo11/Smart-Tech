import { displayToast } from "./main.js";

const addAttributeBtn = document.querySelector(".add--attribute");
const inputContainer = document.querySelector(".input--container");
const confirmBtns = document.querySelectorAll("#confirmDelete button");
const confirmPopover = document.querySelector("#confirmDelete");

let categoryId;
confirmBtns.forEach((btn) =>
  btn.addEventListener("click", () => {
    confirmPopover.hidePopover();

    if (btn.id == "confirmDelete") {
      deleteAttribute(categoryId);
    }
  })
);
addAttributeBtn.addEventListener("click", (e) => {
  console.log("click");
  e.stopPropagation();
  inputContainer.classList.add("active");
});
inputContainer.addEventListener("click", (e) => e.stopPropagation());
const categoryAttributes = document.querySelectorAll(
  "[popovertarget='confirmDelete']"
);
categoryAttributes.forEach((btn) =>
  btn.addEventListener(
    "click",
    (e) => (categoryId = e.currentTarget.getAttribute("attributeID"))
  )
);
console.log(categoryAttributes);

async function deleteAttribute(id) {
  console.log(id);
  const data = {
    id: id,
  };
  try {
    const req = await fetch("https://jsonplaceholder.typicode.com/posts", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });
    if (!req.ok) throw new Error("Something Went Wrong");
    return await req.json();
  } catch (e) {
    throw e;
  }
}
async function getCategories() {
  try {
    const req = await fetch("./js/categoriesArr.json");
    console.log(req);

    if (!req.ok) {
      throw new Error("Can't Get Categories List");
    }
    return await req.json();
  } catch (e) {
    displayToast("error", e.message);
    throw e;
  }
}
getCategories().then((arr) => displayCategories(arr));
function displayCategories(arr) {
  const table = document.querySelector(".table");
  console.log(table);
  let html = "";
  for (let i = 0; i < arr.length; i++) {
    const cat = arr[i];
    console.log(cat);
    const { id, name, p_num, sells, quantity } = cat;
    html += `<ul>
              <li>${id}</li>
              <li>${name}</li>
              <li>${p_num}</li>
              <li>${sells}</li>
              <li>${quantity}</li>
              <li>
                <button class="edit--btn btn" category="camera" popovertarget="editPopover">
                  <i class="fa-solid fa-pen-to-square" aria-hidden="true"></i>
                </button>
                <button class="remove--btn btn">
                  <i class="fa-solid fa-trash" aria-hidden="true"></i>
                </button>
              </li>
            </ul>`;
  }

  table.insertAdjacentHTML("beforeend", html);
}
