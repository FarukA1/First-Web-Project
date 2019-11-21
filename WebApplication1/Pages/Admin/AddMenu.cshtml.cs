using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;

namespace WebApplication1.Pages.Admin
{
    public class AddMenuModel : PageModel
    {
        private AppDbContext _db;
        [BindProperty]
        public Menu Menu { get; set; }
        public AddMenuModel(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            _db.Menus.Add(Menu);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}