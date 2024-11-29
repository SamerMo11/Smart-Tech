const logBtn = document.querySelector('.logBtn');
const form = document.querySelector('.form');
const welcoming = document.querySelector('.welcoming');

let state = false;

logBtn.addEventListener('click', function () {
  if (!state) {
    form.classList.remove('active2'); 
    form.classList.add('active');
    welcoming.textContent = 'Dear!!';
    logBtn.textContent = 'Sign Up';
  } else {
    form.classList.remove('active'); 
    form.classList.add('active2');
    welcoming.textContent = 'Welcome!!';
    logBtn.textContent = 'Log In';
  }
  state = !state; 
});
// ----------------
// ----------------
const passwordFields = document.querySelectorAll('.password');

    passwordFields.forEach(passwordInput => {
      const parent = passwordInput.parentElement;
      const seeIcon = parent.querySelector('.see');
      const noseeIcon = parent.querySelector('.nosee');

      seeIcon.addEventListener('click', function() {
        passwordInput.type = 'text';
        seeIcon.style.display = 'none';
        noseeIcon.style.display = 'inline';
      });

      noseeIcon.addEventListener('click', function() {
        passwordInput.type = 'password';
        noseeIcon.style.display = 'none';
        seeIcon.style.display = 'inline';
      });
    });
