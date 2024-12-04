import {
  displayToast,
  handleKababMenus,
  handleItemClicked,
  tableViewContainer,
  dropdownsClick,
  displayFormInputs,
} from "./main.js";

async function getDate(filter) {
  console.log(filter);
  // , {
  //   method: "POST",
  //   headers: {
  //     "Content-Type": "application/json",
  //   },
  //   body: JSON.stringify(filter || {}),
  // }
  try {
    const req = await fetch("./js/users.json");
    if (!req.ok) {
      throw new Error("Cant't Get Users");
    }
    const data = await req.json();

    return data;
  } catch (e) {
    displayToast("error", e.message);
    throw e;
  }
}
main(undefined);
async function main(filter) {
  getDate(filter).then((data) => {
    displayUsers(data);
    dropdownsClick();
  });
}

function displayUsers(users) {
  const table = document.querySelector(".table");
  table.innerHTML = `<ul class="table--head">
              <li>Id</li>
              <li>Name</li>
              <li>Phone</li>
              <li>CreatedDate</li>
              <li class="role">Role</li>
              <li>Action</li>
            </ul>`;
  let html = "";
  users.forEach((user) => {
    const { id, name, phone, createdDate, role } = user;
    const ul = document.createElement("ul");
    ul.innerHTML = `<li>${id}</li>
              <li>${name}</li>
              <li>${phone}</li>
              <li>${createdDate}</li>
              <li class="dropdown--container role">
              <i class="fa-solid fa-caret-down dropdown--arrow" aria-hidden="true"></i>
              <p class="curr--value"><span class="value">${role}</span></p>
              <ul class="dropdown--menu">
                <ul data-value="Status">
                  <li>Admin</li>
                  <li>User</li>
                  <li>Deleivery</li>
                  <li>Manager</li>
                </ul>
              </ul>
            </li>
              <li>
                <button class="edit--btn btn" category="camera" popovertarget="" targetId = ${id}>
                  <i class="fa-solid fa-pen-to-square" aria-hidden="true"></i>
                </button>

                <button class="remove--btn btn" targetId = ${id}>
                  <i class="fa-solid fa-trash" aria-hidden="true"></i>
                </button>
              </li>`;
    table.insertAdjacentElement("beforeend", ul);
    const editBtn = ul.querySelector(".edit--btn");
    const dropdown = ul.querySelector(".dropdown--container");
    const editPopover = document.querySelector("#editPopover");
    editBtn.addEventListener("click", (e) => {
      console.log(editBtn);
      e.stopPropagation();
      dropdown.classList.toggle("edit");
      ul.classList.add("edit");
      editPopover.hidePopover();
    });
    dropdown.addEventListener("click", (e) => {
      if (!dropdown.classList.contains("edit")) e.stopImmediatePropagation();
      console.log("ddddd");
    });
    const roles = dropdown.querySelectorAll("li");
    roles.forEach((role) => {
      role.addEventListener("click", () => {
        changeRole({ id: id, role: role.innerHTML }).then(() => {
          displayToast("sucsses", "Role Has Been changed!");
          dropdown.classList.remove("edit");
        });
      });
    });
    const removeBtn = ul.querySelector(".remove--btn");
    removeBtn.addEventListener("click", () => {
      console.log("click");
      deleteUser(id).then((e) => {
        displayToast("sucsses", "User Have Been Deleted!");
        ul.remove();
      });
    });
  });
}
const filters = document.querySelectorAll(".table--controllers li ");
filters.forEach((filter) => {
  filter.addEventListener("click", () =>
    main({
      [filter.parentElement.getAttribute("data-value")]: filter.innerHTML,
    })
  );
});

async function changeRole(obj) {
  console.log(obj);
  try {
    const req = await fetch("https://jsonplaceholder.typicode.com/posts", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    });
    return req.json();
  } catch (e) {
    displayToast("error", e.message);
  }
}
async function deleteUser(id) {
  try {
    const req = await fetch("https://jsonplaceholder.typicode.com/posts", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ id }),
    });
    if (!req.ok) throw Error("Can't Delete User!");
    return req.json();
  } catch (e) {
    displayToast("error", e.message);
  }
}
