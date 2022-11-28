using CarDealers.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarDealers.API.Controllers;
[Route("api/cars")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarRepository _carRepository;

    public CarsController(ICarRepository carRepository)
	{
        _carRepository = carRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCars()
    {
        var cars = await _carRepository.GetAllCardsAsync();
        return Ok(cars);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCarById(int id)
    {
        var car = await _carRepository.GetCarAsync(id);
        return Ok(car);
    }
}
