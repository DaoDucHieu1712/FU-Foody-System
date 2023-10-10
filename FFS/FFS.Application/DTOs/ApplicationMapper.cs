using AutoMapper;
using FFS.Application.Entities;

namespace FFS.Application.DTOs
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Location, LocationDTO>().ReverseMap();
        }
    }
}
