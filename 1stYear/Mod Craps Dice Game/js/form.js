let fname = document.querySelector("#fname");
let lname = document.querySelector("#lname");
let username = document.querySelector("#username");
let phoneNum = document.querySelector("#num");
let city = document.querySelector("#city");
let email = document.querySelector("#email");
let betAmt = document.querySelector("#amtMoney");
let form = document.querySelector("#form");

$(document).ready(function () {
  if (
    (localStorage.getItem("firstname") && localStorage.getItem("lastName") && localStorage.getItem("username") && localStorage.getItem("phoneNumber"),
    localStorage.getItem("city") && localStorage.getItem("email") && localStorage.getItem("bank"))
  ) {
    window.location.href = "game.html";
  }
  $("#form").validate({
    rules: {
      firstname: {
        required: true,
        pattern: /^[a-zA-Z][a-zA-Z~' -]{0,33}[a-zA-Z~'~-]$/,
      },
      lastname: {
        required: true,
        pattern: /^[a-zA-Z][a-zA-Z~' -]{0,43}[a-zA-Z~'~-]$/,
      },
      username: {
        required: true,
        pattern: /^[0-9][A-Z]{3}[a-z][\$\&\*\@\!]+$/,
      },
      num: {
        required: true,
        pattern: /^\d{3}\.\d{3}\.\d{4}$/,
      },
      city: {
        required: true,
        pattern: /^[a-zA-Z]{1,50}$/,
      },
      email: {
        required: true,
        pattern: /^[a-zA-Z0-9.-]+@[a-zA-Z0-9.-]+\.(com|ca)$/,
      },
      betAmt: {
        required: true,
        pattern: /^([5-9]|[1-4]\d{1,2}|5000)([02468])$/,
      },
    },
    messages: {
      firstname: "Please follow proper format! (No numbers, only letters.)",
      lastname: "Please follow proper format! (No numbers, only letters.)",
      username: "Please follow proper format! (Ex. 1RYAn!)",
      num: "Please follow proper format! (Ex. ###.###.####)",
      city: "Please follow proper format! (No numbers, only letters. 50 characters max length)",
      email: "Please follow proper format! (Ex. dave@gmail.com or dave@gmail.ca)",
      betAmt: "Bank Amount must be between $5-5000 & divisble by 2",
    },
    errorPlacement: function (error, element) {
      // Find or create the error container for each field
      var errorContainerId = element.attr("id") + "Error";
      var errorContainer = $("#" + errorContainerId);
      if (!errorContainer.length) {
        errorContainer = $("<div id='" + errorContainerId + "'></div>").insertAfter(element);
      }
      // Append the error message to the error container
      error.appendTo(errorContainer);
    },
    submitHandler: function (form) {
      localStorage.setItem("firstName", fname.value);
      localStorage.setItem("lastName", lname.value);
      localStorage.setItem("username", username.value);
      localStorage.setItem("phoneNumber", phoneNum.value);
      localStorage.setItem("city", city.value);
      localStorage.setItem("email", email.value);
      localStorage.setItem("bank", betAmt.value);
      let currentDate = new Date().toLocaleDateString();
      let currentTime = new Date().toLocaleTimeString();
      localStorage.setItem("date", currentDate);
      localStorage.setItem("time", currentTime);
      form.submit();
    },
  });
});
