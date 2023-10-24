using AutoMapper;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;

namespace FFS.Application.DTOs
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Entities.Food, FoodDTO>().ReverseMap();
            CreateMap<Entities.Store, StoreInforDTO>().ReverseMap();
            CreateMap<CreateInventoryDTO, Entities.Inventory>().ReverseMap();
            CreateMap<Entities.Inventory, InventoryDTO>()
                .ForMember(dest => dest.FoodName,
                opt => opt.MapFrom(src => src.Food.FoodName))
                 .ForMember(dest => dest.ImageURL,
                opt => opt.MapFrom(src => src.Food.ImageURL))
                 .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Food.Category.CategoryName));
            CreateMap<Entities.Food, ExportFoodDTO>()
                .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.CategoryName)).ReverseMap();
            CreateMap<Entities.Inventory, ExportInventoryDTO>()
               .ForMember(dest => dest.CategoryName,
               opt => opt.MapFrom(src => src.Food.Category.CategoryName))
               .ForMember(dest => dest.FoodName,
               opt => opt.MapFrom(src => src.Food.FoodName))
                .ForMember(dest => dest.FoodId,
               opt => opt.MapFrom(src => src.Food.Id));
        }
    }
}
