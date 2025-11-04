using System;
using System.Security.Cryptography;
using System.Text;

namespace ApiSpotify.UTILS
{
    public static class UtilsContrasenya
    {
        private static readonly string specialChars = "!@#$%^&*()-_=+[]{}<>?/";

        public static string GenerateSalt(int length = 8)
        {
            byte[] saltBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            string baseSalt = Convert.ToBase64String(saltBytes);

            Random rnd = new Random();
            char special = specialChars[rnd.Next(specialChars.Length)];

            return baseSalt + special;
        }

        public static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string combined = password + salt;
                byte[] bytes = Encoding.UTF8.GetBytes(combined);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string salt)
        {
            string enteredHash = HashPassword(enteredPassword, salt);
            return storedHash == enteredHash;
        }

       
    }
}



