namespace FoodShop.Core.CoreInterface
{
    public interface IEmailCore
    {
        Task SendOrderConfirmationEmailAsync(string recipientEmail, string orderDetails);
    }
}
