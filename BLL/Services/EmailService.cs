using System.Net;
using System.Net.Mail;

namespace QlyDaoTao.BLL.Services
{
    public class EmailService
    {
        public async Task GuiOtpAsync(string email, string otp)
        {
            var message = new MailMessage();

            message.From = new MailAddress("abc@gmail.com");
            message.To.Add(email.Trim());

            message.Subject = "Ma OTP dat lai mat khau";
            message.Body = $"Ma OTP cua ban la: {otp}. Ma co hieu luc trong 5 phut.";

            using var smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential(
                "QuanLyDaoTao14@gmail.com",
                "ixbq bguh whfk gpzo"
            );

            smtp.EnableSsl = true;

            await smtp.SendMailAsync(message);
        }
    }
}