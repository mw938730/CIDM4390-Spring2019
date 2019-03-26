using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SendGrid;
using SendGrid.Helpers.Mail;

using sendgridexample.Models;

namespace sendgridexample.Email
{
    public class SendModel : PageModel
    {

        public async Task<string> SendEmail(string email, string subject, string message)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jeff@babbster.net", "Babbster");
            //var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(email, "The Dude");
            var plainTextContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, " ");
            
            var response = await client.SendEmailAsync(msg);

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Body.ReadAsStringAsync().Result); // The message will be here
            Console.WriteLine(response.Headers.ToString());

            return response.StatusCode.ToString();
        }

        [BindProperty]
        public EmailModel Email{ get; set;}

        public void OnGet()
        {

        }

        // public IActionResult OnPost()
        // {
        //     if(!ModelState.IsValid)
        //     {
        //         return Page();
        //     }

        //     return RedirectToPage("/Email/Result");
        // }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var status = await SendEmail(Email.Email, Email.Subject, Email.Message);

            TempData["ResponseCode"] = status;
            TempData["EmailAddress"] = Email.Email;
            TempData["EmailSubject"] = Email.Subject;
            TempData["EmailMessage"] = Email.Message;            

            return RedirectToPage("/Email/Result");            

        }
    }
}