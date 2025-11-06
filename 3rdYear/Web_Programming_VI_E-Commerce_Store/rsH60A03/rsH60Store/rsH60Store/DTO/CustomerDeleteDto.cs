namespace rsH60Store.DTO;

public class CustomerDeleteDto
{
    public int CustomerId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    
    public string LastName { get; set; } = null!;
}