# Dapper Repository

An implementation of repository pattern & unit of work using Dapper.

## About The Project

The idea of the project is a generic repository with two basic methods (+ overload), read and write. Also, to minimize database calls, the unit of work pattern is applied by default. Using this pattern we will track changes made to entities.

Beyond the repository pattern, I've tried to remove as much boilerplate code as possible. For example, as is known to implement a multi mapping scenario for a one-to-many relationship, there is a standard way to achieve this, so to avoid repeating the same code every single time we need to read data for a one-to-many scenario, I used some reflection to be able to reuse the same code over and over again via the base generic repository.

## Installation

For now just download the source code or fork the repo.

## Usage

### Setup entities

```C#
// BusinessEntity<TId> is defined to Repository.Domain.Entities

public class Car : BusinessEntity<int>
{
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Year { get; set; } = null!;
    public string VIN { get; set; } = null!;
    public int VendorId { get; set; }
}

public class Vendor : BusinessEntity<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;

    [ManyRelationship]
    public List<Car> Cars { get; set; } = new ();
}
```

### Setup a repository

```C#
public interface IVendorRepository
{
    Task<Vendor> CreateCarAsync(CreateVendorDto vendor);
    Task<IReadOnlyList<Vendor>> GetAllVendorsAsync();
    Task<IReadOnlyList<Vendor>> GetVendorWithCarsAsync(int id);
}
public sealed class VendorRepository : BaseRepository<Vendor>, IVendorRepository
{
    private readonly IDbContext _context;

    public VendorRepository(IDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Vendor>> GetAllVendorsAsync()
    {
        var sql = """
                 SELECT *
                 FROM Vendor v
                 INNER JOIN Car c on c.VendorId = v.Id
                 """;

        return await ReadAsync<Car, dynamic>(sql, new { });
    }

    public async Task<IReadOnlyList<Vendor>> GetVendorWithCarsAsync(int id)
    {
        var sql = """
                SELECT *
                FROM Vendor v
                INNER JOIN Car c on c.VendorId = v.Id
                WHERE v.Id = @id
                """;

        return await ReadAsync<Car, dynamic>(sql, new { id });
    }

    public async Task<Vendor> CreateCarAsync(CreateVendorDto vendor)
    {
        var sql1 = """
                INSERT INTO Vendor (FirstName, LastName, Email, Phone)
                VALUES (@FirstName, @LastName, @Email, @Phone);
                SELECT last_insert_rowid();
                """;

        var sql2 = """
                INSERT INTO Car (Make, Model, Year, VIN, VendorId)
                VALUES (@Make, @Model, @Year, @VIN, @VendorId);
                SELECT last_insert_rowid();
                """;

        Vendor result = new();
        await _context.UnitOfWork.TrackChangesAsync(async () =>
        {
            var vendorId = await WriteAsync(sql1, new
            {
                vendor.FirstName,
                vendor.LastName,
                vendor.Email,
                vendor.Phone
            });


            result.Id = vendorId;
            result.FirstName = vendor.FirstName;
            result.LastName = vendor.LastName;
            result.Email = vendor.Email;
            result.Phone = vendor.Phone;

            foreach (var car in vendor.Cars)
            {
                var carId = await WriteAsync(sql2, new
                {
                    car.Make,
                    car.Model,
                    car.Year,
                    car.VIN,
                    VendorId = vendorId
                });

                result.Cars.Add(new Car
                {
                    Id = carId,
                    Make = car.Make,
                    Year = car.Year,
                    VIN = car.VIN,
                    VendorId = vendorId
                });
            }
        });

        return result;
    }
}
```

### Setup an API controller

```C#
/* Program.cs */
builder.Services.AddDapperRepository(db => db.UseSqlLite(config.GetConnectionString("Default")!));

builder.Services.AddScoped<IVendorRepository, VendorRepository>();
```

```C#
[Route("api/vendors")]
[ApiController]
public class VendorsController : ControllerBase
{
    private readonly IVendorRepository _vendorRepository;

    public VendorsController(IVendorRepository vendorRepository)
	{
        _vendorRepository = vendorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVendors()
    {
        var vendors = await _vendorRepository.GetAllVendorsAsync();
        return Ok(vendors);
    }

    [HttpGet("{id:int}", Name = "GetVendorWithCars")]
    public async Task<IActionResult> GetCarsFromVendor(int id)
    {
        var vendorCars = await _vendorRepository.GetVendorWithCarsAsync(id);
        return Ok(vendorCars);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVendor(CreateVendorDto request)
    {
        var created = await _vendorRepository.CreateCarAsync(request);
        return CreatedAtRoute("GetVendorWithCars", new { id = created.Id }, created);
    }
}
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/)
