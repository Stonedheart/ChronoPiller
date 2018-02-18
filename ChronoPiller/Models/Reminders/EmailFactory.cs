using System.Net.Mail;
using System.Text;

namespace ChronoPiller.Models.Reminders
{
    public class EmailFactory
    {
        public string From;
        public string To;
        public MailMessage Email;

        public EmailFactory(string @from, string to, MailMessage email)
        {
            From = @from;
            To = to;
            Email = new MailMessage(from, to);
        }

        public static MailMessage GetEmailReminder(Prescription prescription)
        {
            var sb = new StringBuilder();
            prescription.PrescriptedMedicines.ForEach(x => sb.Append(x.Name + ", "));

            var email = new MailMessage("chrono@piller.com", "chronopiller@gmail.com");
            email.Subject = "Take your daily dose!";
            email.Body = $"Hello there!\n\n" +
                         $"Your friendly neighbourhood ChronoPiller would like to remind you about " +
                         $"your daily dose of {sb} from prescription {prescription.Name}!\n\n" +
                         $"Take it or You'll be sorry!\n\n" +
                         $"Cheers!";
            return email;
        }

        public static MailMessage GetEmailConfirmation(Prescription prescription)
        {
            var email = new MailMessage("chrono@piller.com", "chronopiller@gmail.com");
            email.Subject = "A new prescription has been created!";
            email.Body = $"Hello there!\n\n" +
                         $"Your friendly neighbourhood ChronoPiller would like to inform " +
                         $"that a new prescription {prescription.Name} has been created!\n\n" +
                         $"Take your pills or You'll be sorry!\n\n" +
                         $"Cheers!";

            return email;
        }
    }
}