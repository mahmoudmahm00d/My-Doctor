using FinalProject.Models;
using System;
using System.Linq;

namespace FinalProject.Services
{
    public static class AppServices
    {
        public static string GenerateRandomNumber()
        {
            Random random = new Random();
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                code += random.Next(0, 10).ToString();
            }
            return code;
        }

        public static bool CheckEmailIfExist(string email)
        {
            using (MyAppContext db = new MyAppContext())
            {
                int count = db.Users.Where(u => u.UserEmail == email).Count();
                return count != 0;
            }
        }

        public static string TokenEncoding(string email, string password)
        {
            string input = email + ":" + password;
            byte[] array = System.Text.Encoding.ASCII.GetBytes(input);
            return Convert.ToBase64String(array);
        }

        public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public static bool VerifayPasswrod(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}