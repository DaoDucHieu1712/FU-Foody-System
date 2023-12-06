using AutoMapper;
using FFS.Application.DTOs.Auth;
using FFS.Application.DTOs.Category;
using FFS.Application.DTOs.Chat;
using FFS.Application.DTOs.Comment;
using FFS.Application.DTOs.FlashSale;
using FFS.Application.DTOs.Food;
using FFS.Application.DTOs.Inventory;
using FFS.Application.DTOs.Location;
using FFS.Application.DTOs.Order;
using FFS.Application.DTOs.Post;
using FFS.Application.DTOs.Store;
using FFS.Application.Entities;

namespace FFS.Application.DTOs
{
	public class ApplicationMapper : Profile
	{
		public ApplicationMapper()
		{
			_ = CreateMap<Entities.Location, LocationDTO>().ReverseMap();

			_ = CreateMap<Entities.Food, FoodDTO>()
				.ForMember(dest => dest.PriceAfterSale, opt => opt.MapFrom(src => src.FlashSaleDetails != null && src.FlashSaleDetails.Any() ? src.FlashSaleDetails.FirstOrDefault(x => x.FlashSale.Start <= DateTime.Now && x.FlashSale.End >= DateTime.Now).PriceAfterSale : default(decimal?)))
				.ForMember(dest => dest.SalePercent, opt => opt.MapFrom(src => src.FlashSaleDetails != null && src.FlashSaleDetails.Any() ? src.FlashSaleDetails.FirstOrDefault(x => x.FlashSale.Start <= DateTime.Now && x.FlashSale.End >= DateTime.Now).SalePercent : default(int?)))
				.ReverseMap();
			_ = CreateMap<Entities.Store, StoreInforDTO>().ReverseMap();
			_ = CreateMap<Entities.Store, AllStoreDTO>().ReverseMap();
			_ = CreateMap<CreateInventoryDTO, Entities.Inventory>().ReverseMap();
			_ = CreateMap<Entities.Inventory, InventoryDTO>()
				.ForMember(dest => dest.FoodName,
				opt => opt.MapFrom(src => src.Food.FoodName))
				 .ForMember(dest => dest.ImageURL,
				opt => opt.MapFrom(src => src.Food.ImageURL))
				 .ForMember(dest => dest.CategoryName,
				opt => opt.MapFrom(src => src.Food.Category.CategoryName))
				 .ForMember(dest => dest.Price,
					opt => opt.MapFrom(src => src.Food.Price));
			_ = CreateMap<Entities.Food, ExportFoodDTO>()
				.ForMember(dest => dest.CategoryName,
				opt => opt.MapFrom(src => src.Category.CategoryName)).ReverseMap();
			_ = CreateMap<Entities.Inventory, ExportInventoryDTO>()
			   .ForMember(dest => dest.CategoryName,
			   opt => opt.MapFrom(src => src.Food.Category.CategoryName))
			   .ForMember(dest => dest.FoodName,
			   opt => opt.MapFrom(src => src.Food.FoodName))
				.ForMember(dest => dest.FoodId,
			   opt => opt.MapFrom(src => src.Food.Id));



			_ = CreateMap<CreatePostDTO, Entities.Post>().ReverseMap();
			_ = CreateMap<Entities.Post, PostDTO>()
			   .ForMember(dest => dest.Avatar,
			   opt => opt.MapFrom(src => src.User.Avatar))
				.ForMember(dest => dest.Username,
			   opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
			_ = CreateMap<UpdatePostDTO, Entities.Post>().ReverseMap();

			_ = CreateMap<Entities.ReactPost, ReactPostDTO>().ForMember(dest => dest.Username,
			   opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
				.ForMember(dest => dest.Avartar,
			   opt => opt.MapFrom(src => src.User.Avatar));

			_ = CreateMap<Entities.ReactPost, CreateReactPostDTO>();



			_ = CreateMap<Entities.Comment, StoreRatingDTO>()
			.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
			.ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
			.ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
			.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
			.ForMember(dest => dest.ShipperId, opt => opt.MapFrom(src => src.ShipperId))
			.ForMember(dest => dest.NoteForShipper, opt => opt.MapFrom(src => src.NoteForShipper)).ReverseMap();
			_ = CreateMap<ReportDTO, Entities.Report>().ReverseMap();


			_ = CreateMap<Entities.Order, OrderDTO>()
				.ForMember(dest => dest.CustomerName,
				opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
				.ForMember(dest => dest.PhoneNumber,
				opt => opt.MapFrom(src => src.Customer.PhoneNumber))
				.ForMember(dest => dest.ShipperName,
				opt => opt.MapFrom(src => src.Shipper.FirstName + " " + src.Shipper.LastName))
				.ReverseMap();





			_ = CreateMap<Image, ImageCommentDTO>().ReverseMap();


			// CreateMap<FoodRatingDTO, Comment>()
			//.ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.FoodRatings.Select(fr => fr.FoodId).FirstOrDefault()))
			//.ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.FoodRatings.Select(fr => fr.Rate).FirstOrDefault()))
			//.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
			//.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
			//.ForMember(dest => dest.ShipperId, opt => opt.MapFrom(src => src.ShipperId))
			//.ForMember(dest => dest.NoteForShipper, opt => opt.MapFrom(src => src.NoteForShipper))
			//.ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

			_ = CreateMap<Entities.Comment, FoodRatingDTO>()
			.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
			.ForMember(dest => dest.FoodId, opt => opt.MapFrom(src => src.FoodId))
			.ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
			.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
			.ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images)).ReverseMap();
			_ = CreateMap<Entities.Comment, CommentDTO>().ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
				.ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar)).ReverseMap();

