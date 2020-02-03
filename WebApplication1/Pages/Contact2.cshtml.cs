using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;

namespace WebApplication1.Pages
{
    public class Contact2Model : PageModel
    {
        private AppDbContext _db;
        [BindProperty]
        public Contact Contact { get; set; }

        public Contact2Model(AppDbContext db)
        {
            _db = db;
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }

            _db.Contact.Add(Contact);
            await _db.SaveChangesAsync();
            return RedirectToPage("/MessageRecieved");
        }
    }
}