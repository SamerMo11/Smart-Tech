let recognition;
    let isRecording = false;

    // التحقق من دعم المتصفح لميزة التعرف على الصوت وإنشاء كائن recognition
    if ('webkitSpeechRecognition' in window) {
      recognition = new webkitSpeechRecognition();
      recognition.lang = 'en-US'; // تعيين اللغة إلى الإنجليزية
      recognition.interimResults = false;
      recognition.maxAlternatives = 1;

      // التعامل مع النتيجة النهائية للنص
      recognition.onresult = function(event) {
        const transcript = event.results[0][0].transcript;
        document.getElementById("textInput").value = transcript;
        console.log("Recognized text:", transcript);
      };

      // معالجة الخطأ أثناء التسجيل
      recognition.onerror = function(event) {
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
options.forEach(option => {
    option.addEventListener("click", () => {
        options.forEach(opt => opt.classList.remove("active2"));
        option.classList.add("active2");
    });
});



// ----------------------------------------------------------------------
// ----------------------------------------------------------------------





document.querySelectorAll('.item .info .div').forEach(card => {
  const increaseBtn = card.querySelector('.increace');
  const decreaseBtn = card.querySelector('.decreace');
  const quantitySpan = card.querySelector('.quantity');
  const totalSpan = card.querySelector('.total');

  let quantity = parseInt(quantitySpan.textContent);
  let unitPrice = parseInt(totalSpan.textContent.replace(/,/g, ''));

  increaseBtn.addEventListener('click', () => {
    quantity++;
    updatePrice();
  });

  decreaseBtn.addEventListener('click', () => {
    if (quantity > 1) {
      quantity--;
      updatePrice();
    }
  });

  function updatePrice() {
    quantitySpan.textContent = quantity;
    totalSpan.textContent = (quantity * unitPrice).toLocaleString();
  }
});
