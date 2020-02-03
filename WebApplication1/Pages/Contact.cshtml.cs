using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;

namespace WebApplication1.Pages
{
    public class ContactModel : PageModel
    {
        private AppDbContext _db;
      
        [BindProperty]
        public Contact Contact { get; set; }
        private readonly UserManager<ApplicationUser> _UserManager;
        [BindProperty]
        CheckoutCustomer customer { get; set; }
       
        public ContactModel(AppDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _UserManager = um;
        }

        public async Task OnGetAsync()
        {
            var user = await _UserManager.GetUserAsync(User);
            customer = await _db 
            .CheckoutCustomers
            .FindAsync(user.Email);
        }

            public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid){ return Page(); }

            _db.Contact.Add(Contact);
            await _db.SaveChangesAsync();
            return RedirectToPage("/MessageRecieved");
        }
    }
}