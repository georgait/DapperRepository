using CarDealers.API.Data;
using CarDealers.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CarDealers.API.Controllers;
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
        var created = await _vendorRepository.CreateVendorAsync(request);
        return CreatedAtRoute("GetVendorWithCars", new { id = created.Id }, created);
    }
}
