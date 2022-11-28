namespace CarDealers.API.DTOs;

public record CreateVendorDto
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Phone { get; init; } = null!;
    public List<CreateCarDto> Cars { get; init; } = new();
}
