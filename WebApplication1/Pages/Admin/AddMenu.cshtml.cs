using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
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
        [BindProperty]
        public IFormFile Pic { get; set; }
        private readonly IHostingEnvironment _he;
        public AddMenuModel(AppDbContext db, IHostingEnvironment he)
        {
   
            _db = db;
            _he = he;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            if(Pic != null)
            {
                var filename = Path.Combine(_he.WebRootPath, "Img", Path.GetFileName(Pic.FileName));
                Pic.CopyTo(new FileStream(filename, FileMode.Create));
                Menu.Image = Path.Combine("Img", Path.GetFileName(Pic.FileName));
            }
            _db.Menus.Add(Menu);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}