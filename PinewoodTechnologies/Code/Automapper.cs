using AutoMapper;
using Microsoft.Build.Framework;
using Microsoft.IdentityModel.Tokens;
using PinewoodTechnologies.Models;

namespace PinewoodTechnologies.Code
{
    public class Automapper
    {
        public MapperConfiguration configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Customer, ViewCustomer>() 
            .ForMember(dst => dst.Id,opt => opt.MapFrom(src => src.Id.ToString()))
            .AfterMap((src, dst) => {
                dst.address = new ViewAddress();
                dst.address.Street = src.Street;
                dst.address.House = src.House;
                dst.address.Extra = src.Extra;
                dst.address.City = src.City;
                dst.address.Postcode = src.Postcode;
            }).ReverseMap()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id.IsNullOrEmpty() ? 0 : int.Parse(src.Id)))
            .AfterMap((src, dst) => {
                dst.Street = src.address.Street;
                dst.House = src.address.House;
                dst.Extra = src.address.Extra;
                dst.City = src.address.City;
                dst.Postcode =src.address.Postcode;
            });
        });
    }
}
