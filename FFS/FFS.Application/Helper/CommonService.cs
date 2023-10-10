using System.Text.RegularExpressions;

namespace FFS.Application.Helper
{
    public static class CommonService
    {
        private static Random rng = new Random();
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

        public static string GeneratePassword(int length)
        {
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numericChars = "0123456789";
            const string allChars = uppercaseChars + lowercaseChars + numericChars;

            char[] password = new char[length];
            int charIndex;

            // Đảm bảo có ít nhất một chữ hoa, một chữ thường và một số trong mật khẩu
            password[0] = uppercaseChars[rng.Next(uppercaseChars.Length)];
            password[1] = lowercaseChars[rng.Next(lowercaseChars.Length)];
            password[2] = numericChars[rng.Next(numericChars.Length)];

            // Sinh các ký tự ngẫu nhiên cho các vị trí còn lại trong mật khẩu
            for (int i = 3; i < length; i++)
            {
                password[i] = allChars[rng.Next(allChars.Length)];
            }

            // Trộn ngẫu nhiên các ký tự trong mật khẩu
            for (int i = length - 1; i > 0; i--)
            {
                charIndex = rng.Next(i + 1);
                char temp = password[i];
                password[i] = password[charIndex];
                password[charIndex] = temp;
            }

            return new string(password);
        }
    }
}
