using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using FFS.Application.DTOs.Auth;

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
        public static bool IsEmailFPT(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@fpt\.edu\.vn$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        public static bool IsEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public static string ExtractUsername(string emailAddress)
        {
            int atIndex = emailAddress.IndexOf("@");

            if (atIndex != -1)
            {
                return emailAddress.Substring(0, atIndex);
            }
            else
            {
                return emailAddress;
            }
        }
        public static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                // Replace the Vietnamese letter "đ" with "d" (both lowercase and uppercase)
                if (c == 'đ' || c == 'Đ')
                {
                    stringBuilder.Append('d');
                }
                else if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
