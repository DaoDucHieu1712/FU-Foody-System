using AutoMapper;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;
using FFS.Application.DTOs.Post;
using FFS.Application.DTOs.Order;
using AutoMapper;

namespace FFS.Application.DTOs
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
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

           

            CreateMap<CreatePostDTO, Entities.Post>().ReverseMap();
            CreateMap<Entities.Post, PostDTO>()
               .ForMember(dest => dest.Avatar,
               opt => opt.MapFrom(src => src.User.Avatar))
                .ForMember(dest => dest.Username,
               opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
            CreateMap<UpdatePostDTO, Entities.Post>().ReverseMap();

            CreateMap<Entities.ReactPost, ReactPostDTO>();
              

            CreateMap<Comment, StoreRatingDTO>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.ShipperId, opt => opt.MapFrom(src => src.ShipperId))
            .ForMember(dest => dest.NoteForShipper, opt => opt.MapFrom(src => src.NoteForShipper)).ReverseMap();
            CreateMap<ReportDTO, Report>().ReverseMap();
             

            CreateMap<Entities.Order, OrderDTO>()
                .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
                .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                .ForMember(dest => dest.ShipperName,
                opt => opt.MapFrom(src => src.Shipper.FirstName + " " + src.Shipper.LastName))
                .ReverseMap();

            
            CreateMap<Image, ImageCommentDTO>().ReverseMap();

           // CreateMap<FoodRatingDTO, Comment>()
           //.ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.FoodRatings.Select(fr => fr.FoodId).FirstOrDefault()))
           //.ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.FoodRatings.Select(fr => fr.Rate).FirstOrDefault()))
           //.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
           //.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
           //.ForMember(dest => dest.ShipperId, opt => opt.MapFrom(src => src.ShipperId))
           //.ForMember(dest => dest.NoteForShipper, opt => opt.MapFrom(src => src.NoteForShipper))
           //.ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<Comment, FoodRatingDTO>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.FoodId))
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)).ReverseMap();

            CreateMap<Discount, DiscountDTO>()
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.Expired<DateTime.Now)).ReverseMap();
            CreateMap<Comment, CommentPostDTO>().ReverseMap();


            OrderMapper();
        }

        public void OrderMapper()
        {
            CreateMap<Entities.Order, OrderRequestDTO>().ReverseMap();
            CreateMap<Entities.Order, OrderResponseDTO>()
                .ForMember(dest => dest.CustomerName ,opt => opt.MapFrom(src => src.Customer.UserName))
                .ForMember(dest => dest.ShipperName ,opt => opt.MapFrom(src => src.Shipper.FirstName + " " + src.Shipper.LastName))
                .ReverseMap();
            CreateMap<OrderDetail, OrderDetailResponseDTO>()
                .ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food.FoodName))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.Food.ImageURL))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
                .ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
        }
    }
}
