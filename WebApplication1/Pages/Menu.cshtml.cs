using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Pages
{
    public class MenuModel : PageModel
    {
        private readonly AppDbContext _db;
        public IList<Menu> Menu { get; private set; }
        [BindProperty]
        public string Search { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        public MenuModel(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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
            var item = await _db.Menus.FindAsync(itemID);
            if (item != null)
            {
                _db.Menus.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBuyAsync(int itemID)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db
            .CheckoutCustomers
            .FindAsync(user.Email);

            var item = _db.BasketItems.FromSql("SELECT * FROM BasketItems WHERE StockID = {0} " + "AND BasketID = {1}", itemID, customer.BasketID)
            .ToList()
            .FirstOrDefault();

            if (item == null)
            {
                BasketItem newItem = new BasketItem
                {
                    BasketID = customer.BasketID,
                    StockID = itemID,
                    Quantity = 1
                };
                _db.BasketItems.Add(newItem);
                await _db.SaveChangesAsync();
            }
            else
            {
                item.Quantity = item.Quantity + 1;
                _db.Attach(item).State = EntityState.Modified;
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Basket not found!", e);
                }
            }
            return RedirectToPage();
        }



    }
}
