
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VendorManagementSystem.Application.Dtos.UtilityDtos;
using VendorManagementSystem.Application.IServices;

namespace VendorManagementSystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettingsDto _emailSettings;

        public EmailService(IOptions<EmailSettingsDto> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public void SendLoginEmail(EmailDetailsDto emailDetailsDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_emailSettings.From, _emailSettings.FromName),
                Subject = emailDetailsDto.Subject,
                Body = emailDetailsDto.Body,
                IsBodyHtml = true,
            };
            message.To.Add(new MailAddress(emailDetailsDto.ToAddress, emailDetailsDto.ToName));
            using(var client = new SmtpClient(_emailSettings.SmtpClient, _emailSettings.Port))
            {
                client.Credentials = new NetworkCredential(_emailSettings.AuthEmail, _emailSettings.AuthKey);
                client.EnableSsl = true;
                try
                {
                    client.Send(message);
                    Console.WriteLine("Email Sent Successfully to {0}", emailDetailsDto.ToAddress);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error sending email: " + ex.Message);
                    throw;
                }
            }
           
        }

        public void SendAckEmail(AckEmailDto ackEmailDto)
        {
            MailMessage message = new MailMessage
            {
                
                Subject = ackEmailDto.Subject,
                Body = ackEmailDto.Body,
                IsBodyHtml = true,
            };
            message.From = new MailAddress(ackEmailDto.FromEmailAddress, ackEmailDto.FromName);
            message.To.Add(new MailAddress(ackEmailDto.ToEmailAddress, ackEmailDto.ToName));
            message.ReplyToList.Add(new MailAddress(ackEmailDto.FromEmailAddress, ackEmailDto.FromName));
            
            message.Subject = ackEmailDto.Subject;
            using (var pdfStream = new MemoryStream(ackEmailDto.Pdf.Content!))
            {
                var attachement = new Attachment(pdfStream, $"{ackEmailDto.Pdf.Name}.pdf", "application/pdf");
                message.Attachments.Add(attachement);

                using(var client = new SmtpClient(_emailSettings.SmtpClient, _emailSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_emailSettings.AuthEmail, _emailSettings.AuthKey);
                    client.EnableSsl = true;
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email Sent Successfully to {0}", ackEmailDto.ToEmailAddress);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error sending email: " + ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}
