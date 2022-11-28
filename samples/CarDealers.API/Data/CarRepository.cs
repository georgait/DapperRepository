using CarDealers.API.Models;
using Repository;
using Repository.Contracts;

namespace CarDealers.API.Data;

public sealed class CarRepository : BaseRepository<Car>, ICarRepository
{
    public CarRepository(IDbContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<Car>> GetAllCardsAsync()
    {
        var sql = """
                SELECT Id, Make, Model, Year, VIN
                FROM Car
                """;

        return await ReadAsync(sql, new { });
    }

    public async Task<Car> GetCarAsync(int id)
    {
        var sql = """
                SELECT Id, Make, Model, Year, VIN
                FROM Car
                WHERE Id = @id
                """;

        return (await ReadAsync(sql, new { id }))[0];
    }
}
