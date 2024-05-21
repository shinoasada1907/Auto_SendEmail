using AutoSendEmail.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

namespace AutoSendEmail.Services
{
    public class EmailService
    {
        //public void SendEmail(EmailModel emailModel)
        //{
        //    MailMessage mailMessage = new MailMessage();
        //    mailMessage.From = new MailAddress(emailModel.FromEmail);
        //    mailMessage.To.Add(emailModel.ToEmail);
        //    mailMessage.Subject = emailModel.Subject;
        //    mailMessage.IsBodyHtml = true;
        //    mailMessage.Body = emailModel.Body;

        //    SmtpClient smtpClient = new SmtpClient();
        //    smtpClient.Host = "smtp.se.com";
        //    smtpClient.Port = 587;
        //    smtpClient.UseDefaultCredentials = false;
        //    smtpClient.Credentials = new NetworkCredential("Digital.TransformationSEMV@se.com", "3hnPVn&qJ82aVhMXJAmYS3TxL");
        //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    smtpClient.EnableSsl = true;

        //    try
        //    {
        //        smtpClient.Send(mailMessage);
        //        Console.WriteLine("Send Email Successfull!!");
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine("SendEmail: " + ex.Message);
        //    }
        //}

        public void SendEmailWithMailKit(EmailModel emailModel, string chart, string table1, string table2)
        {
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Digital", emailModel.FromEmail));
            mailMessage.To.Add(new MailboxAddress("Tien", emailModel.ToEmail));
            mailMessage.Subject = emailModel.Subject;


            var builder = new BodyBuilder();

            var imageBytes = Convert.FromBase64String(chart.Split(',')[1]);

            var imageAttachment = builder.LinkedResources.Add("chart.png", imageBytes);
            imageAttachment.ContentId = MimeUtils.GenerateMessageId();

            builder.HtmlBody = string.Format("<h1>Here is your chart</h1><br><img src=\"cid:{0}\" /><br><h2>Table 1</h2>{1}<br><h2>Table 2</h2>{2}", imageAttachment.ContentId, table1, table2);

            mailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.se.com", 587, false);
                client.Authenticate("Digital.TransformationSEMV@se.com", "3hnPVn&qJ82aVhMXJAmYS3TxL");
                client.Send(mailMessage);
                client.Disconnect(true);
            }
        }
    }
}