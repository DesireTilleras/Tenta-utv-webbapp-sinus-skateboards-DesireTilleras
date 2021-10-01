using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SinusSkateboards.Domain;
using SinusSkateBoards.Data.Database;

namespace SinusSkateboards.UI.Pages
{
    public class ConfirmationPageModel : PageModel
    {

        [BindProperty]
        public List<ProductModel> CookieProducts { get; set; } = IndexModel.ProductsAddedToCart;

        [BindProperty]
        public decimal TotalPrice { get; set; }

        private readonly AuthDbContext _context;

        [BindProperty]

        public OrderModel Order { get; set; }

        public ConfirmationPageModel(AuthDbContext context)
        {
            _context = context;
        }
        public void OnGet(int id)
        {

            foreach (var product in IndexModel.ProductsAddedToCart)
            {
                TotalPrice += product.Price;

            }

            IndexModel.ProductsAddedToCart.Clear();

            Order = _context.Orders.Where(x => x.Id == id).Include(c => c.CustomerModel).Include(p => p.Products).FirstOrDefault();

        }

    }
}
