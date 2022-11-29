using CarDealers.API.DTOs;
using CarDealers.API.Models;

namespace CarDealers.API.Data;
public interface IVendorRepository
{
    Task<Vendor> CreateVendorAsync(CreateVendorDto vendor);
    Task<IReadOnlyList<Vendor>> GetAllVendorsAsync();
    Task<IReadOnlyList<Vendor>> GetVendorWithCarsAsync(int id);
}