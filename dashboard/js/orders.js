import {
  displayToast,
  handleKababMenus,
  handleItemClicked,
  tableViewContainer,
  handleItemDetailsCloseBtn,
  dropdownsClick,
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
    dropdownsClick();
    const ordersDivs = document.querySelectorAll(
      ".table > ul:not(.table--head)"
    );
    ordersDivs.forEach((orderDiv) => {
      orderDiv.addEventListener("click", (e) => {
        e.stopPropagation();
        getOrderDetails(e.currentTarget.getAttribute("data-value")).then(
          (data) => {
            displayOrderDetails(data);
            handleItemClicked(orderDiv);
          }
        );

        console.log(e.currentTarget.getAttribute("data-value"));
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
        getOrderDetails(e.currentTarget.getAttribute("data-value")).then(
          (data) => {
            displayOrderDetails(data);
          }
        );

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
    html += `  <ul data-value= ${order.id}>
      <li>${order.id}</li>
      <li>${order.createdDay}</li>
      <li>${order.paymentMethod}</li>
      <li class="${order.paymentStatus.toLowerCase().replace(" ", "-")}">
        <span>${order.paymentStatus}</span>
      </li>
      <li class = ${order.orderStatus}>
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

const statusLis = document.querySelectorAll(".table--controllers li");
console.log(statusLis);
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
async function getOrderDetails(orderId) {
  try {
    const req = await fetch("./js/details.json");
    if (!req.ok) throw new Error("Can't Get Order Details");
    return await req.json();
  } catch (e) {
    displayToast("error", e.message);
    throw e;
  }
}
function displayOrderDetails(orderDeatils) {
  console.log(orderDeatils);
  const updateUserInfo = () => {
    const userName = document.querySelector(".user--overview .user--name");
    userName.innerHTML = orderDeatils.customerName;
    const email = document.querySelector(".user--overview .user--email");
    email.innerHTML = orderDeatils.email;
    const phone = document.querySelector(".user--overview .user--phone");
    phone.innerHTML = orderDeatils.phone;
  };
  const updateOrderList = () => {
    const orderListDiv = document.querySelector(".order--items .products");
    orderListDiv.innerHTML = "";
    const orderList = orderDeatils.orders[0]["items"];

    let html = "";
    orderList.forEach((order) => {
      html += `<div>
                <img src="./images/product.png" alt="Product image">
                <p class="product--title">
                 ${order.name} <span class="sells--value">$${order.price} <span>x${order.quantity}</span></span>
                </p>
              </div>`;
    });
    orderListDiv.insertAdjacentHTML("beforeend", html);
  };
  const updateOrderHead = (order) => {
    const totalPrice = document.querySelector(
      ".item-detail-view .price .value"
    );
    totalPrice.innerHTML = order.total;
    const orderDate = document.querySelector(".order--overview .date");
    orderDate.innerHTML = "Placed on " + order.orderDate;
    const orderStatus = document.querySelector(
      ".item-detail-view .order--status"
    );
    orderStatus.innerHTML = order.status;
  };
  updateUserInfo();
  updateOrderList();
  updateOrderHead(orderDeatils.orders[0]);
}
