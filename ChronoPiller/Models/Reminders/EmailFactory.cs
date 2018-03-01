using System.Linq;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNet.Identity;

namespace ChronoPiller.Models.Reminders
{
    public class EmailFactory
    {
        public readonly string From;
        public string To;
        private MailMessage _email;
        private readonly StringBuilder _builder;

        public EmailFactory(string to)
        {
            From = "chronopiller@gmail.com";
            To = to;
            _email = new MailMessage(From, to);
            _builder = new StringBuilder();
        }

        public MailMessage GetIdentityEmailReminder(Prescription prescription)
        {
            if (_builder.Length > 0)
            {
                _builder.Clear();
            }
            prescription.PrescriptedMedicines.ForEach(x => _builder.Append($"{x.Dose} pills of {x.Name}"));

            _email.Subject = "Take your daily dose!";
            _email.Body = $"Hello there!\n\n" +
                          $"Your friendly neighbourhood ChronoPiller would like to remind you about " +
                          $"your daily dose of {_builder} from prescription {prescription.Name}!\n\n" +
                          $"Take it or You'll be sorry!\n\n" +
                          $"Cheers!";

            return _email;
        }

        public MailMessage GetIdentityEmailConfirmation(Prescription prescription)
        {
            _email.Subject = "A new prescription has been created!";
            _email.Body = $"Hello there!\n\n" +
                          $"Your friendly neighbourhood ChronoPiller would like to inform " +
                          $"that a new prescription {prescription.Name} has been created!\n\n" +
                          $"Take your pills or You'll be sorry!\n\n" +
                          $"Cheers!";

            return _email;
        }

        public MailMessage GetIdentityEmailWarning(Prescription prescription)
        {
            _email.Subject = "You've ran out of pills!";
            _email.Body = $"Hello there!\n\n" +
                          $"Your friendly neighbourhood ChronoPiller would like to inform " +
                          $"that prescription {prescription.Name} has been fully realized!\n\n" +
                          $"Either you're finished or fucked :3!\n\n" +
                          $"Cheers!";
            return
                _email;
        }

        private IdentityMessage CreateIdentityMessage(string subject, string body)
        {
            var message = new IdentityMessage {Body = body, Subject = subject, Destination = this.To};
            return message;
        }

        public MailMessage GetIdentityConfirmationEmail(string callbackUrl)
        {
            _email.Subject = "Confirm your account";
            _email.Body = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>\n" +
                          "If link above doesn't work, try this: \n\n" +
                          callbackUrl;
            return _email;
        }


        public MailMessage GetIdentityResetPasswordEmail(string callbackUrl)
        {
            _email.Subject = "Reset Password";
            _email.Body = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>\n" +
                          "If link above doesn't work, try this: \n\n" +
                          callbackUrl;
            return _email;
        }

        
    }

    public static class IdentityMailExtension
    {
        public static IdentityMessage ToIdentityMessage(this MailMessage message)
        {
            var identityMessage = new IdentityMessage
            {
                Body = message.Body,
                Destination = message.To.FirstOrDefault()?.ToString(),
                Subject = message.Subject
            };

            return identityMessage;

        }
    }
}