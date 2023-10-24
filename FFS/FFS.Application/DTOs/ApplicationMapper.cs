using AutoMapper;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;

namespace FFS.Application.DTOs
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Entities.Store, StoreInforDTO>().ReverseMap();
            CreateMap<CreateInventoryDTO, Entities.Inventory>().ReverseMap();
            CreateMap<Entities.Inventory, InventoryDTO>()
                .ForMember(dest => dest.FoodName,
                opt => opt.MapFrom(src => src.Food.FoodName))
                 .ForMember(dest => dest.ImageURL,
                opt => opt.MapFrom(src => src.Food.ImageURL))
                 .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Food.Category.CategoryName));
               

        }
    }
}
