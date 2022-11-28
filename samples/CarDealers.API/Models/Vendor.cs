using Repository.Domain.Attributes;
using Repository.Domain.Entities;

namespace CarDealers.API.Models;

public class Vendor : BusinessEntity<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    [ManyRelationship]
    public List<Car> Cars { get; set; } = new ();
}
