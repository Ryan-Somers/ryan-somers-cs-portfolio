class User {
  firstName;
  lastName;
  username;
  phoneNumber;
  city;
  emailAddress;
  constructor(first, last, user, phone, city, email) {
    this.firstName = first;
    this.lastName = last;
    this.username = user;
    this.phoneNumber = phone;
    this.city = city;
    this.emailAddress = email;
  }

  viewWins() {
    return this.wins;
  }

  viewLoss() {
    return this.losses;
  }

  fullName(first, last) {
    this.firstName = first;
    this.lastName = last;
    return `${first} ${last}`;
  }

  toString() {
    if (this.firstName && this.lastName && this.username && this.phoneNumber && this.city && this.emailAddress) {
      return `Name: ${this.firstName} ${this.lastName} Username: ${this.username} Phone: ${this.phoneNumber} City: ${this.city} Email: ${this.emailAddress}`;
    }
  }
}

class Dice {
  dice1;
  dice2;
  constructor(dice1, dice2) {
    this.dice1 = dice1;
    this.dice2 = dice2;
  }
  rollDice() {
    this.dice1 = Math.floor(Math.random() * 6) + 1;
    this.dice2 = Math.floor(Math.random() * 6) + 1;
    return `Dice1: ${this.dice1} & Dice2: ${this.dice2}`;
  }
  addRoll() {
    let result = this.dice1 + this.dice2;
    return result;
  }
  toString() {
    if (this.dice1 && this.dice2) {
      return `You rolled a ${this.dice1} & a ${this.dice2}!`;
    }
  }
}

class Game {
  betAmount;
  point;
  wins;
  losses;
  bankBalance;
  constructor(betAmount, point, wins, losses, bankBalance) {
    this.betAmount = betAmount;
    this.point = point;
    this.wins = wins;
    this.losses = losses;
    this.bankBalance = bankBalance;
  }

  increaseBankBalance(increase) {
    this.bankBalance += increase;
    return this.bankBalance;
  }

  decreaseBankBalance(decrease) {
    this.bankBalance -= decrease;
    return this.bankBalance;
  }

  playRound(betAmount, point) {
    if (point === 2 || point === 7 || point === 11 || point === 12) {
      return `Invalid point! Do not pick points 2,7,11 or 12.`;
    }
    if (point < 2 || point > 12) {
      return `Invalid point! Do not pick points under 2 or higher than 12.`;
    }
    if (betAmount > this.bankBalance) {
      return `You don't have enough money! Try betting lower a lower amount!`;
    }
    if (betAmount <= 0) {
      return `You don't have any Money! Where did it all go?`;
    }
    if (point === guessField) {
      this.increaseBankBalance(betAmount);
      return `You Won! Your roll was ${point}!`;
    } else if (point != guessField) {
      this.decreaseBankBalance(betAmount);
      return `You Lost! Your roll was ${guessField}, it should have been ${point}!`;
    }
  }

  playGame(betAmount, point) {
    this.bankBalance = betInput;

    if (point === 2 || point === 7 || point === 11 || point === 12) {
      return `Invalid point! Do not pick points 2,7,11 or 12.`;
    }
    if (point < 2 || point > 12) {
      return `Invalid point! Do not pick points under 2 or higher than 12.`;
    }
    if (betAmount > this.bankBalance) {
      return `You don't have enough money! Try betting lower a lower amount!`;
    }
    if (betAmount <= 0) {
      return `You don't have any Money! Where did it all go?`;
    }
    while (this.bankBalance > 0) {
      if (point === roll) {
        this.increaseBankBalance(betAmount);
        return `You Won! Your roll was ${point} and your balance is ${this.bankBalance}`;
      } else if (point != guessField) {
        this.decreaseBankBalance(betAmount);
        return `You Lost! Your roll was ${guessField}, it should have been ${point}. Bank Balance is ${this.bankBalance}`;
      }
    }
  }

  addWin() {
    this.wins++;
    return `${this.wins}`;
  }

  addLoss() {
    this.losses++;
    return `${this.losses}`;
  }

  toString() {
    {
      if (this.betAmount && this.point && this.wins && this.losses && this.bankBalance)
        return `Bet amount: ${this.betAmount}, point: ${this.point}, wins: ${this.wins}, loss: ${this.losses} Bank Balance: ${this.bankBalance}`;
    }
  }
}

// getting local storage
let fname = localStorage.getItem("firstName");
let lname = localStorage.getItem("lastName");
let user = localStorage.getItem("username");
let phoneNum = localStorage.getItem("phoneNumber");
let city = localStorage.getItem("city");
let email = localStorage.getItem("email");
let bank = localStorage.getItem("bank");

let betInput = document.querySelector("#betAmt");
let rollDiceBtn = document.querySelector("#btn");
let guessField = document.querySelector("#guessInput");
let result = document.querySelector("#gameResult");
let quitGameBtn = document.querySelector("#btnQuit");

let dice = new Dice();
let game = new Game();
// Get the dice element
const diceElement = document.getElementById("dice");
const diceElementTwo = document.getElementById("diceTwo");

