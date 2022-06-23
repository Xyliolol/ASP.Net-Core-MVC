using MailKit.Net.Smtp;
using MimeKit;
using MVCApp.Interface;

namespace MVCApp.Models
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(Product product)
        {
           
            string info = "добавлен новый товар";
            string message = $"Id: {product.Id}. Название: {product.Name}. Описание {product.Description}";
            string FromEmail =  "jeeytis@yandex.ru";
            string ToEmail = "asp2022gb@rodion-m.ru";
            int portConnection = 25;
            string smtpServer = "smtp.beget.com";
            string password = "3drtLSa1";

            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Student", FromEmail));
            mimeMessage.To.Add(new MailboxAddress("Student", ToEmail));
            mimeMessage.Subject = info;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, portConnection, false);
                await client.AuthenticateAsync(FromEmail, password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
