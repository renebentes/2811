using System.Net;
using System.Net.Mail;

namespace Blog.Services;

public class SmtpEmailService
{
    public bool Send(string toName,
                     string toEmail,
                     string subject,
                     string body,
                     string fromName = "Equipe",
                     string fromEmail = "email@empresa")
    {
        var smtpClient = new SmtpClient(Configuration.SmtpConfiguration.Host, Configuration.SmtpConfiguration.Port)
        {
            Credentials = new NetworkCredential(Configuration.SmtpConfiguration.Username,
            Configuration.SmtpConfiguration.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName)
        };
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch (Exception e)
        {
            return false;
            throw;
        }
    }
}
