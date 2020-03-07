using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe;
using WebApplication1.Data;

namespace WebApplication1.Pages
{
   [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _UserManager;
        public OrderHistory Order = new OrderHistory();
        public decimal Total = 0;
        public long AmountPayable = 0;
        public int itemCount = 0;

        [BindProperty]
        public IList<CheckoutItem> Items { get; private set; }
        public BasketItem BasketID { get; private set; }

        public CheckoutModel(AppDbContext db, UserManager<ApplicationUser> UserManager)
        {
            _db = db;
            _UserManager = UserManager;

        }

        public async Task OnGetAsync()
        {
            var user = await _UserManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db
            .CheckoutCustomers
            .FindAsync(user.Email);

            Items = _db.CheckoutItems.FromSql(
                "SELECT Menus.ID, Menus.Price, " +
                "Menus.Name, " +
                "BasketItems.BasketID, BasketItems.Quantity " +
                "FROM Menus INNER JOIN BasketItems " +
                "ON Menus.ID = BasketItems.StockID " +
                "WHERE BasketID = {0}", customer.BasketID
                ).ToList();
            Total = 0;
            itemCount = 0;
            foreach (var item in Items)
            {
                Total = Total + (item.Quantity * item.Price);
                itemCount = Items.Count;  
            }
            
            AmountPayable = (long)(Total * 100);
            
         
        }

        public async Task<IActionResult> OnPostBuyAsync()
        {
            var currentOrder = _db.OrderHistories
  .FromSql("SELECT * From OrderHistories")
                .OrderByDescending(b => b.OrderNo)
                .FirstOrDefault();

            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }

            var user = await _UserManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            CheckoutCustomer customer = await _db
                .CheckoutCustomers
                .FindAsync(user.Email);

            var basketItems =
                _db.BasketItems
                .FromSql("SELECT * From BasketItems " +
                "WHERE BasketID = {0}", customer.BasketID)
                .ToList();

            foreach (var item in basketItems)
            {
                Data.OrderItem oi = new Data.OrderItem
                {
                    OrderNo = Order.OrderNo,
                    StockID = item.StockID,
                    Quantity = item.Quantity
                };
                _db.OrderItems.Add(oi);
                _db.BasketItems.Remove(item);
            }

            await _db.SaveChangesAsync();
            return RedirectToPage("/PaymentReceived");

        }
        public async Task<IActionResult> OnPostChargeAsync(
   string stripeEmail,
   string stripeToken,
   long amount)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = amount,
                Description = "CO5227 Stationaries Charge",
                Currency = "gbp",
                Customer = customer.Id
            });
            await OnPostBuyAsync();
            return RedirectToPage("/PaymentReceived");
        }

        public async Task<IActionResult> OnPostDeleteAsync (int itemID)
        {
            var user = await _UserManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db
            .CheckoutCustomers
            .FindAsync(user.Email);


            BasketItem item = _db.BasketItems.FromSql("SELECT * FROM BasketItems WHERE BasketID = {0} AND StockID = {1}", customer.BasketID, itemID).FirstOrDefault();
           

         
            if (item != null)
            {
               
                _db.BasketItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        

    }
}