			_ = CreateMap<Entities.Food, AllFoodDTO>()
				.ForMember(dest => dest.PriceAfterSale, opt => opt.MapFrom(src => src.FlashSaleDetails != null && src.FlashSaleDetails.Any() ? src.FlashSaleDetails.FirstOrDefault(x => x.FlashSale.Start <= DateTime.Now && x.FlashSale.End >= DateTime.Now).PriceAfterSale : 0))
				.ForMember(dest => dest.SalePercent, opt => opt.MapFrom(src => src.FlashSaleDetails != null && src.FlashSaleDetails.Any() ? src.FlashSaleDetails.FirstOrDefault(x => x.FlashSale.Start <= DateTime.Now && x.FlashSale.End >= DateTime.Now).SalePercent : 0))
				.ForMember(dest => dest.NumberOfProductSale, opt => opt.MapFrom(src => src.FlashSaleDetails != null && src.FlashSaleDetails.Any() ? src.FlashSaleDetails.FirstOrDefault(x => x.FlashSale.Start <= DateTime.Now && x.FlashSale.End >= DateTime.Now).NumberOfProductSale : 0))
				.ReverseMap();

			_ = CreateMap<Discount, DiscountDTO>()
				.ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.Expired < DateTime.Now)).ReverseMap();
			_ = CreateMap<Entities.Comment, CommentPostDTO>()
			  .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
			  .ForMember(dest => dest.Avartar, opt => opt.MapFrom(src => src.User.Avatar))
			  .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.CreatedAt))
			  .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

			_ = CreateMap<Image, ImageCommentDTO>();

			_ = CreateMap<ApplicationUser, UserInfoDTO>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));


			OrderMapper();
			CategoryMapper();
			FlashSaleMapper();
			ChatMapper();
			ComboMapper();
		}

		public void OrderMapper()
		{
			_ = CreateMap<Entities.Order, OrderRequestDTO>().ReverseMap();
			_ = CreateMap<Entities.Order, OrderResponseDTO>()
				.ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.UserName))
				.ForMember(dest => dest.ShipperName, opt => opt.MapFrom(src => src.Shipper.FirstName + " " + src.Shipper.LastName))
				.ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment.PaymentMethod))
				.ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Payment.Status))
				.ReverseMap();
			_ = CreateMap<OrderDetail, OrderDetailResponseDTO>()
				.ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food.FoodName))
				.ForMember(dest => dest.ComboName, opt => opt.MapFrom(src => src.Combo.Name))
				.ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src =>
		src.Food != null ? src.Food.ImageURL : (src.Combo != null ? src.Combo.Image : null)
	))
				.ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
				.ReverseMap();
			_ = CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
			_ = CreateMap<Entities.Order, CreateOrderDTO>().ReverseMap();
		}

		public void CategoryMapper()
		{
			_ = CreateMap<Entities.Category, CategoryResponseDTO>().ReverseMap();
			_ = CreateMap<Entities.Category, CategoryDTO>().ReverseMap();
			_ = CreateMap<Entities.Category, CategoryRequestDTO>().ReverseMap();
			_ = CreateMap<Entities.Category, CategoryPopularDTO>().ReverseMap();
		}

		public void ChatMapper()
		{
			_ = CreateMap<Entities.Chat, ChatResponseDTO>()
				.ForMember(dest => dest.ToUserImage, opt => opt.MapFrom(src => src.ToUser.Avatar))
				.ForMember(dest => dest.FromUserImage, opt => opt.MapFrom(src => src.FormUser.Avatar))
				.ForMember(dest => dest.FromUserName, opt => opt.MapFrom(src => src.FormUser.UserName))
				.ForMember(dest => dest.ToUserName, opt => opt.MapFrom(src => src.ToUser.UserName))
				.ReverseMap();
			_ = CreateMap<Entities.Chat, ChatRequestDTO>().ReverseMap();
			_ = CreateMap<Message, MessageResponseDTO>().ReverseMap();
			_ = CreateMap<Message, MessageRequestDTO>().ReverseMap();
		}

		public void FlashSaleMapper()
		{
			_ = CreateMap<Entities.FlashSaleDetail, FlashSaleDetailDTO>()
	.ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food != null ? src.Food.FoodName : null))
	.ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.Food != null ? src.Food.ImageURL : null))
	.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Food != null ? src.Food.Price : default(decimal?)))
	.ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Food != null && src.Food.Inventories != null && src.Food.Inventories.Any() ? src.Food.Inventories.First().quantity : default(int?)))
	.ReverseMap();
			_ = CreateMap<Entities.FlashSale, FlashSaleDTO>().ReverseMap();
			_ = CreateMap<Entities.Food, FoodFlashSaleDTO>()
	 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
	 .ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.FoodName))
	 .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.ImageURL))
	 .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
	  .ForMember(dest => dest.CategoryName,
				opt => opt.MapFrom(src => src.Category.CategoryName))
	 .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Inventories != null && src.Inventories.Any() ? src.Inventories.Sum(inv => inv.quantity) : 0));

		}


		public void ComboMapper()
		{
			_ = CreateMap<Combo, ComboResponseDTO>().ReverseMap();
			_ = CreateMap<Entities.Food, FoodCombo>().ReverseMap();
		}
	}
}
