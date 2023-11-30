
using TestTelcoHub.Model.Helper;
using TestTelcoHub.Model.Prototype;

namespace TestTelcoHub.Service.Interface
{
    public interface IEmailService
    {
        Task SendSuccessEmailAsync(ResposeResult subscriptionResponse, string userEmail);
        Task SendRenewalReminderAsync(string userEmail);
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
