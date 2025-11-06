using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using rsH60Store.DTO;
using rsH60Store.Models;
using rsH60Store.Models.Interfaces;

namespace rsH60Store.Controllers
{
    [Authorize(Roles = "Manager,Clerk")]
    public class CustomerController : Controller
    {
        private readonly ICustomersRepository _customerRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerController(ICustomersRepository customerRepository, UserManager<IdentityUser> userManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
        }

        // GET: CustomerController
        public async Task<IActionResult> Index()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();

            return View(customers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // GET: CustomerController/Create
        // This is where the new IdentityUser gets created and linked to the Customer
        [HttpPost]
        public async Task<IActionResult> Create(CustomerRegistrationDto model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation messages
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            if (model.Password.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, "Password is required.");
            }

            var result = await _userManager.CreateAsync(user, model.Password);


            if (result.Succeeded)
            {
                var customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Province = model.Province,
                    CreditCard = model.CreditCard,
                    UserId = user.Id
                };

                await _customerRepository.AddCustomerAsync(customer);

                await _userManager.AddToRoleAsync(user, "Customer");

                return RedirectToAction("Index", "Customer");
            }

            // If creation failed, log errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        // GET: CustomerController/Edit/5
        // GET: CustomerController/Edit/5
        public async Task<IActionResult> Update(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            // Create a model to pass to the view
            var model = new CustomerEditDto() // Create a DTO for the view model
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Province = customer.Province,
                CreditCard = customer.CreditCard,
            };

            return View(model);
        }


        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CustomerEditDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Fetch the existing customer and user
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                // Update the customer fields
                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Email = model.Email;
                customer.PhoneNumber = model.PhoneNumber;
                customer.Province = model.Province;
                customer.CreditCard = model.CreditCard;

                // Save changes to the customer repository
                await _customerRepository.UpdateCustomerAsync(customer);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }


        // GET: CustomerController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            // Create a model for deletion confirmation
            var model = new CustomerDeleteDto() // Create a DTO for confirmation
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
            };

            return View(model);
        }


        // POST: CustomerController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Fetch the customer and the associated user
                var customer = await _customerRepository.GetCustomerByIdAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }

                // Delete the customer from your store database
                await _customerRepository.DeleteCustomerAsync(customer.CustomerId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Customer cannot be deleted because they have a shopping cart or order.";
                var customer = await _customerRepository.GetCustomerByIdAsync(id);

                if (customer == null)
                {
                    return NotFound();
                }

                ModelState.AddModelError(string.Empty, ex.Message);
                var model = new CustomerDeleteDto()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                };
                return View(model);
            }
        }
    }
}