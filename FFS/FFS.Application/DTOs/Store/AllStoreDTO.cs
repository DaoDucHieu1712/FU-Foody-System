namespace FFS.Application.DTOs.Store
{
    public class AllStoreDTO
    {
        public string UserId { get; set; }
        public string StoreName { get; set; }
        public string AvatarURL { get; set; }
        public string Address { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public decimal RateAverage { get; set; } 
        public int RatingCount { get; set; } 
    }
}
