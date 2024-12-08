// // ----------------------------------------------------------
// // -----------------ARROW-TO-FIRST-SECTION-------------------
// // ----------------------------------------------------------

// let arr = document.getElementById("arrow");

// window.onscroll = function () {
//   if (scrollY >= 740) {
//     arr.style.display = "block";
//   } else {
//     arr.style.display = "none";
//   }
// };
// // ---------------------------------------------------
// // ---------------------------------------------------
// // ---------------------------------------------------

// ---------------------------------------------------
// ---------------------------------------------------
// ---------------------------------------------------
let searchIcon = document.getElementById("searchIcon");

searchIcon.addEventListener("click", function () {

    qsearch.classList.add("show");
    qsearch.classList.remove("hide");
    searchIcon.classList.add("color");
});
// ---------------------------------------------------
// ---------------------------------------------------
// ---------------------------------------------------
let nextButton = document.getElementById("after");
let prevButton = document.getElementById("before");
let carousel = document.querySelector(".carousel");
let listHTML = document.querySelector(".carousel .list .items");
let spans = document.querySelectorAll(".carousel .list .arrows span");
let currentSpanIndex = 0;

nextButton.onclick = function () {
  showSlider("after");
};

prevButton.onclick = function () {
  showSlider("before");
};

const updateSpanHighlight = () => {
  spans.forEach((span, index) => {
    span.classList.remove("active");
  });
  spans[currentSpanIndex].classList.add("active");
};

const showSlider = (type) => {
  carousel.classList.remove("after", "before");
  let items = document.querySelectorAll(".carousel .list .items .item");

  if (type === "after") {
    listHTML.appendChild(items[0]);
    carousel.classList.add("after");
    currentSpanIndex = (currentSpanIndex + 1) % spans.length;
  } else {
    listHTML.prepend(items[items.length - 1]);
    carousel.classList.add("before");
    currentSpanIndex = (currentSpanIndex - 1 + spans.length) % spans.length;
  }

  updateSpanHighlight();
};

updateSpanHighlight();

// ---------------------------------------------------
// ---------------------------------------------------
// ---------------------------------------------------
const tracks = document.querySelectorAll(".track");
const nextButtons = document.querySelectorAll(".next");
const prevButtons = document.querySelectorAll(".prev");

tracks.forEach((track, index) => {
  const next = nextButtons[index];
  const prev = prevButtons[index];
  if (track && next && prev) {
    next.addEventListener("click", () => {
      track.scrollTo({
        left: track.scrollLeft + track.firstElementChild.offsetWidth,
        behavior: "smooth",
      });
    });
    prev.addEventListener("click", () => {
      track.scrollTo({
        left: track.scrollLeft - track.firstElementChild.offsetWidth,
        behavior: "smooth",
      });
    });
  }
});
// ---------------------------------------------------
// ---------------------------------------------------
// ---------------------------------------------------

function best(bestData) {
  const track = document.querySelector(".best .track");

  bestData.forEach((best) => {
    track.innerHTML += `
    <a class="cat" href="../htmlFiles/product.html?variantid=${best.id}">
    <img src="${best.img}" alt="categorImg" loading="lazy">
    <p>${best.title}</p>
    </a>
    `;
  });
}
// best();
async function getDate() {
  try {
    const req = await fetch("./jsFiles/HomeData.json");
    return await req.json();
  } catch (e) {
    throw e;
  }
}
getDate().then((data) => {
  console.log(data);
  best(data.bestSales);
  deals(data.dealsoftheday);
  colls(data.offers);
  console.log(data.bestSales);
});
// ---------------------------------------------------
// ---------------------------------------------------
// ---------------------------------------------------
function servs() {
  const servsData = [
    { img: "main/1.png", title: "safe delivery" },
    { img: "main/2.png", title: "quality service" },
    { img: "main/3.png", title: "service & technology" },
    { img: "main/4.png", title: "experienced team" },
    { img: "main/5.png", title: "best price guarantee" },
  ];

  const content = document.querySelector(".servs .content");

  servsData.forEach((serv) => {
    content.innerHTML += `
      <div class="serv">
            <p>${serv.title}</p>
            <p>Lorem Ipsum has been the industry's  standard. Lorem Ipsum has <br> been the industry's that <br> standard.</p>
        <img src="${serv.img}" alt="servImg" loading="lazy">
        <img src="main/blob.png" alt="servImg" loading="lazy">
    </div>
      `;
  });
}
servs();
// ---------------------------------------------------
// ---------------------------------------------------
// ---------------------------------------------------
function colls(collsData) {
  const content = document.querySelector(".colls .track");

  collsData.forEach((coll) => {
    content.innerHTML += `
      <div class="coll" style="align-self: ${coll.align} ;">
            <div class="prods">
                <div class="prod">
                    <img src="${coll.img1}" alt="collImg" loading="lazy">
                    <p>${coll.title1}</p>
                </div>
                <span>+</span>
                <div class="prod">
                    <img src="${coll.img2}" alt="collImg" loading="lazy">
                    <p>${coll.title2}</p>
                </div>
            </div>
            <p class="price">price only: <span>$${coll.price}</span></p>
            <p class="info">Excellent materials and there is a guarantee for them.</p>
            <a href="#">shop now!!</a>
        </div>
      `;
  });
}

