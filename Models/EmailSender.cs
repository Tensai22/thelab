using System.Net.Mail;
using System.Net;

namespace TheLab.Models
{
    public class EmailSender : IMessage
    {
        public bool SendMessage(string to, string messageBody, string subject)
        {
            var fromAddress = new MailAddress("gersen.e.a@gmail.com", "From Name");
            var toAddress = new MailAddress("gersen.e.a@gmail.com", "To Name");
            const string fromPassword = "";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = messageBody
            })
            {
                try
                {
                    smtp.Send(message);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
