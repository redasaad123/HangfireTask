using System.Net;
using System.Net.Mail;

namespace HangfireTask.Services
{
    public interface IService
    {
        int CountOfClicked(int number);
        void DailyReport();
        Task SendEmail(string Email);

        Task CancellationToken(CancellationToken cancellationToken);
    }

    public class Service : IService
    {
        private readonly IConfiguration configuration;

        public Service(IConfiguration configuration)
        {
            this.configuration = configuration;
        }



        public int CountOfClicked(int number)
        {
            number++;
            return number;
        }

        public async Task SendEmail(string Email)
        {
            var email = configuration.GetValue<string>("email-conf:Email");
            var password = configuration.GetValue<string>("email-conf:password");
            var host = "smtp.gmail.com";
            var port = 587;


            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;

            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage(email!, Email, "mail form Task ya Ziad", "hello in abasia");

            await smtpClient.SendMailAsync(message);








        }

        public void DailyReport()
        {

            string report = "daily report ... ";

            Console.WriteLine($"{report} ----> {DateTime.Now}");

        }

        public async Task CancellationToken(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 20 ; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine($"step {i}");
                await Task.Delay(1000);
            }
             
        }
    }
}
