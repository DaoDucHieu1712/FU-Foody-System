namespace FFS.Application.DTOs
{
    public class LocationDTO
    {
        public string UserId { get; set; }
        public virtual int Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual bool IsDelete { get; set; }
        public string Address { get; set; }
        public bool IsDefault { get; set; }
        //public string? Description { get; set; }
    }
}