diceElement.style.backgroundImage = `url(./images/Dice1.png)`;
diceElementTwo.style.backgroundImage = `url(./images/Dice1.png)`;
// when the user clicks on roll dice
rollDiceBtn.addEventListener("click", function () {
  let betAmount = Number(betInput.value);
  let bankInput = Number(bank);
  let guess = Number(guessField.value);
  this.wins = 0;
  this.losses = 0;

  // guess field checking
  if (guess < 2 || guess > 12 || guess == 2 || guess == 7 || guess == 11 || guess == 12) {
    alert("Guess has to be between 2-12 but not 2,7,11 or 12");
    return;
  }
  if (isNaN(betAmount) || betAmount < 1 || betAmount % 1 !== 0) {
    alert("Bet amount must be an integer number greater than 0.");
    return;
  }
  if (bankInput <= 0) {
    window.location.href = "./gameOver.html";
    return;
  }
  if (betAmount > bankInput) {
    alert("Bet amount can't be higher than your current bank balance!");
    return;
  } else {
    dice.rollDice();
    let point = dice.addRoll();
    console.log(point);
    dice.addRoll();
    diceResult.textContent = dice.toString();
    if (guessField.value == point && guessField != 2 && guess != 7 && guess != 11 && guess != 12) {
      bank += betAmount;
      localStorage.setItem("bank", bank);
      this.wins++;
      document.querySelector("#betAmountMsg").textContent = `Bank Balance: $${localStorage.getItem("bank")}`;
      result.textContent = `You Won! Your guess was a ${point}! Wins: ${this.wins} Losses: ${this.losses}`;
    } else if (point == 3 || point == 4 || point == 5 || point == 6 || point == 8 || point == 9 || point == 10) {
      result.textContent = `Your roll was ${point}, please re-roll!`;
    } else if (guess != point || point == 2 || point == 7 || point == 11 || point == 12) {
      bank -= betAmount;
      localStorage.setItem("bank", bank);
      this.losses++;
      document.querySelector("#betAmountMsg").textContent = `Bank Balance: $${localStorage.getItem("bank")}`;
      result.textContent = `You Lost. Your guess was a ${guess}, but you got ${point}. You have ${this.wins} wins & ${this.losses} losses`;
    }
  }

  // Array of dice images
  const diceImages = ["./images/Dice1.png", "./images/Dice2.png", "./images/Dice3.png", "./images/Dice4.png", "./images/Dice5.png", "./images/Dice6.png"];

  // Number of times to roll the dice
  const totalRolls = 20; // Adjust as needed
  let currentRoll = 0;

  // Function to roll the dice
  function rollDice() {
    if (currentRoll >= totalRolls) {
      // Animation complete, stop rolling
      return;
    }

    // Randomly select a dice image
    const randomIndex = Math.floor(Math.random() * diceImages.length);
    const randomDiceImage = diceImages[randomIndex];

    // Set the background image of the dice element
    diceElement.style.backgroundImage = `url(${randomDiceImage})`;
    diceElementTwo.style.backgroundImage = `url(${randomDiceImage})`;

    // Increment the current roll count
    currentRoll++;

    if (currentRoll === totalRolls) {
      // Animation complete, get the final dice value
      const finalDiceValue = dice.dice1;
      const finalSecondDiceValue = dice.dice2;
      console.log(finalSecondDiceValue);

      // Set the image to match the final dice value
      const finalDiceImage = diceImages[finalDiceValue - 1];
      const finalSecondDiceImage = diceImages[finalSecondDiceValue - 1];
      diceElement.style.backgroundImage = `url(${finalDiceImage})`;
      diceElementTwo.style.backgroundImage = `url(${finalSecondDiceImage})`;
    }
  }
  // Call rollDice function when the page loads
  rollDice();
  // Interval time in milliseconds
  const intervalTime = 100; // Adjust as needed
  // Start the animation
  setInterval(rollDice, intervalTime);
});

quitGameBtn.addEventListener("click", function () {
  alert(`Thanks for playing! The total amount in your bank after leaving is $${localStorage.getItem("bank")}!`);
  localStorage.clear();
  window.location.href = "intro.html";

  return;
});
// to enter the information from local storage.
document.querySelector("#welcome").textContent = `Welcome ${fname} ${lname} to Mod Craps!`;
document.querySelector("#usernameMsg").textContent = `Username: ${user}`;
document.querySelector("#phoneNumMsg").textContent = `Phone Number: ${phoneNum}`;
document.querySelector("#cityMsg").textContent = `City: ${city}`;
document.querySelector("#emailMsg").textContent = `Email: ${email}`;
document.querySelector("#betAmountMsg").textContent = `Bank Balance: $${localStorage.getItem("bank")}`;
document.querySelector("#date").textContent = `Your Last Visit was on ${localStorage.getItem("date")} at ${localStorage.getItem("time")}`;
document.querySelector("#change").textContent = `Not ${fname} ${lname}? `;
let changeBtn = document.querySelector("#changeBtn");
changeBtn.addEventListener("click", function (e) {
  localStorage.clear();
  window.location.href = `intro.html`;
});

if (
  (localStorage.getItem("firstname") && localStorage.getItem("lastName") && localStorage.getItem("username") && localStorage.getItem("phoneNumber"),
  localStorage.getItem("city") && localStorage.getItem("email") && localStorage.getItem("bank") && localStorage.getItem("date"))
) {
  document.querySelector("#welcome").textContent = `Welcome Back ${fname} ${lname} to Mod Craps!`;
} else {
}
