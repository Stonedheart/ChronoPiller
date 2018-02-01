using System.Net;
using System.Net.Mail;

namespace ChronoPiller.Models
{
    public class DefaultEmailClient : SmtpClient
    {


        public DefaultEmailClient(string name, string pass)
        {

            Host = "smtp.gmail.com";
            Port = 587;
            EnableSsl = true;
            Timeout = 20000;
            DeliveryMethod = SmtpDeliveryMethod.Network;
            UseDefaultCredentials = false;
            Credentials = new NetworkCredential(name, pass);

        }
    }
}