// --------------------------------
// --------------------------------

function deals(dealsData) {
  const track = document.querySelector(".deals .track");
  window.onload = () => {
    track.scrollLeft = track.children[1].offsetLeft;
  };

  // إضافة الكروت للتراك
  dealsData.forEach((deal) => {
    track.innerHTML += `
      <div class="card">
        <div class="div1">
          <img src="${deal.img}" alt="dealsImg" class="${deal.class}" loading="lazy">
          <div class="rate">
            <p>${deal.title}</p>
            <div class="stars">
              <i class="fa-sharp fa-solid fa-star"></i>                    
              <i class="fa-sharp fa-solid fa-star"></i>                    
              <i class="fa-sharp fa-solid fa-star"></i>                    
              <i class="fa-sharp fa-solid fa-star"></i>                    
              <i class="fa-sharp fa-solid fa-star"></i>                    
            </div>
          </div>
        </div>
        <p class="info">Lorem Ipsum has been the industry's standard Lorem Ipsum has been the</p>
        <div class="div2">
          <span>$500</span>
          <span>$250</span>
        </div>
        <p class="para">hurry up offer ends in</p>
        <div class="timer">
          <div class="time">
          <span>days</span>
          <span class="days" ></span>
          </div>
          <span>:</span>
          <div class="time">
          <span>hrs</span>
          <span class="hours" ></span>
          </div>
          <span>:</span>
          <div class="time">
          <span>mins</span>
          <span class="mins"></span>
          </div>
          <span>:</span>
          <div class="time">
          <span>secs</span>
          <span class="secs"></span>
          </div>
        </div>
      </div>
    `;
  });
}

// let dealstrack = document.querySelectorAll('.deals .content .track');
// let items = document.querySelectorAll('.deals .content .track .card');
//     let next = document.getElementById('nextdeal');
//     let prev = document.getElementById('prevdeal');

//     let active = 1;
//     function loadShow(){
//         let stt = 0;
//         items[active].style.filter = 'none';
//         items[active].style.opacity = 1;

//         for(var i = active + 1; i < items.length; i++){
//             stt++;
//             items[i].style.filter = 'blur(1px)';
//         }
//         stt = 0;
//         for(var i = active - 1; i >= 0; i--){
//             stt++;
//             items[i].style.filter = 'blur(1px)';
//         }
//     }
//     loadShow();
//     next.onclick = function(){
//         active = active + 1 < items.length ? active + 1 : active;
//         loadShow();
//         dealstrack.scrollTo({
//           left: dealstrack.scrollLeft + dealstrack.firstElementChild.offsetWidth,
//           behavior: 'smooth'
//         });
//     }
//     prev.onclick = function(){
//         active = active - 1 >= 0 ? active - 1 : active;
//         loadShow();
//         dealstrack.scrollTo({
//           left: dealstrack.scrollLeft - dealstrack.firstElementChild.offsetWidth,
//           behavior: 'smooth'
//         });
//     }

// let dealsitems = document.querySelectorAll('.dealstrack .card');
//     let nextdeal = document.getElementById('nextdeal');
//     let prevdeal = document.getElementById('prevdeal');

