using FFS.Application.DTOs.Location;

namespace FFS.Application.DTOs.Auth
{
	public class StoreRegisterDTO
	{
		public string StoreName { get; set; }
		public string AvatarURL { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Description { get; set; }
		public DateTime TimeStart { get; set; }
		public DateTime TimeEnd { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string PasswordConfirm { get; set; }
		// -----------------------------------
		public string Avatar { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Gender { get; set; }
		public bool Allow { get; set; }
		public DateTime BirthDay { get; set; }

		public LocationDTO location { get; set; }
	}
}
