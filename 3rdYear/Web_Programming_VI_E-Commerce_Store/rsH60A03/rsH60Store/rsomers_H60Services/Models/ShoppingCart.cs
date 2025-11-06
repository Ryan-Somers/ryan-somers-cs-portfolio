using System;
using System.Collections.Generic;

namespace rsomers_H60Services.Models;

public partial class ShoppingCart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Customer Customer { get; set; } = null!;
}