//     let active = 3;
//     function loadShow(){
//         let stt = 0;
//         dealsitems[active].style.transform = `none`;
//         dealsitems[active].style.zIndex = 1;
//         dealsitems[active].style.filter = 'none';
//         dealsitems[active].style.opacity = 1;
//         for(var i = active + 1; i < dealsitems.length; i++){
//             stt++;
//             dealsitems[i].style.transform = ` scale(${1 - 0.2*stt}) perspective(16px) rotateY(-1deg)`;

//             // dealsitems[i].style.transform = `translateX(${120*stt}px) scale(${1 - 0.2*stt}) perspective(16px) rotateY(-1deg)`;
//             // dealsitems[i].style.zIndex = -stt;
//             // dealsitems[i].style.filter = 'blur(5px)';
//             // dealsitems[i].style.opacity = stt > 2 ? 0 : 0.6;
//         }
//         stt = 0;
//         for(var i = active - 1; i >= 0; i--){
//             stt++;
//             dealsitems[i].style.transform = `scale(${1 - 0.2*stt}) perspective(16px) rotateY(1deg)`;
//             // dealsitems[i].style.transform = `translateX(${-120*stt}px) scale(${1 - 0.2*stt}) perspective(16px) rotateY(1deg)`;
//             // dealsitems[i].style.zIndex = -stt;
//             // dealsitems[i].style.filter = 'blur(5px)';
//             // dealsitems[i].style.opacity = stt > 2 ? 0 : 0.6;
//         }
//     }
//     loadShow();
//     nextdeal.onclick = function(){
//         active = active + 1 < dealsitems.length ? active + 1 : active;
//         loadShow();
//     }
//     prevdeal.onclick = function(){
//         active = active - 1 >= 0 ? active - 1 : active;
//         loadShow();
//     }

// const dealstrack = document.querySelector('.deals .content .track');
// const dealscards = document.querySelectorAll('.deals .content .track .card');
// const prevdeal = document.querySelector('#prevdeal');
// const nextdeal = document.querySelector('#nextdeal');

// let currentIndex = 1; // Start with the third card as the middle card

// // Function to update the active card
// function updateCards() {
//   dealscards.forEach((card, index) => {
//     // Remove 'active2' and reset all cards to default state
//     card.classList.remove('active2', 'left', 'right');
//     if (index < currentIndex) {
//       card.classList.add('left'); // Cards to the left
//     } else if (index > currentIndex) {
//       card.classList.add('right'); // Cards to the right
//     }
//   });

//   // Add 'active2' only to the current card
//   dealscards[currentIndex].classList.add('active2');
// }

// // Event listeners for buttons
// prevdeal.addEventListener('click', () => {
//   currentIndex = (currentIndex > 0) ? currentIndex - 1 : dealscards.length - 1;
//   updateCards();
// });

// nextdeal.addEventListener('click', () => {
//   currentIndex = (currentIndex < dealscards.length - 1) ? currentIndex + 1 : 0;
//   updateCards();
// });

// // Initialize carousel
// updateCards();

// -------------------------

// const dealstrack = document.querySelector(".deals .content .track");
// const nextdeal = document.querySelector("#nextdeal");
// const prevdeal = document.querySelector("#prevdeal");

// if (dealstrack) {
//   const dealscards = dealstrack.querySelectorAll(".deals .content .track .card");
//   let activeIndex = 1; // العنصر النشط الافتراضي

//   // وظيفة لتحديث الكلاس "active2"
//   function updateActiveClass(newIndex) {
//     // إزالة الكلاس من العنصر الحالي
//     dealscards[activeIndex].classList.remove("active2");

//     // تحديث العنصر الجديد بعد فترة قصيرة لتطبيق التأثير
//     setTimeout(() => {
//       activeIndex = newIndex;
//       dealscards[activeIndex].classList.add("active2");

//       // الحصول على عرض العنصر
//       const itemWidth = dealscards[activeIndex].clientWidth;

//       // تحريك التراك بمقدار عرض العنصر
//       dealstrack.scrollTo({
//         left: dealscards[activeIndex].offsetLeft,
//         behavior: "smooth",
//       });
//     }, 500); // التأخير بعد إزالة الكلاس
//   }

