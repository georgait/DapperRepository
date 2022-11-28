namespace CarDealers.API.DTOs;

public record CreateCarDto
{
    public string Make { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string Year { get; init; } = null!;
    public string VIN { get; init; } = null!;
}
