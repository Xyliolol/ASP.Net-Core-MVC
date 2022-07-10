using MVCApp.Models;

namespace MVCApp.Interface
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(Product product, CancellationTokenSource cancelTokenSource);
    }
}
