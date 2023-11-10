﻿namespace FFS.Application.DTOs.Location
{
    public class LocationDTO
    {
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? Receiver { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsDefault { get; set; } = false;
    }
}