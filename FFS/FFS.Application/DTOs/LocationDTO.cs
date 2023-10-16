namespace FFS.Application.DTOs
{
    public class LocationDTO
    {
        public virtual int? Id { get; set; }
        public string Address { get; set; }
        public string? Description { get; set; }
        public string Receiver { get; set; }
        public string PhoneNumber { get; set; }
    }
}
