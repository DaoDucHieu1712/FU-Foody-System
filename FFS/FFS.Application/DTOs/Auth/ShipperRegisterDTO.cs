namespace FFS.Application.DTOs.Auth {
    public class ShipperRegisterDTO {
		public string PasswordConfirm { get; set; }
		public string AvatarURL { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Gender { get; set; }
		public bool Allow { get; set; }
	}
}
