using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Pages
{
    [Authorize]
    public class MenuModel : PageModel
    {
        private readonly AppDbContext _db;
        public IList<Menu> Menu { get; private set; }
        [BindProperty]
        public string Search { get; set; }
        public MenuModel(AppDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            Menu = _db.Menus.FromSql("SELECT * FROM Menus").ToList();
        }
        public IActionResult OnPostSearch()
        {
            Menu = _db.Menus.FromSql("SELECT* FROM Menus WHERE Name LIKE '" + Search + "%'").ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int itemID)
        {
            var item =await _db.Menus.FindAsync(itemID);
            if(item != null)
            {
                _db.Menus.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}