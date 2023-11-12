using System.Reflection.Metadata.Ecma335;

namespace FFS.Application.DTOs.User
{
    public class UserExportDTO
    {
        public int Number { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
