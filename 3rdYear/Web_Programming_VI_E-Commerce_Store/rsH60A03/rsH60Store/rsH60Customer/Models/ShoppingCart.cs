using System;
using System.Collections.Generic;

namespace rsH60Customer.Models;

public partial class ShoppingCart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }
    
    public string UserId { get; set; } = null!; // Link to Identity User

    public DateTime DateCreated { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Customer Customer { get; set; } = null!;
}
