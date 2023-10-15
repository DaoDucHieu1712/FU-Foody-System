namespace FFS.Application.DTOs.Store {
    public class StoreInforDTO {
        public string UserId { get; set; }
        public string StoreName { get; set; }
        public string AvatarURL { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }
}
