import { displayFormInputs, displayToast, dropdownsClick } from "./main.js";
const uploadBtn = document.querySelector("input[type=file]");
const imgContainer = document.querySelector(".img--upload > div");
const categoryForm = document.querySelector(".category--form");
const Form = document.querySelector("form");
function displayUploadedImg(file) {
  const img = document.createElement("img");
  img.src = URL.createObjectURL(file); // Use a temporary URL for preview
  img.alt = "Product Image";
  imgContainer.appendChild(img);
}
uploadBtn.addEventListener("change", (e) => {
  displayUploadedImg(uploadBtn.files[0]);
  console.log(uploadBtn.files);
});
dropdownsClick();
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
Form.addEventListener("submit", (e) => {
  e.preventDefault();
  const inputs = Form.querySelectorAll("input");
  const productInfo = new FormData();
  inputs.forEach((input) => {
    console.log(productInfo.has([input.getAttribute("name")]));

    productInfo.append([input.getAttribute("name")], input.value);
    console.log(productInfo.has([input.getAttribute("name")]));

    productInfo.has([input.getAttribute("name")]);
  });

  Array.from(uploadBtn.files).forEach((file) => {
    productInfo.append("images", file);
  });

  sendProductInfo(productInfo);
  console.log(productInfo);
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
async function sendProductInfo(product) {
  try {
    const req = await fetch("https://jsonplaceholder.typicode.com/posts", {
      method: "POST",
      body: product,
    });
    if (!req.ok) throw new Error("Can't Add Product");
    displayToast("succes", "Product Added Correctly!");
    return await req.json();
  } catch (e) {
    displayToast("error", e.message);
    throw e;
  }
}
