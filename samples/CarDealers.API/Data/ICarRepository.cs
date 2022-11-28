using CarDealers.API.Models;

namespace CarDealers.API.Data;
public interface ICarRepository
{
    Task<IReadOnlyList<Car>> GetAllCardsAsync();
    Task<Car> GetCarAsync(int id);
}