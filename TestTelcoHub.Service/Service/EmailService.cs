using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TestTelcoHub.Model.Helper;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Service.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings> ops)
        {
            this.emailSettings = ops.Value;
        }

        public async Task SendSuccessEmailAsync(ResposeResult subscriptionResponse, string userEmail)
        {
            var emailRequest = new MailRequest
            {
                ToMail = userEmail,
                Subject = "Successful Subscription",
                Body = $"Your subscription was successful.<br>" +
                        $" \n Subscription ID: {subscriptionResponse.SubscriptionId}<br>" +
                        $" \n ActivationCode: {subscriptionResponse.ActivationCode}<br>" +
                        $" \n LicenseId {subscriptionResponse.LicenseId}"
            };

            await SendEmailAsync(emailRequest);
        }
        public async Task SendRenewalReminderAsync(string userEmail)
        {
            var emailRequest = new MailRequest
            {
                ToMail = userEmail,
                Subject = "Renewal Reminder",
                Body = "This is a reminder to renew your subscription. Please renew it to continue enjoying our services."
            };
            await SendEmailAsync(emailRequest);
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToMail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            smtp.Authenticate(emailSettings.Email, emailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
