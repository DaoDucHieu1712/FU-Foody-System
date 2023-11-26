namespace FFS.Application.DTOs.QueryParametter {
    public class UserParameters : QueryStringParameters {
        public string id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public int? Status { get; set; }
    }
}