//   // تعيين العنصر النشط عند تحميل الصفحة
//   function initializeActiveClass() {
//     dealscards.forEach((card, idx) => {
//       if (idx === activeIndex) {
//         card.classList.add("active2");
//       } else {
//         card.classList.remove("active2");
//       }
//     });
//   }

//   initializeActiveClass(); // تعيين الكلاس الأولي

//   // التعامل مع زر "التالي"
//   nextdeal.addEventListener("click", () => {
//     if (activeIndex + 1 < dealscards.length) {
//       updateActiveClass(activeIndex + 1);
//     }
//   });

//   // التعامل مع زر "السابق"
//   prevdeal.addEventListener("click", () => {
//     if (activeIndex - 1 >= 0) {
//       updateActiveClass(activeIndex - 1);
//     }
//   });
// }

// ---------------------

const dealstrack = document.querySelector(".deals .content .track");
const nextdeal = document.querySelector("#nextdeal");
const prevdeal = document.querySelector("#prevdeal");

if (dealstrack) {
  // إضافة active للعنصر الأول عند تحميل الصفحة
  const items = dealstrack.children;
  if (items.length > 0) {
    items[1].classList.add("active2"); // إضافة active للعنصر الذي يحمل index 1
  }

  nextdeal.addEventListener("click", () => {
    const activeItem = dealstrack.querySelector(".active2");
    const nextItem = activeItem.nextElementSibling || items[0]; // إذا كان العنصر الأخير، نرجع للأول

    // إزالة الـ active من العنصر الحالي
    activeItem.classList.remove("active2");

    // إضافة الـ active للعنصر التالي
    nextItem.classList.add("active2");

    dealstrack.scrollTo({
      left: dealstrack.scrollLeft + dealstrack.firstElementChild.offsetWidth,
    });
  });

  prevdeal.addEventListener("click", () => {
    const activeItem = dealstrack.querySelector(".active2");
    const prevItem =
      activeItem.previousElementSibling || items[items.length - 1]; // إذا كان العنصر الأول، نرجع لآخر عنصر

    // إزالة الـ active من العنصر الحالي
    activeItem.classList.remove("active2");

    // إضافة الـ active للعنصر السابق
    prevItem.classList.add("active2");

    dealstrack.scrollTo({
      left: dealstrack.scrollLeft - dealstrack.firstElementChild.offsetWidth,
    });
  });
}

// // -----------------TIMER----------------

let count = new Date("december 14, 2024 22:03:00").getTime(); // تاريخ الهدف الأولي
let imageIndex1 = 0; // مؤشر الصورة الحالي
let imageIndex2 = 0; // مؤشر الصورة الحالي
let imageIndex3 = 0; // مؤشر الصورة الحالي
let imageIndex4 = 0; // مؤشر الصورة الحالي
let imageIndex5 = 0; // مؤشر الصورة الحالي
let imageIndex6 = 0; // مؤشر الصورة الحالي

// مجموعة الصور لتحديثها
const images1 = [
  "products/product1/1.png",
  "products/product1/2.png",
  "products/product1/3.png",
  "products/product1/4.png",
];
const images2 = [
  "products/product2/1.png",
  "products/product2/2.png",
  "products/product2/3.png",
  "products/product2/4.png",
];
const images3 = [
  "products/product3/1.png",
  "products/product3/2.png",
  "products/product3/3.png",
  "products/product3/4.png",
];

const images4 = [
  "products/product4/1.png",
  "products/product4/2.png",
  "products/product4/3.png",
  "products/product4/4.png",
];
const images5 = [
  "products/product5/1.png",
  "products/product5/2.png",
  "products/product5/3.png",
  "products/product5/4.png",
];
const images6 = [
  "products/product6/1.png",
  "products/product6/2.png",
  "products/product6/3.png",
  "products/product6/4.png",
];

