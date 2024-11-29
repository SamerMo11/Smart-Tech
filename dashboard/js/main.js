const navDropDownsBtns = document.querySelectorAll(
  ".menu li:has(.dropdown--arrow) a"
);
export const tableViewContainer = document.querySelector(".item-detail-view");
export let activeItem;
export const closeViewBtn = document.querySelector(".colse--btn");
if (closeViewBtn) {
  closeViewBtn.addEventListener("click", () => {
    handleItemDetailsCloseBtn();
  });
}
export function displayFormInputs(inputs, form) {
  console.log(inputs);
  let html = "";
  form.innerHTML = "";
  for (const [d, values] of Object.entries(inputs)) {
    const name = values.name;
    console.log(name);
    const div = `<div>
          <label for="${name}">${name}</label>
          <input type="text" name="${name}" id="${name}" required >
        </div>`;
    html += div;
  }
  form.insertAdjacentHTML("beforeend", html);
}
navDropDownsBtns.forEach((btn) => {
  btn.addEventListener("click", (e) => {
    console.log(e.target);
    e.stopPropagation();
    console.log(e.target.classList.contains("dropdown--arrow"));
    if (e.target.classList.contains("dropdown--arrow")) {
      e.preventDefault();
      e.currentTarget.classList.toggle("active");
    }
  });
});
const transitionTime = 300;

export function displayToast(type, message) {
  const toast = document.createElement("div");
  toast.classList.add("toast");

  const icon =
    type === "error"
      ? '<i class="fa-solid fa-circle-exclamation icon"></i>'
      : '<i class="fa-regular fa-circle-check icon"></i>';
  toast.innerHTML = `
    ${icon}
    <p class="message">${message}</p>
  `;

  document.querySelector("main").appendChild(toast);

  setTimeout(() => {
    toast.classList.add("active");
  }, transitionTime);

  let timeoutId;

  const startTimeout = () => {
    timeoutId = setTimeout(() => {
      toast.classList.remove("active");
      setTimeout(() => {
        toast.remove();
      }, transitionTime);
    }, transitionTime + 4000);
  };

  const stopTimeout = () => {
    clearTimeout(timeoutId);
  };

  startTimeout();

  toast.addEventListener("mouseenter", stopTimeout);
  toast.addEventListener("mouseleave", startTimeout);
}
export function handleItemClicked(itemDiv) {
  if (!activeItem) activeItem = itemDiv;
  if (activeItem && !itemDiv.classList.contains("active")) {
    activeItem.classList.remove("active");
    itemDiv.classList.add("active");
    activeItem = itemDiv;
    tableViewContainer.classList.add("show");
    return;
  }
  itemDiv.classList.remove("active");
  tableViewContainer.classList.remove("show");
}
export function handleItemDetailsCloseBtn() {
  if (tableViewContainer.classList.contains("show")) {
    tableViewContainer.classList.remove("show");
    if (activeItem) activeItem.classList.remove("active");
  }
}
const dropdownsContainers = document.querySelectorAll(".dropdown--container");
console.log(dropdownsContainers);
dropdownsContainers.forEach((div) => {
  const dropdownItems = div.querySelectorAll(".dropdown--menu ul li");
  const currValue = div.querySelector(".value");
  dropdownItems.forEach((item) => {
    item.addEventListener("click", (e) => {
      e.stopPropagation();
      currValue.innerHTML = item.innerHTML;
      div.classList.remove("active");
    });
  });
  div.addEventListener("click", (e) => {
    console.log(e);
    e.stopPropagation();
    e.currentTarget.classList.toggle("active");
  });
});
document.body.addEventListener("click", (e) => {
  const activeElements = document.querySelectorAll(
    ".active:not(.table ul):not(.toast)"
  );

  activeElements.forEach((ele) => {
    if (e.currentTarget == ele) return;
    console.log(e.currentTarget == ele);
    ele.classList.remove("active");
  });
});
const headerArrow = document.querySelector(".header--arrow");
const nav = document.querySelector("nav");
headerArrow.addEventListener("click", () => {
  if (nav.getAttribute("aria-expanded") == "false") {
    nav.setAttribute("aria-expanded", "true");
    headerArrow.classList.remove("fa-arrow-right");
    headerArrow.classList.add("fa-arrow-left");
  } else {
    nav.setAttribute("aria-expanded", "false");
    headerArrow.classList.add("fa-arrow-right");
    headerArrow.classList.remove("fa-arrow-left");
  }
});

export function handleKababMenus() {
  const kababMenus = document.querySelectorAll(".kabab--menu");
  console.log(kababMenus);
  kababMenus.forEach((div) => {
    div.addEventListener("click", (e) => {
      e.stopPropagation();
      const kababMenus = document.querySelectorAll(".kabab--menu.active");
      kababMenus.forEach((d) => d.classList.remove("active"));
      div.classList.add("active");
    });
  });
}
