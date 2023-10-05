using Microsoft.AspNetCore.Identity;

namespace FFS.Application.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
