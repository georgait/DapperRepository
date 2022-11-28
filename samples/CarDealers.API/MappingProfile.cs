using AutoMapper;
using CarDealers.API.DTOs;
using CarDealers.API.Models;

namespace CarDealers.API;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Car, CreateCarDto>();
		CreateMap<Vendor, CreateVendorDto>();
	}
}
