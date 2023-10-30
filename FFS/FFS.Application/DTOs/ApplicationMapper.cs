using AutoMapper;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.DTOs.Order;

namespace FFS.Application.DTOs
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() {
            CreateMap<Location, LocationDTO>()
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.User.Email)).ReverseMap();
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
            CreateMap<StoreRatingDTO, Comment>().ReverseMap();
            CreateMap<StoreReportDTO, Report>()
             .ForMember(dest => dest.ReportType, opt => opt.MapFrom(src => 1));

            CreateMap<Entities.Order, OrderDTO>()
                .ForMember(dest => dest.StoreName,
                opt => opt.MapFrom(src => src.Store.StoreName))
                .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
                .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                .ForMember(dest => dest.ShipperName,
                opt => opt.MapFrom(src => src.Shipper.FirstName + " " + src.Shipper.LastName))
                .ReverseMap();

            CreateMap<Entities.Order, OrderRequestDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
        }
    }
}
