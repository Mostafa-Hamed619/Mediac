using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using MediacApi.DTOs.Account;

namespace MediacApi.Services
{
    public class EmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration _config)
        {
            config = _config;
        }

        public async Task<bool> sendEmail(EmailSendDto model)
        {
            MailjetClient client = new MailjetClient(config["MailJet:APIKey"], config["MailJet:SecretKey"]);

            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(config["Email:From"], config["Email:ApplicationName"]))
                .WithTo(new SendContact(model.To))
                .WithHtmlPart(model.Body)
                .WithSubject(model.Subject)
                .Build();

            var result = await client.SendTransactionalEmailAsync(email);

            if(result.Messages != null)
            {
                if (result.Messages[0].Status == "success")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
