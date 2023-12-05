namespace FFS.Application.DTOs.Location
{
    public class LocationDTO
    {
		public int ProvinceID { get; set; }
		public string ProvinceName { get; set; }
		public int DistrictID { get; set; }
		public string DistrictName { get; set; }
		public string WardCode { get; set; }
		public string WardName { get; set; }
		public string Address { get; set; }
		public bool? IsDefault { get; set; } = false;
		public string? Description { get; set; }
		public string Receiver { get; set; }
		public string PhoneNumber { get; set; }
	}
}
