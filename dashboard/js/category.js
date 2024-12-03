import { displayToast } from "./main.js";

const addAttributeBtns = document.querySelectorAll(
  ".add--attribute:not(#confirmAdd)"
);
const inputContainers = document.querySelectorAll(".input--container");
const confirmBtns = document.querySelectorAll("#confirmDelete button");
const confirmPopover = document.querySelector("#confirmDelete");
const newCategoryAttr = document.querySelector("#newCategory #confirmAdd");
let attributeID;
let categoryId;
confirmBtns.forEach((btn) =>
  btn.addEventListener("click", () => {
    confirmPopover.hidePopover();
    if (btn.id == "confirmDelete") {
      deleteAttribute(attributeID);
    }
  })
);
addAttributeBtns.forEach((btn) =>
  btn.addEventListener("click", (e) => {
    console.log("click");
    e.stopPropagation();
    e.target.parentElement
      .querySelector(".input--container")
      .classList.add("active");
  })
);
inputContainers.forEach((cont) =>
  cont.addEventListener("click", (e) => e.stopPropagation())
);

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
                <button class="edit--btn btn" categoryId=${id} popovertarget="editPopover">
                  <i class="fa-solid fa-pen-to-square" aria-hidden="true"></i>
                </button>
                <button class="remove--btn btn">
                  <i class="fa-solid fa-trash" aria-hidden="true"></i>
                </button>
              </li>
            </ul>`;
  }

  table.insertAdjacentHTML("beforeend", html);
  const editBtns = document.querySelectorAll(".edit--btn");
  editBtns.forEach((btn) =>
    btn.addEventListener("click", () => {
      categoryId = btn.getAttribute("categoryid");
      console.log(categoryId);
      getCategoryAttribute(btn.getAttribute("categoryid")).then((data) => {
        displayCategorieAttribute(data);
      });
    })
  );
}
async function getCategoryAttribute(catId) {
  const data = {
    categoryId: catId,
  };

  try {
    const req = await fetch("./js/att.json");
    if (!req.ok) throw new Error("Something Went Wrong");
    return await req.json();
  } catch (e) {
    throw e;
  }
}
function displayCategorieAttribute(arr) {
  const attributeDiv = document.querySelector(".attribute");
  attributeDiv.innerHTML = "";
  let html = "";
  arr.forEach(
    (attr) =>
      (html += `<button popovertarget="confirmDelete" attributeid="${attr.id}">${attr.name}</button>`)
  );
  attributeDiv.insertAdjacentHTML("beforeend", html);
  const categoryAttributes = document.querySelectorAll(
    "[popovertarget='confirmDelete']"
  );

  categoryAttributes.forEach((btn) =>
    btn.addEventListener("click", (e) => {
      console.log(btn);
      attributeID = e.currentTarget.getAttribute("attributeID");
      console.log(e.currentTarget.getAttribute("attributeID"));
      console.log(attributeID);
    })
  );
}
const confirmAddAttributeBtn = document.querySelector("#confirmAdd");
confirmAddAttributeBtn.addEventListener("click", () => {
  const attrName = document.querySelector("#attributeName").value;
  addNewAttr(attrName);
});
async function addNewAttr(name) {
  const data = {
    categoryId: categoryId,
    attrName: name,
  };
  console.log(data);

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
const newCategoryForm = document.querySelector("#newCategory .attributes");
console.log(newCategoryForm);
const submitNewCategoryForm = document.querySelector("#submitNewCategoryForm");
newCategoryAttr.addEventListener("click", (e) => {
  const newAttrVal =
    e.target.parentElement.querySelector("#attributeName").value;

  const input = `
  <input class="attr" type="text" value = ${newAttrVal}>
    `;
  e.target.parentElement.querySelector("#attributeName").value = "";
  console.log(e.target.parentElement.parentElement);

  e.target.parentElement.parentElement.classList.remove("active");
  newCategoryForm.insertAdjacentHTML("beforeend", input);
});
submitNewCategoryForm.addEventListener("click", () => {
  const name = newCategoryForm.querySelector("#nCategoryname").value;
  const inputsText = Array.from(newCategoryForm.querySelectorAll(".attr"));
  const result = {
    name: name,
    attributes: inputsText.map((e) => e.value),
  };
  addNewCategory(result);
});
async function addNewCategory(data) {
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
