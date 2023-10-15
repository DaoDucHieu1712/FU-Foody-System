using AutoMapper;

using FFS.Application.DTOs.Store;
using FFS.Application.Entities;

namespace FFS.Application.DTOs
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Entities.Store, StoreInforDTO>().ReverseMap();

        }
    }
}
