using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RazorPagesTutorial.Data;

namespace RazorPagesTutorial.Pages
{
    public class CreateModel : PageModel
    {
        public void OnGet()
        {
        }

        private readonly AppDbContext _db;

        public CreateModel(AppDbContext db)
        {
            _db = db;
        }        

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Customers.Add(Customer);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Index");
        }        

    }
}