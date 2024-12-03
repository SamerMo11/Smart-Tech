const buttons = document.querySelectorAll('.sticky');

buttons.forEach((button) => {
  button.addEventListener('click', () => {
    const card = button.closest('.order'); 
    const infos = card.querySelectorAll('.info');  
    
    infos.forEach((info) => { 
      if (info.classList.contains('active')) {
        info.classList.remove('active');
        info.classList.add('hide');
      } else {
        info.classList.remove('hide');
        info.classList.add('active');
      }
    });

    button.classList.toggle('active'); 
});
})
// ------------------------------------
// ------------------------------------
// ------------------------------------

const items = document.querySelectorAll('.item');
const mainCard = document.querySelectorAll('.mainCard');

mainCard.forEach(card => {
    card.classList.add('hide2');
  });
  
  const savedCardIndex = localStorage.getItem('selectedCard');
  
  if (savedCardIndex) {
    const selectedCard = document.getElementById(savedCardIndex);
    selectedCard.classList.add('active2');
    selectedCard.classList.remove('hide2');
    mainCard.forEach(card => {
      if (card !== selectedCard) {
        card.classList.add('hide2');
      }
    });
  }
  
  items.forEach((item, index) => {
    item.addEventListener('click', () => {
      const relatedCard = mainCard[index];
  
      relatedCard.classList.add('active2');
      relatedCard.classList.remove('hide2');
  
      mainCard.forEach((card, cardIndex) => {
        if (cardIndex !== index) {
          card.classList.add('hide2');
        }
      });
  
      localStorage.setItem('selectedCard', relatedCard.id);
    });
  });