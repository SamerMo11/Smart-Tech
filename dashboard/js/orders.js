import {
  displayToast,
  handleKababMenus,
  handleItemClicked,
  tableViewContainer,
  handleItemDetailsCloseBtn,
  activeItem,
  closeViewBtn,
} from "./main.js";

function handleEditBtnClicked() {
  tableViewContainer.classList.add("show");
}

async function getOrders() {
  try {
    const req = await fetch("./js/orders.json");
    const data = await req.json();
    return data;
  } catch {
    throw Error("");
  }
}
getOrders()
  .then((orders) => {
    displayOrders(orders);
    const ordersDivs = document.querySelectorAll(
      ".table > ul:not(.table--head)"
    );
    ordersDivs.forEach((orderDiv) => {
      orderDiv.addEventListener("click", (e) => {
        e.stopPropagation();

        handleItemClicked(orderDiv);
        removeActiveKababMenus();
      });
    });
    handleKababMenus();
    const editBtns = document.querySelectorAll(".edit--btn");
    console.log(editBtns);
    editBtns.forEach((btn) => {
      btn.addEventListener("click", (e) => {
        e.stopPropagation();
        console.log(btn);
        tableViewContainer.classList.add("show");
      });
    });
  })
  .catch(() => {
    displayToast("error", "Not Able to Reload Content");
  });

function removeActiveKababMenus() {
  const kababMenus = document.querySelectorAll(".kabab--menu.active");
  kababMenus.forEach((d) => d.classList.remove("active"));
}

function displayOrders(orders) {
  let html = "";
  const ordersTable = document.querySelector(".table");
  console.log(ordersTable);

  for (let i = 0; i < orders.length; i++) {
    const order = orders[i];
    console.log(order);
    html += `  <ul>
      <li>${order.id}</li>
      <li>${order.createdDay}</li>
      <li>${order.paymentMethod}</li>
      <li class="${order.paymentStatus.toLowerCase().replace(" ", "-")}">
        <span>${order.paymentStatus}</span>
      </li>
      <li>
        <span>${order.orderStatus}</span>
      </li>
      <li>$${order.total.toFixed(2)}</li>
      <li class="kabab--menu">
        <i class="fa-solid fa-ellipsis" aria-hidden="true"></i>
         <ul class="dropdown--menu">
          <ul>
            <li class="edit--btn">Edit</li>
            <li>Remove</li>
          </ul>
        </ul>
      </li>
     
    </ul>
    `;
  }
  ordersTable.innerHTML += html;
}

const statusLis = document.querySelectorAll(".status--input li");
statusLis.forEach((s) =>
  s.addEventListener("click", () =>
    ordersFilter({ status: s.innerHTML }).catch((e) =>
      displayToast("error", e.message)
    )
  )
);
console.log(statusLis);
async function ordersFilter(obj) {
  console.log(obj);
  try {
    const req = await fetch("./d.json", {
      method: "GET",
      headers: obj,
    });
    if (!req.ok) {
      throw new Error("Can't Get Orders");
    }
    return await req.json();
  } catch (e) {
    throw e;
  }
}
