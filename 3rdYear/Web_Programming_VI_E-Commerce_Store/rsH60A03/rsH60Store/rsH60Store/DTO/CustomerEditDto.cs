using System.ComponentModel.DataAnnotations;

namespace rsH60Store.DTO;

public class CustomerEditDto
{
    public int CustomerId { get; set; }
    // Identity-related fields
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ' -]+$", ErrorMessage = "First name can only contain letters, apostrophes, dashes, and spaces.")]
    public string FirstName { get; set; } = null!;
    
    [Required(ErrorMessage = "Last Name is required.")]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ' -]+$", ErrorMessage = "Last Name can only contain letters, apostrophes, dashes, and spaces.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "Province is required.")]
    [RegularExpression(@"^(ON|QC|NB|MN)$", ErrorMessage = "Province must be ON, QC, NB, or MN.")]
    public string Province { get; set; } = null!;

    [Required(ErrorMessage = "Credit card number is required.")]
    [RegularExpression(@"^\d{16}$", ErrorMessage = "Credit card number must be 16 digits.")]
    public string CreditCard { get; set; } = null!;
}