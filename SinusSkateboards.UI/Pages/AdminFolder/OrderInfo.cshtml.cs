using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SinusSkateboards.Domain;
using SinusSkateBoards.Data.Database;

namespace SinusSkateboards.UI.Pages.AdminFolder
{
    public class OrderInfoModel : PageModel
    {
        [BindProperty]
        public OrderModel Order { get; set; }

        [BindProperty]
        public decimal TotalPrice { get; set; }

        [BindProperty]

        public int AmountOfProducts { get; set; }

        private readonly AuthDbContext _context;

        public OrderInfoModel(AuthDbContext context)
        {
            _context = context;
        }
        public void OnGet(int id)
        {

            Order = _context.Orders.Where(x => x.Id == id).Include(c => c.CustomerModel).Include(p => p.Products).FirstOrDefault();

            foreach (var amount in Order.Products)
            {
                TotalPrice += amount.Price;
                AmountOfProducts += 1;
            }

        }
    }
}
