using System.Net;
using System.Net.Mail;

namespace FoodShop.Core.CoreImplement
{
    public class EmailCore 
    {
        private readonly SmtpClient _smtpClient;

        public EmailCore(string smtpHost, int smtpPort, string smtpUsername, string smtpPassword)
        {
            _smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true // Habilitar SSL si es necesario
            };
        }

        public async Task SendOrderConfirmationEmailAsync(string recipientEmail, string orderDetails, string subject)
        {
            var fromAddress = new MailAddress("shopfood957@gmail.com", "Food Shop");
            var toAddress = new MailAddress(recipientEmail);

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = $"¡Gracias por tu compra! Aquí están los detalles de tu pedido:\n\n{orderDetails}",
                IsBodyHtml = false
            };

            try
            {
                await _smtpClient.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
                throw;
            }
            finally
            {
                // Liberar recursos
                message.Dispose();
            }
        }
    }
}
