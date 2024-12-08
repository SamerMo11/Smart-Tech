let recognition;
let isRecording = false;

// التحقق من دعم المتصفح لميزة التعرف على الصوت وإنشاء كائن recognition
if ("webkitSpeechRecognition" in window) {
  recognition = new webkitSpeechRecognition();
  recognition.lang = "en-US"; // تعيين اللغة إلى الإنجليزية
  recognition.interimResults = false;
  recognition.maxAlternatives = 1;

  // التعامل مع النتيجة النهائية للنص
  recognition.onresult = function (event) {
    const transcript = event.results[0][0].transcript;
    document.getElementById("textInput").value = transcript;
    console.log("Recognized text:", transcript);
  };

  // معالجة الخطأ أثناء التسجيل
  recognition.onerror = function (event) {
    console.error("Recognition error:", event.error);
  };
} else {
  alert("Sorry, your browser does not support speech recognition.");
}

// بدء التسجيل
function startRecording() {
  if (recognition && !isRecording) {
    recognition.start();
    isRecording = true;
    console.log("Recording started...");
    document.getElementById("startButton").style.display = "none";
    document.getElementById("stopButton").style.display = "flex";
  }
}

// إنهاء التسجيل
function stopRecording() {
  if (recognition && isRecording) {
    recognition.stop();
    isRecording = false;
    console.log("Recording stopped.");
    document.getElementById("startButton").style.display = "flex";
    document.getElementById("stopButton").style.display = "none";
  }
}

function hideQsearch() {
  const qsearch = document.getElementById("qsearch");
  qsearch.classList.add("hide");
  qsearch.classList.remove("show");

  setTimeout(() => {
    qsearch.style.display = "none";
  }, 10);
}

// ------------------------------------------------
// ------------------------------------------------

const optionMenu = document.querySelector(".filter");
const selectBtn = optionMenu.querySelector(".select-btn");
const options = optionMenu.querySelectorAll(".option");

selectBtn.addEventListener("click", () => {
  if (optionMenu.classList.contains("active1")) {
    optionMenu.classList.remove("active1");
    optionMenu.classList.add("hide");
  } else if (optionMenu.classList.contains("hide")) {
    optionMenu.classList.remove("hide");
    optionMenu.classList.add("active1");
  } else {
    optionMenu.classList.add("active1");
  }
});
options.forEach((option) => {
  option.addEventListener("click", () => {
    options.forEach((opt) => opt.classList.remove("active2"));
    option.classList.add("active2");
  });
});

// ----------------------------------------------------------------------
// ----------------------------------------------------------------------

function handleQuantityInput(card) {
  const increaseBtn = card.querySelector(".increace");
  const decreaseBtn = card.querySelector(".decreace");
  const quantitySpan = card.querySelector(".quantity");
  const totalSpan = card.querySelector(".total");

  let quantity = parseInt(quantitySpan.textContent);
  let unitPrice = parseInt(totalSpan.textContent.replace(/,/g, ""));

  increaseBtn.addEventListener("click", () => {
    quantity++;
    updatePrice();
  });

  decreaseBtn.addEventListener("click", () => {
    if (quantity > 1) {
      quantity--;
      updatePrice();
    }
  });

  function updatePrice() {
    quantitySpan.textContent = quantity;
    totalSpan.textContent = (quantity * unitPrice).toLocaleString();
  }
}
async function getDate(filter) {
  console.log(filter);
  // , {
  //   method: "POST",
  //   body: JSON.stringify(filter || {}),
  // }
  const req = await fetch("/jsFiles/shop.json");
  const data = await req.json();
  displayProductsCard(data);
  return data;
}
getDate();
function displayProductsCard(products) {
  console.log(products);
  const itemsContainer = document.querySelector(".items");
  itemsContainer.innerHTML = "";
  let html = "";
  products.forEach((item) => {
    const { id, name, description, price, image, rating, quantity } = item;
    html += `<div class="item" itemId =${id} >
            <i class="fa-solid fa-heart fav"></i>
            <div class="img">
                <img src="../products/product4/1.png" alt="">
            </div>
            <div class="info">
                <p class="name">${name}</p>
                <span>${description}</span>
                <div class="div">
                    <div class="qty">
                        <span class="decreace">-</span>
                        <span class="quantity">1</span>
                        <span class="increace">+</span>
                    </div>
                    <p class="price"><span class="total">${price}</span> le</p>
                </div>
                <div class="div2">
                    <div class="stars">
                        <i class="fa-sharp fa-solid fa-star"></i>
                        <i class="fa-sharp fa-solid fa-star"></i>
                        <i class="fa-sharp fa-solid fa-star"></i>
                        <i class="fa-sharp fa-solid fa-star"></i>
                        <i class="fa-sharp fa-solid fa-star"></i>
                    </div>
                    <button>add to cart</button>
                </div>
            </div>
        </div>`;
  });
  itemsContainer.insertAdjacentHTML("beforeend", html);
  document
    .querySelectorAll(".item .info .div")
    .forEach((card) => handleQuantityInput(card));
}
const categories = document.querySelectorAll(".filter .options li");
categories.forEach((cat) =>
  cat.addEventListener("click", () => getDate({ category: cat.innerHTML }))
);
const priceRange = document.querySelector("#rangeInput");
priceRange.addEventListener("mouseup", (e) => {
  getDate({ price: e.target.value });
});
