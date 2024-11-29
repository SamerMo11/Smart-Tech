import { displayFormInputs, displayToast } from "./main.js";
const uploadBtn = document.querySelector("input[type=file]");
const imgContainer = document.querySelector(".img--upload > div");
const categoryForm = document.querySelector(".category--form");
const Form = document.querySelector("form");
function displayUploadedImg(name) {
  const img = document.createElement("img");
  img.src = "./images/" + name;
  img.alt = "product Image";
  imgContainer.appendChild(img);
}
console.log(uploadBtn);
uploadBtn.addEventListener("change", (e) => {
  displayUploadedImg(uploadBtn.files[0].name);
});

async function getCategoriesAttributes(cat) {
  try {
    const req = await fetch(`./js/${cat}.json`);
    if (!req.ok) {
      throw new Error("Can't Dddd");
    }
    return await req.json();
  } catch (e) {
    console.log(e.message);
    displayToast("error", e.message);
  }
}
console.log("Form");

Form.addEventListener("submit", (e) => {
  e.preventDefault();
  const inputs = Form.querySelectorAll("input");
  inputs.forEach((input) => console.log(input.checkValidity()));

  console.log(Form);
});
async function getCategories() {
  try {
    const req = await fetch("./js/categoriesArr.json");
    if (req.ok !== true) {
      throw Error("Can't Load Categories");
    }
    return req.json();
  } catch (e) {
    displayToast("error", e.message);
    throw e;
  }
}
console.log();
function displayCategroiesArr(arr) {
  const categoryInput = document.querySelector(
    ".category--input .dropdown--menu ul"
  );
  categoryInput.innerHTML = "";
  let html = "";

  for (let i = 0; i < arr.length; i++) {
    console.log(i);
    html += `<li aria-data = ${arr[i].id} >${arr[i].name}</li>`;
  }
  categoryInput.insertAdjacentHTML("beforeend", html);
  const categoryText = document.querySelectorAll(".dropdown--menu ul li");

  categoryText.forEach((li) =>
    li.addEventListener("click", async () => {
      const inputs = document.querySelectorAll("form > div:nth-child(n+4)");
      console.log(inputs);

      try {
        const data = await getCategoriesAttributes(li.innerHTML);
        console.log(li.innerHTML);

        displayFormInputs(data, categoryForm);
        categoryForm.insertAdjacentHTML(
          "beforeend",
          `<div class="cspan2">
          <button class="add--btn">Add</button>
        </div>`
        );
        console.log(data[0].attributes);
      } catch (error) {
        // console.error("Error fetching category attributes:", error.message);
      }
    })
  );
}
getCategories().then((data) => {
  console.log(data);
  displayCategroiesArr(data);
});
