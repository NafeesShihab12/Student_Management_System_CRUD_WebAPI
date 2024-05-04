using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagementSystem.Service.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessData();

                await GenerateNotifications();

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task ProcessData()
        {
            _logger.LogInformation("Starting data processing...");

            // Simulated data processing tasks
            await Task.Delay(TimeSpan.FromSeconds(10));

            _logger.LogInformation("Data processing complete.");
        }

        private async Task GenerateNotifications()
        {
            _logger.LogInformation("Generating notifications...");

            // Check for upcoming student enrollment deadlines (example)
            DateTime today = DateTime.Today;
            DateTime enrollmentDeadline = new DateTime(today.Year, 5, 31); // Example enrollment deadline (May 31st)
            if (today.AddDays(7) >= enrollmentDeadline)
            {

                _logger.LogInformation("Upcoming enrollment deadline: {0}", enrollmentDeadline);

                SendEmailNotification("enrollment@example.com", "Upcoming Enrollment Deadline", $"The enrollment deadline is approaching. Please complete your enrollment by {enrollmentDeadline.ToShortDateString()}.");
            }

            // Simulated notification generation tasks
            await Task.Delay(TimeSpan.FromSeconds(5)); // Simulated notification generation time

            _logger.LogInformation("Notifications generated.");
        }

        // Example method for sending email notifications
        private void SendEmailNotification(string recipient, string subject, string body)
        {
            _logger.LogInformation("Sending email notification to {0}...", recipient);

            /*
            SmtpClient client = new SmtpClient("smtp.example.com");
            client.Port = 587;
            client.Credentials = new NetworkCredential("username", "password");
            MailMessage message = new MailMessage();
            message.From = new MailAddress("sender@example.com");
            message.To.Add(recipient);
            message.Subject = subject;
            message.Body = body;
            client.Send(message);
            */
            _logger.LogInformation("Email notification sent.");
        }
    }
}
