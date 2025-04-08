using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace Phonebook.Services
{
    /// <summary>
    /// Manages sending email from the application
    /// </summary>
    internal class EmailService
    {

        public UserInteractionService UiService { get; set; }
        private SmtpClient _smtpClient;
        private string _fromEmail = string.Empty;
        /// <summary>
        /// Initializes new object of EmailService Class
        /// </summary>
        /// <param name="UiService"></param>
        public EmailService(UserInteractionService UiService)
        {
            this.UiService = UiService;
            _smtpClient = new SmtpClient();
            SetupClient();
        }
        /// <summary>
        /// Setups SMTP client with values contained in secrets file
        /// </summary>
        private void SetupClient()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("secrets.json");

            var config = builder.Build();

            var snmpSettings = config.GetSection("Smtp");

            _smtpClient.Host = snmpSettings["Host"]!;
            _smtpClient.Credentials = new System.Net.NetworkCredential(snmpSettings["Username"],
                                        snmpSettings["Password"]);
            _smtpClient.EnableSsl = true;
            _smtpClient.Port = int.Parse(snmpSettings["Port"]!);

            _fromEmail = snmpSettings["From"]!;

        }
        /// <summary>
        /// Gets email subject and body from user and sends it to passed email address
        /// </summary>
        /// <param name="destinationEmail">string representing destination email address</param>
        /// <returns>true, if email was successfully sent, false otherwise</returns>
        public bool SendEmail(string destinationEmail)
        {
            try
            {
                MailAddress emailFrom = new MailAddress(_fromEmail);
                MailAddress emailTo = new MailAddress(destinationEmail);
                string subject = UiService.GetEmailSubject();
                string body = UiService.GetEmailBody();

                MailMessage myMail = new MailMessage(emailFrom, emailTo);
                myMail.Subject = subject;
                myMail.Body = body;

                _smtpClient.Send(myMail);

                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("SmtpException has occured: " + ex.Message);
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has occured: " + ex.Message);
                return false;
            }
        }

    }
}
