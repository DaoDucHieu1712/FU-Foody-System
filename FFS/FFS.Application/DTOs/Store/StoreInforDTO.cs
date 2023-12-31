﻿using FFS.Application.DTOs.Location;
using FFS.Application.Entities;

namespace FFS.Application.DTOs.Store
{
	public class StoreInforDTO
	{
		public string? UserId { get; set; }
		public string StoreName { get; set; }
		public string? AvatarURL { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Description { get; set; }
		public string TimeStart { get; set; }
		public string TimeEnd { get; set; }
		public ICollection<Discount>? Discounts { get; set; }
		public ICollection<FFS.Application.Entities.Inventory>? Inventories { get; set; }
		public ICollection<Entities.Comment>? Comments { get; set; }
		public ICollection<Entities.Category>? Categories { get; set; }
		public ICollection<Combo>? Combos { get; set; }
		public ICollection<FFS.Application.Entities.Food>? Foods { get; set; }
		public ICollection<FoodCombo>? FoodCombos { get; set; }
		public LocationDTO? Location { get; set; }

	}


}
