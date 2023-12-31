﻿using System.ComponentModel.DataAnnotations.Schema;
using FFS.Application.Entities.Common;

namespace FFS.Application.Entities
{

	[Table("Store")]
	public class Store : BaseEntity<int>
	{

		public string UserId { get; set; }
		public string StoreName { get; set; }
		public string AvatarURL { get; set; }
		public string PhoneNumber { get; set; }
		public string Description { get; set; }
		public string TimeStart { get; set; }
		public string TimeEnd { get; set; }
		public decimal RateAverage { get; set; } = 0;
		public int TotalRate { get; set; } = 0;
		public int RatingCount { get; set; } = 0;
		public string Address { get; set; }
		public ICollection<Discount> Discounts { get; set; }
		public ICollection<Inventory> Inventories { get; set; }
		public ICollection<Comment> Comments { get; set; }
		public ICollection<Category> Categories { get; set; }
		public ICollection<Combo> Combos { get; set; }
		public ICollection<Food> Foods { get; set; }
		public ICollection<FoodCombo> FoodCombos { get; set; }
		public ICollection<FlashSale>? FlashSales { get; set; }
	}

}
