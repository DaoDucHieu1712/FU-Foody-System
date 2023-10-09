using System.Text.RegularExpressions;

namespace FFS.Application.Helper {
    public static class ValidatePassword {
        public static bool IsStrongPassword(string password)
        {
            // Check if the password is at least 8 characters long
            if (password.Length < 8)
            {
                return false;
            }

            // Check if the password contains at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // Check if the password contains at least one lowercase letter
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // Check if the password contains at least one digit (0-9)
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // Check if the password contains at least one special character
            if (!Regex.IsMatch(password, @"[@#$%^&+=]"))
            {
                return false;
            }

            return true;
        }
    }
}
