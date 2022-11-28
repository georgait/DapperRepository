using Repository.Domain.Entities;

namespace CarDealers.API.Models;

public class Car : BusinessEntity<int>
{
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Year { get; set; } = null!;
    public string VIN { get; set; } = null!;
    public int VendorId { get; set; }
}
