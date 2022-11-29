using CarDealers.API.DTOs;
using CarDealers.API.Models;
using Repository;
using Repository.Contracts;

namespace CarDealers.API.Data;

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

    public async Task<Vendor> CreateVendorAsync(CreateVendorDto vendor)
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
