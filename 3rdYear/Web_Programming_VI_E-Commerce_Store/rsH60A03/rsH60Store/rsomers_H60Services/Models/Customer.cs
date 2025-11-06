using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace rsomers_H60Services.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string CreditCard { get; set; } = null!;
    
    public string UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
