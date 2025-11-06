using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace rsH60Customer.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    [JsonIgnore]
    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
