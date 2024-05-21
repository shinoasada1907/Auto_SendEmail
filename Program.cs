using AutoSendEmail.Models;
using AutoSendEmail.Services;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

string startTime = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
string endTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

EmailModel emailModel = new EmailModel();
EmailService emailService = new EmailService();

ChartService.CreateChart(startTime: startTime, endTime: endTime);

string imageFilePath = "Assets/chart.png";
byte[] imageBytes = File.ReadAllBytes(imageFilePath);
string base64String = Convert.ToBase64String(imageBytes);

// Console.WriteLine(base64String);

emailModel.FromEmail = "Digital.TransformationSEMV@se.com";
emailModel.ToEmail = "tien.nguyenthanh@se.com";
emailModel.CCEmail = "";
emailModel.Subject = "Test";

MimeMessage mailMessage = new MimeMessage();
mailMessage.From.Add(new MailboxAddress("Digital", emailModel.FromEmail));
mailMessage.To.Add(new MailboxAddress("Tien", emailModel.ToEmail));
mailMessage.Subject = emailModel.Subject;


var builder = new BodyBuilder();

var imageBytes1 = Convert.FromBase64String(base64String);

var imageAttachment = builder.LinkedResources.Add("chart.png", imageBytes1);
imageAttachment.ContentId = MimeUtils.GenerateMessageId();

builder.HtmlBody = string.Format("<img src=\"cid:{0}\" /> </br> </br> <h3>KE By Target</h3> </br> {1} </br> </br> <h3>Sub Indusoft key in vs Sub FG Needed GAP</h3> </br> {2}", imageAttachment.ContentId, TableService.CreateTableTarget(startTime: startTime, endTime: endTime), TableService.CreateTableSubQuantity(startTime: startTime, endTime: endTime));

mailMessage.Body = builder.ToMessageBody();

using (var client = new SmtpClient())
{
    client.Connect("smtp.se.com", 587, false);
    client.Authenticate("Digital.TransformationSEMV@se.com", "3hnPVn&qJ82aVhMXJAmYS3TxL");
    client.Send(mailMessage);
    client.Disconnect(true);
}

Console.WriteLine("Done!!!!!");
