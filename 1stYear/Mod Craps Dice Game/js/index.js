const canvas = document.getElementById("canvas");
const context = canvas.getContext("2d");
const messageContainer = document.getElementById("message-container");
const messageElement = document.getElementById("message");

const diceImage = new Image();
diceImage.src = "./images/Dice1.png"; // Replace with your dice image path
const diceSize = Math.min(window.innerWidth, window.innerHeight) * 0.8; // Adjust the dice size based on the window dimensions
const animationDuration = 5000; // 5 seconds
const pauseDuration = animationDuration / 2; // Pause at halfway

let currentSize = diceSize;
let startTime = null;
let animationId = null;
let animationCompleted = false;

function animate(currentTime) {
  if (!startTime) {
    startTime = currentTime;
  }
  const elapsedTime = currentTime - startTime;
  const progress = Math.min(elapsedTime / animationDuration, 1); // Calculate animation progress between 0 and 1

  // Clear the canvas
  context.clearRect(0, 0, canvas.width, canvas.height);

  if (progress < 0.5) {
    // Draw the dice with the current size
    const size = diceSize - diceSize * progress * 2; // Double the progress for the first half
    const x = (canvas.width - size) / 2;
    const y = (canvas.height - size) / 2;
    context.drawImage(diceImage, x, y, size, size);
  } else if (!animationCompleted) {
    // Pause the dice animation at halfway
    animationCompleted = true;
    context.clearRect(0, 0, canvas.width, canvas.height);
    messageContainer.style.display = "flex";
    messageElement.style.display = "block";

    // Set timeout to redirect to a different page after the message is displayed
    setTimeout(() => {
      window.location.href = "intro.html";
    }, 2000);
  }

  if (progress < 1) {
    // Animation is not finished, request the next frame
    animationId = requestAnimationFrame(animate);
  }
}

function startAnimation() {
  // Adjust the canvas size based on the window dimensions
  canvas.width = window.innerWidth;
  canvas.height = window.innerHeight;

  // Start the animation
  animationId = requestAnimationFrame(animate);

  // Function to cancel the animation
  function cancelAnimation() {
    cancelAnimationFrame(animationId);
  }

  // Set timeout to cancel the animation after the specified duration
  setTimeout(cancelAnimation, animationDuration);
}

function skipToNextPage() {
  window.location.href = "intro.html"; // Replace "intro.html" with the path to the desired HTML page
}

window.addEventListener("load", function () {
  startAnimation();

  const skipButton = document.getElementById("skipButton");
  skipButton.addEventListener("click", skipToNextPage);
});
