using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace rsH60Customer.Models;

public partial class Customer
{
    public int CustomerId { get; set; }
    
    [Required(ErrorMessage = "First name is required.")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ' -]+$", ErrorMessage = "First name can only contain letters, apostrophes, dashes, and spaces.")]
    public string FirstName { get; set; } = null!;
    
    [Required(ErrorMessage = "Last Name is required.")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ' -]+$", ErrorMessage = "Last Name can only contain letters, apostrophes, dashes, and spaces.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Province is required.")]
    [RegularExpression(@"^(ON|QC|NB|MN)$", ErrorMessage = "Province must be ON, QC, NB, or MN.")]
    public string Province { get; set; } = null!;

    [Required(ErrorMessage = "Credit card number is required.")]
    [RegularExpression(@"^\d{16}$", ErrorMessage = "Credit card number must be 16 digits.")]
    public string CreditCard { get; set; } = null!;
    
    public string UserId { get; set; }
    
    public IdentityUser User { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
