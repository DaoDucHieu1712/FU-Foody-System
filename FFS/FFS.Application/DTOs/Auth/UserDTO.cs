using System.Text.Json.Serialization;

namespace FFS.Application.DTOs.Auth
{
    public class UserDTO
    {

        public string Id { get; set; }
        public string? Avatar { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public bool? Allow { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
