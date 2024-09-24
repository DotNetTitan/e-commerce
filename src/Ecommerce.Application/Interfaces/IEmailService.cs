namespace Ecommerce.Application.Interfaces;

public interface IEmailService
{   
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="to">The recipient email address.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="body">The email body.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(string to, string subject, string body);
    
    /// <summary>
    /// Sends an order confirmation email asynchronously.
    /// </summary>
    /// <param name="orderId">The order ID.</param>
    /// <param name="email">The customer email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendOrderConfirmationEmail(Guid orderId, string email);
    
    /// <summary>
    /// Sends an order cancellation email asynchronously.
    /// </summary>
    /// <param name="orderId">The order ID.</param>
    /// <param name="email">The customer email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendOrderCancellationEmail(Guid orderId, string email);
}