namespace FFS.Application.DTOs.Store
{
    public class AllStoreDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string StoreName { get; set; }
        public string AvatarURL { get; set; }
        public string Address { get; set; }
		public string TimeStart { get; set; }
		public string TimeEnd { get; set; }
		public decimal RateAverage { get; set; } 
        public int RatingCount { get; set; } 
    }
}