function updateCountdown() {
  let now = new Date().getTime();
  let dis = count - now;

  let days = Math.floor(dis / (1000 * 60 * 60 * 24));
  let hours = Math.floor((dis % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
  let mins = Math.floor((dis % (1000 * 60 * 60)) / (1000 * 60));
  let secs = Math.floor((dis % (1000 * 60)) / 1000);

  document.querySelectorAll(".days").forEach((element) => {
    element.innerHTML = days;
  });

  document.querySelectorAll(".hours").forEach((element) => {
    element.innerHTML = hours;
  });

  document.querySelectorAll(".mins").forEach((element) => {
    element.innerHTML = mins;
  });

  document.querySelectorAll(".secs").forEach((element) => {
    element.innerHTML = secs;
  });

  // إذا كان العد التنازلي قد انتهى
  if (dis < 0) {
    // تعيين كل القيم إلى "00"
    document.querySelectorAll(".days").forEach((element) => {
      element.innerHTML = "00";
    });
    document.querySelectorAll(".hours").forEach((element) => {
      element.innerHTML = "00";
    });
    document.querySelectorAll(".mins").forEach((element) => {
      element.innerHTML = "00";
    });
    document.querySelectorAll(".secs").forEach((element) => {
      element.innerHTML = "00";
    });

    // تحديث الصورة
    updateImage();

    // إعادة تعيين العد التنازلي لشهر آخر
    count = addMonthToDate(count);
  }
}

// دالة لإضافة شهر واحد إلى التاريخ
function addMonthToDate(date) {
  let currentDate = new Date(date);
  let newDate = new Date(currentDate.setMonth(currentDate.getMonth() + 1));
  return newDate.getTime();
}

// دالة لتحديث الصورة
function updateImage() {
  imageIndex1 = (imageIndex1 + 1) % images1.length; // تحديث مؤشر الصورة
  document.querySelector(".dimg1").src = images1[imageIndex1];
  imageIndex2 = (imageIndex2 + 1) % images2.length; // تحديث مؤشر الصورة
  document.querySelector(".dimg2").src = images2[imageIndex2];
  imageIndex3 = (imageIndex3 + 1) % images3.length; // تحديث مؤشر الصورة
  document.querySelector(".dimg3").src = images3[imageIndex3];
  imageIndex4 = (imageIndex4 + 1) % images4.length; // تحديث مؤشر الصورة
  document.querySelector(".dimg4").src = images4[imageIndex4];
  imageIndex5 = (imageIndex5 + 1) % images5.length; // تحديث مؤشر الصورة
  document.querySelector(".dimg5").src = images5[imageIndex5];
  imageIndex6 = (imageIndex6 + 1) % images6.length; // تحديث مؤشر الصورة
  document.querySelector(".dimg6").src = images6[imageIndex6];
}

// تعيين فاصل لتحديث العد التنازلي كل ثانية

// تهيئة العد التنازلي عند تحميل الصفحة

// const items = document.querySelectorAll(".deals .track .card");
// let currentIndex = 0;

// function updateActiveItem(newIndex) {
//   // إزالة الفئة 'active' من العنصر الحالي
//   items[currentIndex].classList.remove("active");

//   // تحديث الفهرس إلى العنصر الجديد
//   currentIndex = newIndex;

//   // إضافة الفئة 'active' للعنصر الجديد
//   items[currentIndex].classList.add("active");
// }

// document.getElementById("nextdeal").addEventListener("click", () => {
//   // التأكد من عدم تجاوز آخر عنصر
//   if (currentIndex < items.length - 1) {
//     updateActiveItem(currentIndex + 1);
//   }
// });

// document.getElementById("prevdeal").addEventListener("click", () => {
//   // التأكد من عدم تجاوز أول عنصر
//   if (currentIndex > 0) {
//     updateActiveItem(currentIndex - 1);
//   }
// });

// const cards = document.querySelectorAll('.card');
// let currentIndex = 1; // الكارت الأوسط يكون في البداية عند 1

// // تحديث مظهر الكروت وتطبيق التكبير للكارت الأوسط
// function updateCards() {
//     cards.forEach((card, index) => {
//         card.classList.toggle('active', index === currentIndex);
//     });
// }

// // عند الضغط على السهم التالي
// document.getElementById('nextdeal').addEventListener('click', () => {
//     currentIndex = (currentIndex + 1) % cards.length;

//     updateCards();
// });

// // عند الضغط على السهم السابق
// document.getElementById('prevdeal').addEventListener('click', () => {
//     currentIndex = (currentIndex - 1 + cards.length) % cards.length;
//     updateCards();
// });

// // تفعيل الكارت الأوسط في البداية
// updateCards();
