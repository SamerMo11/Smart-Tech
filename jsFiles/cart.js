document.querySelectorAll('.content .card').forEach(card => {
    const increaseBtn = card.querySelector('.increace');
    const decreaseBtn = card.querySelector('.decreace');
    const quantitySpan = card.querySelector('.num');
    const priceSpan = card.querySelector('.price'); // السعر الفردي
    const totalSpan = card.querySelector('.total'); // الإجمالي الفرعي
  
    let quantity = parseInt(quantitySpan.textContent);
    let unitPrice = parseInt(priceSpan.textContent.replace(/,/g, ''));
  
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
      priceSpan.textContent = (quantity * unitPrice).toLocaleString();
      totalSpan.textContent = (quantity * unitPrice).toLocaleString();
    }
  });
// ------------------------------


document.querySelectorAll('.content .card').forEach(card => {
    const closeBtn = card.querySelector('.remove');
  
    closeBtn.addEventListener('click', () => {
      card.classList.add('hide'); 
      setTimeout(() => {
        card.remove(); 
      }, 1000); 
    });
  });
  