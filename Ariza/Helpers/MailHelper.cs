using System.Net;
using System.Net.Mail;

namespace Ariza.Helpers
{
    public class MailHelper
    {
        public static void SendMail(string to, string subject, string body)
        {
            var fromAddress = new MailAddress("youremail@example.com", "Ariza Destek");
            var toAddress = new MailAddress(to);
            const string fromPassword = "yourpassword";

            var smtp = new SmtpClient
            {
                Host = "smtp.example.com", // SMTP sunucunuzun adresini girin
                Port = 587, // Genellikle kullanılan port
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // HTML formatında e-posta göndermek için
            })
            {
                smtp.Send(message);
            }
        }
    }
}
