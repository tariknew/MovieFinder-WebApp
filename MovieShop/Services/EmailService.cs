﻿using MovieShop.IServices;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MovieShop.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("tarik.smajlovic@edu.fit.ba", "MovieShop");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
