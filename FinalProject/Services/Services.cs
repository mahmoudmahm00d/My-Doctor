using FinalProject.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FinalProject.Services
{
    public static class AppServices
    {
        public static TSelf TrimStringProperties<TSelf>(TSelf input)
        {
            var stringProperties = input.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = (string)stringProperty.GetValue(input, null);
                if (currentValue != null)
                    stringProperty.SetValue(input, currentValue.Trim(), null);
            }
            return input;
        }

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

        public static bool IsValidTime(string time)
        {
            Regex regex1 = new Regex("2[0-3]{1}\\:[0-5]{1}[0-9]{1}");
            Regex regex2 = new Regex("[0-2]{1}[0-9]{1}\\:[0-5]{1}[0-9]{1}");
            if (time.StartsWith("2"))
                return regex1.IsMatch(time);
            return regex2.IsMatch(time);
        }

        public static string TokenEncoding(string email, string password)
        {
            string input = email + ":" + password;
            byte[] array = System.Text.Encoding.ASCII.GetBytes(input);
            return Convert.ToBase64String(array);
        }

        public static void SendConfirmEmail(string email, string code, int id)
        {
            string url = "http://mydoctorapp-001-site1.ctempurl.com/accounts/confirmuser/"+id;

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("mydoctorappteam@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Confirm Your Account";
                mail.Body = $"<h1> Your Verfication Code </h1><br>{code}<br>click on link below to confirm your account<br><a href={url}>Validate Here</a> ";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mydoctorappteam@gmail.com", "MYDOC2021tor");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch
            {
            }
        }

        public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public static bool VerifayPasswrod(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}