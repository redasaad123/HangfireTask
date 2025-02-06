using Hangfire;
using HangfireTask.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace HangfireTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangFireController : ControllerBase
    {
        
        private readonly IBackgroundJobService service;
        int counter = 0;

        public HangFireController(IBackgroundJobService service)
        {
            this.service = service;
        }

         //Enqueue a Job to Process an Order

        [HttpGet]
        public IActionResult EnqueueJob()
        {
           var count =  BackgroundJob.Enqueue(() => service.CountOfClicked(counter));
            
            return Ok($"this Endpoint is Clicked {count} --> {DateTime.Now}");

        }


        //Schedule a Delayed Job to Send an Email Notification
        [HttpPost("Schedule")]
        public IActionResult Schedule(string Email)
        {
            BackgroundJob.Schedule(() => service.SendEmail(Email), TimeSpan.FromSeconds(5));
            return Ok();

        }


        //Create a Recurring Job to Generate a Daily Report

        [HttpGet("Recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate(() => service.DailyReport(), Cron.Daily(12,0));
            return Ok();

        }

        //Demonstrate CancellationToken to Cancel a Loop Operation

        [HttpGet("CancellationToken")]
        public async Task<IActionResult> CancellationToken(CancellationToken cancellationToken)
        {
            try
            {
                
                await service.CancellationToken(cancellationToken);
                return Ok("All steps is completed");


            }
            catch(OperationCanceledException)
            {
                return BadRequest("canceled");

            }

        }






    }
}
