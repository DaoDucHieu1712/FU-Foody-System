namespace FFS.Application.DTOs.QueryParametter {
    public class UserParameters : QueryStringParameters {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
