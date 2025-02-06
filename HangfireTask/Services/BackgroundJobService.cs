using System.Net;
using System.Net.Mail;

namespace HangfireTask.Services
{
    public interface IBackgroundJobService
    {
        int CountOfClicked(int number);
        void DailyReport();
        Task SendEmail(string Email);

        Task CancellationToken(CancellationToken cancellationToken);
    }

    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IConfiguration configuration;

        public BackgroundJobService(IConfiguration configuration)
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
            var email = configuration.GetValue<string>("email-confg:Email");
            var password = configuration.GetValue<string>("email-confg:password");
            var host = configuration.GetValue<string>("email-confg:Host");
            var port = configuration.GetValue<int>("email-confg:Port");


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
