// Navigation menu toggle
const menuToggle = document.querySelector('.menu-toggle');
const nav = document.querySelector('nav');

menuToggle.addEventListener('click', () => {
  nav.classList.toggle('show');
});

// Contact form validation
const form = document.querySelector('form');
const nameInput = document.querySelector('#name');
const emailInput = document.querySelector('#email');
const messageInput = document.querySelector('#message');
const submitBtn = document.querySelector('#submit');

submitBtn.addEventListener('click', (event) => {
  event.preventDefault();

  if (nameInput.value === '') {
    alert('Please enter your name');
    return;
  }

  if (emailInput.value === '') {
    alert('Please enter your email address');
    return;
  }

  if (messageInput.value === '') {
    alert('Please enter a message');
    return;
  }

  alert('Your message has been sent!');
  form.reset();
});
