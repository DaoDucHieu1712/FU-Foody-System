﻿namespace FFS.Application.DTOs.QueryParametter
{
    public class InventoryParameters: QueryStringParameters
    {
        public int StoreId { get; set; }
        public string? FoodName { get; set; }
       
    }
}
