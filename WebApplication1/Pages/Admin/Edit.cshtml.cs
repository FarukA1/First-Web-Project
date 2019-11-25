using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Pages.Admin
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Menu Item { get; set; }
        private readonly AppDbContext _db;
        public EditModel(AppDbContext db) { _db = db; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _db.Menus.FindAsync(id);
            if(Item == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Attach(Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new Exception($"Item{Item.ID} not found", e);
            }
            return RedirectToPage("/Index");
        }
    }
}