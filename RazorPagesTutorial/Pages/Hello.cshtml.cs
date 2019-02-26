using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesTutorial
{
    public class HelloModel : PageModel
    {

        public string Message {get; private set;} = "Dude, ";

        public void OnGet()
        {
            Message += $"Server time is { DateTime.Now }";
        }
    }
}