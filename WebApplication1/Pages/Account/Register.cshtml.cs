using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegistrationModel Input { get; set; }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        private AppDbContext _db;
        public CheckoutCustomer Customer = new CheckoutCustomer();
        public Basket Basket = new Basket();


        public RegisterModel(AppDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }
        public  async Task<IActionResult>  OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    NewBasket(Input.Email);
                    NewCustomer(Input.Email);
                    await _db.SaveChangesAsync();

                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
        public void NewBasket(string Email)
        {
            Basket.Email = Email;
            var currentBasket = _db.Baskets.FromSql("SELECT * From Baskets")
                .OrderByDescending(b => b.BasketID)
                .FirstOrDefault();
            if (currentBasket == null)
            {
                Basket.BasketID = 1;
            }
            else
            {
                Basket.BasketID = currentBasket.BasketID + 1;
            }

            _db.Baskets.Add(Basket);
        }

        public void NewCustomer(string Email)
        {
            Customer.Email = Email;
            Customer.BasketID = Basket.BasketID;
            _db.CheckoutCustomers.Add(Customer);
        }




    }
}